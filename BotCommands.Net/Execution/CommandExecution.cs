using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using BotCommands.Entities;
using BotCommands.Events;
using BotCommands.Interfaces;
using BotCommands.Matching;
using BotCommands.Parsing;

namespace BotCommands.Execution
{
    internal sealed class CommandExecution<TContext> where TContext : IContext
    {
        
        internal (Task,CommandExecutedEventArgs) ExecuteCommand(Command<TContext> commandMatch, ParsedCommand parsedCommand)
        {
            var commandMethod = commandMatch?.Method;
            if (commandMethod is null)
                return (Task.CompletedTask,
                    new CommandExecutedEventArgs(EventExecutionStatus.CommandNotFound, parsedCommand.Context));
            var argArray = ConstructArgumentArray(commandMatch, parsedCommand);
            if(!CheckUserHasSufficientPriviliges(commandMatch,parsedCommand)) return 
                (Task.CompletedTask, new CommandExecutedEventArgs(EventExecutionStatus.InsufficientPermissions, parsedCommand.Context));
            return ((Task)commandMethod.Invoke(commandMatch.DeclaringModuleInstance, argArray),
                new CommandExecutedEventArgs(EventExecutionStatus.Executed, parsedCommand.Context));
        }
        
        private object[] ConstructArgumentArray(Command<TContext> matchedCommand, ParsedCommand parsedCommand)
        {
            // TODO: Fix this so it works properly.
            var argArray = new object[matchedCommand.Arguments.Count];
            argArray[0] = parsedCommand.Context;
            var currArg = parsedCommand.CommandArgsStart;
            for (int i = 0, j = 1; j < argArray.Length; i++, j++)
            {
                var commandArg = matchedCommand.ArgumentsWithoutContext[i];
                if (commandArg.IsArray)
                {
                    var arrayList = new List<object>();
                    while (currArg.ArgType.IsAssignableFrom(commandArg.GetElementType()))
                    {
                        arrayList.Add(currArg.ArgObj);
                        if (currArg.Next == null)
                            break;
                        currArg = currArg.Next;
                    }
                    var newArray = Array.CreateInstance(commandArg.GetElementType(), arrayList.Count);
                    Array.Copy(arrayList.ToArray(), newArray, newArray.Length);
                    argArray[j] = newArray;
                    i++;
                    continue;
                }
                argArray[j] = currArg?.ArgObj;
                currArg = currArg?.Next;
            }
            return argArray;
        }

        private bool CheckUserHasSufficientPriviliges(Command<TContext> commandMatch, ParsedCommand parsedCommand)
        {
            var permissionCheckList = new List<Module<TContext>>();
            var currentModule = commandMatch.DeclaringModule;
            while (currentModule != null)
            {
                permissionCheckList.Add(currentModule);
                currentModule = currentModule.Parent;
            }

            foreach (var toCheck in permissionCheckList)
            {
                if (!toCheck.ModuleRequiresPermissionValidation) continue;
                var userCanExecute = ((IModulePermissions<TContext>) toCheck.Instance)
                    .UserHasSufficientPermissions((TContext)parsedCommand.Context);
                if (!userCanExecute)
                    return false;
            }
            return true;
        }
    }
}
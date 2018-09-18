using System.Linq;
using System.Threading.Tasks;
using BotCommands.Entities;
using BotCommands.Events;
using BotCommands.Parsing;

namespace BotCommands.Execution
{
    internal sealed class CommandExecution
    {
        
        internal (Task,CommandExecutedEventArgs) ExecuteCommand(MatchedCommand commandMatch, ParsedCommand parsedCommand)
        {
            var commandMethod = commandMatch.Command?.Method;
            if (commandMethod is null)
                return (Task.CompletedTask,
                    new CommandExecutedEventArgs(EventExecutionStatus.CommandNotFound, parsedCommand.Context));
            var argArray = ConstructArgumentArray(commandMatch, parsedCommand);
            return ((Task)commandMethod.Invoke(commandMatch.Command.ContainingTypeInstance, argArray),
                new CommandExecutedEventArgs(EventExecutionStatus.Executed, parsedCommand.Context));
        }
        
        private object[] ConstructArgumentArray(MatchedCommand matchedCommand, ParsedCommand parsedCommand)
        {
            var argArray = new object[matchedCommand.Command.Arguments.Count];
            argArray[0] = parsedCommand.Context;
            for (int i = matchedCommand.SubModuleIncrement, j = 1; j < argArray.Length; i++, j++)
                argArray[j] = parsedCommand.CommandArgs[i].ArgObj;
            if (matchedCommand.UseRemainder)
                argArray[argArray.Length - 1] =
                    parsedCommand.CommandArgs
                        .Skip(matchedCommand.RemainderIncrement)
                        .Select(x => x.ArgObj.ToString())
                        .Aggregate((x, y) => $"{x} {y}");
            return argArray;
        }
    }
}
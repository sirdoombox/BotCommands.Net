using System;
using System.Collections.Generic;
using System.Linq;
using BotCommands.Entities;
using BotCommands.Interfaces;
using BotCommands.Parsing;

namespace BotCommands.Matching
{
    internal class CommandMatcher<TContext> where TContext : IContext
    {
        
        internal Command<TContext> MatchCommand(IReadOnlyList<Module<TContext>> modules, ParsedCommand command)
        {
            var commandArgs = command.FullArgsStart;
            if (commandArgs is null)
                return default(Command<TContext>);
            if (commandArgs.ArgType != typeof(string))
                return default(Command<TContext>);
            var matchModule = modules.FirstOrDefault(x =>
                ModuleIsMatchToString(x, (string)commandArgs.ArgObj));
            if (matchModule is null)
                return default(Command<TContext>);
            var matchedSubModule = TryMatchSubModule(matchModule, command);
            command.CommandArgsStart = matchedSubModule.newStart;
            var matchedCommand = TryMatchCommand(matchedSubModule.Item2, command);
            return matchedCommand;
        }

        private bool ModuleIsMatchToString(Module<TContext> module, string toCompare) =>
            module.Names.Any(x => string.Compare(x, toCompare, StringComparison.InvariantCultureIgnoreCase) == 0);

        private (ParsedArgument newStart, Module<TContext> matchSubmodule) TryMatchSubModule(Module<TContext> module, ParsedCommand command)
        {
            var arg = command.FullArgsStart.Next;
            while (true)
            {
                if (arg == null)
                    return (null, module);
                if (arg.ArgType != typeof(string)) 
                    return (arg, module);
                var matchModule = module.Children?.FirstOrDefault(x => ModuleIsMatchToString(x, (string)arg.ArgObj));
                if (matchModule is null) 
                    return (arg, module);
                module = matchModule;
                arg = arg.Next;
            }
        }

        private Command<TContext> TryMatchCommand(Module<TContext> module, ParsedCommand command)
        {
            return module.Commands.FirstOrDefault(x => ArgumentSignatureMatch(x, command));
        }

        private bool ArgumentSignatureMatch(Command<TContext> command, ParsedCommand parsedCommand)
        {
            if (command.ArgCountWithoutContext <= 0 && parsedCommand.CommandArgsStart is null)
                return true;
            if (command.ArgCountWithoutContext <= 0)
                return false;
            var currArg = parsedCommand.CommandArgsStart;
            for(var i = 0; i < command.ArgCountWithoutContext;)
            {
                var commandArg = command.ArgumentsWithoutContext[i];
                if (commandArg.IsArray && currArg.ArgType.IsAssignableFrom(commandArg.GetElementType()))
                {
                    currArg = currArg.Next;
                    if (currArg == null && command.ArgumentsWithoutContext.Count == i)
                        return true;
                    if (currArg == null)
                        return false;
                    if (!currArg.ArgType.IsAssignableFrom(commandArg.GetElementType()))
                        i++;
                    continue;
                }
                if (commandArg != currArg.ArgType)
                    return false;
                i++;
                currArg = currArg.Next;
            }
            return true;
        }
    }
}
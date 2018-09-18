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
        
        internal MatchedCommand MatchCommand(IReadOnlyList<Module<TContext>> modules, ParsedCommand command)
        {
            var commandArgs = command.CommandArgs;
            if (commandArgs is null)
                return default(MatchedCommand);
            if (commandArgs.FirstOrDefault()?.ArgType != typeof(string))
                return default(MatchedCommand);
            var matchModule = modules.FirstOrDefault(x =>
                ModuleIsMatchToString(x, (string) commandArgs.FirstOrDefault()?.ArgObj));
            if (matchModule is null)
                return default(MatchedCommand);
            var matchedSubModule = TryMatchSubModule(matchModule, command);
            var matchedCommand = TryMatchCommand(matchedSubModule.Item2, matchedSubModule.Item1, command);
            return new MatchedCommand(matchedCommand.command, matchedSubModule.submoduleIncrement, matchedCommand.remainderIncrement);
        }

        private bool ModuleIsMatchToString(Module<TContext> module, string toCompare) =>
            module.Names.Any(x => string.Compare(x, toCompare, StringComparison.InvariantCultureIgnoreCase) == 0);

        private (int submoduleIncrement, Module<TContext> matchSubmodule) TryMatchSubModule(Module<TContext> module, ParsedCommand command)
        {
            var increment = 1;
            while (true)
            {
                if (increment >= command.CommandArgs.Count)
                    return (increment, module);
                var arg = command.CommandArgs[increment];
                if (arg.ArgType != typeof(string)) 
                    return (increment,module);
                var matchModule = module.Children?.FirstOrDefault(x => ModuleIsMatchToString(x, (string) arg.ArgObj));
                if (matchModule is null) 
                    return (increment,module);
                module = matchModule;
                increment++;
            }
        }

        private (Command command, int remainderIncrement) TryMatchCommand(Module<TContext> module, int commandArgStart, ParsedCommand command)
        {
            //var commandArgs = command.CommandArgs.Skip(commandArgStart);
            var hardCommandMatch = 
                module.Commands.FirstOrDefault(x => ArgumentSignatureMatch(x, command, commandArgStart));
            if (hardCommandMatch != null)
                return (hardCommandMatch,-1);
            if (!module.Commands.Any(x => x.SupportsRemainders))
                return (null,-1);
            var remainderIncrement = -1;
            var commandMatchWithRemainders =
                module.Commands.Where(x => x.SupportsRemainders).FirstOrDefault(x =>
                {
                    var sigMatch = ArgumentSignatureMatchWithRemainders(x, command, commandArgStart);
                    if (sigMatch < 0) return false;
                    remainderIncrement = sigMatch;
                    return true;
                });
            return (commandMatchWithRemainders, remainderIncrement);
        }

        private bool ArgumentSignatureMatch(Command command, ParsedCommand parsedCommand, int increment)
        {
            if (command.ArgCountWithoutContext != parsedCommand.CommandArgs.Count - increment)
                return false;
            foreach (var arg in command.ArgumentsWithoutContext)
            {
                if (parsedCommand.CommandArgs[increment].ArgType != arg)
                    return false;
                increment++;
            }
            return true;
        }

        private int ArgumentSignatureMatchWithRemainders(Command command, ParsedCommand parsedCommand, int subModuleIncrement)
        {
            var increment = subModuleIncrement;
            if (command.ArgCountWithoutContext == 1)
                return 1;
            if (command.ArgumentsWithoutContext.Last() != typeof(string))
                return 0;
            var argsWithoutRemainderString = command.ArgumentsWithoutContext.Reverse().Skip(1).Reverse();
            foreach (var arg in argsWithoutRemainderString)
            {
                if (parsedCommand.CommandArgs[increment].ArgType != arg)
                    return -1;
                increment++;
            }
            return increment;
        }
    }
}
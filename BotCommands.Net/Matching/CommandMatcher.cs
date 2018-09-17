using System;
using System.Collections.Generic;
using System.Linq;
using BotCommands.Context;
using BotCommands.Entities;
using BotCommands.Parsing;

namespace BotCommands.Matching
{
    internal class CommandMatcher<TContext> where TContext : IContext
    {
        internal (Command match, bool requiresRemainders) MatchCommand(IReadOnlyList<Module<TContext>> modules, ParsedCommand command)
        {
            var commandArgs = command.CommandArgs;
            if (commandArgs is null)
                return (null,false);
            if (commandArgs.FirstOrDefault()?.ArgType != typeof(string))
                return (null,false);
            var matchModule = modules.FirstOrDefault(x =>
                ModuleIsMatchToString(x, (string) commandArgs.FirstOrDefault()?.ArgObj));
            if (matchModule is null)
                return (null,false);
            var matchedSubModule = TryMatchSubModule(matchModule, command);
            return TryMatchCommand(matchedSubModule.Item2, matchedSubModule.Item1, command);
        }

        private bool ModuleIsMatchToString(Module<TContext> module, string toCompare) =>
            module.Aliases.Any(x => string.Compare(x, toCompare, StringComparison.InvariantCultureIgnoreCase) == 0);

        private (int,Module<TContext>) TryMatchSubModule(Module<TContext> module, ParsedCommand command)
        {
            var increment = 1;
            while (true)
            {
                if (increment > command.CommandArgs.Count)
                    return (increment, module);
                var arg = command.CommandArgs[increment];
                if (arg.ArgType != typeof(string)) 
                    return (increment,module);
                var matchModule = module.Children.FirstOrDefault(x => ModuleIsMatchToString(x, (string) arg.ArgObj));
                if (matchModule is null) 
                    return (increment,module);
                module = matchModule;
                increment++;
            }
        }

        private (Command,bool) TryMatchCommand(Module<TContext> module, int commandArgStart, ParsedCommand command)
        {
            var commandArgs = command.CommandArgs.Skip(commandArgStart);
            var hardCommandMatch = 
                module.Commands.FirstOrDefault(x => ArgumentSignatureMatch(x, command, commandArgStart));
            if (hardCommandMatch != null)
                return (hardCommandMatch,false);
            if (!module.Commands.Any(x => x.SupportsRemainders))
                return (null,false);
            var commandMatchWithRemainders =
                module.Commands.FirstOrDefault(x => ArgumentSignatureMatch(x, command, commandArgStart));
            return (commandMatchWithRemainders, true);
        }

        private bool ArgumentSignatureMatch(Command command, ParsedCommand parsedCommand, int increment)
        {
            foreach (var arg in command.ArgumentsWithoutContext)
            {
                if (parsedCommand.CommandArgs[increment].ArgType != arg)
                    return false;
                increment++;
            }
            return true;
        }

        private bool ArgumentSignatureMatchWithRemainders(Command command, ParsedCommand parsedCommand, int increment)
        {
            if (command.ArgumentsWithoutContext.Count() == 1)
                return true;
            var argsWithoutRemainderString = command.ArgumentsWithoutContext.Reverse().Skip(1).Reverse();
            foreach (var arg in argsWithoutRemainderString)
            {
                if (parsedCommand.CommandArgs[increment].ArgType != arg)
                    return false;
                increment++;
            }
            return true;
        }
    }
}
using System.Collections.Generic;
using BotCommands.Interfaces;

namespace BotCommands.Parsing
{
    internal struct ParsedCommand
    {
        internal readonly IReadOnlyList<ParsedArgument> CommandArgs;
        internal readonly IContext Context;

        public ParsedCommand(IReadOnlyList<ParsedArgument> commandArgs, IContext context)
        {
            CommandArgs = commandArgs;
            Context = context;
        }
    }
}
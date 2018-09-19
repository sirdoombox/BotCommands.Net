using System.Collections.Generic;
using BotCommands.Interfaces;

namespace BotCommands.Parsing
{
    internal class ParsedCommand
    {
        internal readonly ParsedArgument FullArgsStart;
        internal ParsedArgument CommandArgsStart;
        internal readonly IContext Context;

        public ParsedCommand(ParsedArgument fullArgsStart, IContext context)
        {
            FullArgsStart = fullArgsStart;
            Context = context;
        }
    }
}
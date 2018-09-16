using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using BotCommands.Context;

namespace BotCommands.Parsing
{
    internal struct ParsedCommand
    {
        internal readonly IReadOnlyList<ParsedArgument> Command;
        internal readonly IContext Context;
    }
}
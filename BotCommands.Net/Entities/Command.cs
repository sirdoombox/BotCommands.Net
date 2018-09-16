using System;
using System.Collections.Generic;
using System.Reflection;

namespace BotCommands.Entities
{
    public sealed class Command
    {
        public MethodInfo Method { get; internal set; }
        public IReadOnlyList<Type> Arguments { get; internal set; }
        public IReadOnlyList<string> Aliases { get; internal set; }
        public bool SupportsRemainders { get; internal set; }

        public Command()
        {
            
        }
    }
}
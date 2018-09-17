using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BotCommands.Interfaces;

namespace BotCommands.Entities
{
    internal sealed class Command
    {
        internal MethodInfo Method { get; set; }
        internal IReadOnlyList<Type> Arguments { get; set; }
        internal object ContainingTypeInstance { get;  set; }
        internal bool SupportsRemainders { get; set; }
        internal int ArgCountWithoutContext => Arguments.Count - 1;
        internal IEnumerable<Type> ArgumentsWithoutContext => Arguments.Skip(1);

        public Command()
        {
            
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BotCommands.Interfaces;

namespace BotCommands.Entities
{
    internal sealed class Command<TContext> where TContext : IContext
    {
        internal MethodInfo Method { get; set; }
        internal IReadOnlyList<Type> Arguments { get; set; }
        internal object DeclaringModuleInstance { get;  set; }
        internal int ArgCountWithoutContext => Arguments.Count - 1;
        internal IReadOnlyList<Type> ArgumentsWithoutContext => Arguments.Skip(1).ToList();
        internal Module<TContext> DeclaringModule { get; set; }
    }
}
using System.Collections.Generic;
using BotCommands.Context;
using BotCommands.Interfaces;

namespace BotCommands.Entities
{
    /// <summary>
    /// A Module will maintain an instance of the type 
    /// </summary>
    public sealed class Module<TContext> where TContext : IContext
    {
        internal string Name { get; set; }
        internal IReadOnlyList<string> Aliases { get; set; }
        internal IModule<TContext> ModuleInstance { get; set; }
        internal Module<TContext> Parent { get; set; }
        internal List<Module<TContext>> Children { get; set; }
        internal IReadOnlyList<Command> Commands { get; set; }
        internal object Instance { get; set; }
    }
}
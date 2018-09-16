using System.Collections.Generic;
using System.Reflection;
using BotCommands.Context;
using BotCommands.Interfaces;

namespace BotCommands.Entities
{
    /// <summary>
    /// A Module will maintain an instance of the type 
    /// </summary>
    public sealed class Module<TContext> where TContext : IContext
    {
        public string Name { get; internal set; }
        public IReadOnlyList<string> Aliases { get; internal set; }
        public IBotCommandModule<TContext> ModuleInstance { get; internal set; }
        public Module<TContext> Parent { get; internal set; }
        public IReadOnlyList<Module> Children { get; internal set; }
        public IReadOnlyList<Command> Commands { get; internal set; }
        public object Instance { get; internal set; }
    }
}
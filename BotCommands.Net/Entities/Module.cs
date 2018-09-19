using System.Collections.Generic;
using BotCommands.Interfaces;

namespace BotCommands.Entities
{
    /// <summary>
    /// A Module will maintain an instance of the type 
    /// </summary>
    public sealed class Module<TContext> where TContext : IContext
    {
        internal IReadOnlyList<string> Names { get; set; }
        internal Module<TContext> Parent { get; set; }
        internal List<Module<TContext>> Children { get; set; }
        internal IReadOnlyList<Command<TContext>> Commands { get; set; }
        internal object Instance { get; set; }
        internal bool ModuleRequiresPermissionValidation { get; set; }
    }
}
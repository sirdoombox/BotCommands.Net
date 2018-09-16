using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using BotCommands.Builders;
using BotCommands.Context;
using BotCommands.Entities;
using BotCommands.Interfaces;
using BotCommands.Parsing;

namespace BotCommands.Core
{
    public sealed class Commander<TContext> where TContext : IContext
    {
        public string Prefix { get; set; }
        public int PrefixLength => Prefix.Length;

        private readonly ModuleBuilder<TContext> _moduleBuilder;
        private readonly Parser<TContext> _parser;
        
        private readonly List<Module<TContext>> _registeredModules;

        public Commander()
        {
            _moduleBuilder = new ModuleBuilder<TContext>();
            _parser = new Parser<TContext>();
            _registeredModules = new List<Module<TContext>>();
        }

        /// <summary>
        /// Provides <c>VERY</c> simplistic <c>CONSTRUCTOR ONLY</c> DI for modules.
        /// NOTE: Ensure that all dependencies you require for your modules are registered before you register modules.
        /// </summary>
        public void RegisterDependency(object dependencyInstance) =>
            _moduleBuilder.AddDependency(dependencyInstance);
        
        /// <inheritdoc cref="RegisterDependency"/>
        public void RegisterDependencies(params object[] dependencyInstances)
        {
            foreach (var dependencyInstance in dependencyInstances)
                RegisterDependency(dependencyInstance);
        }

        /// <inheritdoc cref="RegisterDependency"/>
        /// <summary>
        /// Creates an instance of the type and returns it. This method requires the service has a parameterless constructor.
        /// </summary>
        /// <returns>A new instance of <see cref="T"/> that was registered.</returns>
        public T RegisterDependency<T>()
        {
            var newInstance = Activator.CreateInstance<T>();
            RegisterDependency(newInstance);
            return newInstance;
        }
        
        /// <summary>
        /// Register a module, it's commands, sub modules, sub commands and so on.
        /// </summary>
        /// <typeparam name="T">The type of command to register.</typeparam>
        public void RegisterModule<T>() where T : IBotCommandModule<TContext>
        {
            _registeredModules.Add(_moduleBuilder.BuildModule<T>());
        }
        
        /// <summary>
        /// Gather all top level types that implement <see cref="IBotCommandModule"/> and register them as commands.
        /// NOTE: I don't recommend this.
        /// </summary>
        public void RegisterAllModules()
        {
            
        }

        /// <summary>
        /// Processes a context generated from whatever service you are providing commands for.
        /// It will parse the message, check if it is a command, find the correct command and execute it.
        /// </summary>
        /// <param name="ctx">The context created by you from a message from the service you're providing commands for.</param>
        public void ProcessMessage(TContext ctx)
        {
            if (ctx.Message.StartsWith(Prefix))
            {
                var result = _parser.ParseContext(ctx, PrefixLength);
                // TODO: Basic implementation for finding commands.
                // TODO: Add a bunch of new attributes.
            }
        }
    }
}
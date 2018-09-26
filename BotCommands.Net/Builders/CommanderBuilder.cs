using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BotCommands.Builders.Internal;
using BotCommands.Core;
using BotCommands.Interfaces;
using BotCommands.Parsing;

namespace BotCommands.Builders
{
    /// <summary>
    /// Used to create new instance of the Commander class.
    /// </summary>
    /// <typeparam name="TContext">The IContext your commander will process.</typeparam>
    public class CommanderBuilder<TContext> where TContext : IContext
    {
        internal readonly Commander<TContext> Instance;
        internal readonly Dictionary<Type, object> Dependencies;
        internal readonly List<Type> Modules;
        internal bool UseRegisterAll = false;
        
        public CommanderBuilder()
        {
            Instance = new Commander<TContext>();
            Dependencies = new Dictionary<Type, object>();
            Modules = new List<Type>();
        }
        
        /// <summary>
        /// Utilises the <see cref="DefaultParser{TContext}"/> for parsing input.
        /// </summary>
        public CommanderBuilder<TContext> WithDefaultParser()
        {
            Instance.Parser = new DefaultParser<TContext>();
            return this;
        }

        /// <summary>
        /// Allows you to supply your own <see cref="IParser{TContext}"/>.
        /// </summary>
        /// <typeparam name="TParser">Your own <see cref="IParser{TContext}"/> - You can also use DefaultParser.</typeparam>
        public CommanderBuilder<TContext> WithParser<TParser>() where TParser : IParser<TContext>, new()
        {
            Instance.Parser = Activator.CreateInstance<TParser>();
            return this;
        }
        
        /// <summary>
        /// Specify the prefix that will be used by your <see cref="Commander{TContext}"/> instance.
        /// </summary>
        /// <param name="prefix">The prefix by which commands will be recognised - Must be at least 1 non whitespace character.</param>
        /// <exception cref="ArgumentException">Thrown if the provided prefix is null, empty or whitespace.</exception>
        public CommanderBuilder<TContext> WithPrefix(string prefix)
        {
            if (string.IsNullOrWhiteSpace(prefix))
                throw new ArgumentException("Provided prefix is null, whitespace or empty.", nameof(prefix));
            Instance.Prefix = prefix;
            return this;
        }
    
        /// <summary>
        /// Provides <c>VERY</c> simplistic <c>CONSTRUCTOR ONLY</c> DI for modules.
        /// NOTE: Ensure that all dependencies you require for your modules are registered before you register modules.
        /// </summary>
        public CommanderBuilder<TContext> WithDependencies(params Type[] dependencyTypes)
        {
            foreach (var type in dependencyTypes)
            {
                if(type is null)
                    throw new ArgumentNullException(nameof(type));
                var ctor = type.GetConstructor(
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, 
                    null, Type.EmptyTypes, null);
                if(ctor is null)
                    throw new ArgumentException("The provided type must have a parameterless constructor.");
                if (Dependencies.ContainsKey(type))
                    throw new ArgumentException($"{type.Name} is already registered.");
                Dependencies.Add(type, Activator.CreateInstance(type));
            }
            return this;
        }
        
        /// <inheritdoc cref="WithDependencies(System.Type[])"/>
        public CommanderBuilder<TContext> WithDependency<TDependency>() where TDependency : new() =>
            WithDependencies(typeof(TDependency));     
        
        /// <inheritdoc cref="WithDependencies(System.Type[])"/>
        public CommanderBuilder<TContext> WithDependencies(params object[] dependencyInstances)
        {
            foreach (var dependencyInstance in dependencyInstances)
            {
                if(dependencyInstance is null)
                    throw new ArgumentNullException(nameof(dependencyInstance));
                if (Dependencies.ContainsKey(dependencyInstance.GetType()))
                    throw new ArgumentException($"A dependency of type {dependencyInstance.GetType().Name} is already registered.");
                Dependencies.Add(dependencyInstance.GetType(), dependencyInstance);
            }
            return this;
        }
        
        /// <summary>
        /// Register a top level module which will include it's submodules.
        /// </summary>
        /// <param name="modules">The types of the modules you wish to use.</param>
        /// <exception cref="InvalidOperationException">Thrown when you attempt to register modules explicitly after setting WithAllModules.</exception>
        public CommanderBuilder<TContext> WithModules(params Type[] modules)
        {
            if(UseRegisterAll)
                throw new InvalidOperationException("You cannot register modules explicitly whilst also using WithAllModules.");
            if(modules.Length <= 0)
                throw new ArgumentException("You must supply at least one or more module types.", nameof(modules));
            foreach (var module in modules)
            {
                if(module is null)
                    throw new ArgumentNullException(nameof(module), "One of the provided modules is null.");
                if(!typeof(IModule<TContext>).IsAssignableFrom(module))
                    throw new ArgumentException($"{module.Name} is not an IModule<{typeof(TContext).Name}> and cannot be registered.", nameof(module));
                if(Modules.Any(x => x == module))
                    throw new ArgumentException($"{module.Name} has already been registered.", nameof(module));
                Modules.Add(module);
            }
            return this;
        }
        
        /// <summary>
        /// Tell the builder to find all valid modules in the assembly and register them.
        /// </summary>
        public CommanderBuilder<TContext> WithAllModules()
        {
            if(Modules.Count >= 1)
                throw new InvalidOperationException("You cannot use WithAllModules whilst registering modules explicitly.");
            UseRegisterAll = true;
            return this;
        }
        
        /// <inheritdoc cref="WithModules"/>
        public CommanderBuilder<TContext> WithModule<TModule>() where TModule : IModule<TContext> =>
            WithModules(typeof(TModule));
        
        /// <summary>
        /// Verifies and builds the <see cref="Commander{TContext}"/>.
        /// </summary>
        /// <returns>A ready-to-use instance of <see cref="Commander{TContext}"/></returns>
        /// <exception cref="InvalidOperationException">Thrown in the case that the Commander configuration is invalid.</exception>
        public Commander<TContext> Build()
        {
            if(Instance.Parser is null)
                throw new InvalidOperationException("You have not specified a parser, please use .WithDefaultParser() or supply your own via .WithParser<T>().");
            if(string.IsNullOrWhiteSpace(Instance.Prefix))
                throw new InvalidOperationException("You have not specified a prefix, please use .WithPrefix().");
            Instance.ModuleBuilder = new ModuleBuilder<TContext>(Instance.Parser.GetValidTypes());
            foreach (var dependency in Dependencies)
                Instance.ModuleBuilder.AddDependency(dependency.Value);
            if (UseRegisterAll)
                Instance.RegisteredModules = Instance.ModuleBuilder.BuildAllModules();
            else
                Instance.RegisteredModules = Modules.Select(x =>
                {
                    var newModule = Instance.ModuleBuilder.BuildModule(x);
                    Instance.ModuleBuilder.BuildModuleRecursive(newModule);
                    return newModule;
                }).ToList();
            return Instance;
        }
    }
}
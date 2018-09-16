using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BotCommands.Attributes;
using BotCommands.Context;
using BotCommands.Entities;
using BotCommands.Interfaces;

namespace BotCommands.Builders
{
    internal sealed class ModuleBuilder<TContext> where TContext : IContext
    {

        private readonly Dictionary<Type, object> _dependencies;

        internal ModuleBuilder()
        {
            _dependencies = new Dictionary<Type, object>();
        }

        internal void AddDependency(object obj)
        {
            if(obj is null) 
                throw new ArgumentNullException(nameof(obj));
            if (_dependencies.ContainsKey(obj.GetType())) 
                throw new ArgumentException("A dependency for this type has already been registered", nameof(obj));
            _dependencies.Add(obj.GetType(), obj);
        }
        
        internal Module<TContext> BuildModule<T>() where T : IBotCommandModule<TContext>
        {
            var newModule = new Module<TContext>();
            newModule.Aliases = typeof(T).GetCustomAttribute<ModuleAliases>().Aliases;
            var ctor = typeof(T).GetConstructors().First();
            var paramsLayout = ctor.GetParameters().Select(x => x.ParameterType).ToArray();
            var paramsInstanceArray = new object[paramsLayout.Length];
            for(var i = 0; i < paramsInstanceArray.Length; i++)
            {
                if (!_dependencies.ContainsKey(paramsLayout[i]))
                    throw new Exception($"Unable to find an instance of {paramsLayout[i].Name} required for the constructor of {nameof(T)}");
                paramsInstanceArray[i] = _dependencies[paramsLayout[i]];
            }
            newModule.Instance = (T)Activator.CreateInstance(typeof(T), paramsInstanceArray);
            return newModule;
            // TODO: Clean this messy ass method up
        }

        private Module<TContext> BuildModuleRecursive<T>(T module) where T : IBotCommandModule<TContext>
        {
            return new Module<TContext>();
            // TODO: Implement basic recursive module building in order to have sub-modules funcitoning correctly.
        }

        private void BuildModuleCommands(Module<TContext> module)
        {
            var commands = new List<Command>();
            var allMethods = module.Instance.GetType().GetMethods();
            foreach (var method in allMethods)
            {
                if (!ParameterIsCommandContext(method.GetParameters().FirstOrDefault())) continue;
                var newCommand = new Command
                {
                    Aliases = method.GetCustomAttribute<CommandAliases>().Aliases,
                    Method = method,
                    Arguments = method.GetParameters().Select(x => x.GetType()).ToList(),
                    SupportsRemainders = method.GetCustomAttribute<CommandSupportsRemainders>() != null
                };
                commands.Add(newCommand);
            }

            module.Commands = commands;
        }

        private static bool ParameterIsCommandContext(ParameterInfo info) =>
            typeof(IContext).IsAssignableFrom(info?.ParameterType);
    }
}
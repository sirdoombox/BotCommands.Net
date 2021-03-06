﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BotCommands.Attributes;
using BotCommands.Entities;
using BotCommands.Interfaces;

namespace BotCommands.Builders.Internal
{
    internal sealed class ModuleBuilder<TContext> where TContext : IContext
    {
        private readonly Dictionary<Type, object> _dependencies;
        private readonly IEnumerable<Type> _allowedTypes;

        internal ModuleBuilder(IEnumerable<Type> allowedTypes)
        {
            _dependencies = new Dictionary<Type, object>();
            _allowedTypes = allowedTypes;
        }

        internal void AddDependency(object obj)
        {
            if(obj is null) 
                throw new ArgumentNullException(nameof(obj));
            if (_dependencies.ContainsKey(obj.GetType())) 
                throw new ArgumentException("A dependency for this type has already been registered", nameof(obj));
            _dependencies.Add(obj.GetType(), obj);
        }

        internal List<Module<TContext>> BuildAllModules()
        {
            var types = Assembly.GetCallingAssembly()
                .GetTypes()
                .Where(x => typeof(IModule<TContext>).IsAssignableFrom(x) && !x.IsNested);
            return types.Select(BuildModule).ToList();
        }
        
        internal Module<TContext> BuildModule(Type type)
        {
            var newModule = new Module<TContext>();
            var namesAttrib = type.GetCustomAttribute<ModuleNames>();
            if(namesAttrib == null)
                throw new Exception($"{type.Name} MUST have the [ModuleNames()] attribute.");
            newModule.Names = type.GetCustomAttribute<ModuleNames>().Names;
            var ctor = type.GetConstructors().First();
            var paramsLayout = ctor.GetParameters().Select(x => x.ParameterType).ToArray();
            var paramsInstanceArray = new object[paramsLayout.Length];
            for(var i = 0; i < paramsInstanceArray.Length; i++)
            {
                if (!_dependencies.ContainsKey(paramsLayout[i]))
                    throw new Exception($"Unable to find an instance of {paramsLayout[i].Name} required for the constructor of {type.Name}");
                paramsInstanceArray[i] = _dependencies[paramsLayout[i]];
            }
            newModule.Instance = Activator.CreateInstance(type, paramsInstanceArray);
            newModule.ModuleRequiresPermissionValidation =
                type.GetInterfaces().Any(x => x == typeof(IModulePermissions<TContext>));
            BuildModuleCommands(newModule);
            VerifyModuleCommandsArguments(newModule);
            BuildModuleRecursive(newModule);
            return newModule;
        }
        
        // Recurses through nested types to create a full map for that module.
        internal void BuildModuleRecursive(Module<TContext> module)
        {
            var nestedTypes = module.Instance.GetType().GetNestedTypes();
            if (nestedTypes.Length <= 0)
                return;
            module.Children = new List<Module<TContext>>();
            foreach (var type in nestedTypes)
            {
                var newModule = BuildModule(type);
                module.Children.Add(newModule);
                newModule.Parent = module;
                BuildModuleRecursive(newModule);
            }
        }

        private void VerifyModuleCommandsArguments(Module<TContext> module)
        {
            foreach (var command in module.Commands)
            {
                for (int i = 1; i < command.ArgCountWithoutContext; i++)
                {
                    var prevArg = command.ArgumentsWithoutContext[i - 1];
                    var arg = command.ArgumentsWithoutContext[i];
                    if(!_allowedTypes.Any(x=> x == prevArg || x == arg))
                        throw new InvalidOperationException($"{command.Method.Name} is using parameter types that the parser does not support.");
                    if (arg.IsArray)
                    {
                        if(arg.GetElementType() == prevArg)
                            throw new Exception($"{command.Method.Name} is invalid - you cannot have an array next to an argument of the same type.");
                    }
                    if (!prevArg.IsArray) continue;
                    if(prevArg.GetElementType() == arg)
                        throw new Exception($"{command.Method.Name} is invalid - you cannot have an array next to an argument of the same type.");
                }
            }
        }

        private void BuildModuleCommands(Module<TContext> module)
        {
            var commands = new List<Command<TContext>>();
            var allMethods = module.Instance.GetType().GetMethods();
            foreach (var method in allMethods)
            {
                // TODO: Find a better way to filter this method out.
                if (method.Name == "UserHasSufficientPermissions") continue;
                if (!ParameterIsCommandContext(method.GetParameters().FirstOrDefault())) continue;
                var newCommand = new Command<TContext>
                {
                    Method = method,
                    Arguments = method.GetParameters().Select(x => x.ParameterType).ToList(),
                    DeclaringModuleInstance = module.Instance,
                    DeclaringModule = module
                };
                commands.Add(newCommand);
            }
            module.Commands = commands;
        }

        private static bool ParameterIsCommandContext(ParameterInfo info) =>
            typeof(IContext).IsAssignableFrom(info?.ParameterType);
    }
}
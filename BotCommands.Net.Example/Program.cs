﻿using System;
using System.ComponentModel;
using System.Reflection;
using System.Threading.Tasks;
using BotCommands.Attributes;
using BotCommands.Core;
using BotCommands.Entities;
using BotCommands.Interfaces;

namespace BotCommands.Net.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            var cmdr = new Commander<ConsoleContext>();
            cmdr.RegisterModule<MathsModule>();
            cmdr.RegisterModule<RemaindersTestModule>();
            cmdr.Prefix = "!";
            while (true)
            {
                var newContext = new ConsoleContext(Console.ReadLine());
                cmdr.ProcessMessageAsync(newContext).GetAwaiter().GetResult();
            }
        }
    }
    
    [ModuleAliases("Maths")]
    public class MathsModule : IModule<ConsoleContext>
    {
        public Task Execute(ConsoleContext ctx)
        {
            Console.WriteLine("Invalid use of command - Use !Maths Add");
            return Task.CompletedTask;
        }
        
        [ModuleAliases("Add")]
        public class AddSubModule : IModule<ConsoleContext>
        {
            public Task Execute(ConsoleContext ctx)
            {
                Console.WriteLine("Invalid use of !Maths Add - please supply 2 numbers.");
                return Task.CompletedTask;
            }
            
            public Task Add(ConsoleContext ctx, int a, int b)
            {
                Console.WriteLine($"Result: {a} + {b} = {a+b}");
                return Task.CompletedTask;
            }
        }
        
        [ModuleAliases("Multiply")]
        public class MultiplySubModule : IModule<ConsoleContext>
        {
            public Task Execute(ConsoleContext ctx)
            {
                Console.WriteLine("Invalid use of !Mats Multiply - please supply 2 numbers");
                return Task.CompletedTask;
            }

            public Task Multiply(ConsoleContext ctx, int a, int b)
            {
                Console.WriteLine($"Result: {a} x {b} = {a*b}");
                return Task.CompletedTask;
            }
        }
    }

    [ModuleAliases("RemaindersTest")]
    public class RemaindersTestModule : IModule<ConsoleContext>
    {
        public Task Execute(ConsoleContext ctx)
        {
            throw new NotImplementedException();
        }

        [CommandSupportsRemainders]
        public Task ThreeArgRemaindersTest(ConsoleContext ctx, bool testBool, int testInt, string remainderString)
        {
            Console.WriteLine($"Bool: {testBool} | Int: {testInt} | Remainder: '{remainderString}'");
            return Task.CompletedTask;
        }
        
        [CommandSupportsRemainders]
        public Task RemaindersTest(ConsoleContext ctx, int testInt, string remainderString)
        {
            Console.WriteLine($"IntVal: {testInt} | Remainders: '{remainderString}'");
            return Task.CompletedTask;
        }
        
        [CommandSupportsRemainders]
        public Task OtherRemaindersTest(ConsoleContext ctx, string remainderString)
        {
            Console.WriteLine($"Remainders: '{remainderString}'");
            return Task.CompletedTask;
        }
    }

    public class ConsoleContext : IContext
    {
        public string Message { get; }
        public string Author { get; }

        public ConsoleContext(string message)
        {
            Message = message;
        }
    }
}
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using BotCommands.Attributes;
using BotCommands.Context;
using BotCommands.Core;
using BotCommands.Interfaces;

namespace BotCommands.Net.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            var cmdr = new Commander<ConsoleContext>();
            cmdr.RegisterModule<MathsModule>();
            cmdr.Prefix = "!";
            while (true)
            {
                var newContext = new ConsoleContext(Console.ReadLine());
                cmdr.ProcessMessageAsync(newContext).GetAwaiter().GetResult();
            }
        }
    }
    
    [ModuleAliases("Maths")]
    class MathsModule : IModule<ConsoleContext>
    {
        public Task Execute(ConsoleContext ctx)
        {
            Console.WriteLine("Invalid use of command - Use !Maths Add/Subtract");
            return Task.CompletedTask;
        }
        
        [ModuleAliases("Add")]
        class AddSubModule : IModule<ConsoleContext>
        {
            public Task Execute(ConsoleContext ctx)
            {
                Console.WriteLine("Invalid use of !Maths Add - please supply 2 numbers.");
                return Task.CompletedTask;
            }
            
            public Task Add(int a, int b)
            {
                Console.WriteLine($"Result: {a} + {b} = {a+b}");
                return Task.CompletedTask;
            }
        }
    }

    class ConsoleContext : IContext
    {
        public string Message { get; }
        public string Author { get; }

        public ConsoleContext(string message)
        {
            Message = message;
        }
    }
}
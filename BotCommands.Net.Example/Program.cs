using System;
using System.Linq;
using System.Threading.Tasks;
using BotCommands.Attributes;
using BotCommands.Core;
using BotCommands.Events;
using BotCommands.Interfaces;

namespace BotCommands.Example
{
    // TODO: Tidy up example and make it actually good.

    internal class Program
    {
        private static void Main(string[] args)
        {
            var cmdr = new Commander<ConsoleContext>();
            cmdr.RegisterModule<MathsModule>();
            cmdr.Prefix = "!";
            cmdr.OnCommmandExecuted += (source, eventArgs) =>
            {
                switch (eventArgs.Status)
                {
                    case EventExecutionStatus.Executed:
                        Console.ForegroundColor = ConsoleColor.Green;
                        break;
                    case EventExecutionStatus.InsufficientPermissions:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        break;
                    case EventExecutionStatus.CommandNotFound:
                        Console.ForegroundColor = ConsoleColor.Red;
                        break;
                }
                Console.Write($"{eventArgs.Status}");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($": {eventArgs.Context.Message}");
            };
            while (true)
            {
                var newContext = new ConsoleContext(Console.ReadLine());
                cmdr.ProcessMessageAsync(newContext).GetAwaiter().GetResult();
            }
        }
    }
    
    [ModuleNames("Maths")]
    public class MathsModule : IModule<ConsoleContext>
    {
        public Task Execute(ConsoleContext ctx)
        {
            Console.WriteLine("Invalid use of command - Use !Maths Add");
            return Task.CompletedTask;
        }
        
        [ModuleNames("Add")]
        public class AddSubModule : IModule<ConsoleContext>
        {
            public Task Execute(ConsoleContext ctx)
            {
                Console.WriteLine("Invalid use of !Maths Add - please supply at least 1 number.");
                return Task.CompletedTask;
            }
            
            public Task Add(ConsoleContext ctx, int[] input)
            {
                var stringRepresentation = input.Select(x => x.ToString()).Aggregate((x, y) => $"{x} + {y}");
                var result = input.Aggregate((x, y) => x + y);
                Console.WriteLine($"Result: {stringRepresentation} = {result}");
                return Task.CompletedTask;
            }
        }
        
        [ModuleNames("Multiply")]
        public class MultiplySubModule : IModule<ConsoleContext>, IModulePermissions<ConsoleContext>
        {
            public Task Execute(ConsoleContext ctx)
            {
                Console.WriteLine("Invalid use of !Maths Multiply - please supply at least 1 number.");
                return Task.CompletedTask;
            }

            public Task Multiply(ConsoleContext ctx, int[] input)
            {
                var stringRepresentation = input.Select(x => x.ToString()).Aggregate((x, y) => $"{x} x {y}");
                var result = input.Aggregate((x, y) => x * y);
                Console.WriteLine($"Result: {stringRepresentation} = {result}");
                return Task.CompletedTask;
            }

            public bool UserHasSufficientPermissions(ConsoleContext ctx)
            {
                // Provide your own logic to test if the user who provided the context is sufficiently
                // elevated to utilise this command.
                return true;
            }
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
using System;
using BotCommands.Builders;
using BotCommands.Events;
using BotCommands.Example.Context;
using BotCommands.Example.Modules;

namespace BotCommands.Example
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var cmdr = new CommanderBuilder<ConsoleContext>()
                .WithPrefix("!")
                .WithDefaultParser()
                .WithModule<MathsModule>()
                .Build();
            cmdr.OnCommandExecuted += (source, eventArgs) =>
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
            Console.WriteLine("Valid Commands are !maths [add/multiply] [number(s)]");
            Console.WriteLine("e.g. !maths add 1 5 100");
            while (true)
            {
                var newContext = new ConsoleContext(Console.ReadLine());
                cmdr.ProcessMessageAsync(newContext).GetAwaiter().GetResult();
            }
        }
    }
}
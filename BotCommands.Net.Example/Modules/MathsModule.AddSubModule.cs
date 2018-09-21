using System;
using System.Linq;
using System.Threading.Tasks;
using BotCommands.Attributes;
using BotCommands.Example.Context;
using BotCommands.Interfaces;

namespace BotCommands.Example.Modules
{
    public partial class MathsModule
    {              
        [ModuleNames("Add", "A")]
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
    }
}
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
        [ModuleNames("Multiply", "Multi", "M")]
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
}
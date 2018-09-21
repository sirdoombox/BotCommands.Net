using System;
using System.Threading.Tasks;
using BotCommands.Attributes;
using BotCommands.Example.Context;
using BotCommands.Interfaces;

namespace BotCommands.Example.Modules
{
    [ModuleNames("Maths", "Math", "M")]
    public partial class MathsModule : IModule<ConsoleContext>
    {
        public Task Execute(ConsoleContext ctx)
        {
            Console.WriteLine("Invalid use of command - Use !Maths Add");
            return Task.CompletedTask;
        }
    }
}
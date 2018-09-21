using System.Threading.Tasks;
using BotCommands.Interfaces;
using BotCommands.Tests.Contexts;

namespace BotCommands.Tests.Modules.InvalidTestModules
{
    public class InvalidTestModuleIncorrectCommandArgumentLayout : IModule<TestContext>
    {
        public Task Execute(TestContext ctx)
        {
            return Task.CompletedTask;
        }

        public Task InvalidMethodLayout(TestContext ctx, string[] strings, string invalidStringArgument)
        {
            return Task.CompletedTask;
        }
    }
}
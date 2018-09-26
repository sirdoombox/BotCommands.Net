using System.Threading.Tasks;
using BotCommands.Interfaces;
using BotCommands.Tests.Contexts;

namespace BotCommands.Tests.Modules.InvalidTestModules
{
    public class InvalidTestModuleUsingTypeNotSupportedByParser : IModule<TestContext>
    {
        public Task Execute(TestContext ctx)
        {
            return Task.CompletedTask;
        }

        public Task SomeInvalidMethod(byte argumentOfInvalidType)
        {
            return Task.CompletedTask;
        }
    }
}
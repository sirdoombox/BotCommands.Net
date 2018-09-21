using System.Threading.Tasks;
using BotCommands.Interfaces;
using BotCommands.Tests.Contexts;

namespace BotCommands.Tests.Modules.InvalidTestModules
{
    public class InvalidTestModuleNoNamesAttribute : IModule<TestContext>
    {
        public Task Execute(TestContext ctx)
        {
            return Task.CompletedTask;
        }
    }
}
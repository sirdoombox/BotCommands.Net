using System.Threading.Tasks;
using BotCommands.Interfaces;
using BotCommands.Tests.Context;

namespace BotCommands.Tests.Commands
{
    public class InvalidTestModule : IModule<TestContext>
    {
        public Task Execute(TestContext ctx)
        {
            return Task.CompletedTask;
        }
    }
}
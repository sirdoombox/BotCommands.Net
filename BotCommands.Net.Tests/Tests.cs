using BotCommands.Core;
using BotCommands.Net.Tests.Context;
using BotCommands.Net.Tests.Services;
using BotCommands.Tests.Commands;
using Xunit;

namespace BotCommands.Net.Tests
{
    public class Class1
    {
        [Fact]
        public void PassingTest()
        {
            var commander = new Commander<TestContext>();
            commander.RegisterDependency<TestService>();
            commander.RegisterModule<TestCommandModule>();
            Assert.Equal(0,0);
        }
    }
}
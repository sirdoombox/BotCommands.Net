using BotCommands.Core;
using BotCommands.Tests.Commands;
using BotCommands.Tests.Context;
using BotCommands.Tests.Services;
using Xunit;

namespace BotCommands.Tests
{
    public class Tests
    {
        // TODO: Test Suite.
        
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
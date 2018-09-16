using BotCommands.Context;

namespace BotCommands.Net.Tests.Context
{
    public class TestContext : IContext
    {
        public string Message { get; set; }
        public string Author { get; set; }
    }
}
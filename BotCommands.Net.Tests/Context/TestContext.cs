using BotCommands.Interfaces;

namespace BotCommands.Tests.Context
{
    public class TestContext : IContext
    {
        public string Message { get; set; }
        public string Author { get; set; }
    }
}
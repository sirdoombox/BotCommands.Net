using BotCommands.Interfaces;

namespace BotCommands.Tests.Contexts
{
    public class TestContext : IContext
    {
        public string Message { get; set; }
        public string Author { get; set; }
    }
}
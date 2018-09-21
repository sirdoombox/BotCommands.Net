using BotCommands.Interfaces;

namespace BotCommands.Example.Context
{
    public class ConsoleContext : IContext
    {
        public string Message { get; }
        public string Author { get; }

        public ConsoleContext(string message)
        {
            Message = message;
        }
    }
}
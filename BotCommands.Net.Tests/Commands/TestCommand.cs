using BotCommands.Attributes;
using BotCommands.Interfaces;

namespace BotCommands.Tests.Commands
{
    [CommandDescription("anus")]
    public class TestCommand : IBotCommand
    {
        public string Execute()
        {
            throw new System.NotImplementedException();
        }
    }
}
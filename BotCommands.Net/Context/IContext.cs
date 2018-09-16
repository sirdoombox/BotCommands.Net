namespace BotCommands.Context
{
    public interface IContext
    {
        string Message { get; }
        string Author { get; }
    }
}
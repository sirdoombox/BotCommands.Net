using BotCommands.Interfaces;

namespace BotCommands.Events
{
    public sealed class CommandExecutedEventArgs
    {
        public EventExecutionStatus Status { get; }
        public IContext Context { get; }

        public CommandExecutedEventArgs(EventExecutionStatus status, IContext context)
        {
            Status = status;
            Context = context;
        }
    }

    public enum EventExecutionStatus
    {
        Executed,
        InsufficientPermissions,
        CommandNotFound
    }
}
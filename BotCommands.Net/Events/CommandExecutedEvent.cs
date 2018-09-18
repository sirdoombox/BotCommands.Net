using BotCommands.Interfaces;

namespace BotCommands.Events
{
    public delegate void CommandExecutedEvent<TContext>(object source, CommandExecutedEventArgs eventArgs)
        where TContext : IContext;
}
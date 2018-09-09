using BotCommands.Interfaces;

namespace BotCommands.Core
{
    public sealed class Commander
    {
        public char PrefixChar { get; set; }

        public Commander()
        {
            
        }
        
        /// <summary>
        /// Register a command.
        /// </summary>
        /// <typeparam name="T">The type of command to register.</typeparam>
        public void RegisterCommand<T>() where T : IBotCommand
        {
            
        }
        
        /// <summary>
        /// Gather all top level types that implement <see cref="IBotCommand"/> and register them as commands.
        /// </summary>
        public void RegisterAllCommands()
        {
            
        }
    }
}
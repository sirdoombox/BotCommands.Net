using System.Threading.Tasks;
using BotCommands.Context;

namespace BotCommands.Interfaces
{
    /// <summary>
    /// The interface which all commands must implement.
    /// </summary>
    public interface IBotCommandModule<TContext> where TContext : IContext
    {
        /// <summary>
        /// Called when the command is executed without additional arguments.
        /// </summary>
        /// <param name="ctx">The context in which the command was executed.</param>
        /// <returns>The response to the command - Return <see cref="string.Empty"/> if none is required.</returns>
        Task Execute(TContext ctx);
    }
}
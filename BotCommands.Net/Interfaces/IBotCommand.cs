namespace BotCommands.Interfaces
{
    /// <summary>
    /// The interface which all commands must implement.
    /// </summary>
    public interface IBotCommand
    {
        /// <summary>
        /// Called when the command is executed without additional arguments.
        /// </summary>
        /// <returns>The response to the command - Return <see cref="string.Empty"/> if none is required.</returns>
        string Execute();
    }
}
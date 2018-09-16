using System;

namespace BotCommands.Attributes
{
    /// <summary>
    /// Indicates that a given command supports remainders.
    /// If the final argument of a method is a string, the rest of the command will be returned to a single string.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class CommandSupportsRemainders : Attribute
    {
        
    }
}
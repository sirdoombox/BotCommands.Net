using System;

namespace BotCommands.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class CommandDescription : Attribute
    {
        public string Description { get; }
        
        /// <summary>
        /// Provide a description for a given command - Used for automating "Help" command creation.
        /// </summary>
        /// <param name="description">Description Text.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="description"/> string is null, empty or whitespace.</exception>
        public CommandDescription(string description)
        {
            if(string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Description string must not be null, empty or whitespace.");
            Description = description;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace BotCommands.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class CommandAliases : Attribute
    {
        public IReadOnlyList<string> Aliases { get; }
        
        /// <summary>
        /// Provide aliases for a given command.
        /// </summary>
        /// <param name="aliases">Aliases for the command.</param>
        /// <exception cref="ArgumentException">Thrown if any of the <paramref name="aliases"/> are null, empty or whitespace.</exception>
        public CommandAliases(params string[] aliases)
        {
            if(aliases.Any(string.IsNullOrWhiteSpace))
                throw new ArgumentException("Alias strings must not be null, empty or whitespaces.");
            Aliases = new List<string>(aliases);
        }
    }
}
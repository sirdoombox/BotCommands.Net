using System;
using System.Collections.Generic;
using System.Linq;

namespace BotCommands.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ModuleNames : Attribute
    {
        public IReadOnlyList<string> Names { get; }
        
        /// <summary>
        /// Provide aliases for a given command.
        /// </summary>
        /// <param name="names">Names for the command.</param>
        /// <exception cref="ArgumentException">Thrown if any of the <paramref name="names"/> are null, empty or whitespace.</exception>
        public ModuleNames(params string[] names)
        {
            if(names.Any(string.IsNullOrWhiteSpace))
                throw new ArgumentException("Alias strings must not be null, empty or whitespaces.");
            Names = new List<string>(names);
        }
    }
}
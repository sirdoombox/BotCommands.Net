using System;
using System.Collections.Generic;
using BotCommands.Interfaces;

namespace BotCommands.Parsing
{
    /// <summary>
    /// Allows you to define a custom parser.
    /// </summary>
    public interface IParser<in TContext> where TContext : IContext
    {
        ParsedCommand ParseContext(TContext ctx, int prefixLength);
        IEnumerable<Type> GetValidTypes();
    }
}
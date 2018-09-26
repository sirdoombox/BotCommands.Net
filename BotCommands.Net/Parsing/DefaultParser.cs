using System;
using System.Collections.Generic;
using System.Linq;
using BotCommands.Interfaces;

namespace BotCommands.Parsing
{
    // TODO: More robust default parser.
    public class DefaultParser<TContext> : IParser<TContext> where TContext : IContext
    {       
        public ParsedCommand ParseContext(TContext ctx, int prefixLength)
        {
            ParsedArgument firstArg = null;
            ParsedArgument currentArg = null;
            var argArray = ctx.Message.Split(' ');
            argArray[0] = argArray[0].Substring(prefixLength);
            var cleanedArgs = argArray.Where(x => !string.IsNullOrWhiteSpace(x));
            foreach (var arg in cleanedArgs)
            {
                ParsedArgument newCurrent = null;
                if (bool.TryParse(arg, out var boolOut))
                    newCurrent = new ParsedArgument(typeof(bool), boolOut);
                else if(int.TryParse(arg, out var intOut))
                    newCurrent = new ParsedArgument(typeof(int), intOut);
                else if(double.TryParse(arg, out var doubleOut))
                    newCurrent = new ParsedArgument(typeof(double), doubleOut);
                else
                    newCurrent = new ParsedArgument(typeof(string), arg);

                if (firstArg == null)
                {
                    firstArg = newCurrent;
                    currentArg = newCurrent;
                    continue;
                }
                currentArg.Next = newCurrent;
                currentArg = newCurrent;
            }
            return new ParsedCommand(firstArg, ctx);
        }

        public IEnumerable<Type> GetValidTypes()
        {
            return new[]
            {
                typeof(bool),
                typeof(int),
                typeof(double),
                typeof(string)
            };
        }
    }
}
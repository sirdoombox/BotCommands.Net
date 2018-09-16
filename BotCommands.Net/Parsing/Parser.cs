using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using BotCommands.Context;

namespace BotCommands.Parsing
{
    internal class Parser<TContext> where TContext : IContext
    {
        internal ParsedCommand ParseContext(TContext ctx, int prefixLength)
        {
            var result = new ParsedCommand();
            var resultContents = new List<ParsedArgument>();
            var argArray = ctx.Message.Split(' ');
            argArray[0] = argArray[0].Substring(prefixLength);
            var cleanedArgs = argArray.Where(x => !string.IsNullOrWhiteSpace(x));
            foreach (var arg in cleanedArgs)
            {
                if(bool.TryParse(arg, out var boolOut))
                    resultContents.Add(new ParsedArgument(typeof(bool), arg, boolOut));
                else if(int.TryParse(arg, out var intOut))
                    resultContents.Add(new ParsedArgument(typeof(int), arg, intOut));
                else if(double.TryParse(arg, out var doubleOut))
                    resultContents.Add(new ParsedArgument(typeof(double), arg, doubleOut));
                else
                    resultContents.Add(new ParsedArgument(typeof(string), arg, arg));
            }
            return result;
            // TODO: More robust parsing because this shit is whack.
        }
    }
}
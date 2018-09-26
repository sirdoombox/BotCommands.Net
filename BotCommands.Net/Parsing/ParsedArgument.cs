using System;

namespace BotCommands.Parsing
{
    public class ParsedArgument
    {
        internal Type ArgType { get; }
        internal object ArgObj { get; }
        internal ParsedArgument Next { get; set; }
        internal bool IsPartOfArray { get; set; } = false;
                
        public ParsedArgument(Type argType, object argObj)
        {
            ArgType = argType;
            ArgObj = argObj;
        }
    }
}
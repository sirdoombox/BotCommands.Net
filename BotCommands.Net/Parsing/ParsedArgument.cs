﻿using System;

namespace BotCommands.Parsing
{
    internal class ParsedArgument
    {
        internal Type ArgType { get; }
        internal string StringRepresentation { get; }
        internal object ArgObj { get; }
                
        internal ParsedArgument(Type argType, string stringRepresentation, object argObj)
        {
            ArgType = argType;
            StringRepresentation = stringRepresentation;
            ArgObj = argObj;
        }
    }
}
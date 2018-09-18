using System;
using BotCommands.Interfaces;

namespace BotCommands.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ModuleRequiresPermissions : Attribute
    {
        
    }
}
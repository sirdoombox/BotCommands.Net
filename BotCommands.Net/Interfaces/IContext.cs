﻿namespace BotCommands.Interfaces
{
    public interface IContext
    {
        string Message { get; }
        string Author { get; }
    }
}
namespace BotCommands.Entities
{
    internal struct MatchedCommand
    {
        internal Command Command { get; }
        internal int SubModuleIncrement { get; }
        internal int RemainderIncrement { get; }
        internal bool UseRemainder => RemainderIncrement >= 0;
           
        public MatchedCommand(Command command, int subModuleIncrement, int remainderIncrement)
        {
            Command = command;
            SubModuleIncrement = subModuleIncrement;
            RemainderIncrement = remainderIncrement;
        }
    }
}
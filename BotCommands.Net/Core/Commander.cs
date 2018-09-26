using System.Collections.Generic;
using System.Threading.Tasks;
using BotCommands.Builders.Internal;
using BotCommands.Entities;
using BotCommands.Events;
using BotCommands.Execution;
using BotCommands.Interfaces;
using BotCommands.Matching;
using BotCommands.Parsing;

namespace BotCommands.Core
{
    public sealed class Commander<TContext> where TContext : IContext
    {
        public string Prefix { get; internal set; }
        public int PrefixLength => Prefix.Length;

        public event CommandExecutedEvent<TContext> OnCommandExecuted;

        internal ModuleBuilder<TContext> ModuleBuilder;
        internal IParser<TContext> Parser;
        private readonly CommandMatcher<TContext> _matcher;
        private readonly CommandExecution<TContext> _execution;
        
        internal IReadOnlyList<Module<TContext>> RegisteredModules;

        internal Commander()
        {
            _matcher = new CommandMatcher<TContext>();
            _execution = new CommandExecution<TContext>();
        }

        /// <summary>
        /// Processes a context generated from whatever service you are providing commands for.
        /// It will parse the message, check if it is a command, find the correct command and execute it.
        /// </summary>
        /// <param name="ctx">The context created by you from a message from the service you're providing commands for.</param>
        public async Task ProcessMessageAsync(TContext ctx)
        {
            if (ctx.Message.StartsWith(Prefix))
            {
                var parsedCommand = Parser.ParseContext(ctx, PrefixLength);
                var commandMatch = _matcher.MatchCommand(RegisteredModules, parsedCommand);
                var result = _execution.ExecuteCommand(commandMatch, parsedCommand);
                await result.Item1;
                OnCommandExecuted?.Invoke(this, result.Item2);
            }
        }
    }
}
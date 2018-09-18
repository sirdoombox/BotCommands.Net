using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using BotCommands.Builders;
using BotCommands.Entities;
using BotCommands.Execution;
using BotCommands.Matching;
using BotCommands.Parsing;
using BotCommands.Tests.Commands;
using BotCommands.Tests.Context;
using BotCommands.Tests.Services;
using Xunit;

namespace BotCommands.Tests.Execution
{
    public class CommandExecutionTests
    {
        private readonly ModuleBuilder<TestContext> _moduleBuilder = new ModuleBuilder<TestContext>();
        private readonly Parser<TestContext> _parser = new Parser<TestContext>();
        private readonly CommandMatcher<TestContext> _matcher = new CommandMatcher<TestContext>();
        private readonly CommandExecution _execution = new CommandExecution();
        
        [Theory]
        [InlineData("!testmodule")]
        [InlineData("!testmodule randomstring")]
        [InlineData("!testmodule testchildmodule")]
        [InlineData("!testmodule testchildmodule 420")]
        private async Task CommandExecutionShouldNotThrowExceptions_WhenValidCommandIsExecuted(string commandText)
        {
            var matchedCommand = MatchCommand(commandText);
            await _execution.ExecuteCommand(matchedCommand.Item1, matchedCommand.Item2);
            Assert.True(true);
        }
        
        private (MatchedCommand,ParsedCommand) MatchCommand(string commandText)
        {
            var parsedCommand = _parser.ParseContext(new TestContext {Message = commandText}, 1);
            var modules = GetModules();
            return (_matcher.MatchCommand(modules, parsedCommand),parsedCommand);
        }

        private IReadOnlyList<Module<TestContext>> GetModules()
        {
            _moduleBuilder.AddDependency(new TestService(0));
            var module = _moduleBuilder.BuildModule<TestModule>();
            return new List<Module<TestContext>> {module};
        }
    }
}
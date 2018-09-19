using System.Collections.Generic;
using System.Threading.Tasks;
using BotCommands.Builders;
using BotCommands.Entities;
using BotCommands.Events;
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
        private readonly CommandExecution<TestContext> _execution = new CommandExecution<TestContext>();
        
        [Theory]
        [InlineData("!testmodule")]
        [InlineData("!testmodule testchildmodule")]
        [InlineData("!testmodule randomstring")]
        [InlineData("!testmodule testchildmodule 420")]
        [InlineData("!TeStMoDuLe randomstring 100 384 243 5 6 true")]
        [InlineData("!TestModule true false true false randomstring")]
        private async Task CommandExecutionShouldReturnCommandExecuted_WhenValidCommandIsExecuted(string commandText)
        {
            var matchedCommand = MatchCommand(commandText);
            var result = _execution.ExecuteCommand(matchedCommand.Item1, matchedCommand.Item2);
            await result.Item1;
            Assert.Equal(EventExecutionStatus.Executed, result.Item2.Status);
        }

        [Fact]
        private async Task CommandExecutionShouldReturnInsufficientPermissions_WhenInsufficientPermissionsForValidCommand()
        {
            var matchedCommand = MatchCommand("!testmodule testchildmodule testtwicenestedchildmodule");
            var result = _execution.ExecuteCommand(matchedCommand.Item1, matchedCommand.Item2);
            await result.Item1;
            Assert.Equal(EventExecutionStatus.InsufficientPermissions, result.Item2.Status);
        }

        [Theory]
        [InlineData("invalid command")]
        [InlineData("!testmodule true")]
        private async Task CommandExecutionShouldReturnCommandNotFound_WhenInvalidCommandIsExecuted(string commandText)
        {
            var matchedCommand = MatchCommand(commandText);
            var result = _execution.ExecuteCommand(matchedCommand.Item1, matchedCommand.Item2);
            await result.Item1;
            Assert.Equal(EventExecutionStatus.CommandNotFound, result.Item2.Status);
        }
        
        private (Command<TestContext>,ParsedCommand) MatchCommand(string commandText)
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
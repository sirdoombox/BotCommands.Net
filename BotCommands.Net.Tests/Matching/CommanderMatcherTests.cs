﻿using System.Collections.Generic;
using BotCommands.Builders;
using BotCommands.Entities;
using BotCommands.Matching;
using BotCommands.Parsing;
using BotCommands.Tests.Commands;
using BotCommands.Tests.Context;
using BotCommands.Tests.Services;
using Xunit;

namespace BotCommands.Tests.Matching
{
    public class CommanderMatcherTests
    {
        private readonly ModuleBuilder<TestContext> _moduleBuilder = new ModuleBuilder<TestContext>();
        private readonly Parser<TestContext> _parser = new Parser<TestContext>();
        private readonly CommandMatcher<TestContext> _matcher = new CommandMatcher<TestContext>();

        [Fact]
        private void MatcherCorrectlyMatchesTopLevelModule()
        {    
            var matchedCommand = MatchCommand("!testmodule");
            Assert.Equal(typeof(TestModule), matchedCommand.Method.DeclaringType);
            Assert.Equal("Execute", matchedCommand.Method.Name);
        }
        
        [Fact]
        private void MatcherCorrectlyMatchesFirstLevelNestedModule()
        {
            var matchedCommand = MatchCommand("!testmodule testchildmodule");
            Assert.Equal(typeof(TestModule.TestChildModule), matchedCommand.Method.DeclaringType);
            Assert.Equal("Execute", matchedCommand.Method.Name);
        }
        
        [Fact]
        private void MatcherCorrectlyMatchesSecondLevelNestedModule()
        {
            var matchedCommand = MatchCommand("!testmodule testchildmodule TestTwiceNestedChildModule");
            Assert.Equal(typeof(TestModule.TestChildModule.TestTwiceNestedChildModule), matchedCommand.Method.DeclaringType);
            Assert.Equal("Execute", matchedCommand.Method.Name);
        }

        [Fact]
        private void MatcherCorrectlyMatchesAgainstCommandBasedOnArguments()
        {
            var matchedCommand = MatchCommand("!testmodule randomstringofcharacters");
            Assert.Equal(typeof(TestModule), matchedCommand.Method.DeclaringType);
            Assert.Equal("SomeOtherMethod", matchedCommand.Method.Name);
        }

        [Fact]
        private void MatcherCorrectlyReturnsNullAgainstNonExistantCommand()
        {
            var matchedCommand = MatchCommand("!thiscommanddoesnotexist");
            Assert.Equal(default, matchedCommand);
        }

        private Command<TestContext> MatchCommand(string commandText)
        {
            var parsedCommand = _parser.ParseContext(new TestContext {Message = commandText}, 1);
            var modules = GetModules();
            return _matcher.MatchCommand(modules, parsedCommand);
        }

        private IReadOnlyList<Module<TestContext>> GetModules()
        {
            _moduleBuilder.AddDependency(new TestService(0));
            var module = _moduleBuilder.BuildModule<TestModule>();
            return new List<Module<TestContext>> {module};
        }
    }
}
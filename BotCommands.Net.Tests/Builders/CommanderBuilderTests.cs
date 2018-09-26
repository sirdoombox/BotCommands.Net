using System;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using BotCommands.Builders;
using BotCommands.Parsing;
using BotCommands.Tests.Contexts;
using BotCommands.Tests.Modules.InvalidTestModules;
using BotCommands.Tests.Modules.ValidTestModules;
using BotCommands.Tests.Services;
using Xunit;

namespace BotCommands.Tests.Builders
{
    public class CommanderBuilderTests
    {
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void BuilderWithPrefixShouldThrowException_OnInvalidPrefixString(string testPrefix) =>
            Assert.Throws<ArgumentException>(() => { new CommanderBuilder<TestContext>().WithPrefix(testPrefix); });

        [Theory]
        [InlineData("?")]
        [InlineData("random long string of words")]
        [InlineData("////")]
        [InlineData("\\n")]
        public void BuilderWithPrefixShouldNotThrowExceptionAndAssignPrefix_OnValidPrefixString(string testPrefix)
        {
            var commanderBuilder = new CommanderBuilder<TestContext>().WithPrefix(testPrefix);
            Assert.Equal(testPrefix, commanderBuilder.Instance.Prefix);
        }

        [Theory]
        [InlineData(null, typeof(string))]
        [InlineData(null, null)]
        public void BuilderWithDependenciesTypeOverloadShouldThrowException_OnNullInArgs(params Type[] types) =>
            Assert.Throws<ArgumentNullException>(() =>
            {
                new CommanderBuilder<TestContext>().WithDependencies(types);
            });

        [Theory]
        [InlineData(typeof(string))]
        [InlineData(typeof(TestContext), typeof(double))]
        public void BuilderWithDependenciesTypeOverloadShouldThrowException_OnTypeWithNoParameterlessCtorInArgs(params Type[] types) =>
            Assert.Throws<ArgumentException>(() =>
            {
                new CommanderBuilder<TestContext>().WithDependencies(types);
            });

        [Fact]
        public void BuilderWithDependenciesTypeOverloadShouldThrowException_OnTypeBeingRegisteredTwice() =>
            Assert.Throws<ArgumentException>(() =>
            {
                new CommanderBuilder<TestContext>().WithDependencies(typeof(string), typeof(string)); 
            });

        [Fact]
        public void BuilderWithDependenciesTypeOverloadShouldAddTypeToCollection_OnValidTypeRegistration()
        {
            var commanderBuilder = new CommanderBuilder<TestContext>().WithDependencies(typeof(TestContext), typeof(StringBuilder));
            Assert.Contains(commanderBuilder.Dependencies, (x) => x.Key == typeof(StringBuilder));
            Assert.Contains(commanderBuilder.Dependencies, (x) => x.Key == typeof(TestContext));
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData(10, null)]
        public void BuilderWithDependenciesObjectInstanceOverloadShouldThrowException_OnNull(params object[] instances) =>     
            Assert.Throws<ArgumentNullException>(() =>
            {
                new CommanderBuilder<TestContext>().WithDependencies(instances);
            });

        [Fact]
        public void BuilderWithDependenciesObjectInstanceOverloadShouldThrowException_OnMultipleObjectsOfSameType() =>
            Assert.Throws<ArgumentException>(() =>
            {
                    new CommanderBuilder<TestContext>().WithDependencies(string.Empty, string.Empty);
            });

        [Theory]
        [InlineData("", 10)]
        [InlineData(100.4f, "randomstring", true)]
        public void BuilderWithDependenciesObjectInstanceOverloadShouldAddDependencies_OnValidDependenciesBeingRegistered(params object[] instances)
        {
            var commanderBuilder = new CommanderBuilder<TestContext>().WithDependencies(instances);
            foreach (var instance in instances)
            {
                Assert.Contains(commanderBuilder.Dependencies, x => x.Key == instance.GetType());
            }
        }
        
        [Fact]
        public void BuilderWithModulesShouldThrowException_OnFirstUsingWithAllModules() =>
            Assert.Throws<InvalidOperationException>(() =>
            {
                new CommanderBuilder<TestContext>().WithAllModules().WithModules(typeof(TestModule));
            });

        [Fact]
        public void BuilderWithAllModulesShouldThrowException_OnFirstUsingWithModules() =>
            Assert.Throws<InvalidOperationException>(() =>
            {
                new CommanderBuilder<TestContext>().WithModules(typeof(TestModule)).WithAllModules();
            });

        [Fact]
        public void BuilderWithModulesShouldThrowException_OnCalledWithNoArguments() =>
            Assert.Throws<ArgumentException>(() =>
            {
                new CommanderBuilder<TestContext>().WithModules();
            });

        [Theory]
        [InlineData(typeof(TestModule), null)]
        [InlineData(typeof(TestModule), typeof(string))]
        [InlineData(typeof(TestModule), typeof(TestModule))]
        public void BuilderWithModulesShouldThrowException_OnCalledWithInvalidArguments(params Type[] types) =>
            Assert.ThrowsAny<Exception>(() =>
            {
                new CommanderBuilder<TestContext>().WithModules(types);
            });
        
        // This test does use an invalid module, however the builder doesn't test the integrity of the modules themselves.
        [Fact]
        public void BuilderWithModulesShouldAddModule_OnCallingWithValidModuleTypeAsArgument()
        {
            var commanderBuilder = new CommanderBuilder<TestContext>().WithModules(typeof(TestModule), typeof(InvalidTestModuleNoNamesAttribute));
            Assert.Contains(commanderBuilder.Modules, x => x == typeof(TestModule));
            Assert.Contains(commanderBuilder.Modules, x => x == typeof(InvalidTestModuleNoNamesAttribute));
        }

        [Fact]
        public void BuildingBuilderShouldThrowException_OnCallingWithoutParser() =>
            Assert.Throws<InvalidOperationException>(() =>
            {
                new CommanderBuilder<TestContext>().WithPrefix("!").Build();
            });

        [Fact]
        public void BuildingBuilderShouldThrowException_OnCallingWithoutPrefix() =>
            Assert.Throws<InvalidOperationException>(() =>
            {
                new CommanderBuilder<TestContext>().WithDefaultParser().Build();
            });

        [Fact]
        public void BuildingBuilderShouldBuildCorrectCommanderInstances_WhenUsedCorrectly()
        {
            var commander = new CommanderBuilder<TestContext>()
                .WithDependencies(new TestService(10))
                .WithModule<TestModule>()
                .WithPrefix("!")
                .WithDefaultParser()
                .Build();
            Assert.Equal(typeof(DefaultParser<TestContext>), commander.Parser.GetType());
            Assert.Equal(typeof(TestModule), commander.RegisteredModules.First().Instance.GetType());
        }
    }
}
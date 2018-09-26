using System;
using System.Linq;
using BotCommands.Builders.Internal;
using BotCommands.Entities;
using BotCommands.Parsing;
using BotCommands.Tests.Contexts;
using BotCommands.Tests.Modules.InvalidTestModules;
using BotCommands.Tests.Modules.ValidTestModules;
using BotCommands.Tests.Services;
using Xunit;

namespace BotCommands.Tests.Builders.Internal
{
    public class ModuleBuilderTests
    {
        // TODO: Test cases where parser doesn't allow for certain method signatures.
        
        [Fact]
        public void BuiltTestModule_ShouldHaveTwoCommands()
        {
            var builtModule = BuildValidModule();
            Assert.Equal(4, builtModule.Commands.Count);
        }

        [Fact]
        public void ValidBuiltTestModuleCommands_ShouldHaveCorrectArgumentCounts()
        {
            var builtModule = BuildValidModule();
            Assert.Collection(builtModule.Commands, 
                item => Assert.Equal(item.ArgCountWithoutContext, 0),
                item => Assert.Equal(item.ArgCountWithoutContext, 1),
                item => Assert.Equal(item.ArgCountWithoutContext, 2),
                item => Assert.Equal(item.ArgCountWithoutContext, 3));
        }

        [Fact]
        public void ValidBuiltTestModule_ShouldHaveCorrectDependencyInjected()
        {
            var moduleBuilder = new ModuleBuilder<TestContext>(new DefaultParser<TestContext>().GetValidTypes());
            moduleBuilder.AddDependency(new TestService(int.MaxValue));
            moduleBuilder.AddDependency("Unused Dependency String.");
            var builtModule = moduleBuilder.BuildModule(typeof(TestModule));
            Assert.Equal(int.MaxValue, ((TestModule)builtModule.Instance).Service.TestValue);
        }

        [Fact]
        public void BuildingModuleWithoutNamesAttribute_ShouldThrowException()
        {
            var moduleBuilder = new ModuleBuilder<TestContext>(new DefaultParser<TestContext>().GetValidTypes());
            Assert.Throws<Exception>(() => moduleBuilder.BuildModule(typeof(InvalidTestModuleNoNamesAttribute)));
        }

        [Fact]
        public void BuildingModuleWithoutDependencyAvailable_ShouldThrowException()
        {
            var moduleBuilder= new ModuleBuilder<TestContext>(new DefaultParser<TestContext>().GetValidTypes());
            Assert.Throws<Exception>(() => moduleBuilder.BuildModule(typeof(TestModule)));
        }
        
        [Fact]
        public void ValidBuiltTestModule_ShouldHaveNestedChildrenOfCorrectType()
        {
            var builtModule = BuildValidModule();
            var firstChild = builtModule.Children.First();
            var twiceNestedChild = firstChild.Children.First();
            Assert.Equal(typeof(TestModule.TestChildModule), firstChild.Instance.GetType());
            Assert.Equal(typeof(TestModule.TestChildModule.TestTwiceNestedChildModule), twiceNestedChild.Instance.GetType());
        }

        [Fact]
        public void ModuleWithInvalidCommand_ShouldThrowException()
        {
            var moduleBuilder = new ModuleBuilder<TestContext>(new DefaultParser<TestContext>().GetValidTypes());
            Assert.Throws<Exception>(() =>
                moduleBuilder.BuildModule(typeof(InvalidTestModuleIncorrectCommandArgumentLayout)));
        }

        [Fact]
        public void ModuleWithCommandWithArgumentTypeNotSupportedByParser_ShouldThrowException()
        {
            var moduleBuilder = new ModuleBuilder<TestContext>(new DefaultParser<TestContext>().GetValidTypes());
            Assert.Throws<Exception>(() => 
                moduleBuilder.BuildModule(typeof(InvalidTestModuleUsingTypeNotSupportedByParser)));
        }

        [Fact]
        public void BuildAllModules_ShouldReturnValidModuleList()
        {
            var moduleBuilder = new ModuleBuilder<TestContext>(new DefaultParser<TestContext>().GetValidTypes());
            Assert.Throws<Exception>(() => moduleBuilder.BuildAllModules());
        }

        private Module<TestContext> BuildValidModule()
        {
            var moduleBuilder = new ModuleBuilder<TestContext>(new DefaultParser<TestContext>().GetValidTypes());
            moduleBuilder.AddDependency(new TestService(int.MaxValue));
            return moduleBuilder.BuildModule(typeof(TestModule));
        }
    }
}
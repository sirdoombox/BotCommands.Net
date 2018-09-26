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
                item => Assert.Equal(0, item.ArgCountWithoutContext),
                item => Assert.Equal(1, item.ArgCountWithoutContext),
                item => Assert.Equal(2, item.ArgCountWithoutContext),
                item => Assert.Equal(3, item.ArgCountWithoutContext));
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
            var moduleBuilder = new ModuleBuilder<TestContext>(new DefaultParser<TestContext>().GetValidTypes());
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
        
        // TODO: This test sucks and it behaves super weirdly depending on release/debug configurations.
        [Fact]
        public void BuildAllModulesShouldThrowException_DueToInvalidModulesBeingInAssembly()
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
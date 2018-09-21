using System;
using System.Linq;
using BotCommands.Builders;
using BotCommands.Entities;
using BotCommands.Tests.Contexts;
using BotCommands.Tests.Modules;
using BotCommands.Tests.Modules.InvalidTestModules;
using BotCommands.Tests.Modules.ValidTestModules;
using BotCommands.Tests.Services;
using Xunit;

namespace BotCommands.Tests.Builders
{
    public class ModuleBuilderTests
    {

        [Fact]
        public void BuiltTestModule_ShouldHaveTwoCommands()
        {
            var builtModule = BuildValidModule();
            Assert.Equal(builtModule.Commands.Count, 4);
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
            var moduleBuilder = new ModuleBuilder<TestContext>();
            moduleBuilder.AddDependency(new TestService(int.MaxValue));
            moduleBuilder.AddDependency("Unused Dependency String.");
            var builtModule = moduleBuilder.BuildModule<TestModule>();
            Assert.Equal(((TestModule)builtModule.Instance).Service.TestValue, int.MaxValue);
        }

        [Fact]
        public void BuildingModuleWithoutNamesAttribute_ShouldThrowException()
        {
            var moduleBuilder = new ModuleBuilder<TestContext>();
            Assert.Throws<Exception>(() => moduleBuilder.BuildModule<InvalidTestModuleNoNamesAttribute>());
        }

        [Fact]
        public void BuildingModuleWithoutDependencyAvailable_ShouldThrowException()
        {
            var moduleBuilder= new ModuleBuilder<TestContext>();
            Assert.Throws<Exception>(() => moduleBuilder.BuildModule<TestModule>());
        }
        
        [Fact]
        public void ValidBuiltTestModule_ShouldHaveNestedChildrenOfCorrectType()
        {
            var builtModule = BuildValidModule();
            var firstChild = builtModule.Children.First();
            var twiceNestedChild = firstChild.Children.First();
            Assert.Equal(firstChild.Instance.GetType(), typeof(TestModule.TestChildModule));
            Assert.Equal(twiceNestedChild.Instance.GetType(), typeof(TestModule.TestChildModule.TestTwiceNestedChildModule));
        }

        [Fact]
        public void ModuleWithInvalidCommand_ShouldThrowException()
        {
            var moduleBuilder = new ModuleBuilder<TestContext>();
            Assert.Throws<Exception>(() =>
                moduleBuilder.BuildModule<InvalidTestModuleIncorrectCommandArgumentLayout>());
        }

        [Fact]
        public void BuildAllModules_ShouldReturnValidModuleList()
        {
            var moduleBuilder = new ModuleBuilder<TestContext>();
            Assert.Throws<Exception>(() => moduleBuilder.BuildAllModules());
        }

        private Module<TestContext> BuildValidModule()
        {
            var moduleBuilder = new ModuleBuilder<TestContext>();
            moduleBuilder.AddDependency(new TestService(int.MaxValue));
            return moduleBuilder.BuildModule<TestModule>();
        }
    }
}
using System.Threading.Tasks;
using BotCommands.Attributes;
using BotCommands.Interfaces;
using BotCommands.Tests.Contexts;
using BotCommands.Tests.Services;

namespace BotCommands.Tests.Modules.ValidTestModules
{
    [ModuleNames("TestModule")]
    public partial class TestModule : IModule<TestContext>
    {
        public TestService Service { get; }
        
        public TestModule(TestService service)
        {
            Service = service;
        }
        
        public Task Execute(TestContext ctx)
        {
            return Task.CompletedTask;
        }
        
        public Task SomeOtherMethod(TestContext ctx, string[] testArg)
        {
            return Task.CompletedTask;
        }

        public Task SomeMethodWithArrayArgument(TestContext ctx, bool[] bools, string testString)
        {
            return Task.CompletedTask;
        }

        public  Task SomeOtherMethodWithArrayArguments(TestContext ctx, string testString, int[] intArray, bool boolTest)
        {
            return Task.CompletedTask;
        }
    }
}
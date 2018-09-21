using System.Threading.Tasks;
using BotCommands.Attributes;
using BotCommands.Interfaces;
using BotCommands.Tests.Contexts;
using BotCommands.Tests.Services;

namespace BotCommands.Tests.Modules.ValidTestModule
{
    [ModuleNames("TestModule")]
    public partial class TestModule : IModule<TestContext>
    {
        public TestService Service { get; }
        
        public TestModule(TestService service)
        {
            Service = service;
        }
        
        public async Task Execute(TestContext ctx)
        {
            
        }
        
        public async Task SomeOtherMethod(TestContext ctx, string[] testArg)
        {
            
        }

        public async Task SomeMethodWithArrayArgument(TestContext ctx, bool[] bools, string testString)
        {
            
        }

        public async Task SomeOtherMethodWithArrayArguments(TestContext ctx, string testString, int[] intArray, bool boolTest)
        {
            
        }
    }
}
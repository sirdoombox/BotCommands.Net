using System.Threading.Tasks;
using BotCommands.Attributes;
using BotCommands.Interfaces;
using BotCommands.Tests.Context;
using BotCommands.Tests.Services;

namespace BotCommands.Tests.Commands
{
    [ModuleNames("TestModule")]
    public class TestModule : IModule<TestContext>
    {
        public TestService Service { get; }
        
        public TestModule(TestService service)
        {
            Service = service;
        }
        
        public async Task Execute(TestContext ctx)
        {
            
        }
        
        public async Task SomeOtherMethod(TestContext ctx, string testArg)
        {
            
        }
        
        [ModuleNames("TestChildModule")]
        public class TestChildModule : IModule<TestContext>
        {          
            public TestService Service { get; }
        
            public TestChildModule(TestService service)
            {
                Service = service;
            }
            
            public Task Execute(TestContext ctx)
            {
                return Task.CompletedTask;
            }

            public Task SomeOtherCommand(TestContext ctx, int testValue)
            {
                return Task.CompletedTask;
            }
            
            [ModuleNames("TestTwiceNestedChildModule")]
            public class TestTwiceNestedChildModule : IModule<TestContext>
            {          
                public TestService Service { get; }
        
                public TestTwiceNestedChildModule(TestService service)
                {
                    Service = service;
                }
            
                public Task Execute(TestContext ctx)
                {
                    return Task.CompletedTask;
                }
            }
        }
    }
}
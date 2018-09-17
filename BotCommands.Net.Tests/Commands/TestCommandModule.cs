using System.Diagnostics;
using System.Threading.Tasks;
using BotCommands.Attributes;
using BotCommands.Entities;
using BotCommands.Interfaces;
using BotCommands.Tests.Context;
using BotCommands.Tests.Services;
using Xunit;

namespace BotCommands.Tests.Commands
{
    [ModuleAliases("MEMEEEES")]
    public class TestCommandModule : IModule<TestContext>
    {
        public TestService Service { get; }
        
        public TestCommandModule(TestService service)
        {
            Service = service;
            Debug.WriteLine($"CommandModule Instantiated with {service}");
        }
        
        public async Task Execute(TestContext ctx)
        {
            Assert.NotNull(ctx);
        }
        
        public async Task SomeOtherMethod(TestContext ctx, string Anus)
        {
            var ass = Anus;
            var megaAss = ctx.Author;
        }
        
        [ModuleAliases("Dicks")]
        public class TestChildCommandModule : IModule<TestContext>
        {          
            public TestService Service { get; }
        
            public TestChildCommandModule(TestService service)
            {
                Service = service;
                Debug.WriteLine($"TestChildCommandModule Instantiated with {service}");
            }
            
            public Task Execute(TestContext ctx)
            {
                return null;
            }
            
            [ModuleAliases("Dicks")]
            public class TestTwiceNestedChildCommandModule : IModule<TestContext>
            {          
                public TestService Service { get; }
        
                public TestTwiceNestedChildCommandModule(TestService service)
                {
                    Service = service;
                    Debug.WriteLine($"TestTwiceNestedChildCommandModule Instantiated with {service}");
                }
            
                public Task Execute(TestContext ctx)
                {
                    return null;
                }
            }
        }
    }
}
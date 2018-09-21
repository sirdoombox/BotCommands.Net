using System.Threading.Tasks;
using BotCommands.Attributes;
using BotCommands.Interfaces;
using BotCommands.Tests.Contexts;
using BotCommands.Tests.Services;

namespace BotCommands.Tests.Modules.ValidTestModules
{
    public partial class TestModule
    {
        public partial class TestChildModule
        {
            [ModuleNames("TestTwiceNestedChildModule")]
            public class TestTwiceNestedChildModule : IModule<TestContext>, IModulePermissions<TestContext>
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

                public bool UserHasSufficientPermissions(TestContext ctx)
                {
                    return false;
                }
            }
        }
    }
}
using System.Diagnostics;
using System.Threading.Tasks;
using BotCommands.Attributes;
using BotCommands.Entities;
using BotCommands.Interfaces;
using BotCommands.Net.Tests.Context;
using BotCommands.Net.Tests.Services;
using Xunit;

namespace BotCommands.Tests.Commands
{
    [CommandDescription("anus")]
    [ModuleAliases("MEMEEEES")]
    public class TestCommandModule : IBotCommandModule<TestContext>
    {
        public TestService Service { get; }
        
        public TestCommandModule(TestService service)
        {
            Service = service;
            Debug.WriteLine(service);
        }
        
        [CommandAliases("Dingbat")]
        public async Task Execute(TestContext ctx)
        {
            Assert.NotNull(ctx);
        }
        
        [CommandAliases("Memelord")]
        public async Task SomeOtherMethod(TestContext ctx, string Anus)
        {
            var ass = Anus;
            var megaAss = ctx.Author;
        }
    }
}
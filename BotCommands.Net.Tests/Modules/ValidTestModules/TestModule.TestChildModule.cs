﻿using System.Threading.Tasks;
using BotCommands.Attributes;
using BotCommands.Interfaces;
using BotCommands.Tests.Contexts;
using BotCommands.Tests.Services;

namespace BotCommands.Tests.Modules.ValidTestModule
{
    public partial class TestModule
    {
        [ModuleNames("TestChildModule")]
        public partial class TestChildModule : IModule<TestContext>
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
        }
    }
}
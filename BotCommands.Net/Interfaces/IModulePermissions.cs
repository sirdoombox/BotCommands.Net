namespace BotCommands.Interfaces
{
    public interface IModulePermissions<in TContext> where TContext : IContext
    {
        bool UserHasSufficientPermissions(TContext ctx);
    }
}
namespace PrettyBots.Model.Descriptors;

public class InteractionModuleInfo
{
    public Type Type { get; }
    public IServiceProvider ServiceProvider { get; }
    public IInteractionModule Instance { get; }
    public List<InteractionHandlerInfo> HandlerInfos { get; } = new();
    
    public InteractionModuleInfo(Type type, IServiceProvider serviceProvider, 
        IInteractionModule instance)
    {
        Type         = type;
        ServiceProvider    = serviceProvider;
        Instance      = instance;
    }
}
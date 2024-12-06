namespace EasyDI;

public static class ContainerExtensions
{
    public static IContainer AttachSingleton(this IContainer container, Type typeToRegister,
        object implementation)
    {
        ArgumentNullException.ThrowIfNull(container);
        ArgumentNullException.ThrowIfNull(typeToRegister);
        ArgumentNullException.ThrowIfNull(implementation);

        container.Register(typeToRegister, implementation.GetType(), () => implementation,
            LifeTime.Singleton);
        return container;
    }

    public static IContainer AttachSingleton(this IContainer container, Type typeToRegister,
        Type implementationType)
    {
        ArgumentNullException.ThrowIfNull(container);
        ArgumentNullException.ThrowIfNull(typeToRegister);
        ArgumentNullException.ThrowIfNull(implementationType);
        
        container.Register(typeToRegister, implementationType, null, LifeTime.Singleton);
        return container;
    }

    public static T Resolve<T>(this IContainer container)
        where T : class
    {
        //TODO: write an implementation
        throw new NotImplementedException();
    }
}
namespace EasyDI;

public static class ContainerExtensions
{
    public static IContainer AttachSingleton(this IContainer container, Type typeToRegister,
        object implementation)
    {
        //TODO: write an implementation
        throw new NotImplementedException();
    }

    public static IContainer AttachSingleton(this IContainer container, Type typeToRegister,
        Type implementationType)
    {
        //TODO: write an implementation
        throw new NotImplementedException();
    }

    public static T Resolve<T>(this IContainer container)
        where T : class
    {
        //TODO: write an implementation
        throw new NotImplementedException();
    }
}
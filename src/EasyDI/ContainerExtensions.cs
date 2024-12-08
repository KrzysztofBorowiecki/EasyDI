namespace EasyDI;

public static class ContainerExtensions
{
    #region Singleton

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

    public static IContainer
        AttachSingleton(this IContainer container,
            Type implementationType)
    {
        ArgumentNullException.ThrowIfNull(container);
        ArgumentNullException.ThrowIfNull(implementationType);

        container.Register(implementationType, implementationType, null,
            LifeTime.Singleton);

        return container;
    }

    public static IContainer AttachSingleton(this IContainer container, Type typeToRegister,
        Func<object> implementationFactory)
    {
        ArgumentNullException.ThrowIfNull(container);
        ArgumentNullException.ThrowIfNull(typeToRegister);
        ArgumentNullException.ThrowIfNull(implementationFactory);

        container.Register(typeToRegister, implementationFactory.GetType(), implementationFactory,
            LifeTime.Singleton);

        return container;
    }

    public static IContainer
        AttachSingleton<TService, TImplementation>(
            this IContainer container)
        where TService : class
        where TImplementation : class, TService

    {
        ArgumentNullException.ThrowIfNull(container);

        container.Register(typeof(TService), typeof(TImplementation), null,
            LifeTime.Singleton);

        return container;
    }

    public static IContainer AttachSingleton<TService, TImplementation>(this IContainer container,
        Func<object> implementationFactory)
        where TService : class
        where TImplementation : class, TService
    {
        ArgumentNullException.ThrowIfNull(container);
        ArgumentNullException.ThrowIfNull(implementationFactory);

        container.Register(typeof(TService), typeof(TImplementation), implementationFactory,
            LifeTime.Singleton);

        return container;
    }

    public static IContainer AttachSingleton<TService>(this IContainer container)
        where TService : class
    {
        ArgumentNullException.ThrowIfNull(container);

        container.Register(typeof(TService), typeof(TService), null,
            LifeTime.Singleton);

        return container;
    }

    public static IContainer AttachSingleton<TService>(this IContainer container,
        Func<object> implementationFactory)
        where TService : class
    {
        ArgumentNullException.ThrowIfNull(container);
        ArgumentNullException.ThrowIfNull(implementationFactory);

        container.Register(typeof(TService), implementationFactory.GetType(), implementationFactory,
            LifeTime.Singleton);

        return container;
    }

    public static IContainer AttachSingleton<TService>(this IContainer container,
        TService implementationType)
        where TService : class
    {
        ArgumentNullException.ThrowIfNull(container);
        ArgumentNullException.ThrowIfNull(implementationType);

        container.Register(typeof(TService), typeof(TService), null,
            LifeTime.Singleton);

        return container;
    }

    #endregion

    #region Transient

    public static IContainer AttachTransient(this IContainer container, Type typeToRegister,
        Type implementationType)
    {
        ArgumentNullException.ThrowIfNull(container);
        ArgumentNullException.ThrowIfNull(typeToRegister);
        ArgumentNullException.ThrowIfNull(implementationType);

        container.Register(typeToRegister, implementationType, null, LifeTime.Transient);

        return container;
    }

    public static IContainer
        AttachTransient(this IContainer container,
            Type implementationType)
    {
        ArgumentNullException.ThrowIfNull(container);
        ArgumentNullException.ThrowIfNull(implementationType);

        container.Register(implementationType, implementationType, null, LifeTime.Transient);

        return container;
    }

    public static IContainer AttachTransient(this IContainer container, Type typeToRegister,
        Func<object> implementationFactory)
    {
        ArgumentNullException.ThrowIfNull(container);
        ArgumentNullException.ThrowIfNull(typeToRegister);
        ArgumentNullException.ThrowIfNull(implementationFactory);

        container.Register(typeToRegister, implementationFactory.GetType(), implementationFactory, LifeTime.Transient);

        return container;
    }

    public static IContainer
        AttachTransient<TService, TImplementation>(
            this IContainer container)
        where TService : class
        where TImplementation : class, TService
    {
        ArgumentNullException.ThrowIfNull(container);

        container.Register(typeof(TService), typeof(TImplementation), null, LifeTime.Transient);

        return container;
    }

    public static IContainer AttachTransient<TService, TImplementation>(this IContainer container,
        Func<object> implementationFactory)
        where TService : class
        where TImplementation : class, TService
    {
        ArgumentNullException.ThrowIfNull(container);
        ArgumentNullException.ThrowIfNull(implementationFactory);

        container.Register(typeof(TService), typeof(TImplementation), implementationFactory, LifeTime.Transient);

        return container;
    }

    public static IContainer AttachTransient<TService>(this IContainer container)
        where TService : class
    {
        ArgumentNullException.ThrowIfNull(container);

        container.Register(typeof(TService), typeof(TService), null, LifeTime.Transient);

        return container;
    }

    public static IContainer AttachTransient<TService>(this IContainer container,
        Func<object> implementationFactory)
        where TService : class
    {
        ArgumentNullException.ThrowIfNull(container);
        ArgumentNullException.ThrowIfNull(implementationFactory);

        container.Register(typeof(TService), implementationFactory.GetType(), implementationFactory,
            LifeTime.Transient);

        return container;
    }

    public static IContainer AttachTransient<TService>(this IContainer container,
        TService implementationType)
        where TService : class
    {
        ArgumentNullException.ThrowIfNull(container);
        ArgumentNullException.ThrowIfNull(implementationType);

        container.Register(typeof(TService), typeof(TService), null, LifeTime.Transient);

        return container;
    }

    #endregion

    public static T Resolve<T>(this IContainer container)
        where T : class
        => (T)container.GetService(typeof(T));

    public static object Resolve(this IContainer container, Type type)
        => container.GetService(type);
}
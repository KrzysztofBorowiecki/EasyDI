namespace EasyDI;

/// <summary>
/// Provides extension methods for registering and resolving services in a dependency injection container,
/// includes methods for registering services with different lifetimes (Singleton, Scoped, Transient)
/// and for resolving services by type.
/// </summary>
public static class ContainerExtensions
{
    #region Singleton

    /// <summary>
    /// Registers a singleton service with a specified implementation instance.
    /// </summary>
    /// <param name="container">The container to which the service will be registered.</param>
    /// <param name="typeToRegister">The type of the service to register.</param>
    /// <param name="implementation">The implementation instance to use for the service.</param>
    /// <returns>The container with the registered service.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="container"/>, <paramref name="typeToRegister"/>, or <paramref name="implementation"/> is null.
    /// </exception>
    public static IContainer AttachSingleton(this IContainer container, Type typeToRegister, object implementation)
    {
        ArgumentNullException.ThrowIfNull(container);
        ArgumentNullException.ThrowIfNull(typeToRegister);
        ArgumentNullException.ThrowIfNull(implementation);

        container.Register(typeToRegister, implementation.GetType(), () => implementation,
            LifeTime.Singleton);
        return container;
    }

    /// <summary>
    /// Registers a singleton service with a specified implementation type.
    /// </summary>
    /// <param name="container">The container to which the service will be registered.</param>
    /// <param name="typeToRegister">The type of the service to register.</param>
    /// <param name="implementationType">The type of the implementation to use for the service.</param>
    /// <returns>The container with the registered service.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="container"/>, <paramref name="typeToRegister"/>, or <paramref name="implementationType"/> is null.
    /// </exception>
    public static IContainer AttachSingleton(this IContainer container, Type typeToRegister, Type implementationType)
    {
        ArgumentNullException.ThrowIfNull(container);
        ArgumentNullException.ThrowIfNull(typeToRegister);
        ArgumentNullException.ThrowIfNull(implementationType);

        container.Register(typeToRegister, implementationType, null, LifeTime.Singleton);
        return container;
    }

    /// <summary>
    /// Registers a singleton service where the service type and implementation type are the same.
    /// </summary>
    /// <param name="container">The container to which the service will be registered.</param>
    /// <param name="implementationType">The type of the service and implementation.</param>
    /// <returns>The container with the registered service.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="container"/> or <paramref name="implementationType"/> is null.
    /// </exception>
    public static IContainer AttachSingleton(this IContainer container, Type implementationType)
    {
        ArgumentNullException.ThrowIfNull(container);
        ArgumentNullException.ThrowIfNull(implementationType);

        container.Register(implementationType, implementationType, null,
            LifeTime.Singleton);

        return container;
    }

    /// <summary>
    /// Registers a singleton service with a factory method for creating the implementation.
    /// </summary>
    /// <param name="container">The container to which the service will be registered.</param>
    /// <param name="typeToRegister">The type of the service to register.</param>
    /// <param name="implementationFactory">A factory method to create the implementation instance.</param>
    /// <returns>The container with the registered service.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="container"/>, <paramref name="typeToRegister"/>, or <paramref name="implementationFactory"/> is null.
    /// </exception>
    public static IContainer AttachSingleton(this IContainer container, Type typeToRegister, Func<object> implementationFactory)
    {
        ArgumentNullException.ThrowIfNull(container);
        ArgumentNullException.ThrowIfNull(typeToRegister);
        ArgumentNullException.ThrowIfNull(implementationFactory);

        container.Register(typeToRegister, implementationFactory.Method.ReturnType, implementationFactory,
            LifeTime.Singleton);

        return container;
    }

    /// <summary>
    /// Registers a singleton service using generic type parameters for the service and implementation types.
    /// </summary>
    /// <typeparam name="TService">The type of the service to register.</typeparam>
    /// <typeparam name="TImplementation">The type of the implementation to use for the service.</typeparam>
    /// <param name="container">The container to which the service will be registered.</param>
    /// <returns>The container with the registered service.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="container"/> is null.
    /// </exception>
    public static IContainer AttachSingleton<TService, TImplementation>(this IContainer container)
        where TService : class
        where TImplementation : class, TService
    {
        ArgumentNullException.ThrowIfNull(container);

        container.Register(typeof(TService), typeof(TImplementation), null,
            LifeTime.Singleton);

        return container;
    }

    /// <summary>
    /// Registers a singleton service with a factory method using generic type parameters for the service and implementation types.
    /// </summary>
    /// <typeparam name="TService">The type of the service to register.</typeparam>
    /// <typeparam name="TImplementation">The type of the implementation to use for the service.</typeparam>
    /// <param name="container">The container to which the service will be registered.</param>
    /// <param name="implementationFactory">A factory method to create the implementation instance.</param>
    /// <returns>The container with the registered service.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="container"/> or <paramref name="implementationFactory"/> is null.
    /// </exception>
    public static IContainer AttachSingleton<TService, TImplementation>(this IContainer container, Func<object> implementationFactory)
        where TService : class
        where TImplementation : class, TService
    {
        ArgumentNullException.ThrowIfNull(container);
        ArgumentNullException.ThrowIfNull(implementationFactory);

        container.Register(typeof(TService), typeof(TImplementation), implementationFactory,
            LifeTime.Singleton);

        return container;
    }

    /// <summary>
    /// Registers a singleton service using the same type for the service and implementation.
    /// </summary>
    /// <typeparam name="TService">The type of the service to register.</typeparam>
    /// <param name="container">The container to which the service will be registered.</param>
    /// <returns>The container with the registered service.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="container"/> is null.
    /// </exception>
    public static IContainer AttachSingleton<TService>(this IContainer container)
        where TService : class
    {
        ArgumentNullException.ThrowIfNull(container);

        container.Register(typeof(TService), typeof(TService), null,
            LifeTime.Singleton);

        return container;
    }

    /// <summary>
    /// Registers a singleton service with a factory method for the same type as the service and implementation.
    /// </summary>
    /// <typeparam name="TService">The type of the service to register.</typeparam>
    /// <param name="container">The container to which the service will be registered.</param>
    /// <param name="implementationFactory">A factory method to create the implementation instance.</param>
    /// <returns>The container with the registered service.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="container"/> or <paramref name="implementationFactory"/> is null.
    /// </exception>
    public static IContainer AttachSingleton<TService>(this IContainer container, Func<object> implementationFactory)
        where TService : class
    {
        ArgumentNullException.ThrowIfNull(container);
        ArgumentNullException.ThrowIfNull(implementationFactory);

        container.Register(typeof(TService), implementationFactory.Method.ReturnType, implementationFactory,
            LifeTime.Singleton);

        return container;
    }

    #endregion
    
    #region Scoped

    /// <summary>
    /// Registers a scoped service with a specified implementation instance.
    /// </summary>
    /// <param name="container">The container to which the service will be registered.</param>
    /// <param name="typeToRegister">The type of the service to register.</param>
    /// <param name="implementation">The implementation instance to use for the service.</param>
    /// <returns>The container with the registered service.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="container"/>, <paramref name="typeToRegister"/>, or <paramref name="implementation"/> is null.
    /// </exception>
    public static IContainer AttachScoped(this IContainer container, Type typeToRegister, object implementation)
    {
        ArgumentNullException.ThrowIfNull(container);
        ArgumentNullException.ThrowIfNull(typeToRegister);
        ArgumentNullException.ThrowIfNull(implementation);

        container.Register(typeToRegister, implementation.GetType(), () => implementation,
            LifeTime.Scoped);

        return container;
    }

    /// <summary>
    /// Registers a scoped service with a specified implementation type.
    /// </summary>
    /// <param name="container">The container to which the service will be registered.</param>
    /// <param name="typeToRegister">The type of the service to register.</param>
    /// <param name="implementationType">The type of the implementation to use for the service.</param>
    /// <returns>The container with the registered service.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="container"/>, <paramref name="typeToRegister"/>, or <paramref name="implementationType"/> is null.
    /// </exception>
    public static IContainer AttachScoped(this IContainer container, Type typeToRegister, Type implementationType)
    {
        ArgumentNullException.ThrowIfNull(container);
        ArgumentNullException.ThrowIfNull(typeToRegister);
        ArgumentNullException.ThrowIfNull(implementationType);

        container.Register(typeToRegister, implementationType, null, LifeTime.Scoped);

        return container;
    }

    /// <summary>
    /// Registers a scoped service where the service type and implementation type are the same.
    /// </summary>
    /// <param name="container">The container to which the service will be registered.</param>
    /// <param name="implementationType">The type of the service and implementation.</param>
    /// <returns>The container with the registered service.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="container"/> or <paramref name="implementationType"/> is null.
    /// </exception>
    public static IContainer AttachScoped(this IContainer container, Type implementationType)
    {
        ArgumentNullException.ThrowIfNull(container);
        ArgumentNullException.ThrowIfNull(implementationType);

        container.Register(implementationType, implementationType, null, LifeTime.Scoped);

        return container;
    }


    /// <summary>
    /// Registers a scoped service with a factory method for creating the implementation.
    /// </summary>
    /// <param name="container">The container to which the service will be registered.</param>
    /// <param name="typeToRegister">The type of the service to register.</param>
    /// <param name="implementationFactory">A factory method to create the implementation instance.</param>
    /// <returns>The container with the registered service.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="container"/>, <paramref name="typeToRegister"/>, or <paramref name="implementationFactory"/> is null.
    /// </exception>
    public static IContainer AttachScoped(this IContainer container, Type typeToRegister, Func<object> implementationFactory)
    {
        ArgumentNullException.ThrowIfNull(container);
        ArgumentNullException.ThrowIfNull(typeToRegister);
        ArgumentNullException.ThrowIfNull(implementationFactory);

        container.Register(typeToRegister, implementationFactory.Method.ReturnType, implementationFactory,
            LifeTime.Scoped);

        return container;
    }

    /// <summary>
    /// Registers a scoped service using generic type parameters for the service and implementation types.
    /// </summary>
    /// <typeparam name="TService">The type of the service to register.</typeparam>
    /// <typeparam name="TImplementation">The type of the implementation to use for the service.</typeparam>
    /// <param name="container">The container to which the service will be registered.</param>
    /// <returns>The container with the registered service.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="container"/> is null.
    /// </exception>
    public static IContainer AttachScoped<TService, TImplementation>(this IContainer container)
        where TService : class
        where TImplementation : class, TService
    {
        ArgumentNullException.ThrowIfNull(container);

        container.Register(typeof(TService), typeof(TImplementation), null, LifeTime.Scoped);

        return container;
    }

    /// <summary>
    /// Registers a scoped service with a factory method using generic type parameters for the service and implementation types.
    /// </summary>
    /// <typeparam name="TService">The type of the service to register.</typeparam>
    /// <typeparam name="TImplementation">The type of the implementation to use for the service.</typeparam>
    /// <param name="container">The container to which the service will be registered.</param>
    /// <param name="implementationFactory">A factory method to create the implementation instance.</param>
    /// <returns>The container with the registered service.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="container"/> or <paramref name="implementationFactory"/> is null.
    /// </exception>
    public static IContainer AttachScoped<TService, TImplementation>(this IContainer container, Func<object> implementationFactory)
        where TService : class
        where TImplementation : class, TService
    {
        ArgumentNullException.ThrowIfNull(container);
        ArgumentNullException.ThrowIfNull(implementationFactory);

        container.Register(typeof(TService), typeof(TImplementation), implementationFactory,
            LifeTime.Scoped);

        return container;
    }

    /// <summary>
    /// Registers a scoped service with a factory method where the service type and implementation type are the same.
    /// </summary>
    /// <typeparam name="TService">The type of the service to register.</typeparam>
    /// <param name="container">The container to which the service will be registered.</param>
    /// <param name="implementationFactory">A factory method to create the implementation instance.</param>
    /// <returns>The container with the registered service.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="container"/> or <paramref name="implementationFactory"/> is null.
    /// </exception>
    public static IContainer AttachScoped<TService>(this IContainer container, Func<object> implementationFactory)
        where TService : class
    {
        ArgumentNullException.ThrowIfNull(container);
        ArgumentNullException.ThrowIfNull(implementationFactory);

        container.Register(typeof(TService), implementationFactory.Method.ReturnType, implementationFactory,
            LifeTime.Scoped);

        return container;
    }

    /// <summary>
    /// Registers a scoped service where the service type and implementation type are the same.
    /// </summary>
    /// <typeparam name="TService">The type of the service to register.</typeparam>
    /// <param name="container">The container to which the service will be registered.</param>
    /// <returns>The container with the registered service.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="container"/> is null.
    /// </exception>
    public static IContainer AttachScoped<TService>(this IContainer container)
        where TService : class
    {
        ArgumentNullException.ThrowIfNull(container);

        container.Register(typeof(TService), typeof(TService), null, LifeTime.Scoped);

        return container;
    }

    #endregion

    #region Transient

    /// <summary>
    /// Registers a transient service with a specified implementation type.
    /// </summary>
    /// <param name="container">The container to which the service will be registered.</param>
    /// <param name="typeToRegister">The type of the service to register.</param>
    /// <param name="implementationType">The type of the implementation to use for the service.</param>
    /// <returns>The container with the registered service.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="container"/>, <paramref name="typeToRegister"/>, or <paramref name="implementationType"/> is null.
    /// </exception>
    public static IContainer AttachTransient(this IContainer container, Type typeToRegister, Type implementationType)
    {
        ArgumentNullException.ThrowIfNull(container);
        ArgumentNullException.ThrowIfNull(typeToRegister);
        ArgumentNullException.ThrowIfNull(implementationType);

        container.Register(typeToRegister, implementationType, null, LifeTime.Transient);

        return container;
    }

    /// <summary>
    /// Registers a transient service where the service type and implementation type are the same.
    /// </summary>
    /// <param name="container">The container to which the service will be registered.</param>
    /// <param name="implementationType">The type of the service and implementation.</param>
    /// <returns>The container with the registered service.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="container"/> or <paramref name="implementationType"/> is null.
    /// </exception>
    public static IContainer AttachTransient(this IContainer container, Type implementationType)
    {
        ArgumentNullException.ThrowIfNull(container);
        ArgumentNullException.ThrowIfNull(implementationType);

        container.Register(implementationType, implementationType, null, LifeTime.Transient);

        return container;
    }

    /// <summary>
    /// Registers a transient service with a factory method for creating the implementation.
    /// </summary>
    /// <param name="container">The container to which the service will be registered.</param>
    /// <param name="typeToRegister">The type of the service to register.</param>
    /// <param name="implementationFactory">A factory method to create the implementation instance.</param>
    /// <returns>The container with the registered service.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="container"/>, <paramref name="typeToRegister"/>, or <paramref name="implementationFactory"/> is null.
    /// </exception>
    public static IContainer AttachTransient(this IContainer container, Type typeToRegister, Func<object> implementationFactory)
    {
        ArgumentNullException.ThrowIfNull(container);
        ArgumentNullException.ThrowIfNull(typeToRegister);
        ArgumentNullException.ThrowIfNull(implementationFactory);

        container.Register(typeToRegister, implementationFactory.Method.ReturnType, implementationFactory,
            LifeTime.Transient);

        return container;
    }

    /// <summary>
    /// Registers a transient service using generic type parameters for the service and implementation types.
    /// </summary>
    /// <typeparam name="TService">The type of the service to register.</typeparam>
    /// <typeparam name="TImplementation">The type of the implementation to use for the service.</typeparam>
    /// <param name="container">The container to which the service will be registered.</param>
    /// <returns>The container with the registered service.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="container"/> is null.
    /// </exception>
    public static IContainer AttachTransient<TService, TImplementation>(this IContainer container)
        where TService : class
        where TImplementation : class, TService
    {
        ArgumentNullException.ThrowIfNull(container);

        container.Register(typeof(TService), typeof(TImplementation), null, LifeTime.Transient);

        return container;
    }

    /// <summary>
    /// Registers a transient service with a factory method using generic type parameters for the service and implementation types.
    /// </summary>
    /// <typeparam name="TService">The type of the service to register.</typeparam>
    /// <typeparam name="TImplementation">The type of the implementation to use for the service.</typeparam>
    /// <param name="container">The container to which the service will be registered.</param>
    /// <param name="implementationFactory">A factory method to create the implementation instance.</param>
    /// <returns>The container with the registered service.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="container"/> or <paramref name="implementationFactory"/> is null.
    /// </exception>
    public static IContainer AttachTransient<TService, TImplementation>(this IContainer container, Func<object> implementationFactory)
        where TService : class
        where TImplementation : class, TService
    {
        ArgumentNullException.ThrowIfNull(container);
        ArgumentNullException.ThrowIfNull(implementationFactory);

        container.Register(typeof(TService), typeof(TImplementation), implementationFactory, LifeTime.Transient);

        return container;
    }

    /// <summary>
    /// Registers a transient service where the service type and implementation type are the same.
    /// </summary>
    /// <typeparam name="TService">The type of the service to register.</typeparam>
    /// <param name="container">The container to which the service will be registered.</param>
    /// <returns>The container with the registered service.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="container"/> is null.
    /// </exception>
    public static IContainer AttachTransient<TService>(this IContainer container)
        where TService : class
    {
        ArgumentNullException.ThrowIfNull(container);

        container.Register(typeof(TService), typeof(TService), null, LifeTime.Transient);

        return container;
    }

    /// <summary>
    /// Registers a transient service with a factory method where the service type and implementation type are the same.
    /// </summary>
    /// <typeparam name="TService">The type of the service to register.</typeparam>
    /// <param name="container">The container to which the service will be registered.</param>
    /// <param name="implementationFactory">A factory method to create the implementation instance.</param>
    /// <returns>The container with the registered service.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="container"/> or <paramref name="implementationFactory"/> is null.
    /// </exception>
    public static IContainer AttachTransient<TService>(this IContainer container, Func<object> implementationFactory)
        where TService : class
    {
        ArgumentNullException.ThrowIfNull(container);
        ArgumentNullException.ThrowIfNull(implementationFactory);

        container.Register(typeof(TService), implementationFactory.Method.ReturnType, implementationFactory,
            LifeTime.Transient);

        return container;
    }

    #endregion

    /// <summary>
    /// Resolves a service of the specified type from the container.
    /// </summary>
    /// <typeparam name="T">The type of the service to resolve.</typeparam>
    /// <param name="container">The container from which the service will be resolved.</param>
    /// <returns>An instance of the requested service.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="container"/> is null.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the requested service is not registered in the container.
    /// </exception>
    public static T Resolve<T>(this IContainer container)
        where T : class
        => (T)container.GetService(typeof(T))!;
}
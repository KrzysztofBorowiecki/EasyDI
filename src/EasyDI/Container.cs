namespace EasyDI;

/// <summary>
/// Represents a registered dependency with its service type, implementation type, factory function, and lifetime.
/// </summary>
/// <param name="ServiceType">The type of the service.</param>
/// <param name="ImplementationType">The type that implements the service.</param>
/// <param name="Factory">A factory function to create instances of the service.</param>
/// <param name="LifeTime">The lifetime of the dependency.</param>
public record Dependency(Type ServiceType, Type ImplementationType, Func<object> Factory, LifeTime LifeTime);

/// <summary>
/// Defines the lifetime of a dependency in the container.
/// </summary>
public enum LifeTime
{
    /// <summary>
    /// A new instance is created every time the service is requested.
    /// </summary>
    Transient,
    
    /// <summary>
    /// A single instance is shared across all requests for the lifetime of the container.
    /// </summary>
    Singleton,
    
    /// <summary>
    /// A single instance is shared within a specific scope.
    /// </summary>
    Scoped,
}

/// <summary>
/// Defines a container for managing dependency injection and service lifetimes.
/// </summary>
public interface IContainer : IDisposable, IServiceProvider
{
    /// <summary>
    /// Registers a service type and its implementation with the specified factory and lifetime.
    /// </summary>
    /// <param name="serviceType">The type of the service.</param>
    /// <param name="implementationType">The type that implements the service.</param>
    /// <param name="factory">A factory function to create instances of the service.</param>
    /// <param name="lifeTime">The lifetime of the service.</param>
    void Register(Type serviceType, Type implementationType, Func<object> factory, LifeTime lifeTime);
    
    /// <summary>
    /// Creates a new scope for managing scoped dependencies.
    /// </summary>
    /// <returns>A new scoped container.</returns>
    IContainer CreateScope();
}

/// <summary>
/// A concrete implementation of the dependency injection container.
/// </summary>
public class Container : IContainer
{
    private readonly Dictionary<Type, Dependency> _registeredDependencies;

    /// <summary>
    /// Initializes a new instance of the <see cref="Container"/> class.
    /// </summary>
    public Container()
    {
        _registeredDependencies = new();
    }
    
    private Container(Dictionary<Type, Dependency> parentRegisteredDependencies)
    {
        _registeredDependencies = parentRegisteredDependencies;
        FireUpScopedFactory();
    }
    
    /// <summary>
    /// Registers a service with the specified implementation type, factory, and lifetime.
    /// </summary>
    /// <param name="serviceType">The type of the service.</param>
    /// <param name="implementationType">The type that implements the service.</param>
    /// <param name="factory">An optional factory function to create service instances.</param>
    /// <param name="lifeTime">The lifetime of the service.</param>
    public void Register(Type serviceType, Type implementationType, Func<object>? factory, LifeTime lifeTime)
    {
        Func<object> instanceProvider = factory ?? TypeFactory.CreateFactory(implementationType, this);

        switch (lifeTime)
        {
            case LifeTime.Singleton:
            case LifeTime.Scoped:
                var instance = instanceProvider();
                _registeredDependencies[serviceType] =
                    new Dependency(serviceType, implementationType, () => instance, lifeTime);
                break;

            case LifeTime.Transient:
                _registeredDependencies[serviceType] =
                    new Dependency(serviceType, implementationType, instanceProvider, LifeTime.Transient);
                break;

            default:
                throw new ArgumentOutOfRangeException($"Invalid dependency lifetime: {lifeTime}");
        }
    }

    /// <summary>
    /// Creates a new scoped container based on the current container's dependencies.
    /// </summary>
    /// <returns>A new <see cref="Container"/> instance for the scope.</returns>
    public IContainer CreateScope() => new Container(_registeredDependencies);
    
    private void FireUpScopedFactory()
    {
        _registeredDependencies.Values
            .Where(d => d.LifeTime == LifeTime.Scoped)
            .ToList()
            .ForEach(dependency => Register(dependency.ServiceType, dependency.ImplementationType, null, dependency.LifeTime));
    }

    /// <summary>
    /// Retrieves a service of the specified type.
    /// </summary>
    /// <param name="serviceType">The type of the service to retrieve.</param>
    /// <returns>The service instance, or throws an exception if not registered.</returns>
    public object? GetService(Type serviceType)
    {
        if (_registeredDependencies.TryGetValue(serviceType, out var dependency))
        {
            var instance = dependency.Factory();
            if (instance is not null)
            {
                return instance;
            }
        }

        throw new InvalidOperationException($"No service for type {serviceType} has been registered");
    }

    /// <summary>
    /// Disposes the container and releases all managed resources.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    
    private void Dispose(bool disposing)
    {
        if (!disposing) return;

        foreach (var dependency in _registeredDependencies.Values)
        {
            if (dependency.Factory() is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }

        _registeredDependencies.Clear();
    }
}

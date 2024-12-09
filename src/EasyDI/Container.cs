namespace EasyDI;

public record Dependency(Type ServiceType, Type ImplementationType, Func<object> Factory, LifeTime LifeTime);

public enum LifeTime
{
    Transient,
    Singleton,
    Scoped,
}

public interface IContainer : IDisposable, IServiceProvider
{
    void Register(Type serviceType, Type implementationType, Func<object> factory, LifeTime lifeTime);
    IContainer CreateScope();
}

public class Container : IContainer
{
    private readonly Dictionary<Type, Dependency> _registeredDependencies;

    public Container()
    {
        _registeredDependencies = new();
    }

    private Container(Dictionary<Type, Dependency> parentRegisteredDependencies)
    {
        _registeredDependencies = parentRegisteredDependencies;
        FireUpScopedFactory();
    }
    
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

    public IContainer CreateScope() => new Container(_registeredDependencies);
    
    private void FireUpScopedFactory()
    {
        _registeredDependencies.Values
            .Where(d => d.LifeTime == LifeTime.Scoped)
            .ToList()
            .ForEach(dependency => Register(dependency.ServiceType, dependency.ImplementationType, null, dependency.LifeTime));
    }

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
namespace EasyDI;

public record Dependency(Type Key, Type ImplementationType, Func<object> Factory, LifeTime LifeTime);

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
        switch (lifeTime)
        {
            case LifeTime.Singleton:

                if (factory is not null)
                {
                    var instanceFromFactory = factory();
                    _registeredDependencies[serviceType] =
                        new Dependency(serviceType, implementationType, () => instanceFromFactory, LifeTime.Singleton);
                }
                else
                {
                    var createdInstance = TypeFactory.CreateFactory(implementationType, this).Invoke();
                    _registeredDependencies[serviceType] =
                        new Dependency(serviceType, implementationType, () => createdInstance, LifeTime.Singleton);
                }

                break;
            case LifeTime.Scoped:
                if (factory is not null)
                {
                    var instanceFromFactory1 = factory();
                    _registeredDependencies[serviceType] =
                        new Dependency(serviceType, implementationType, () => instanceFromFactory1, LifeTime.Scoped);
                }
                else
                {
                    var createdInstance = TypeFactory.CreateFactory(implementationType, this).Invoke();
                    _registeredDependencies[serviceType] =
                        new Dependency(serviceType, implementationType, () => createdInstance, LifeTime.Scoped);
                }

                break;
            case LifeTime.Transient:
                var instanceFactory = factory ?? TypeFactory.CreateFactory(implementationType, this);
                _registeredDependencies[serviceType] = new Dependency(serviceType, implementationType, instanceFactory,
                    LifeTime.Transient);

                break;
            default:
                throw new ArgumentOutOfRangeException(
                    $"Invalid dependency lifetime: {lifeTime}");
        }
    }

    public IContainer CreateScope() => new Container(_registeredDependencies);
    
    private void FireUpScopedFactory()
    {
        _registeredDependencies.Values
            .Where(d => d.LifeTime == LifeTime.Scoped)
            .ToList()
            .ForEach(dependency => Register(dependency.Key, dependency.ImplementationType, null, dependency.LifeTime));
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
namespace EasyDI;

public interface IContainer : IDisposable, IServiceProvider
{
    void Register(Type serviceType, Type implementationType, Func<object> factory, LifeTime lifeTime);
}

public class Container : IContainer
{
    private readonly Dictionary<Type, Func<object>> _registeredDependencies = new();

    public void Register(Type serviceType, Type implementationType, Func<object>? factory, LifeTime lifeTime)
    {
        if (factory is not null)
        {
            var instanceFromFactory = factory();
            _registeredDependencies[serviceType] = () => instanceFromFactory;
        }

        var createdInstance = TypeFactory.CreateFactory(implementationType).Invoke();
        _registeredDependencies[serviceType] = () => createdInstance;
    }

    public object GetService(Type serviceType)
    {
        if (_registeredDependencies.TryGetValue(serviceType, out var factory))
        {
            return factory();
        }

        throw new InvalidOperationException($"Type {serviceType} is not registered.");
    }

    public void Dispose()
    {
        foreach (var dependency in _registeredDependencies.Values)
        {
            if (dependency() is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }

        _registeredDependencies.Clear();
    }
}
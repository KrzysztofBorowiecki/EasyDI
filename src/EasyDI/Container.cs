namespace EasyDI;

public interface IContainer : IDisposable, IServiceProvider
{
    void Register(Type serviceType, Type implementationType, Func<object> factory, LifeTime lifeTime);
    IContainer CreateScope();
}

public class Container : IContainer
{
    private readonly Dictionary<Type, Func<object>> _registeredDependencies;

    public Container()
    {
        _registeredDependencies = new();
    }

    private Container(Dictionary<Type, Func<object>> parentRegisteredDependencies)
    {
        _registeredDependencies = parentRegisteredDependencies;
    }

    public void Register(Type serviceType, Type implementationType, Func<object>? factory, LifeTime lifeTime)
    {
        switch (lifeTime)
        {
            case LifeTime.Singleton:
            case LifeTime.Scoped:
                if (factory is not null)
                {
                    var instanceFromFactory = factory();
                    _registeredDependencies[serviceType] = () => instanceFromFactory;
                }
                else
                {
                    var createdInstance = TypeFactory.CreateFactory(implementationType, this).Invoke();
                    _registeredDependencies[serviceType] = () => createdInstance;
                }

                break;
            case LifeTime.Transient:
                _registeredDependencies[serviceType] = factory ?? TypeFactory.CreateFactory(implementationType, this);
                
                break;
            default:
                throw new ArgumentOutOfRangeException(
                    $"Invalid dependency lifetime: {lifeTime}");
        }
    }

    public IContainer CreateScope() => new Container(_registeredDependencies);


    public object? GetService(Type serviceType)
    {
        if (_registeredDependencies.TryGetValue(serviceType, out var factory))
        {
            var instance = factory();
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
            if (dependency() is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }

        _registeredDependencies.Clear();
    }
}
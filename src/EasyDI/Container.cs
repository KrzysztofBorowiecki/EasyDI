namespace EasyDI;

public interface IContainer : IDisposable
{
    void Register(Type registerType, Type implementationType, Func<object> factory, LifeTime lifeTime);
}

public class Container : IContainer
{
    private readonly Dictionary<Type, Func<object>> _registeredDependencies = new();

    public void Register(Type registerType, Type implementationType, Func<object> factory, LifeTime lifeTime)
    {
        //TODO: write an implementation
        //   throw new NotImplementedException();
    }

    public void Dispose()
    {
        // TODO: release managed resources here
    }
}
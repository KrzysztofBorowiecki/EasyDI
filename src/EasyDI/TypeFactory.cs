namespace EasyDI;

public static class TypeFactory
{
    public static Func<object> CreateFactory(Type implementationType)
    {
        ArgumentNullException.ThrowIfNull(implementationType);

        //TODO: write an implementation
        throw new NotImplementedException();
    }
}
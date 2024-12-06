namespace EasyDI;

public interface IContainer : IDisposable
{
    
}

public class Container : IContainer
{
    public void Dispose()
    {
        // TODO: release managed resources here
    }
}
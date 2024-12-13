namespace EasyDI.Tests;

public abstract class ContainerFixture : IDisposable
{
    private bool _disposed;

    protected IContainer Container { get; }

    protected ContainerFixture()
    {
        Container = new Container();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (_disposed) return;

        if (disposing)
        {
            Container.Dispose();
        }

        _disposed = true;
    }
}
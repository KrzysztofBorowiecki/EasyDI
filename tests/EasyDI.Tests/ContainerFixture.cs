namespace EasyDI.Tests;

public abstract class ContainerFixture : IDisposable
{
    protected IContainer Container { get; } = new Container();

    public void Dispose()
    {
        Container.Dispose();
    }
}
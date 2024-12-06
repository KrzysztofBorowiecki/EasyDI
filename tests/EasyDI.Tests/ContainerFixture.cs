using System.ComponentModel;

namespace EasyDI.Tests;

public abstract class ContainerFixture : IDisposable
{
    protected IContainer Container { get; private set; }
    
    public ContainerFixture()
    {
        Container = new Container();
    }

    public void Dispose()
    {
        Container.Dispose();
    }
}
namespace EasyDI.Tests;

public class SingletonTests : ContainerFixture
{
    [Fact]
    public void Register_By_Interface_And_Implementation_Provided_As_Parameters()
    {
        //Arrange
        var foo = new Foo();
        var bar = new Bar(foo);
        var baz = new Baz(foo, bar);

        Container
            .AttachSingleton(typeof(IFoo), foo)
            .AttachSingleton(typeof(IBar), bar)
            .AttachSingleton(typeof(IBaz), baz);

        //Act
        var foo1 = Container.Resolve<IFoo>();
        var foo2 = Container.Resolve<IFoo>();
        var bar1 = Container.Resolve<IBar>();
        var bar2 = Container.Resolve<IBar>();
        var baz1 = Container.Resolve<IBaz>();
        var baz2 = Container.Resolve<IBaz>();

        //Assert
        Assert.IsType<IFoo>(foo1);
        Assert.Same(foo, foo1);
        Assert.Same(foo1, foo2);

        Assert.IsType<IBar>(bar1);
        Assert.Same(bar, bar1);
        Assert.Same(bar1, bar2);
        Assert.IsType<IFoo>(bar1.Foo);
        Assert.Same(bar1.Foo, bar2.Foo);

        Assert.IsType<IBaz>(baz1);
        Assert.Same(baz, baz1);
        Assert.Same(baz1, baz2);
        Assert.IsType<IFoo>(baz1.Foo);
        Assert.Same(baz1.Foo, baz2.Foo);
        Assert.IsType<IBar>(baz1.Bar);
        Assert.Same(baz1.Bar, baz2.Bar);
    }
}
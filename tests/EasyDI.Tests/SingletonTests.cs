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
        Assert.IsAssignableFrom<Foo>(foo1);
        Assert.Same(foo, foo1);
        Assert.Same(foo1, foo2);

        Assert.IsAssignableFrom<Bar>(bar1);
        Assert.Same(bar, bar1);
        Assert.Same(bar1, bar2);
        Assert.IsAssignableFrom<Foo>(bar1.Foo);
        Assert.Same(bar1.Foo, bar2.Foo);

        Assert.IsAssignableFrom<Baz>(baz1);
        Assert.Same(baz, baz1);
        Assert.Same(baz1, baz2);
        Assert.IsAssignableFrom<Foo>(baz1.Foo);
        Assert.Same(baz1.Foo, baz2.Foo);
        Assert.IsAssignableFrom<Bar>(baz1.Bar);
        Assert.Same(baz1.Bar, baz2.Bar);
    }
    
    [Fact]
    public void Register_By_Self_Type_And_Implementation_Provided_As_Parameters()
    {
        //Arrange
        var foo = new Foo();
        var bar = new Bar(foo);
        var baz = new Baz(foo, bar);

        Container
            .AttachSingleton(typeof(Foo), foo)
            .AttachSingleton(typeof(Bar), bar)
            .AttachSingleton(typeof(Baz), baz);

        //Act
        var foo1 = Container.Resolve<Foo>();
        var foo2 = Container.Resolve<Foo>();
        var bar1 = Container.Resolve<Bar>();
        var bar2 = Container.Resolve<Bar>();
        var baz1 = Container.Resolve<Baz>();
        var baz2 = Container.Resolve<Baz>();

        //Assert
        Assert.IsType<Foo>(foo1);
        Assert.Same(foo, foo1);
        Assert.Same(foo1, foo2);

        Assert.IsType<Bar>(bar1);
        Assert.Same(bar, bar1);
        Assert.Same(bar1, bar2);
        Assert.IsType<Foo>(bar1.Foo);
        Assert.Same(bar1.Foo, bar2.Foo);

        Assert.IsType<Baz>(baz1);
        Assert.Same(baz, baz1);
        Assert.Same(baz1, baz2);
        Assert.IsType<Foo>(baz1.Foo);
        Assert.Same(baz1.Foo, baz2.Foo);
        Assert.IsType<Bar>(baz1.Bar);
        Assert.Same(baz1.Bar, baz2.Bar);
    }
    
    [Fact]
    public void Register_By_Interface_And_Type_Of_Implementation_Provided_As_Parameters()
    {
        //Arrange
        Container
            .AttachSingleton(typeof(IFoo), typeof(Foo))
            .AttachSingleton(typeof(IBar), typeof(Bar))
            .AttachSingleton(typeof(IBaz), typeof(Baz));

        //Act
        var foo1 = Container.Resolve<IFoo>();
        var foo2 = Container.Resolve<IFoo>();
        var bar1 = Container.Resolve<IBar>();
        var bar2 = Container.Resolve<IBar>();
        var baz1 = Container.Resolve<IBaz>();
        var baz2 = Container.Resolve<IBaz>();

        //Assert
        Assert.IsAssignableFrom<Foo>(foo1);
        Assert.Same(foo1, foo2);

        Assert.IsAssignableFrom<Bar>(bar1);
        Assert.Same(bar1, bar2);
        Assert.IsAssignableFrom<Foo>(bar1.Foo);
        Assert.Same(bar1.Foo, bar2.Foo);

        Assert.IsAssignableFrom<Baz>(baz1);
        Assert.Same(baz1, baz2);
        Assert.IsAssignableFrom<Foo>(baz1.Foo);
        Assert.Same(baz1.Foo, baz2.Foo);
        Assert.IsAssignableFrom<Bar>(baz1.Bar);
        Assert.Same(baz1.Bar, baz2.Bar);
    }
    
    [Fact]
    public void Register_By_Self_Type_And_Type_Of_Implementation_Provided_As_Parameters()
    {
        //Arrange
        Container
            .AttachSingleton(typeof(Foo), typeof(Foo))
            .AttachSingleton(typeof(IFoo), typeof(Foo))
            .AttachSingleton(typeof(Bar), typeof(Bar))
            .AttachSingleton(typeof(IBar), typeof(Bar))
            .AttachSingleton(typeof(Baz), typeof(Baz));

        //Act
        var foo1 = Container.Resolve<Foo>();
        var foo2 = Container.Resolve<Foo>();
        var bar1 = Container.Resolve<Bar>();
        var bar2 = Container.Resolve<Bar>();
        var baz1 = Container.Resolve<Baz>();
        var baz2 = Container.Resolve<Baz>();

        //Assert
        Assert.IsType<Foo>(foo1);
        Assert.Same(foo1, foo2);

        Assert.IsType<Bar>(bar1);
        Assert.Same(bar1, bar2);
        Assert.IsAssignableFrom<Foo>(bar1.Foo);
        Assert.Same(bar1.Foo, bar2.Foo);

        Assert.IsType<Baz>(baz1);
        Assert.Same(baz1, baz2);
        Assert.IsAssignableFrom<Foo>(baz1.Foo);
        Assert.Same(baz1.Foo, baz2.Foo);
        Assert.IsAssignableFrom<Bar>(baz1.Bar);
        Assert.Same(baz1.Bar, baz2.Bar);
    }
}
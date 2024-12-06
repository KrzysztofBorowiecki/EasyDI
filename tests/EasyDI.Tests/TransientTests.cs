namespace EasyDI.Tests;

public class TransientTests : ContainerFixture
{
    [Fact]
    public void By_Interface_Provides_As_Parameter()
    {
        //Arrange
        var expectedMessage = "Cannot instantiate implementation type 'EasyDI.Tests.IFoo' because it is an interface or abstract class.";
        
        //Act and Assert
        var ex = Assert.Throws<ArgumentException>(() => Container.AttachTransient(typeof(IFoo)));
        
        Assert.Equal(ex.Message, expectedMessage);
    }
    
    [Fact]
    public void By_Self_Provides_As_Parameter()
    {
        //Arrange
        Container
            .AttachTransient(typeof(Foo))
            .AttachTransient(typeof(IFoo), typeof(Foo))
            .AttachTransient(typeof(Bar))
            .AttachTransient(typeof(IBar), typeof(Bar))
            .AttachTransient(typeof(Baz));

        //Act
        var foo1 = Container.Resolve<Foo>();
        var foo2 = Container.Resolve<Foo>();
        var bar1 = Container.Resolve<Bar>();
        var bar2 = Container.Resolve<Bar>();
        var baz1 = Container.Resolve<Baz>();
        var baz2 = Container.Resolve<Baz>();

        //Assert
        Assert.IsType<Foo>(foo1);
        Assert.NotSame(foo1, foo2);
        Assert.IsType<Foo>(foo1);
        Assert.NotSame(bar1, bar2);
        //Assert.NotSame(bar1.Foo, bar2.Foo);
        Assert.IsType<Foo>(bar1.Foo);
        Assert.IsType<Baz>(baz1);
        //Assert.NotSame(baz1.Foo, baz1.Foo);
        Assert.IsType<Foo>(bar1.Foo);
        //Assert.NotSame(baz1.Bar, baz1.Bar);
        Assert.IsType<Bar>(baz1.Bar);
        Assert.NotSame(baz1, baz2);
    }
    
    [Fact]
    public void By_Interface_And_Type_Of_Implementation_Provided_As_Parameters()
    {
        //Arrange
        Container
            .AttachTransient(typeof(IFoo), typeof(Foo))
            .AttachTransient(typeof(IBar), typeof(Bar))
            .AttachTransient(typeof(IBaz), typeof(Baz));

        //Act
        var foo1 = Container.Resolve<IFoo>();
        var foo2 = Container.Resolve<IFoo>();
        var bar1 = Container.Resolve<IBar>();
        var bar2 = Container.Resolve<IBar>();
        var baz1 = Container.Resolve<IBaz>();
        var baz2 = Container.Resolve<IBaz>();
        
        //Assert
        Assert.IsAssignableFrom<Foo>(foo1);
        Assert.NotSame(foo1, foo2);
        Assert.IsAssignableFrom<Foo>(foo1);
        Assert.NotSame(bar1, bar2);
        //Assert.NotSame(bar1.Foo, bar2.Foo);
        Assert.IsAssignableFrom<Foo>(bar1.Foo);
        Assert.IsAssignableFrom<Baz>(baz1);
        //Assert.NotSame(baz1.Foo, baz1.Foo);
        Assert.IsAssignableFrom<Foo>(bar1.Foo);
//        Assert.NotSame(baz1.Bar, baz1.Bar);
        Assert.IsAssignableFrom<Bar>(baz1.Bar);
        Assert.NotSame(baz1, baz2);
    }
    
    [Fact]
    public void By_Self_Type_And_Type_Of_Implementation_Provided_As_Parameters()
    {
        //Arrange
        Container
            .AttachTransient(typeof(Foo), typeof(Foo))
            .AttachTransient(typeof(IFoo), typeof(Foo))
            .AttachTransient(typeof(Bar), typeof(Bar))
            .AttachTransient(typeof(IBar), typeof(Bar))
            .AttachTransient(typeof(Baz), typeof(Baz));

        //Act
        var foo1 = Container.Resolve<Foo>();
        var foo2 = Container.Resolve<Foo>();
        var bar1 = Container.Resolve<Bar>();
        var bar2 = Container.Resolve<Bar>();
        var baz1 = Container.Resolve<Baz>();
        var baz2 = Container.Resolve<Baz>();
        
        var bar1foo = bar1.Foo;
        var bar2foo = bar2.Foo;
        
        //Assert
        Assert.IsType<Foo>(foo1);
        Assert.NotSame(foo1, foo2);
        Assert.IsType<Foo>(foo1);
        Assert.NotSame(bar1, bar2);
        //Assert.NotSame(bar1.Foo, bar2.Foo);
        Assert.IsAssignableFrom<Foo>(bar1.Foo);
        Assert.IsType<Baz>(baz1);
        //Assert.NotSame(baz1.Foo, baz1.Foo);
        Assert.IsAssignableFrom<Foo>(bar1.Foo);
        //Assert.NotSame(baz1.Bar, baz1.Bar);
        Assert.IsAssignableFrom<Bar>(baz1.Bar);
        Assert.NotSame(baz1, baz2);
    }
}
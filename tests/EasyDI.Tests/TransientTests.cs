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
        //Assert.NotSame(baz1.Bar, baz1.Bar);
        Assert.IsAssignableFrom<Bar>(baz1.Bar);
        //Assert.NotSame(baz1, baz2);
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

    [Fact]
    public void
        By_Interface_And_Factory_With_Implementation_Provided_As_Parameters() //AttachTransient(IServiceCollection, Type, Func<IServiceProvider,Object>)
    {
        //Arrange
        Func<Foo> fooFactory = () => new Foo();
        Func<Bar> barFactory = () => new Bar(new Foo());
        Func<Baz> bazFactory = () => new Baz(new Foo(), new Bar(new Foo()));

        Container
            .AttachTransient(typeof(IFoo), fooFactory)
            .AttachTransient(typeof(IBar), barFactory)
            .AttachTransient(typeof(IBaz), bazFactory);

        //Act
        var foo1 = Container.Resolve<IFoo>();
        var foo2 = Container.Resolve<IFoo>();
        var bar1 = Container.Resolve<IBar>();
        var bar2 = Container.Resolve<IBar>();
        var baz1 = Container.Resolve<IBaz>();
        var baz2 = Container.Resolve<IBaz>();

        //Assert
        Assert.IsAssignableFrom<IFoo>(foo1);
        Assert.NotSame(foo1, foo2);
        Assert.IsAssignableFrom<IBar>(bar1);
        Assert.NotSame(bar1, bar2);
        Assert.NotSame(bar1.Foo, bar2.Foo);
        Assert.IsAssignableFrom<IFoo>(bar1.Foo);
        Assert.IsType<Baz>(baz1);
        Assert.NotSame(baz1.Foo, baz2.Foo);
        Assert.IsAssignableFrom<IFoo>(bar1.Foo);
        //Assert.NotSame(baz1.Bar, baz1.Bar);
        Assert.IsAssignableFrom<Bar>(baz1.Bar);
        Assert.NotSame(baz1, baz2);
    }

    [Fact]
    public void By_Interface_And_Factory_With_Null_Implementation_Provided_As_Parameters()
    {
        //Arrange
        var expectedMessage = "No service for type EasyDI.Tests.IFoo has been registered";
        Func<Foo> fooFactory = () => null;
        Func<Bar> barFactory = () => null;
        Func<Baz> bazFactory = () => null;

        //Act
        Container
            .AttachTransient(typeof(IFoo), fooFactory)
            .AttachTransient(typeof(IBar), barFactory)
            .AttachTransient(typeof(IBaz), bazFactory);

        //Assert
        var ex = Assert.Throws<InvalidOperationException>(() => Container.Resolve<IFoo>());

        Assert.Equal(ex.Message, expectedMessage);
    }

    [Fact]
    public void
        By_Self_Type_And_Factory_With_Implementation_Provided_As_Parameters() //AttachTransient(IServiceCollection, Type, Func<IServiceProvider,Object>)
    {
        //Arrange
        Func<Foo> fooFactory = () => new Foo();
        Func<Bar> barFactory = () => new Bar(new Foo());
        Func<Baz> bazFactory = () => new Baz(new Foo(), new Bar(new Foo()));

        Container
            .AttachTransient(typeof(Foo), fooFactory)
            .AttachTransient(typeof(Bar), barFactory)
            .AttachTransient(typeof(Baz), bazFactory);

        //Act
        var foo1 = Container.Resolve<Foo>();
        var foo2 = Container.Resolve<Foo>();
        var bar1 = Container.Resolve<Bar>();
        var bar2 = Container.Resolve<Bar>();
        var baz1 = Container.Resolve<Baz>();
        var baz2 = Container.Resolve<Baz>();

        //Assert
        Assert.IsType<Foo>(foo1);
        Assert.IsType<Foo>(foo1);
        Assert.NotSame(foo1, foo2);
        Assert.IsType<Bar>(bar1);
        Assert.NotSame(bar1, bar2);
        Assert.NotSame(bar1.Foo, bar2.Foo);
        Assert.IsType<Foo>(bar1.Foo);
        Assert.IsType<Baz>(baz1);
        Assert.NotSame(baz1.Foo, baz2.Foo);
        Assert.IsType<Foo>(baz1.Foo);
        //Assert.NotSame(baz1.Bar, baz1.Bar);
        Assert.IsType<Bar>(baz1.Bar);
        Assert.NotSame(baz1, baz2);
    }

    [Fact]
    public void By_Self_And_Factory_With_Null_Implementation_Provided_As_Parameters()
    {
        //Arrange
        var expectedMessage = "No service for type EasyDI.Tests.IFoo has been registered";
        Func<Foo> fooFactory = () => null;
        Func<Bar> barFactory = () => null;
        Func<Baz> bazFactory = () => null;

        //Act
        Container
            .AttachTransient(typeof(IFoo), fooFactory)
            .AttachTransient(typeof(IBar), barFactory)
            .AttachTransient(typeof(IBaz), bazFactory);

        //Assert
        var ex = Assert.Throws<InvalidOperationException>(() => Container.Resolve<IFoo>());

        Assert.Equal(ex.Message, expectedMessage);
    }

    [Fact]
    public void
        By_Interface_And_Implementation_Type_Provided_As_Generics() //AttachTransient<TService,TImplementation>(IServiceCollection)
    {
        //Arrange
        Container
            .AttachTransient<IFoo, Foo>()
            .AttachTransient<IBar, Bar>()
            .AttachTransient<IBaz, Baz>();

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
        //Assert.NotSame(baz1.Bar, baz1.Bar);
        Assert.IsAssignableFrom<Bar>(baz1.Bar);
        Assert.NotSame(baz1, baz2);
    }

    [Fact]
    public void
        By_Self_Type_And_Implementation_Type_Provided_As_Generics() //AttachTransient<TService,TImplementation>(IServiceCollection)
    {
        //Arrange
        Container
            .AttachTransient<Foo, Foo>()
            .AttachTransient<IFoo, Foo>()
            .AttachTransient<Bar, Bar>()
            .AttachTransient<IBar, Bar>()
            .AttachTransient<Baz, Baz>();

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
        Assert.IsType<Bar>(bar1);
        Assert.NotSame(bar1, bar2);
        //Assert.NotSame(bar1.Foo, bar2.Foo);
        Assert.IsType<Foo>(bar1.Foo);
        Assert.IsType<Baz>(baz1);
        //Assert.NotSame(baz1.Foo, baz2.Foo);
        Assert.IsType<Foo>(baz1.Foo);
        //Assert.NotSame(baz1.Bar, baz1.Bar);
        Assert.IsType<Bar>(baz1.Bar);
        Assert.NotSame(baz1, baz2);
    }

    [Fact]
    public void
        By_Interface_And_Implementation_Type_Provided_As_Generics_And_Parameter() //AttachTransient<TService,TImplementation>(IServiceCollection, Func<IServiceProvider,TImplementation>)
    {
        //Arrange
        Func<Foo> fooFactory = () => new Foo();
        Func<Bar> barFactory = () => new Bar(new Foo());
        Func<Baz> bazFactory = () => new Baz(new Foo(), new Bar(new Foo()));

        Container
            .AttachTransient<IFoo, Foo>(fooFactory)
            .AttachTransient<IBar, Bar>(barFactory)
            .AttachTransient<IBaz, Baz>(bazFactory);

        //Act
        var foo1 = Container.Resolve<IFoo>();
        var foo2 = Container.Resolve<IFoo>();
        var bar1 = Container.Resolve<IBar>();
        var bar2 = Container.Resolve<IBar>();
        var baz1 = Container.Resolve<IBaz>();
        var baz2 = Container.Resolve<IBaz>();

        //Assert
        Assert.IsAssignableFrom<IFoo>(foo1);
        Assert.NotSame(foo1, foo2);
        Assert.IsAssignableFrom<IBar>(bar1);
        Assert.NotSame(bar1, bar2);
        Assert.NotSame(bar1.Foo, bar2.Foo);
        Assert.IsAssignableFrom<IFoo>(bar1.Foo);
        Assert.IsType<Baz>(baz1);
        Assert.NotSame(baz1.Foo, baz2.Foo);
        Assert.IsAssignableFrom<IFoo>(bar1.Foo);
        //Assert.NotSame(baz1.Bar, baz1.Bar);
        Assert.IsAssignableFrom<Bar>(baz1.Bar);
        Assert.NotSame(baz1, baz2);
    }

    [Fact]
    public void
        By_Self_Type_And_Implementation_Type_Provided_As_Generics_And_Parameter() //AttachTransient<TService,TImplementation>(IServiceCollection, Func<IServiceProvider,TImplementation>)
    {
        //Arrange
        Func<Foo> fooFactory = () => new Foo();
        Func<Bar> barFactory = () => new Bar(new Foo());
        Func<Baz> bazFactory = () => new Baz(new Foo(), new Bar(new Foo()));


        Container
            .AttachTransient<Foo, Foo>(fooFactory)
            .AttachTransient<Bar, Bar>(barFactory)
            .AttachTransient<Baz, Baz>(bazFactory);

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
    public void By_Interface_Provided_As_Generics() //AttachTransient<TService>(IServiceCollection)
    {
        var expectedMessage =
            "Cannot instantiate implementation type 'EasyDI.Tests.IFoo' because it is an interface or abstract class.";

        //Act and Assert
        var ex = Assert.Throws<ArgumentException>(() => Container.AttachTransient<IFoo>());

        Assert.Equal(ex.Message, expectedMessage);
    }

    [Fact]
    public void By_Self_Type_Provided_As_Generics() //AttachTransient<TService>(IServiceCollection)
    {
        //Arrange
        Container
            .AttachTransient<Foo>()
            .AttachTransient<IFoo, Foo>()
            .AttachTransient<Bar>()
            .AttachTransient<IBar, Bar>()
            .AttachTransient<Baz>();

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
        Assert.IsType<Bar>(bar1);
        Assert.NotSame(bar1, bar2);
        //Assert.NotSame(bar1.Foo, bar2.Foo);
        Assert.IsType<Foo>(bar1.Foo);
        Assert.IsType<Baz>(baz1);
        //Assert.NotSame(baz1.Foo, baz2.Foo);
        Assert.IsType<Foo>(baz1.Foo);
        //Assert.NotSame(baz1.Bar, baz1.Bar);
        Assert.IsType<Bar>(baz1.Bar);
        Assert.NotSame(baz1, baz2);
    }

    [Fact]
    public void
        By_Interface_And_Factory_With_Implementation_Provided_As_Generics_And_Parameter() //AttachTransient<TService>(IServiceCollection, Func<IServiceProvider,TService>)
    {
        //Arrange
        Func<Foo> fooFactory = () => new Foo();
        Func<Bar> barFactory = () => new Bar(new Foo());
        Func<Baz> bazFactory = () => new Baz(new Foo(), new Bar(new Foo()));


        Container
            .AttachTransient<IFoo>(fooFactory)
            .AttachTransient<IBar>(barFactory)
            .AttachTransient<IBaz>(bazFactory);

        //Act
        var foo1 = Container.Resolve<IFoo>();
        var foo2 = Container.Resolve<IFoo>();
        var bar1 = Container.Resolve<IBar>();
        var bar2 = Container.Resolve<IBar>();
        var baz1 = Container.Resolve<IBaz>();
        var baz2 = Container.Resolve<IBaz>();

        //Assert
        Assert.IsAssignableFrom<IFoo>(foo1);
        Assert.NotSame(foo1, foo2);
        Assert.IsAssignableFrom<IBar>(bar1);
        Assert.NotSame(bar1, bar2);
        Assert.NotSame(bar1.Foo, bar2.Foo);
        Assert.IsAssignableFrom<IFoo>(bar1.Foo);
        Assert.IsType<Baz>(baz1);
        Assert.NotSame(baz1.Foo, baz2.Foo);
        Assert.IsAssignableFrom<IFoo>(bar1.Foo);
        //Assert.NotSame(baz1.Bar, baz1.Bar);
        Assert.IsAssignableFrom<Bar>(baz1.Bar);
        Assert.NotSame(baz1, baz2);
    }

    [Fact]
    public void By_Interface_And_Factory_With_Null_Implementation_Provided_As_Generics_And_Parameter()
    {
        //Arrange
        var expectedMessage = "No service for type EasyDI.Tests.IFoo has been registered";
        Func<Foo> fooFactory = () => null;
        Func<Bar> barFactory = () => null;
        Func<Baz> bazFactory = () => null;

        //Act
        Container
            .AttachTransient<IFoo>(fooFactory)
            .AttachTransient<IBar>(barFactory)
            .AttachTransient<IBaz>(bazFactory);

        //Assert
        var ex = Assert.Throws<InvalidOperationException>(() => Container.Resolve<IFoo>());

        Assert.Equal(ex.Message, expectedMessage);
    }

    [Fact]
    public void
        By_Self_Type_And_Factory_With_Implementation_Provided_As_Generics_And_Parameter() //AttachTransient<TService>(IServiceCollection, Func<IServiceProvider,TService>)
    {
        //Arrange
        Func<Foo> fooFactory = () => new Foo();
        Func<Bar> barFactory = () => new Bar(new Foo());
        Func<Baz> bazFactory = () => new Baz(new Foo(), new Bar(new Foo()));


        Container
            .AttachTransient<Foo>(fooFactory)
            .AttachTransient<Bar>(barFactory)
            .AttachTransient<Baz>(bazFactory);

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
        Assert.IsType<Bar>(bar1);
        Assert.NotSame(bar1, bar2);
        Assert.NotSame(bar1.Foo, bar2.Foo);
        Assert.IsType<Foo>(bar1.Foo);
        Assert.IsType<Baz>(baz1);
        Assert.NotSame(baz1.Foo, baz2.Foo);
        Assert.IsType<Foo>(baz1.Foo);
        //Assert.NotSame(baz1.Bar, baz1.Bar);
        Assert.IsType<Bar>(baz1.Bar);
        Assert.NotSame(baz1, baz2);
    }

    [Fact]
    public void By_Self_And_Factory_With_Null_Implementation_Provided_As_Generics_And_Parameter()
    {
        //Arrange
        var expectedMessage = "No service for type EasyDI.Tests.IFoo has been registered";

        Func<Foo> fooFactory = () => null;
        Func<Bar> barFactory = () => null;
        Func<Baz> bazFactory = () => null;

        //Act
        Container
            .AttachTransient<Foo>(fooFactory)
            .AttachTransient<Bar>(barFactory)
            .AttachTransient<Baz>(bazFactory);

        //Assert
        var ex = Assert.Throws<InvalidOperationException>(() => Container.Resolve<IFoo>());

        Assert.Equal(expectedMessage, ex.Message);
    }

    [Fact]
    public void Mixed_Method_Definitions()
    {
        //Arrange
        Container
            .AttachTransient(typeof(IFoo), typeof(Foo))
            .AttachTransient(typeof(Bar))
            .AttachTransient<IBar, Bar>()
            .AttachTransient<IBaz, Baz>();

        //Act
        var foo1 = Container.Resolve<IFoo>();
        var foo2 = Container.Resolve<IFoo>();
        var bar1 = Container.Resolve<Bar>();
        var bar2 = Container.Resolve<Bar>();
        var baz1 = Container.Resolve<IBaz>();
        var baz2 = Container.Resolve<IBaz>();

        //Assert
        Assert.IsType<Foo>(foo1);
        Assert.NotSame(foo1, foo2);
        Assert.IsType<Bar>(bar1);
        Assert.NotSame(bar1, bar2);
        //Assert.NotSame(bar1.Foo, bar2.Foo);
        Assert.IsType<Foo>(bar1.Foo);
        Assert.IsType<Baz>(baz1);
        //Assert.NotSame(baz1.Foo, baz2.Foo);
        Assert.IsType<Foo>(baz1.Foo);
        //Assert.NotSame(baz1.Bar, baz1.Bar);
        Assert.IsType<Bar>(baz1.Bar);
        Assert.NotSame(baz1, baz2);
    }

    [Fact]
    public void By_Interface_And_Implementation_Type_Provided_As_Generics_The_Same_Registration_Twice()
    {
        //Arrange
        Container
            .AttachTransient<IFoo, Foo>()
            .AttachTransient<IFoo, Foo>()
            .AttachTransient<IBar, Bar>()
            .AttachTransient<IBar, Bar>()
            .AttachTransient<IBaz, Baz>()
            .AttachTransient<IBaz, Baz>();

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
        //Assert.NotSame(baz1.Bar, baz1.Bar);
        Assert.IsAssignableFrom<Bar>(baz1.Bar);
        Assert.NotSame(baz1, baz2);
    }


    [Fact]
    public void No_Registration()
    {
        //Arrange
        var expectedMessage = "No service for type EasyDI.Tests.IFoo has been registered";

        //Act and Assert
        var ex = Assert.Throws<InvalidOperationException>(() => Container.Resolve<IFoo>());

        Assert.Equal(ex.Message, expectedMessage);
    }
}
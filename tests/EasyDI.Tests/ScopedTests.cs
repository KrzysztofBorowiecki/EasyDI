namespace EasyDI.Tests;

public class ScopedTests : ContainerFixture
{
    [Fact]
    public void Register_By_Interface_Provides_As_Parameter() //AttachScoped(IServiceCollection, Type)
    {
        //Arrange
        var expectedMessage = "Cannot instantiate implementation type 'EasyDI.Tests.IFoo' because it is an interface or abstract class.";

        //Act and Assert
        var ex = Assert.Throws<ArgumentException>(() => Container
            .AttachScoped(typeof(IFoo)));

        Assert.Equal(ex.Message, expectedMessage);
    }

    [Fact]
    public void Register_By_Self_Provides_As_Parameter() //AttachScoped(IServiceCollection, Type)
    {
        //Arrange
        Container
            .AttachScoped(typeof(Foo))
            .AttachScoped(typeof(IFoo), typeof(Foo))
            .AttachScoped(typeof(Bar))
            .AttachScoped(typeof(IBar), typeof(Bar))
            .AttachScoped(typeof(Baz));

        //Act
        var foo1 = Container.Resolve<Foo>();
        var foo2 = Container.Resolve<Foo>();
        var bar1 = Container.Resolve<Bar>();
        var bar2 = Container.Resolve<Bar>();
        var baz1 = Container.Resolve<Baz>();
        var baz2 = Container.Resolve<Baz>();

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

        using var scope = Container.CreateScope();

        //Act
        var foo3 = scope.Resolve<Foo>();
        var foo4 = scope.Resolve<Foo>();
        var bar3 = scope.Resolve<Bar>();
        var bar4 = scope.Resolve<Bar>();
        var baz3 = scope.Resolve<Baz>();
        var baz4 = scope.Resolve<Baz>();

        //Assert
        Assert.IsType<Foo>(foo3);
        Assert.Same(foo3, foo4);

        Assert.IsType<Bar>(bar3);
        Assert.Same(bar3, bar4);
        Assert.IsType<Foo>(bar3.Foo);
        Assert.Same(bar3.Foo, bar4.Foo);

        Assert.IsType<Baz>(baz3);
        Assert.Same(baz3, baz4);
        Assert.IsType<Foo>(baz3.Foo);
        Assert.Same(baz3.Foo, baz4.Foo);
        Assert.IsType<Bar>(baz3.Bar);
        Assert.Same(baz3.Bar, baz4.Bar);


        Assert.NotSame(foo1, foo3);

        Assert.NotSame(bar1, bar3);
        Assert.NotSame(bar1.Foo, bar3.Foo);

        Assert.NotSame(baz1, baz3);
        Assert.NotSame(baz1.Foo, baz3.Foo);
        Assert.NotSame(baz1.Bar, baz3.Bar);
    }

    [Fact]
    public void
        Register_By_Interface_And_Factory_With_Implementation_Provided_As_Parameters() //AttachScoped(IServiceCollection, Type, Func<IServiceProvider,Object>)
    {
        //Arrange
        Func<IServiceProvider, Foo> fooFactory = _ => new Foo();
        Func<IServiceProvider, Bar> barFactory = _ => new Bar(new Foo());
        Func<IServiceProvider, Baz> bazFactory = _ => new Baz(new Foo(), new Bar(new Foo()));

        Container
            .AttachScoped(typeof(IFoo), fooFactory)
            .AttachScoped(typeof(IBar), barFactory)
            .AttachScoped(typeof(IBaz), bazFactory);


        //Act
        var foo1 = Container.Resolve<IFoo>();
        var foo2 = Container.Resolve<IFoo>();
        var bar1 = Container.Resolve<IBar>();
        var bar2 = Container.Resolve<IBar>();
        var baz1 = Container.Resolve<IBaz>();
        var baz2 = Container.Resolve<IBaz>();

        //Assert
        Assert.IsAssignableFrom<IFoo>(foo1);
        Assert.Same(foo1, foo2);

        Assert.IsAssignableFrom<IBar>(bar1);
        Assert.Same(bar1, bar2);
        Assert.IsAssignableFrom<IFoo>(bar1.Foo);
        Assert.Same(bar1.Foo, bar2.Foo);

        Assert.IsAssignableFrom<IFoo>(baz1);
        Assert.Same(baz1, baz2);
        Assert.IsAssignableFrom<IFoo>(baz1.Foo);
        Assert.Same(baz1.Foo, baz2.Foo);
        Assert.IsAssignableFrom<IBar>(baz1.Bar);
        Assert.Same(baz1.Bar, baz2.Bar);

        using var scope = Container.CreateScope();

        //Act
        var foo3 = scope.Resolve<IFoo>();
        var foo4 = scope.Resolve<IFoo>();
        var bar3 = scope.Resolve<IBar>();
        var bar4 = scope.Resolve<IBar>();
        var baz3 = scope.Resolve<IBaz>();
        var baz4 = scope.Resolve<IBaz>();

        //Assert
        Assert.IsAssignableFrom<IFoo>(foo3);
        Assert.Same(foo3, foo4);

        Assert.IsAssignableFrom<IBar>(bar3);
        Assert.Same(bar3, bar4);
        Assert.IsAssignableFrom<IFoo>(bar3.Foo);
        Assert.Same(bar3.Foo, bar4.Foo);

        Assert.IsAssignableFrom<IBaz>(baz3);
        Assert.Same(baz3, baz4);
        Assert.IsAssignableFrom<IFoo>(baz3.Foo);
        Assert.Same(baz3.Foo, baz4.Foo);
        Assert.IsAssignableFrom<IBar>(baz3.Bar);
        Assert.Same(baz3.Bar, baz4.Bar);


        Assert.NotSame(foo1, foo3);

        Assert.NotSame(bar1, bar3);
        Assert.NotSame(bar1.Foo, bar3.Foo);

        Assert.NotSame(baz1, baz3);
        Assert.NotSame(baz1.Foo, baz3.Foo);
        Assert.NotSame(baz1.Bar, baz3.Bar);
    }

    [Fact]
    public void RegisterBy_Interface_And_Factory_With_Null_Implementation_Provided_As_Parameters()
    {
        //Arrange
        var expectedMessage = "No service for type EasyDI.Tests.IFoo has been registered";

        Func<Foo> fooFactory = () => null;
        Func<Bar> barFactory = () => null;
        Func<Baz> bazFactory = () => null;

        //Act
        Container
            .AttachScoped(typeof(IFoo), fooFactory)
            .AttachScoped(typeof(IBar), barFactory)
            .AttachScoped(typeof(IBaz), bazFactory);

        //Assert
        var ex = Assert.Throws<InvalidOperationException>(() => Container.Resolve<IFoo>());

        Assert.Equal(ex.Message, expectedMessage);
    }

    [Fact]
    public void
        Register_By_Self_Type_And_Factory_With_Implementation_Provided_As_Parameters() //AttachScoped(IServiceCollection, Type, Func<IServiceProvider,Object>)
    {
        //Arrange
        Func<IServiceProvider, Foo> fooFactory = _ => new Foo();
        Func<IServiceProvider, Bar> barFactory = _ => new Bar(new Foo());
        Func<IServiceProvider, Baz> bazFactory = _ => new Baz(new Foo(), new Bar(new Foo()));

        Container
            .AttachScoped(typeof(Foo), fooFactory)
            .AttachScoped(typeof(Bar), barFactory)
            .AttachScoped(typeof(Baz), bazFactory);


        //Act
        var foo1 = Container.Resolve<Foo>();
        var foo2 = Container.Resolve<Foo>();
        var bar1 = Container.Resolve<Bar>();
        var bar2 = Container.Resolve<Bar>();
        var baz1 = Container.Resolve<Baz>();
        var baz2 = Container.Resolve<Baz>();

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

        using var scope = Container.CreateScope();

        //Act
        var foo3 = scope.Resolve<Foo>();
        var foo4 = scope.Resolve<Foo>();
        var bar3 = scope.Resolve<Bar>();
        var bar4 = scope.Resolve<Bar>();
        var baz3 = scope.Resolve<Baz>();
        var baz4 = scope.Resolve<Baz>();

        //Assert
        Assert.IsType<Foo>(foo3);
        Assert.Same(foo3, foo4);

        Assert.IsType<Bar>(bar3);
        Assert.Same(bar3, bar4);
        Assert.IsType<Foo>(bar3.Foo);
        Assert.Same(bar3.Foo, bar4.Foo);

        Assert.IsType<Baz>(baz3);
        Assert.Same(baz3, baz4);
        Assert.IsType<Foo>(baz3.Foo);
        Assert.Same(baz3.Foo, baz4.Foo);
        Assert.IsType<Bar>(baz3.Bar);
        Assert.Same(baz3.Bar, baz4.Bar);


        Assert.NotSame(foo1, foo3);

        Assert.NotSame(bar1, bar3);
        Assert.NotSame(bar1.Foo, bar3.Foo);

        Assert.NotSame(baz1, baz3);
        Assert.NotSame(baz1.Foo, baz3.Foo);
        Assert.NotSame(baz1.Bar, baz3.Bar);
    }

    [Fact]
    public void Register_By_Self_And_Factory_With_Null_Implementation_Provided_As_Parameters()
    {
        //Arrange
        var expectedMessage = "No service for type EasyDI.Tests.IFoo has been registered";

        Func<IServiceProvider, Foo> fooFactory = _ => null;
        Func<IServiceProvider, Bar> barFactory = _ => null;
        Func<IServiceProvider, Baz> bazFactory = _ => null;

        //Act
        Container
            .AttachScoped(typeof(IFoo), fooFactory)
            .AttachScoped(typeof(IBar), barFactory)
            .AttachScoped(typeof(IBaz), bazFactory);

        //Assert
        var ex = Assert.Throws<InvalidOperationException>(() => Container.Resolve<IFoo>());

        Assert.Equal(ex.Message, expectedMessage);
    }

    [Fact]
    public void
        Register_By_Interface_And_Type_Of_Implementation_Provided_As_Parameters() //AttachScoped(IServiceCollection, Type, Type)
    {
        //Arrange
        Container
            .AttachScoped(typeof(IFoo), typeof(Foo))
            .AttachScoped(typeof(IBar), typeof(Bar))
            .AttachScoped(typeof(IBaz), typeof(Baz));

        //Act
        var foo1 = Container.Resolve<IFoo>();
        var foo2 = Container.Resolve<IFoo>();
        var bar1 = Container.Resolve<IBar>();
        var bar2 = Container.Resolve<IBar>();
        var baz1 = Container.Resolve<IBaz>();
        var baz2 = Container.Resolve<IBaz>();

        //Assert
        Assert.IsAssignableFrom<IFoo>(foo1);
        Assert.Same(foo1, foo2);

        Assert.IsAssignableFrom<IBar>(bar1);
        Assert.Same(bar1, bar2);
        Assert.IsAssignableFrom<IFoo>(bar1.Foo);
        Assert.Same(bar1.Foo, bar2.Foo);

        Assert.IsAssignableFrom<IBaz>(baz1);
        Assert.Same(baz1, baz2);
        Assert.IsAssignableFrom<IFoo>(baz1.Foo);
        Assert.Same(baz1.Foo, baz2.Foo);
        Assert.IsAssignableFrom<IBar>(baz1.Bar);
        Assert.Same(baz1.Bar, baz2.Bar);

        using var scope = Container.CreateScope();

        //Act
        var foo3 = scope.Resolve<IFoo>();
        var foo4 = scope.Resolve<IFoo>();
        var bar3 = scope.Resolve<IBar>();
        var bar4 = scope.Resolve<IBar>();
        var baz3 = scope.Resolve<IBaz>();
        var baz4 = scope.Resolve<IBaz>();

        //Assert
        Assert.IsAssignableFrom<IFoo>(foo3);
        Assert.Same(foo3, foo4);

        Assert.IsAssignableFrom<IBar>(bar3);
        Assert.Same(bar3, bar4);
        Assert.IsAssignableFrom<IFoo>(bar3.Foo);
        Assert.Same(bar3.Foo, bar4.Foo);

        Assert.IsAssignableFrom<IBaz>(baz3);
        Assert.Same(baz3, baz4);
        Assert.IsAssignableFrom<IFoo>(baz3.Foo);
        Assert.Same(baz3.Foo, baz4.Foo);
        Assert.IsAssignableFrom<IBar>(baz3.Bar);
        Assert.Same(baz3.Bar, baz4.Bar);


        Assert.NotSame(foo1, foo3);

        Assert.NotSame(bar1, bar3);
        Assert.NotSame(bar1.Foo, bar3.Foo);

        Assert.NotSame(baz1, baz3);
        Assert.NotSame(baz1.Foo, baz3.Foo);
        Assert.NotSame(baz1.Bar, baz3.Bar);
    }

    [Fact]
    public void
        Register_By_Self_Type_And_Type_Of_Implementation_Provided_As_Parameters() //AttachScoped(IServiceCollection, Type, Type)
    {
        //Arrange
        Container
            .AttachScoped(typeof(Foo), typeof(Foo))
            .AttachScoped(typeof(IFoo), typeof(Foo))
            .AttachScoped(typeof(Bar), typeof(Bar))
            .AttachScoped(typeof(IBar), typeof(Bar))
            .AttachScoped(typeof(Baz), typeof(Baz));

        //Act
        var foo1 = Container.Resolve<Foo>();
        var foo2 = Container.Resolve<Foo>();
        var bar1 = Container.Resolve<Bar>();
        var bar2 = Container.Resolve<Bar>();
        var baz1 = Container.Resolve<Baz>();
        var baz2 = Container.Resolve<Baz>();

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

        using var scope = Container.CreateScope();

        //Act
        var foo3 = scope.Resolve<Foo>();
        var foo4 = scope.Resolve<Foo>();
        var bar3 = scope.Resolve<Bar>();
        var bar4 = scope.Resolve<Bar>();
        var baz3 = scope.Resolve<Baz>();
        var baz4 = scope.Resolve<Baz>();

        //Assert
        Assert.IsType<Foo>(foo3);
        Assert.Same(foo3, foo4);

        Assert.IsType<Bar>(bar3);
        Assert.Same(bar3, bar4);
        Assert.IsType<Foo>(bar3.Foo);
        Assert.Same(bar3.Foo, bar4.Foo);

        Assert.IsType<Baz>(baz3);
        Assert.Same(baz3, baz4);
        Assert.IsType<Foo>(baz3.Foo);
        Assert.Same(baz3.Foo, baz4.Foo);
        Assert.IsType<Bar>(baz3.Bar);
        Assert.Same(baz3.Bar, baz4.Bar);


        Assert.NotSame(foo1, foo3);

        Assert.NotSame(bar1, bar3);
        Assert.NotSame(bar1.Foo, bar3.Foo);

        Assert.NotSame(baz1, baz3);
        Assert.NotSame(baz1.Foo, baz3.Foo);
        Assert.NotSame(baz1.Bar, baz3.Bar);
    }

    [Fact]
    public void
        Register_By_Interface_And_Implementation_Type_Provided_As_Generics() //AttachScoped<TService,TImplementation>(IServiceCollection)
    {
        //Arrange
        Container
            .AttachScoped<IFoo, Foo>()
            .AttachScoped<IBar, Bar>()
            .AttachScoped<IBaz, Baz>();

        //Act
        var foo1 = Container.Resolve<IFoo>();
        var foo2 = Container.Resolve<IFoo>();
        var bar1 = Container.Resolve<IBar>();
        var bar2 = Container.Resolve<IBar>();
        var baz1 = Container.Resolve<IBaz>();
        var baz2 = Container.Resolve<IBaz>();

        //Assert
        Assert.IsAssignableFrom<IFoo>(foo1);
        Assert.Same(foo1, foo2);

        Assert.IsAssignableFrom<IBar>(bar1);
        Assert.Same(bar1, bar2);
        Assert.IsAssignableFrom<IFoo>(bar1.Foo);
        Assert.Same(bar1.Foo, bar2.Foo);

        Assert.IsAssignableFrom<IBaz>(baz1);
        Assert.Same(baz1, baz2);
        Assert.IsAssignableFrom<IFoo>(baz1.Foo);
        Assert.Same(baz1.Foo, baz2.Foo);
        Assert.IsAssignableFrom<IBar>(baz1.Bar);
        Assert.Same(baz1.Bar, baz2.Bar);

        using var scope = Container.CreateScope();

        //Act
        var foo3 = scope.Resolve<IFoo>();
        var foo4 = scope.Resolve<IFoo>();
        var bar3 = scope.Resolve<IBar>();
        var bar4 = scope.Resolve<IBar>();
        var baz3 = scope.Resolve<IBaz>();
        var baz4 = scope.Resolve<IBaz>();

        //Assert
        Assert.IsAssignableFrom<IFoo>(foo3);
        Assert.Same(foo3, foo4);

        Assert.IsAssignableFrom<IBar>(bar3);
        Assert.Same(bar3, bar4);
        Assert.IsAssignableFrom<IFoo>(bar3.Foo);
        Assert.Same(bar3.Foo, bar4.Foo);

        Assert.IsAssignableFrom<IBaz>(baz3);
        Assert.Same(baz3, baz4);
        Assert.IsAssignableFrom<IFoo>(baz3.Foo);
        Assert.Same(baz3.Foo, baz4.Foo);
        Assert.IsAssignableFrom<IBar>(baz3.Bar);
        Assert.Same(baz3.Bar, baz4.Bar);


        Assert.NotSame(foo1, foo3);

        Assert.NotSame(bar1, bar3);
        Assert.NotSame(bar1.Foo, bar3.Foo);

        Assert.NotSame(baz1, baz3);
        Assert.NotSame(baz1.Foo, baz3.Foo);
        Assert.NotSame(baz1.Bar, baz3.Bar);
    }

    [Fact]
    public void
        Register_By_Self_Type_And_Implementation_Type_Provided_As_Generics() //AttachScoped<TService,TImplementation>(IServiceCollection)
    {
        //Arrange
        Container
            .AttachScoped<Foo, Foo>()
            .AttachScoped<IFoo, Foo>()
            .AttachScoped<Bar, Bar>()
            .AttachScoped<IBar, Bar>()
            .AttachScoped<Baz, Baz>();

        //Act
        var foo1 = Container.Resolve<Foo>();
        var foo2 = Container.Resolve<Foo>();
        var bar1 = Container.Resolve<Bar>();
        var bar2 = Container.Resolve<Bar>();
        var baz1 = Container.Resolve<Baz>();
        var baz2 = Container.Resolve<Baz>();

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

        using var scope = Container.CreateScope();

        //Act
        var foo3 = scope.Resolve<Foo>();
        var foo4 = scope.Resolve<Foo>();
        var bar3 = scope.Resolve<Bar>();
        var bar4 = scope.Resolve<Bar>();
        var baz3 = scope.Resolve<Baz>();
        var baz4 = scope.Resolve<Baz>();

        //Assert
        Assert.IsType<Foo>(foo3);
        Assert.Same(foo3, foo4);

        Assert.IsType<Bar>(bar3);
        Assert.Same(bar3, bar4);
        Assert.IsType<Foo>(bar3.Foo);
        Assert.Same(bar3.Foo, bar4.Foo);

        Assert.IsType<Baz>(baz3);
        Assert.Same(baz3, baz4);
        Assert.IsType<Foo>(baz3.Foo);
        Assert.Same(baz3.Foo, baz4.Foo);
        Assert.IsType<Bar>(baz3.Bar);
        Assert.Same(baz3.Bar, baz4.Bar);


        Assert.NotSame(foo1, foo3);

        Assert.NotSame(bar1, bar3);
        Assert.NotSame(bar1.Foo, bar3.Foo);

        Assert.NotSame(baz1, baz3);
        Assert.NotSame(baz1.Foo, baz3.Foo);
        Assert.NotSame(baz1.Bar, baz3.Bar);
    }

    [Fact]
    public void
        Register_By_Interface_And_Implementation_Type_Provided_As_Generics_And_Parameter() //AttachScoped<TService,TImplementation>(IServiceCollection, Func<IServiceProvider,TImplementation>)
    {
        //Arrange
        Func<Foo> fooFactory = () => new Foo();
        Func<Bar> barFactory = () => new Bar(new Foo());
        Func<Baz> bazFactory = () => new Baz(new Foo(), new Bar(new Foo()));

        Container
            .AttachScoped<IFoo, Foo>(fooFactory)
            .AttachScoped<IBar, Bar>(barFactory)
            .AttachScoped<IBaz, Baz>(bazFactory);

        //Act
        var foo1 = Container.Resolve<IFoo>();
        var foo2 = Container.Resolve<IFoo>();
        var bar1 = Container.Resolve<IBar>();
        var bar2 = Container.Resolve<IBar>();
        var baz1 = Container.Resolve<IBaz>();
        var baz2 = Container.Resolve<IBaz>();

        //Assert
        Assert.IsAssignableFrom<IFoo>(foo1);
        Assert.Same(foo1, foo2);

        Assert.IsAssignableFrom<IBar>(bar1);
        Assert.Same(bar1, bar2);
        Assert.IsAssignableFrom<IFoo>(bar1.Foo);
        Assert.Same(bar1.Foo, bar2.Foo);

        Assert.IsAssignableFrom<IBaz>(baz1);
        Assert.Same(baz1, baz2);
        Assert.IsAssignableFrom<IFoo>(baz1.Foo);
        Assert.Same(baz1.Foo, baz2.Foo);
        Assert.IsAssignableFrom<IBar>(baz1.Bar);
        Assert.Same(baz1.Bar, baz2.Bar);

        using var scope = Container.CreateScope();

        //Act
        var foo3 = scope.Resolve<IFoo>();
        var foo4 = scope.Resolve<IFoo>();
        var bar3 = scope.Resolve<IBar>();
        var bar4 = scope.Resolve<IBar>();
        var baz3 = scope.Resolve<IBaz>();
        var baz4 = scope.Resolve<IBaz>();

        //Assert
        Assert.IsAssignableFrom<IFoo>(foo3);
        Assert.Same(foo3, foo4);

        Assert.IsAssignableFrom<IBar>(bar3);
        Assert.Same(bar3, bar4);
        Assert.IsAssignableFrom<IFoo>(bar3.Foo);
        Assert.Same(bar3.Foo, bar4.Foo);

        Assert.IsAssignableFrom<IBaz>(baz3);
        Assert.Same(baz3, baz4);
        Assert.IsAssignableFrom<IFoo>(baz3.Foo);
        Assert.Same(baz3.Foo, baz4.Foo);
        Assert.IsAssignableFrom<IBar>(baz3.Bar);
        Assert.Same(baz3.Bar, baz4.Bar);


        Assert.NotSame(foo1, foo3);

        Assert.NotSame(bar1, bar3);
        Assert.NotSame(bar1.Foo, bar3.Foo);

        Assert.NotSame(baz1, baz3);
        Assert.NotSame(baz1.Foo, baz3.Foo);
        Assert.NotSame(baz1.Bar, baz3.Bar);
    }

    [Fact]
    public void
        Register_By_Self_Type_And_Implementation_Type_Provided_As_Generics_And_Parameter() //AttachScoped<TService,TImplementation>(IServiceCollection, Func<IServiceProvider,TImplementation>)
    {
        //Arrange
        Func<Foo> fooFactory = () => new Foo();
        Func<Bar> barFactory = () => new Bar(new Foo());
        Func<Baz> bazFactory = () => new Baz(new Foo(), new Bar(new Foo()));

        Container
            .AttachScoped<Foo, Foo>(fooFactory)
            .AttachScoped<Bar, Bar>(barFactory)
            .AttachScoped<Baz, Baz>(bazFactory);

        //Act
        var foo1 = Container.Resolve<Foo>();
        var foo2 = Container.Resolve<Foo>();
        var bar1 = Container.Resolve<Bar>();
        var bar2 = Container.Resolve<Bar>();
        var baz1 = Container.Resolve<Baz>();
        var baz2 = Container.Resolve<Baz>();

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

        using var scope = Container.CreateScope();

        //Act
        var foo3 = scope.Resolve<Foo>();
        var foo4 = scope.Resolve<Foo>();
        var bar3 = scope.Resolve<Bar>();
        var bar4 = scope.Resolve<Bar>();
        var baz3 = scope.Resolve<Baz>();
        var baz4 = scope.Resolve<Baz>();

        //Assert
        Assert.IsType<Foo>(foo3);
        Assert.Same(foo3, foo4);

        Assert.IsType<Bar>(bar3);
        Assert.Same(bar3, bar4);
        Assert.IsType<Foo>(bar3.Foo);
        Assert.Same(bar3.Foo, bar4.Foo);

        Assert.IsType<Baz>(baz3);
        Assert.Same(baz3, baz4);
        Assert.IsType<Foo>(baz3.Foo);
        Assert.Same(baz3.Foo, baz4.Foo);
        Assert.IsType<Bar>(baz3.Bar);
        Assert.Same(baz3.Bar, baz4.Bar);


        Assert.NotSame(foo1, foo3);

        Assert.NotSame(bar1, bar3);
        Assert.NotSame(bar1.Foo, bar3.Foo);

        Assert.NotSame(baz1, baz3);
        Assert.NotSame(baz1.Foo, baz3.Foo);
        Assert.NotSame(baz1.Bar, baz3.Bar);
    }

    [Fact]
    public void Register_By_Interface_Provided_As_Generics() //AttachScoped<TService>(IServiceCollection)
    {
        var expectedMessage = "Cannot instantiate implementation type 'EasyDI.Tests.IFoo' because it is an interface or abstract class.";

        //Act and Assert
        var ex = Assert.Throws<ArgumentException>(() => Container
            .AttachScoped<IFoo>());

        Assert.Equal(ex.Message, expectedMessage);
    }

    [Fact]
    public void Register_By_Self_Type_Provided_As_Generics() //AttachScoped<TService>(IServiceCollection)
    {
        //Arrange
        Container
            .AttachScoped<Foo>()
            .AttachScoped<IFoo, Foo>()
            .AttachScoped<Bar>()
            .AttachScoped<IBar, Bar>()
            .AttachScoped<Baz>();

        //Act
        var foo1 = Container.Resolve<Foo>();
        var foo2 = Container.Resolve<Foo>();
        var bar1 = Container.Resolve<Bar>();
        var bar2 = Container.Resolve<Bar>();
        var baz1 = Container.Resolve<Baz>();
        var baz2 = Container.Resolve<Baz>();

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

        using var scope = Container.CreateScope();

        //Act
        var foo3 = scope.Resolve<Foo>();
        var foo4 = scope.Resolve<Foo>();
        var bar3 = scope.Resolve<Bar>();
        var bar4 = scope.Resolve<Bar>();
        var baz3 = scope.Resolve<Baz>();
        var baz4 = scope.Resolve<Baz>();

        //Assert
        Assert.IsType<Foo>(foo3);
        Assert.Same(foo3, foo4);

        Assert.IsType<Bar>(bar3);
        Assert.Same(bar3, bar4);
        Assert.IsType<Foo>(bar3.Foo);
        Assert.Same(bar3.Foo, bar4.Foo);

        Assert.IsType<Baz>(baz3);
        Assert.Same(baz3, baz4);
        Assert.IsType<Foo>(baz3.Foo);
        Assert.Same(baz3.Foo, baz4.Foo);
        Assert.IsType<Bar>(baz3.Bar);
        Assert.Same(baz3.Bar, baz4.Bar);


        Assert.NotSame(foo1, foo3);

        Assert.NotSame(bar1, bar3);
        Assert.NotSame(bar1.Foo, bar3.Foo);

        Assert.NotSame(baz1, baz3);
        Assert.NotSame(baz1.Foo, baz3.Foo);
        Assert.NotSame(baz1.Bar, baz3.Bar);
    }

    [Fact]
    public void
        Register_By_Interface_And_Factory_With_Implementation_Provided_As_Generics_And_Parameter() //AttachScoped<TService>(IServiceCollection, Func<IServiceProvider,TService>)
    {
        //Arrange
        Func<Foo> fooFactory = () => new Foo();
        Func<Bar> barFactory = () => new Bar(new Foo());
        Func<Baz> bazFactory = () => new Baz(new Foo(), new Bar(new Foo()));

        Container
            .AttachScoped<IFoo>(fooFactory)
            .AttachScoped<IBar>(barFactory)
            .AttachScoped<IBaz>(bazFactory);

        //Act
        var foo1 = Container.Resolve<IFoo>();
        var foo2 = Container.Resolve<IFoo>();
        var bar1 = Container.Resolve<IBar>();
        var bar2 = Container.Resolve<IBar>();
        var baz1 = Container.Resolve<IBaz>();
        var baz2 = Container.Resolve<IBaz>();

        //Assert
        Assert.IsAssignableFrom<IFoo>(foo1);
        Assert.Same(foo1, foo2);

        Assert.IsAssignableFrom<IBar>(bar1);
        Assert.Same(bar1, bar2);
        Assert.IsAssignableFrom<IFoo>(bar1.Foo);
        Assert.Same(bar1.Foo, bar2.Foo);

        Assert.IsAssignableFrom<IBaz>(baz1);
        Assert.Same(baz1, baz2);
        Assert.IsAssignableFrom<IFoo>(baz1.Foo);
        Assert.Same(baz1.Foo, baz2.Foo);
        Assert.IsAssignableFrom<IBar>(baz1.Bar);
        Assert.Same(baz1.Bar, baz2.Bar);

        using var scope = Container.CreateScope();

        //Act
        var foo3 = scope.Resolve<IFoo>();
        var foo4 = scope.Resolve<IFoo>();
        var bar3 = scope.Resolve<IBar>();
        var bar4 = scope.Resolve<IBar>();
        var baz3 = scope.Resolve<IBaz>();
        var baz4 = scope.Resolve<IBaz>();

        //Assert
        Assert.IsAssignableFrom<IFoo>(foo3);
        Assert.Same(foo3, foo4);

        Assert.IsAssignableFrom<IBar>(bar3);
        Assert.Same(bar3, bar4);
        Assert.IsAssignableFrom<IFoo>(bar3.Foo);
        Assert.Same(bar3.Foo, bar4.Foo);

        Assert.IsAssignableFrom<IBaz>(baz3);
        Assert.Same(baz3, baz4);
        Assert.IsAssignableFrom<IFoo>(baz3.Foo);
        Assert.Same(baz3.Foo, baz4.Foo);
        Assert.IsAssignableFrom<IBar>(baz3.Bar);
        Assert.Same(baz3.Bar, baz4.Bar);


        Assert.NotSame(foo1, foo3);

        Assert.NotSame(bar1, bar3);
        Assert.NotSame(bar1.Foo, bar3.Foo);

        Assert.NotSame(baz1, baz3);
        Assert.NotSame(baz1.Foo, baz3.Foo);
        Assert.NotSame(baz1.Bar, baz3.Bar);
    }

    [Fact]
    public void Register_By_Interface_And_Factory_With_Null_Implementation_Provided_As_Generics_And_Parameter()
    {
        //Arrange
        var expectedMessage = "No service for type EasyDI.Tests.IFoo has been registered";

        Func<Foo> fooFactory = () => null;
        Func<Bar> barFactory = () => null;
        Func<Baz> bazFactory = () => null;

        //Act
        Container
            .AttachScoped<IFoo>(fooFactory)
            .AttachScoped<IBar>(barFactory)
            .AttachScoped<IBaz>(bazFactory);

        //Assert
        var ex = Assert.Throws<InvalidOperationException>(() => Container.Resolve<IFoo>());

        Assert.Equal(ex.Message, expectedMessage);
    }

    [Fact]
    public void
        Register_By_Self_Type_And_Factory_With_Implementation_Provided_As_Generics_And_Parameter() //AttachScoped<TService>(IServiceCollection, Func<IServiceProvider,TService>)
    {
        //Arrange
        Func<Foo> fooFactory = () => new Foo();
        Func<Bar> barFactory = () => new Bar(new Foo());
        Func<Baz> bazFactory = () => new Baz(new Foo(), new Bar(new Foo()));

        Container
            .AttachScoped<Foo>(fooFactory)
            .AttachScoped<Bar>(barFactory)
            .AttachScoped<Baz>(bazFactory);

        //Act
        var foo1 = Container.Resolve<Foo>();
        var foo2 = Container.Resolve<Foo>();
        var bar1 = Container.Resolve<Bar>();
        var bar2 = Container.Resolve<Bar>();
        var baz1 = Container.Resolve<Baz>();
        var baz2 = Container.Resolve<Baz>();

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

        using var scope = Container.CreateScope();

        //Act
        var foo3 = scope.Resolve<Foo>();
        var foo4 = scope.Resolve<Foo>();
        var bar3 = scope.Resolve<Bar>();
        var bar4 = scope.Resolve<Bar>();
        var baz3 = scope.Resolve<Baz>();
        var baz4 = scope.Resolve<Baz>();

        //Assert
        Assert.IsType<Foo>(foo3);
        Assert.Same(foo3, foo4);

        Assert.IsType<Bar>(bar3);
        Assert.Same(bar3, bar4);
        Assert.IsType<Foo>(bar3.Foo);
        Assert.Same(bar3.Foo, bar4.Foo);

        Assert.IsType<Baz>(baz3);
        Assert.Same(baz3, baz4);
        Assert.IsType<Foo>(baz3.Foo);
        Assert.Same(baz3.Foo, baz4.Foo);
        Assert.IsType<Bar>(baz3.Bar);
        Assert.Same(baz3.Bar, baz4.Bar);


        Assert.NotSame(foo1, foo3);

        Assert.NotSame(bar1, bar3);
        Assert.NotSame(bar1.Foo, bar3.Foo);

        Assert.NotSame(baz1, baz3);
        Assert.NotSame(baz1.Foo, baz3.Foo);
        Assert.NotSame(baz1.Bar, baz3.Bar);
    }

    [Fact]
    public void Register_By_Self_And_Factory_With_Null_Implementation_Provided_As_Generics_And_Parameter()
    {
        //Arrange
        var expectedMessage = "No service for type EasyDI.Tests.IFoo has been registered";

        Func<Foo> fooFactory = () => null;
        Func<Bar> barFactory = () => null;
        Func<Baz> bazFactory = () => null;

        //Act
        Container
            .AttachScoped<Foo>(fooFactory)
            .AttachScoped<Bar>(barFactory)
            .AttachScoped<Baz>(bazFactory);

        //Assert
        var ex = Assert.Throws<InvalidOperationException>(() => Container.Resolve<IFoo>());

        Assert.Equal(ex.Message, expectedMessage);
    }

    [Fact]
    public void Resgiter_By_Mixed_Method_Definitions()
    {
        //Arrange
        Container
            .AttachScoped(typeof(IFoo), typeof(Foo))
            .AttachScoped(typeof(Bar))
            .AttachScoped<IBar, Bar>()
            .AttachScoped<IBaz, Baz>();

        //Act
        var foo1 = Container.Resolve<IFoo>();
        var foo2 = Container.Resolve<IFoo>();
        var bar1 = Container.Resolve<Bar>();
        var bar2 = Container.Resolve<Bar>();
        var baz1 = Container.Resolve<IBaz>();
        var baz2 = Container.Resolve<IBaz>();

        //Assert
        Assert.IsAssignableFrom<IFoo>(foo1);
        Assert.Same(foo1, foo2);

        Assert.IsAssignableFrom<Bar>(bar1);
        Assert.Same(bar1, bar2);
        Assert.IsAssignableFrom<IFoo>(bar1.Foo);
        Assert.Same(bar1.Foo, bar2.Foo);

        Assert.IsAssignableFrom<IBaz>(baz1);
        Assert.Same(baz1, baz2);
        Assert.IsAssignableFrom<IFoo>(baz1.Foo);
        Assert.Same(baz1.Foo, baz2.Foo);
        Assert.IsAssignableFrom<Bar>(baz1.Bar);
        Assert.Same(baz1.Bar, baz2.Bar);

        using var scope = Container.CreateScope();

        //Act
        var foo3 = scope.Resolve<IFoo>();
        var foo4 = scope.Resolve<IFoo>();
        var bar3 = scope.Resolve<Bar>();
        var bar4 = scope.Resolve<Bar>();
        var baz3 = scope.Resolve<IBaz>();
        var baz4 = scope.Resolve<IBaz>();

        //Assert
        Assert.IsAssignableFrom<IFoo>(foo3);
        Assert.Same(foo3, foo4);

        Assert.IsType<Bar>(bar3);
        Assert.Same(bar3, bar4);
        Assert.IsAssignableFrom<IFoo>(bar3.Foo);
        Assert.Same(bar3.Foo, bar4.Foo);

        Assert.IsAssignableFrom<IBaz>(baz3);
        Assert.Same(baz3, baz4);
        Assert.IsAssignableFrom<IFoo>(baz3.Foo);
        Assert.Same(baz3.Foo, baz4.Foo);
        Assert.IsType<Bar>(baz3.Bar);
        Assert.Same(baz3.Bar, baz4.Bar);


        Assert.NotSame(foo1, foo3);

        Assert.NotSame(bar1, bar3);
        Assert.NotSame(bar1.Foo, bar3.Foo);

        Assert.NotSame(baz1, baz3);
        Assert.NotSame(baz1.Foo, baz3.Foo);
        Assert.NotSame(baz1.Bar, baz3.Bar);
    }

    [Fact]
    public void Register_By_Interface_And_Implementation_Type_Provided_As_Generics_The_Same_Registration_Twice()
    {
        //Arrange
        Container
            .AttachScoped<IFoo, Foo>()
            .AttachScoped<IFoo, Foo>()
            .AttachScoped<IBar, Bar>()
            .AttachScoped<IBar, Bar>()
            .AttachScoped<IBaz, Baz>()
            .AttachScoped<IBaz, Baz>();

        //Act
        var foo1 = Container.Resolve<IFoo>();
        var foo2 = Container.Resolve<IFoo>();
        var bar1 = Container.Resolve<IBar>();
        var bar2 = Container.Resolve<IBar>();
        var baz1 = Container.Resolve<IBaz>();
        var baz2 = Container.Resolve<IBaz>();

        //Assert
        Assert.IsAssignableFrom<IFoo>(foo1);
        Assert.Same(foo1, foo2);

        Assert.IsAssignableFrom<IBar>(bar1);
        Assert.Same(bar1, bar2);
        Assert.IsAssignableFrom<IFoo>(bar1.Foo);
        Assert.Same(bar1.Foo, bar2.Foo);

        Assert.IsAssignableFrom<IBaz>(baz1);
        Assert.Same(baz1, baz2);
        Assert.IsAssignableFrom<IFoo>(baz1.Foo);
        Assert.Same(baz1.Foo, baz2.Foo);
        Assert.IsAssignableFrom<IBar>(baz1.Bar);
        Assert.Same(baz1.Bar, baz2.Bar);

        using var scope = Container.CreateScope();

        //Act
        var foo3 = scope.Resolve<IFoo>();
        var foo4 = scope.Resolve<IFoo>();
        var bar3 = scope.Resolve<IBar>();
        var bar4 = scope.Resolve<IBar>();
        var baz3 = scope.Resolve<IBaz>();
        var baz4 = scope.Resolve<IBaz>();

        //Assert
        Assert.IsAssignableFrom<IFoo>(foo3);
        Assert.Same(foo3, foo4);

        Assert.IsAssignableFrom<IBar>(bar3);
        Assert.Same(bar3, bar4);
        Assert.IsAssignableFrom<IFoo>(bar3.Foo);
        Assert.Same(bar3.Foo, bar4.Foo);

        Assert.IsAssignableFrom<IBaz>(baz3);
        Assert.Same(baz3, baz4);
        Assert.IsAssignableFrom<IFoo>(baz3.Foo);
        Assert.Same(baz3.Foo, baz4.Foo);
        Assert.IsAssignableFrom<IBar>(baz3.Bar);
        Assert.Same(baz3.Bar, baz4.Bar);


        Assert.NotSame(foo1, foo3);

        Assert.NotSame(bar1, bar3);
        Assert.NotSame(bar1.Foo, bar3.Foo);

        Assert.NotSame(baz1, baz3);
        Assert.NotSame(baz1.Foo, baz3.Foo);
        Assert.NotSame(baz1.Bar, baz3.Bar);
    }

    [Fact]
    public void Resolve_Without_Registration()
    {
        //Arrange
        var expectedMessage = "No service for type EasyDI.Tests.IFoo has been registered";

        //Act and Assert
        var ex = Assert.Throws<InvalidOperationException>(() => Container
            .Resolve<IFoo>());

        Assert.Equal(ex.Message, expectedMessage);
    }
}
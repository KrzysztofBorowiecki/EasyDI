namespace EasyDI.Tests;

public class SingletonTests : ContainerFixture
{
    [Fact]
    public void RegisterByInterfaceAndImplementationAsParameters_ShouldReturnSameInstanceOnEachResolve()
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
        Assert.Same(bar1.Foo, foo);

        Assert.IsAssignableFrom<Baz>(baz1);
        Assert.Same(baz, baz1);
        Assert.Same(baz1, baz2);
        Assert.IsAssignableFrom<Foo>(baz1.Foo);
        Assert.Same(baz1.Foo, baz2.Foo);
        Assert.Same(baz1.Foo, foo);
        Assert.IsAssignableFrom<Bar>(baz1.Bar);
        Assert.Same(baz1.Bar, baz2.Bar);
        Assert.Same(baz1.Bar, bar);
    }
    
    [Fact]
    public void RegisterBySelfTypeAndImplementationAsParameters_ShouldReturnSameInstanceOnEachResolve()
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
    public void RegisterByInterfaceAndImplementationTypeAsParameters_ShouldReturnSameInstanceOnEachResolve()
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
    public void RegisterBySelfTypeAndImplementationTypeAsParameters_ShouldReturnSameInstanceOnEachResolve()
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
    
    [Fact]
    public void RegisterByInterfaceAsParameters_ShouldThrowArgumentException_WhenNoImplementationProvided()
    {
        //Arrange
        var expectedMessage = "Cannot instantiate implementation type EasyDI.Tests.IFoo because it is an interface or abstract class.";

        //Act and Assert
        var ex = Assert.Throws<ArgumentException>(() => Container
            .AttachSingleton(typeof(IFoo)));

        Assert.Equal(ex.Message, expectedMessage);
    }

    [Fact]
    public void RegisterBySelfTypeAsParameters_ShouldReturnSameInstanceOnEachResolve()
    {
        //Arrange
        Container
            .AttachSingleton(typeof(Foo))
            .AttachSingleton(typeof(IFoo), typeof(Foo))
            .AttachSingleton(typeof(Bar))
            .AttachSingleton(typeof(IBar), typeof(Bar))
            .AttachSingleton(typeof(Baz));

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
    }

    [Fact]
    public void
        RegisterByInterfaceAndFactoryAsParameters_ShouldReturnSameInstanceOnEachResolve()
    {
        //Arrange
        var foo = new Foo();
        var bar = new Bar(foo);
        var baz = new Baz(foo, bar);

        Func<Foo> fooFactory = () => foo;
        Func<Bar> barFactory = () => bar;
        Func<Baz> bazFactory = () => baz;

        Container
            .AttachSingleton(typeof(IFoo), fooFactory)
            .AttachSingleton(typeof(IBar), barFactory)
            .AttachSingleton(typeof(IBaz), bazFactory);

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
    public void RegisterByInterfaceAndFactoryAsParameters_WithNullImplementation_ShouldThrowInvalidOperationException()
    {
        //Arrange
        var expectedMessage = "No service for type EasyDI.Tests.IFoo has been registered";
        Func<Foo> fooFactory = () => null;
        Func<Bar> barFactory = () => null;
        Func<Baz> bazFactory = () => null;

        //Act
        Container
            .AttachSingleton(typeof(IFoo), fooFactory)
            .AttachSingleton(typeof(IBar), barFactory)
            .AttachSingleton(typeof(IBaz), bazFactory);

        //Assert
        var ex = Assert.Throws<InvalidOperationException>(() => Container.Resolve<IFoo>());
        Assert.Equal(ex.Message, expectedMessage);
    }

    [Fact]
    public void
        RegisterBySelfTypeAndFactoryAsParameters_ShouldReturnSameInstanceOnEachResolve()
    {
        //Arrange
        var foo = new Foo();
        var bar = new Bar(foo);
        var baz = new Baz(foo, bar);

        Func<Foo> fooFactory = () => foo;
        Func<Bar> barFactory = () => bar;
        Func<Baz> bazFactory = () => baz;

        Container
            .AttachSingleton(typeof(Foo), fooFactory)
            .AttachSingleton(typeof(Bar), barFactory)
            .AttachSingleton(typeof(Baz), bazFactory);

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
    public void RegisterBySelfAndFactoryWithNullImplementationAsParameters_ShouldThrowInvalidOperationException()
    {
        //Arrange
        var message = "No service for type EasyDI.Tests.IFoo has been registered";
        Func<Foo> fooFactory = () => null;
        Func<Bar> barFactory = () => null;
        Func<Baz> bazFactory = () => null;

        //Act
        Container
            .AttachSingleton(typeof(IFoo), fooFactory)
            .AttachSingleton(typeof(IBar), barFactory)
            .AttachSingleton(typeof(IBaz), bazFactory);

        //Assert
        var ex = Assert.Throws<InvalidOperationException>(() => Container.Resolve<IFoo>());
        Assert.Equal(ex.Message, message);
    }
    
    [Fact]
    public void RegisterByInterfaceAndImplementationTypeProvidedAsGenerics_ShouldReturnSameInstanceOnEachResolve()
    {
        //Arrange
        Container
            .AttachSingleton<IFoo, Foo>()
            .AttachSingleton<IBar, Bar>()
            .AttachSingleton<IBaz, Baz>();

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
    public void
        RegisterBySelfTypeAndImplementationTypeProvidedAsGenerics_ShouldReturnSameInstanceOnEachResolve()
    {
        //Arrange
        Container
            .AttachSingleton<Foo, Foo>()
            .AttachSingleton<IFoo, Foo>()
            .AttachSingleton<Bar, Bar>()
            .AttachSingleton<IBar, Bar>()
            .AttachSingleton<Baz, Baz>();

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

    [Fact]
    public void
        RegisterByInterfaceAndImplementationTypeProvidedAsGenericsAndParameter_ShouldReturnSameInstanceOnEachResolve()
    {
        //Arrange
        var foo = new Foo();
        var bar = new Bar(foo);
        var baz = new Baz(foo, bar);

        Func<Foo> fooFactory = () => foo;
        Func<Bar> barFactory = () => bar;
        Func<Baz> bazFactory = () => baz;

        Container
            .AttachSingleton<IFoo, Foo>(fooFactory)
            .AttachSingleton<IBar, Bar>(barFactory)
            .AttachSingleton<IBaz, Baz>(bazFactory);

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
    public void
        RegisterBySelfTypeAndImplementationTypeProvidedAsGenericsAndParameter_ShouldReturnSameInstanceOnEachResolve()
    {
        //Arrange
        var foo = new Foo();
        var bar = new Bar(foo);
        var baz = new Baz(foo, bar);

        Func<Foo> fooFactory = () => foo;
        Func<Bar> barFactory = () => bar;
        Func<Baz> bazFactory = () => baz;

        Container
            .AttachSingleton<Foo, Foo>(fooFactory)
            .AttachSingleton<Bar, Bar>(barFactory)
            .AttachSingleton<Baz, Baz>(bazFactory);

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
    public void RegisterByInterfaceProvidedAsGenerics_ShouldThrowArgumentExceptionWhenRegisteringAbstractOrInterfaceType()
    {
        var expectedMessage = "Cannot instantiate implementation type EasyDI.Tests.IFoo because it is an interface or abstract class.";

        //Act and Assert
        var ex = Assert.Throws<ArgumentException>(() => Container
            .AttachSingleton<IFoo>());

        Assert.Equal(ex.Message, expectedMessage);
    }

    [Fact]
    public void BySelfTypeProvidedAsGenerics_ShouldReturnSameInstanceOnEachResolve()
    {
        //Arrange
        Container
            .AttachSingleton<Foo>()
            .AttachSingleton<IFoo, Foo>()
            .AttachSingleton<Bar>()
            .AttachSingleton<IBar, Bar>()
            .AttachSingleton<Baz>();

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

    [Fact]
    public void
        RegisterByInterfaceAndFactoryWithImplementationProvidedAsGenericsAndParameter_ShouldReturnSameInstanceOnEachResolve()
    {
        //Arrange
        var foo = new Foo();
        var bar = new Bar(foo);
        var baz = new Baz(foo, bar);

        Func<Foo> fooFactory = () => foo;
        Func<Bar> barFactory = () => bar;
        Func<Baz> bazFactory = () => baz;

        Container
            .AttachSingleton<IFoo>(fooFactory)
            .AttachSingleton<IBar>(barFactory)
            .AttachSingleton<IBaz>(bazFactory);

        //Act
        var foo1 = Container.Resolve<IFoo>();
        var foo2 = Container.Resolve<IFoo>();
        var bar1 = Container.Resolve<IBar>();
        var bar2 = Container.Resolve<IBar>();
        var baz1 = Container.Resolve<IBaz>();
        var baz2 = Container.Resolve<IBaz>();

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
    public void RegisterByInterfaceAndFactoryWithNullImplementationProvidedAsGenericsAndParameter_ShouldThrowInvalidOperationException()
    {
        //Arrange
        var excpectedMessage = "No service for type EasyDI.Tests.IFoo has been registered";
        Func<Foo> fooFactory = () => null;
        Func<Bar> barFactory = () => null;
        Func<Baz> bazFactory = () => null;

        //Act
        Container
            .AttachSingleton<IFoo>(fooFactory)
            .AttachSingleton<IBar>(barFactory)
            .AttachSingleton<IBaz>(bazFactory);

        //Assert
        var ex = Assert.Throws<InvalidOperationException>(() => Container.Resolve<IFoo>());
        
        Assert.Equal(ex.Message, excpectedMessage);
    }

    [Fact]
    public void
        RegisterBySelfTypeAndFactoryWithImplementationProvidedAsGenericsAndParameter_ShouldReturnSameInstanceOnEachResolve()
    {
        //Arrange
        var foo = new Foo();
        var bar = new Bar(foo);
        var baz = new Baz(foo, bar);

        Func<Foo> fooFactory = () => foo;
        Func<Bar> barFactory = () => bar;
        Func<Baz> bazFactory = () => baz;

        Container
            .AttachSingleton<Foo>(fooFactory)
            .AttachSingleton<Bar>(barFactory)
            .AttachSingleton<Baz>(bazFactory);

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
    public void RegisterBySelfAndFactoryWithNullImplementationProvidedAsGenericsAndParameter_ShouldThrowInvalidOperationException()
    {
        //Arrange
        var expectedMessage = "No service for type EasyDI.Tests.IFoo has been registered";

        Func<Foo> fooFactory = () => null;
        Func<Bar> barFactory = () => null;
        Func<Baz> bazFactory = () => null;

        //Act
        Container
            .AttachSingleton<Foo>(fooFactory)
            .AttachSingleton<Bar>(barFactory)
            .AttachSingleton<Baz>(bazFactory);

        //Assert
        var ex = Assert.Throws<InvalidOperationException>(() => Container.Resolve<IFoo>());
        
        Assert.Equal(ex.Message, expectedMessage);
    }
    
    [Fact]
    public void RegisterByMixedMethodDefinitions_ShouldReturnSameInstanceOnEachResolve()
    {
        //Arrange
        Container
            .AttachSingleton(typeof(IFoo), typeof(Foo))
            .AttachSingleton(typeof(Bar))
            .AttachSingleton<IBar, Bar>()
            .AttachSingleton<IBaz, Baz>();

        //Act
        var foo1 = Container.Resolve<IFoo>();
        var foo2 = Container.Resolve<IFoo>();
        var bar1 = Container.Resolve<Bar>();
        var bar2 = Container.Resolve<Bar>();
        var baz1 = Container.Resolve<IBaz>();
        var baz2 = Container.Resolve<IBaz>();

        //Assert
        Assert.IsAssignableFrom<Foo>(foo1);
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


    [Fact]
    public void RegisterByInterfaceAndImplementationTypeProvidedAsGenericsTheSameRegistrationTwice_ShouldReturnSameInstanceOnEachResolve()
    {
        //Arrange
        Container
            .AttachSingleton<IFoo, Foo>()
            .AttachSingleton<IFoo, Foo>()
            .AttachSingleton<IBar, Bar>()
            .AttachSingleton<IBar, Bar>()
            .AttachSingleton<IBaz, Baz>()
            .AttachSingleton<IBaz, Baz>();

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
    public void ResolveWithoutRegistration_ShouldThrowInvalidOperationException()
    {
        //Arrange
        var expectedMessage = "No service for type EasyDI.Tests.IFoo has been registered";

        //Act and Assert
        var ex = Assert.Throws<InvalidOperationException>(() => Container
            .Resolve<IFoo>());

        Assert.Equal(ex.Message, expectedMessage);
    }
}
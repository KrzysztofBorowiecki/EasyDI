namespace EasyDI.Tests;

public class ScopedTests : ContainerFixture
{
    [Fact]
    public void AttachScoped_ByInterfaceProvidesAsParameter_ShouldThrowArgumentExceptionWhenTypeIsInterface()
    {
        //Arrange
        var expectedMessage = "Cannot instantiate implementation type EasyDI.Tests.IFoo because it is an interface or abstract class.";

        //Act and Assert
        var ex = Assert.Throws<ArgumentException>(() => Container
            .AttachScoped(typeof(IFoo)));

        Assert.Equal(ex.Message, expectedMessage);
    }

    [Fact]
    public void AttachScoped_BySelfProvidesAsParameter_ShouldResolveConsistentInstanceWithinScopeAndUniqueAcrossScopes()
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
    public void AttachScoped_ByInterfaceAndFactoryWithImplementationProvidedAsParameters_ShouldResolveConsistentInstanceWithinScopeAndUniqueAcrossScopes()
    {
        //Arrange
        Func<Foo> fooFactory = () => new Foo();
        Func<Bar> barFactory = () => new Bar(new Foo());
        Func<Baz> bazFactory = () => new Baz(new Foo(), new Bar(new Foo()));

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

        Assert.IsAssignableFrom<Baz>(baz1);
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
    public void AttachScoped_ByInterfaceAndImplementationProvidedAsParameters_ShouldResolveConsistentInstanceWithinScopeAndUniqueAcrossScopes()
    {
        //Arrange
        Container
            .AttachScoped(typeof(IFoo), new Foo())
            .AttachScoped(typeof(IBar), new Bar(new Foo()))
            .AttachScoped(typeof(IBaz), new Baz(new Foo(), new Bar(new Foo())));

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

        Assert.IsAssignableFrom<Baz>(baz1);
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
    public void AttachScoped_BySelfAndImplementationProvidedAsParameters_ShouldResolveConsistentInstanceWithinScopeAndUniqueAcrossScopes()
    {
        //Arrange
        Container
            .AttachScoped(typeof(IFoo), new Foo())
            .AttachScoped(typeof(IBar), new Bar(new Foo()))
            .AttachScoped(typeof(IBaz), new Baz(new Foo(), new Bar(new Foo())));

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

        Assert.IsAssignableFrom<Baz>(baz1);
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
    public void AttachScoped_ByInterfaceAndFactoryAsParameter_ShouldThrowInvalidOperationExceptionWhenFactoryReturnsNull()
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
    public void AttachScoped_BySelfTypeAndFactoryWithImplementationProvidedAsParameters_ShouldResolveConsistentInstanceWithinScopeAndUniqueAcrossScopes()
    {
        //Arrange
        Func<Foo> fooFactory = () => new Foo();
        Func<Bar> barFactory = () => new Bar(new Foo());
        Func<Baz> bazFactory = () => new Baz(new Foo(), new Bar(new Foo()));

        Container
            .AttachScoped(typeof(Foo), fooFactory)
            .AttachScoped(typeof(IFoo), fooFactory)
            .AttachScoped(typeof(Bar), barFactory)
            .AttachScoped(typeof(IBar), barFactory)
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
    public void AttachScoped_BySelfAndFactoryAsParameters_ShouldThrowInvalidOperationExceptionWhenFactoryReturnsNull()
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
    public void AttachScoped_ByInterfaceAndTypeAsParameters_ShouldResolveConsistentInstanceWithinScopeAndUniqueAcrossScopes()
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
    public void AttachScoped_BySelfTypeAndTypeOfImplementationProvidedAsParameters_ShouldResolveConsistentInstanceWithinScopeAndUniqueAcrossScopes()
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
    public void AttachScoped_ByInterfaceAndImplementationTypeProvidedAsGenerics_ShouldResolveConsistentInstanceWithinScopeAndUniqueAcrossScopes()
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
    public void AttachScoped_BySelfTypeAndImplementationTypeProvidedAsGenerics_ShouldResolveConsistentInstanceWithinScopeAndUniqueAcrossScopes()
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
    public void AttachScoped_ByInterfaceAndImplementationTypeProvidedAsGenericsAndParameter_ShouldResolveConsistentInstanceWithinScopeAndUniqueAcrossScopes()
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
    public void AttachScoped_BySelfTypeAndImplementationTypeProvidedAsGenericsAndParameter_ShouldResolveConsistentInstanceWithinScopeAndUniqueAcrossScopes()
    {
        //Arrange
        Func<Foo> fooFactory = () => new Foo();
        Func<Bar> barFactory = () => new Bar(new Foo());
        Func<Baz> bazFactory = () => new Baz(new Foo(), new Bar(new Foo()));

        Container
            .AttachScoped<Foo, Foo>(fooFactory)
            .AttachScoped<IFoo, Foo>(fooFactory)
            .AttachScoped<Bar, Bar>(barFactory)
            .AttachScoped<IBar, Bar>(barFactory)
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
    public void AttachScoped_ByInterfaceAsGenerics_ShouldThrowArgumentExceptionWhenResolvingInterface()
    {
        var expectedMessage =
            "Cannot instantiate implementation type EasyDI.Tests.IFoo because it is an interface or abstract class.";

        //Act and Assert
        var ex = Assert.Throws<ArgumentException>(() => Container
            .AttachScoped<IFoo>());

        Assert.Equal(ex.Message, expectedMessage);
    }

    [Fact]
    public void AttachScoped_BySelfTypeProvidedAsGenerics_ShouldResolveConsistentInstanceWithinScopeAndUniqueAcrossScopes()
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
    public void AttachScoped_ByInterfaceAndFactoryWithImplementationProvidedAsGenericsAndParameter_ShouldResolveConsistentInstanceWithinScopeAndUniqueAcrossScopes()
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
    public void AttachScoped_ByInterfaceAndFactoryWithNullImplementation_ShouldThrowExceptionWhenResolving()
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
    public void AttachScoped_BySelfTypeAndFactoryWithImplementationProvidedAsGenericsAndParameter_ShouldResolveConsistentInstanceWithinScopeAndUniqueAcrossScopes()
    {
        //Arrange
        Func<Foo> fooFactory = () => new Foo();
        Func<Bar> barFactory = () => new Bar(new Foo());
        Func<Baz> bazFactory = () => new Baz(new Foo(), new Bar(new Foo()));

        Container
            .AttachScoped<Foo>(fooFactory)
            .AttachScoped<IFoo>(fooFactory)
            .AttachScoped<Bar>(barFactory)
            .AttachScoped<IBar>(barFactory)
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
    public void AttachScoped_BySelfAndFactoryWithNullImplementation_ShouldThrowExceptionWhenResolving()
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
    public void AttachScoped_ByMixedMethodDefinitions_ShouldResolveConsistentInstanceWithinScopeAndUniqueAcrossScopes()
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
    public void AttachScoped_ByInterfaceAndImplementationType_ShouldResolveSameInstanceTwiceAndUniqueAcrossScopes()
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
    public void Resolve_ShouldThrowException_WhenServiceIsNotRegistered()
    {
        //Arrange
        var expectedMessage = "No service for type EasyDI.Tests.IFoo has been registered";

        //Act and Assert
        var ex = Assert.Throws<InvalidOperationException>(() => Container
            .Resolve<IFoo>());

        Assert.Equal(ex.Message, expectedMessage);
    }
}
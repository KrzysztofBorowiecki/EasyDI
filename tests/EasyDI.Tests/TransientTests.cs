namespace EasyDI.Tests;

public class TransientTests : ContainerFixture
{
    [Fact]
    public void AttachTransient_ByInterfaceProvidesAsParameter_ShouldThrowArgumentExceptionWhenTypeIsInterface()
    {
        //Arrange
        var expectedMessage = "Cannot instantiate implementation type EasyDI.Tests.IFoo because it is an interface or abstract class.";

        //Act and Assert
        var ex = Assert.Throws<ArgumentException>(() => Container.AttachTransient(typeof(IFoo)));

        Assert.Equal(ex.Message, expectedMessage);
    }

    [Fact]
    public void AttachTransient_BySelfAndTypeAsParameters_ShouldResolveNewInstanceEachTime()
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
        Assert.NotSame(bar1.Foo, bar2.Foo);
        Assert.IsType<Foo>(bar1.Foo);
        Assert.IsType<Baz>(baz1);
        Assert.NotSame(baz1.Foo, baz2.Foo);
        Assert.IsType<Foo>(bar1.Foo);
        Assert.NotSame(baz1.Bar, baz2.Bar);
        Assert.IsType<Bar>(baz1.Bar);
        Assert.NotSame(baz1, baz2);
    }

    [Fact]
    public void AttachTransient_ByInterfaceAndImplementationTypeAsParameters_ShouldResolveNewInstanceEachTime()
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
        Assert.NotSame(bar1.Foo, bar2.Foo);
        Assert.IsAssignableFrom<Foo>(bar1.Foo);
        Assert.IsAssignableFrom<Baz>(baz1);
        Assert.NotSame(baz1.Foo, baz2.Foo);
        Assert.IsAssignableFrom<Foo>(bar1.Foo);
        Assert.NotSame(baz1.Bar, baz2.Bar);
        Assert.IsAssignableFrom<Bar>(baz1.Bar);
        Assert.NotSame(baz1, baz2);
    }

    [Fact]
    public void AttachTransient_BySelfTypeAndTypeOfImplementationProvidedAsParameters_ShouldResolveNewInstanceEachTime()
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
        Assert.NotSame(bar1.Foo, bar2.Foo);
        Assert.IsAssignableFrom<Foo>(bar1.Foo);
        Assert.IsType<Baz>(baz1);
        Assert.NotSame(baz1.Foo, baz2.Foo);
        Assert.IsAssignableFrom<Foo>(bar1.Foo);
        Assert.NotSame(baz1.Bar, baz2.Bar);
        Assert.IsAssignableFrom<Bar>(baz1.Bar);
        Assert.NotSame(baz1, baz2);
    }

    [Fact]
    public void AttachTransient_ByInterfaceAndFactoryWithImplementation_ShouldResolveNewInstanceOnEachCall()
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
        Assert.NotSame(baz1.Bar, baz2.Bar);
        Assert.IsAssignableFrom<Bar>(baz1.Bar);
        Assert.NotSame(baz1, baz2);
    }

    [Fact]
    public void AttachTransient_ByInterfaceAndFactoryWithNullImplementation_ShouldThrowInvalidOperationException()
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
    public void AttachTransient_BySelfTypeAndFactoryWithImplementation_ShouldResolveNewInstanceOnEachCall()
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
        Assert.NotSame(baz1.Bar, baz2.Bar);
        Assert.IsType<Bar>(baz1.Bar);
        Assert.NotSame(baz1, baz2);
    }

    [Fact]
    public void AttachTransient_BySelfAndFactoryWithNullImplementation_ShouldThrowExceptionOnResolve()
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
    public void AttachTransient_ByInterfaceAndImplementationTypeAsGenerics_ShouldResolveNewInstanceOnEachCall()
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
        Assert.NotSame(bar1.Foo, bar2.Foo);
        Assert.IsAssignableFrom<Foo>(bar1.Foo);
        Assert.IsAssignableFrom<Baz>(baz1);
        Assert.NotSame(baz1.Foo, baz2.Foo);
        Assert.IsAssignableFrom<Foo>(bar1.Foo);
        Assert.NotSame(baz1.Bar, baz2.Bar);
        Assert.IsAssignableFrom<Bar>(baz1.Bar);
        Assert.NotSame(baz1, baz2);
    }

    [Fact]
    public void AttachTransient_BySelfTypeAndImplementationTypeAsGenerics_ShouldResolveNewInstanceOnEachCall()
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
        Assert.NotSame(bar1.Foo, bar2.Foo);
        Assert.IsType<Foo>(bar1.Foo);
        Assert.IsType<Baz>(baz1);
        Assert.NotSame(baz1.Foo, baz2.Foo);
        Assert.IsType<Foo>(baz1.Foo);
        Assert.NotSame(baz1.Bar, baz2.Bar);
        Assert.IsType<Bar>(baz1.Bar);
        Assert.NotSame(baz1, baz2);
    }

    [Fact]
    public void AttachTransient_ByInterfaceAndImplementationTypeAsGenericsAndParameter_ShouldResolveNewInstanceOnEachCall()
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
        Assert.NotSame(baz1.Bar, baz2.Bar);
        Assert.IsAssignableFrom<Bar>(baz1.Bar);
        Assert.NotSame(baz1, baz2);
    }

    [Fact]
    public void AttachTransient_BySelfTypeAndImplementationTypeAsGenericsAndParameter_ShouldResolveNewInstanceOnEachCall()
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
        Assert.NotSame(bar1.Foo, bar2.Foo);
        Assert.IsType<Foo>(bar1.Foo);
        Assert.IsType<Baz>(baz1);
        Assert.NotSame(baz1.Foo, baz2.Foo);
        Assert.IsType<Foo>(bar1.Foo);
        Assert.NotSame(baz1.Bar, baz2.Bar);
        Assert.IsType<Bar>(baz1.Bar);
        Assert.NotSame(baz1, baz2);
    }

    [Fact]
    public void AttachTransient_ByInterfaceProvidedAsGenerics_ShouldThrowArgumentExceptionForAbstractOrInterfaceTypes()
    {
        var expectedMessage =
            "Cannot instantiate implementation type EasyDI.Tests.IFoo because it is an interface or abstract class.";

        //Act and Assert
        var ex = Assert.Throws<ArgumentException>(() => Container.AttachTransient<IFoo>());

        Assert.Equal(ex.Message, expectedMessage);
    }

    [Fact]
    public void AttachTransient_BySelfTypeProvidedAsGenerics_ShouldResolveNewInstanceOnEachCall()
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
        Assert.NotSame(bar1.Foo, bar2.Foo);
        Assert.IsType<Foo>(bar1.Foo);
        Assert.IsType<Baz>(baz1);
        Assert.NotSame(baz1.Foo, baz2.Foo);
        Assert.IsType<Foo>(baz1.Foo);
        Assert.NotSame(baz1.Bar, baz2.Bar);
        Assert.IsType<Bar>(baz1.Bar);
        Assert.NotSame(baz1, baz2);
    }

    [Fact]
    public void AttachTransient_ByInterfaceAndFactoryWithImplementationProvidedAsGenericsAndParameter_ShouldResolveNewInstanceOnEachCall()
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
        Assert.NotSame(baz1.Bar, baz2.Bar);
        Assert.IsAssignableFrom<Bar>(baz1.Bar);
        Assert.NotSame(baz1, baz2);
    }

    [Fact]
    public void AttachTransient_ByInterfaceAndFactoryWithNullImplementationProvidedAsGenericsAndParameter_ShouldThrowInvalidOperationException()
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
    public void AttachTransient_BySelfTypeAndFactoryWithImplementationProvidedAsGenericsAndParameter_ShouldResolveNewInstanceOnEachCall()
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
        Assert.NotSame(baz1.Bar, baz2.Bar);
        Assert.IsType<Bar>(baz1.Bar);
        Assert.NotSame(baz1, baz2);
    }

    [Fact]
    public void AttachTransient_BySelfAndFactoryWithNullImplementationProvidedAsGenericsAndParameter_ShouldThrowInvalidOperationException()
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
    public void AttachTransient_ByMixedMethodDefinitions_ShouldResolveNewInstanceOnEachCall()
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
        Assert.NotSame(bar1.Foo, bar2.Foo);
        Assert.IsType<Foo>(bar1.Foo);
        Assert.IsType<Baz>(baz1);
        Assert.NotSame(baz1.Foo, baz2.Foo);
        Assert.IsType<Foo>(baz1.Foo);
        Assert.NotSame(baz1.Bar, baz2.Bar);
        Assert.IsType<Bar>(baz1.Bar);
        Assert.NotSame(baz1, baz2);
    }

    [Fact]
    public void AttachTransient_ByInterfaceAndImplementationTypeProvidedAsGenericsTheSameRegistrationTwice_ShouldResolveNewInstanceOnEachCall()
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
        Assert.NotSame(bar1.Foo, bar2.Foo);
        Assert.IsAssignableFrom<Foo>(bar1.Foo);
        Assert.IsAssignableFrom<Baz>(baz1);
        Assert.NotSame(baz1.Foo, baz2.Foo);
        Assert.IsAssignableFrom<Foo>(bar1.Foo);
        Assert.NotSame(baz1.Bar, baz2.Bar);
        Assert.IsAssignableFrom<Bar>(baz1.Bar);
        Assert.NotSame(baz1, baz2);
    }


    [Fact]
    public void Resolve_WithoutRegistration_ShouldThrowInvalidOperationException()
    {
        //Arrange
        var expectedMessage = "No service for type EasyDI.Tests.IFoo has been registered";

        //Act and Assert
        var ex = Assert.Throws<InvalidOperationException>(() => Container.Resolve<IFoo>());

        Assert.Equal(ex.Message, expectedMessage);
    }
}
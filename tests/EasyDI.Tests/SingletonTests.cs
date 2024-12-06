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
    
    [Fact]
    public void By_Interface_Provides_As_Parameter()
    {
        //Arrange
        var expectedMessage = "Cannot instantiate implementation type 'EasyDI.Tests.IFoo' because it is an interface or abstract class.";

        //Act and Assert
        var ex = Assert.Throws<ArgumentException>(() => Container
            .AttachSingleton(typeof(IFoo)));

        Assert.Equal(ex.Message, expectedMessage);
    }

    [Fact]
    public void By_Self_Provides_As_Parameter()
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
        By_Interface_And_Factory_With_Implementation_Provided_As_Parameters()
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
    public void By_Interface_And_Factory_With_Null_Implementation_Provided_As_Parameters()
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
        By_Self_Type_And_Factory_With_Implementation_Provided_As_Parameters()
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
    public void By_Self_And_Factory_With_Null_Implementation_Provided_As_Parameters()
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
    public void By_Interface_And_Implementation_Type_Provided_As_Generics()
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
        By_Self_Type_And_Implementation_Type_Provided_As_Generics()
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
        By_Interface_And_Implementation_Type_Provided_As_Generics_And_Parameter()
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
        By_Self_Type_And_Implementation_Type_Provided_As_Generics_And_Parameter()
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
    public void By_Interface_Provided_As_Generics()
    {
        var expectedMessage = "Cannot instantiate implementation type 'EasyDI.Tests.IFoo' because it is an interface or abstract class.";

        //Act and Assert
        var ex = Assert.Throws<ArgumentException>(() => Container
            .AttachSingleton<IFoo>());

        Assert.Equal(ex.Message, expectedMessage);
    }

    [Fact]
    public void By_Self_Type_Provided_As_Generics()
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
        By_Interface_And_Factory_With_Implementation_Provided_As_Generics_And_Parameter()
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
    public void By_Interface_And_Factory_With_Null_Implementation_Provided_As_Generics_And_Parameter()
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
        By_Self_Type_And_Factory_With_Implementation_Provided_As_Generics_And_Parameter()
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
    public void By_Self_And_Factory_With_Null_Implementation_Provided_As_Generics_And_Parameter()
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
    public void Mixed_Method_Definitions()
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
    public void By_Interface_And_Implementation_Type_Provided_As_Generics_The_Same_Registration_Twice()
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
    public void No_Registration()
    {
        //Arrange
        var expectedMessage = "No service for type EasyDI.Tests.IFoo has been registered";

        //Act and Assert
        var ex = Assert.Throws<InvalidOperationException>(() => Container
            .Resolve<IFoo>());

        Assert.Equal(ex.Message, expectedMessage);
    }
}
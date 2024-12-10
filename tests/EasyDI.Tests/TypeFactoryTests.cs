namespace EasyDI.Tests;

public class TypeFactoryTests : ContainerFixture
{
    [Fact]
    public void CreateFactory_ShouldThrowArgumentNullException_WhenImplementationTypeIsNull()
    {
        //Assert
        Assert.Throws<ArgumentNullException>(() => TypeFactory.CreateFactory(null, Container));
    }
    
    [Fact]
    public void CreateFactory_ShouldThrowInvalidOperationException_WhenTypeIsInterface()
    {
        //Assert
        Assert.Throws<ArgumentException>(() => TypeFactory.CreateFactory(typeof(IFoo), Container));
    }
    
    [Fact]
    public void CreateFactory_ShouldCreateInstance_ForSimpleType()
    {
        //Arrange and Act
        var factory = TypeFactory.CreateFactory(typeof(Foo), Container);
        var instance = factory();

        //Assert
        Assert.NotNull(instance);
        Assert.IsType<Foo>(instance);
    }
    
    [Fact]
    public void CreateFactory_ShouldCreateInstance_ForTypeWithSingleDependency()
    {
        //Arrange and Act
        Container.AttachSingleton(typeof(IFoo), typeof(Foo));
        var factory = TypeFactory.CreateFactory(typeof(Bar), Container);
        var instance = factory();

        //Assert
        Assert.NotNull(instance);
        Assert.IsType<Bar>(instance);
    }
    
    [Fact]
    public void CreateFactory_ShouldCreateInstance_ForTypeWithMultipleDependencies()
    {
        //Arrange and Act
        Container.AttachSingleton(typeof(IFoo), typeof(Foo));
        Container.AttachSingleton(typeof(IBar), typeof(Bar));
        var factory = TypeFactory.CreateFactory(typeof(Baz), Container);
        var instance = factory();

        //Assert
        Assert.NotNull(instance);
        Assert.IsType<Baz>(instance);
    }
    
    [Fact]
    public void CreateFactory_ShouldCreateInstance_ForTypeBySelf()
    {
        //Arrange and Act
        Container.AttachSingleton(typeof(IFoo), typeof(Foo));
        Container.AttachSingleton(typeof(IBar), typeof(Bar));
        var factoryFoo = TypeFactory.CreateFactory(typeof(Foo), Container);
        var factoryBar = TypeFactory.CreateFactory(typeof(Bar), Container);
        var factoryBaz = TypeFactory.CreateFactory(typeof(Baz), Container);

        var foo = factoryFoo();
        var bar = factoryBar();
        var baz = factoryBaz();

        //Assert
        Assert.IsType<Foo>(foo);
        Assert.IsType<Bar>(bar);
        Assert.IsType<Baz>(baz);
    }
    
    [Fact]
    public void CreateFactory_ShouldThrowInvalidOperationException_WhenTypeIsAbstract()
    {
        //Assert
        Assert.Throws<ArgumentException>(() => TypeFactory.CreateFactory(typeof(AbstractClass), Container));
    }
}
namespace EasyDI.Tests;

internal interface IFoo { }

internal class Foo : IFoo { }

internal interface IBar
{
    IFoo Foo { get; }
}

internal class Bar(IFoo foo) : IBar
{
    public IFoo Foo { get; } = foo;
}

internal interface IBaz
{
    IFoo Foo { get; }
    IBar Bar { get; }
}

internal class Baz(IFoo foo, IBar bar) : IBaz
{
    public IFoo Foo { get; } = foo;
    public IBar Bar { get; } = bar;
}

internal abstract class AbstractClass { }

namespace EasyDI.Tests;

internal interface IFoo { }

internal class Foo : IFoo { }

internal interface IBar
{
    IFoo Foo { get; set; }
}

internal class Bar(IFoo foo) : IBar
{
    public IFoo Foo { get; set; } = foo;
}

internal interface IBaz
{
    IFoo Foo { get; set; }
    IBar Bar { get; set; }
}

internal class Baz(IFoo foo, IBar bar) : IBaz
{
    public IFoo Foo { get; set; } = foo;
    public IBar Bar { get; set; } = bar;
}

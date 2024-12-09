![logo](/assets/EasyDI.logo.png)

[![Status](https://github.com/KrzysztofBorowiecki/EasyDI/actions/workflows/CI.yml/badge.svg)](https://github.com/KrzysztofBorowiecki/EasyDI/actions/workflows/CI.yml)

**EasyDI** provides a simple and lightweight (DI) dependency injection container for C#. EasyDI makes managing object dependencies easy with minimal setup, keeping your code clean and straightforward.

## Usage
#### Singleton
```c#

var container = new Container();
container
    .AttachSingleton<IFoo, Foo>()
    .AttachSingleton<IBar, Bar>();

var fooSingleton1 = container.Resolve<IFoo>();
var fooSingleton2 = container.Resolve<IFoo>();
var barSingleton1 = container.Resolve<IBar>();
var barSingleton2 = container.Resolve<IBar>();

Console.WriteLine(fooSingleton1.Id);     //49d701d3-86d3-49ea-942e-3eedd08eeaa0
Console.WriteLine(fooSingleton2.Id);     //49d701d3-86d3-49ea-942e-3eedd08eeaa0
Console.WriteLine(barSingleton1.Foo.Id); //49d701d3-86d3-49ea-942e-3eedd08eeaa0
Console.WriteLine(barSingleton2.Foo.Id); //49d701d3-86d3-49ea-942e-3eedd08eeaa0

Console.WriteLine(barSingleton1.Id);     //7a03024e-2a69-422c-8eff-89f9c1a812e1
Console.WriteLine(barSingleton2.Id);     //7a03024e-2a69-422c-8eff-89f9c1a812e1

```

#### Scoped

```c#

var container = new Container();
container
    .AttachScoped<IFoo, Foo>()
    .AttachScoped<IBar, Bar>();

var fooScoped1 = container.Resolve<IFoo>();
var fooScoped2 = container.Resolve<IFoo>();
var barScoped1 = container.Resolve<IBar>();
var barScoped2 = container.Resolve<IBar>();

using var scope = container.CreateScope();

var fooScoped3 = scope.Resolve<IFoo>();
var fooScoped4 = scope.Resolve<IFoo>();
var barScoped3 = scope.Resolve<IBar>();
var barScoped4 = scope.Resolve<IBar>();

Console.WriteLine(fooScoped1.Id);     //a8cfc009-afc1-4e1b-b7a6-ff884bc0c069
Console.WriteLine(fooScoped2.Id);     //a8cfc009-afc1-4e1b-b7a6-ff884bc0c069
Console.WriteLine(barScoped1.Foo.Id); //a8cfc009-afc1-4e1b-b7a6-ff884bc0c069
Console.WriteLine(barScoped2.Foo.Id); //a8cfc009-afc1-4e1b-b7a6-ff884bc0c069

Console.WriteLine(barScoped1.Id);     //d15acd4e-dcfe-4283-8e7c-580fc668e10d
Console.WriteLine(barScoped2.Id);     //d15acd4e-dcfe-4283-8e7c-580fc668e10d
Console.WriteLine(barScoped3.Id);     //32f3c692-03e8-4e73-8ce6-e8b14ec8b7a5
Console.WriteLine(barScoped4.Id);     //32f3c692-03e8-4e73-8ce6-e8b14ec8b7a5

Console.WriteLine(fooScoped3.Id);     //78a8eb45-4dbc-4a58-aac1-4304f09e4711
Console.WriteLine(fooScoped4.Id);     //78a8eb45-4dbc-4a58-aac1-4304f09e4711
Console.WriteLine(barScoped3.Foo.Id); //78a8eb45-4dbc-4a58-aac1-4304f09e4711
Console.WriteLine(barScoped4.Foo.Id); //78a8eb45-4dbc-4a58-aac1-4304f09e4711

```

#### Transient

```c#

var container = new Container();
container
    .AttachTransient<IFoo, Foo>()
    .AttachTransient<IBar, Bar>();

var fooTransient1 = container.Resolve<IFoo>();
var fooTransient2 = container.Resolve<IFoo>();
var barTransient1 = container.Resolve<IBar>();
var barTransient2 = container.Resolve<IBar>();

Console.WriteLine(fooTransient1.Id);     //f252b598-92b4-4993-bba0-650f5c0a648f
Console.WriteLine(fooTransient2.Id);     //95e2c651-38d0-4d42-97f5-392a965fcf5b

Console.WriteLine(barTransient1.Foo.Id); //446c797a-f75e-431c-bf0e-14778fbdbb61
Console.WriteLine(barTransient2.Foo.Id); //2de155c5-b840-4db0-97b2-bea247fccfde

Console.WriteLine(barTransient1.Id);     //9fd750a1-f6e4-4b9d-9362-20e0153a1c71
Console.WriteLine(barTransient2.Id);     //39ccb12d-36da-4f7b-836d-a28e15440f47

```

## Plans
- Introduce benchmark tests to evaluate performance, see [Issue #4](https://github.com/KrzysztofBorowiecki/EasyDI/issues/4)

## Contribution 
Contributions to the project are welcome and appreciated!

## License
This project is licensed under the terms of the MIT License.

![logo](/assets/EasyDI.logo.png)

[![Status](https://github.com/KrzysztofBorowiecki/EasyDI/actions/workflows/CI.yml/badge.svg)](https://github.com/KrzysztofBorowiecki/EasyDI/actions/workflows/CI.yml)

**EasyDI** provides a simple and lightweight (DI) dependency injection container for C#. EasyDI makes managing object dependencies easy with minimal setup, keeping your code clean and straightforward.

## Usage
#### Singleton
```c#

var container = new Container();
container
    .AttachSingleton<IFoo, Foo>();

var fooSingleton1 = container.Resolve<IFoo>();
var fooSingleton2 = container.Resolve<IFoo>();

Console.WriteLine(fooSingleton1.Id); //580d28db-8e8e-4c22-8ae3-2bdec6dc89cd
Console.WriteLine(fooSingleton2.Id); //580d28db-8e8e-4c22-8ae3-2bdec6dc89cd

```

#### Scoped

```c#

var container = new Container();
container
    .AttachScoped<IFoo, Foo>();

var fooScoped1 = container.Resolve<IFoo>();
var fooScoped2 = container.Resolve<IFoo>();

using var scope = container.CreateScope();

var fooScoped3 = scope.Resolve<IFoo>();
var fooScoped4 = scope.Resolve<IFoo>();

Console.WriteLine(fooScoped1.Id); //43ddd521-8cd3-42c4-b1aa-db110eff49ef
Console.WriteLine(fooScoped2.Id); //43ddd521-8cd3-42c4-b1aa-db110eff49ef
Console.WriteLine(fooScoped3.Id); //6f513ced-293c-4f74-aa23-0e8819dac53e
Console.WriteLine(fooScoped4.Id); //6f513ced-293c-4f74-aa23-0e8819dac53e

```

#### Transient

```c#

var container = new Container();
container
    .AttachTransient<IFoo, Foo>();

var fooTransient1 = container.Resolve<IFoo>();
var fooTransient2 = container.Resolve<IFoo>();

Console.WriteLine(fooTransient1.Id); //a3cbc0fb-9f58-4fc2-b353-119297a2f683
Console.WriteLine(fooTransient2.Id); //1cc42ae1-78f8-4062-aa4b-64d68e43def5

```

## Plans
- Introduce benchmark tests to evaluate performance, see [Issue #4](https://github.com/KrzysztofBorowiecki/EasyDI/issues/4)

## Contribution 
Contributions to the project are welcome and appreciated!

## License
This project is licensed under the terms of the MIT License.

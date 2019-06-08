# DotNetFunctional.Maybe

[![Build status](https://ci.appveyor.com/api/projects/status/jokc9aicecq0tvs1/branch/master?svg=true)](https://ci.appveyor.com/project/jotatoledo/maybe/branch/master)
[![NuGet](http://img.shields.io/nuget/v/DotNetFunctional.Maybe.svg?logo=nuget)](https://www.nuget.org/packages/DotNetFunctional.Maybe/)

An Option type monad for C# with LINQ support and rich fluent syntax.

## Installation

`DotNetFunctional.Maybe` can be installed using the NuGet command line or the NuGet Package Manager in Visual Studio.

```bash
PM> Install-Package DotNetFunctional.Maybe
```

## Example

First, you will need to add the following using statement:

```csharp
using DotNetFunctional.Maybe;
```

### Lift values

Both reference type and value type objects can be _lifted_ by using the `Maybe.Lift` static method:

```csharp
Maybe<string> maybeHello = Maybe.Lift("hello");
Maybe<string> maybeTen = Maybe.Lift(10);
```

### Computing using query expressions

```csharp
Maybe<string> maybeHello = Maybe.Lift("hello");
Maybe<string> maybeNothing = Maybe<string>.Nothing;

var maybeConcat = from hello in maybeHello
                  from nothing in maybeNothing
                  select hello + nothing;

if(!maybeConcat.HasValue)
{
    Console.WriteLine("One of the strings was bad, could not concat");
}
```

LINQ will short-circuit any expression if there is a `Maybe<T>.Nothing` at any point in the computation.

### Pattern matching

A pattern match like method is exposed:

```csharp
Maybe<string> maybeHello = Maybe.Lift("hello");
Maybe<string> maybeTen = Maybe<int>.Nothing;

// Yield "Hello matched"
string hello = maybeHello.Match(val => $"Hello matched!", string.Empty);
// Yields 10
int ten = maybeTen.Match(val => val, 10);
```
Transverse - a quick object mapper for .Net
======================================================
I'll publish the source code very soon on github.

[![Nuget downloads](https://img.shields.io/nuget/v/tinymapper.svg)](https://www.nuget.org/packages/StoneKit.TransverseMapper/)
[![License](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)

A tiny and quick object mapper for .Net v8.0+
Inspired by AutoMapper and TinyMapper, I'm trying to create a library named StoneKit which is based on dotnet core 8 features. Transverse is a part of that library and I'm aimed to make it the tiniest and fastest mapping library in dotnet.

## Performance Comparison

Right now it performs faster than AutoMapper and I'm trying to improve it constantly. There is a benchmark project in the source code that I'm developing to showcase the performance.

## Installation

Available on [nuget](https://www.nuget.org/packages/StoneKit.TransverseMapper/)

	PM> Install-Package StoneKit.TransverseMapper

## Getting Started

```csharp
Transverse.Bind<Person, PersonDto>();

var person = new Person
{
	Id = Guid.NewGuid(),
	FirstName = "John",
	LastName = "Doe"
};

var personDto = Transverse.Map<PersonDto>(person);
```

Ignore mapping source members and bind members with different names/types

```csharp
Transverse.Bind<Person, PersonDto>(config =>
{
	config.Ignore(x => x.Id);
	config.Ignore(x => x.Email);
	config.Bind(source => source.LastName, target => target.Surname);
	config.Bind(target => source.Emails, typeof(List<string>));
});

var person = new Person
{
	Id = Guid.NewGuid(),
	FirstName = "John",
	LastName = "Doe",
	Emails = new List<string>{"support@tinymapper.net", "MyEmail@tinymapper.net"}
};

var personDto = Transverse.Map<PersonDto>(person);
```

`Transverse` only supports dotnet core 8.0

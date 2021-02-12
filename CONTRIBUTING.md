# Contributing

Below are some guidelines for contributing to this project. They are mostly **suggestions** rather than rules.

Feel free to submit pull requests, referencing any relevant issues and listing a brief overview of the changes in the pull request.

## Commits

Commit messages should target around a 50 character long headline, with any addition information following two line breaks. [Fork](https://git-fork.com/) and [GitHub](https://github.com/) uses this format when you are writing commit messages.

## Gitmoji

[![Gitmoji](https://img.shields.io/badge/gitmoji-%20😜%20😍-FFDD67.svg)](https://gitmoji.carloscuesta.me)

This project follows commit guidelines specified by [Gitmoji](https://gitmoji.carloscuesta.me), with the exception of using the raw unicode character (✨) rather than a string (`:sparkles:`).

## Code Style

[![Visual Studio](https://img.shields.io/static/v1?label=&message=Visual%20Studio&color=5C2D91&logo=visual-studio)](https://visualstudio.microsoft.com/)
[![Visual Studio](https://img.shields.io/static/v1?label=&message=Visual%20Studio%20Code&color=007ACC&logo=visual-studio-code)](https://code.visualstudio.com/)

An `.editorconfig` is included in the project that will ensure that your IDE follows the projects code style. I recommend you use an IDE that supports `.editorconfig` files such as [Visual Studio](https://visualstudio.microsoft.com/).

### Spacing

Don't insert a space between method name and its opening parenthesis for method declaration or method calls.

```csharp
// Prefer:
private void Bar(int x)
{
    Foo();
}

// Over:
private void Bar (int x)
{
    Foo ();
}
```

Insert space after keywords in control flow statements (`for`, `foreach`, `using`, `while`, `if`, e.t.c).

```csharp
// Prefer:
for (int i; i < 10; i++)
{
}

// Over:
for(int i; i < 10; i++)
{
}
```

### Indentation

Indentation should be denoted using tabs rather than spaces.

```csharp
// Prefer:
  ⇥

// Over:
․․․․
```

### Code Blocks

New lines should be inserted before the opening parenthesis of method blocks and control flow statements.

```csharp
// Prefer:
if (condition)
{
    Console.WriteLine("Output");
}

// Over:
if (condition) {
    Console.WriteLine("Output");
}
```

Single-line code-blocks should have explicit parenthesis.

```csharp
// Prefer:
if (condition)
{
    Console.WriteLine("Output");
}

// Over:
if (condition)
    Console.WriteLine("Output");
```

### Naming

Use **predefined type names** over **framework type names** for **data types**.

```csharp
// Prefer:
string fieldA;
int fieldB;
bool fieldC;

// Over:
String fieldA;
Int32 fieldB;
Boolean fieldC;
```

Use **predefined type names** over **framework type names** for accessing a type's **static members**.

```csharp
// Prefer:
string.Join(", ", names);
int.Parse(input);

// Over:
String.Join(", ", names);
Int32.Parse(input);
```

**Interfaces** should be prefixed with `I`.

```csharp
// Prefer:
public interface IInterfaceName
{
}

// Over:
public interface InterfaceName
{
}
```

**Class** and **struct** names should be `PascalCase`.

```csharp
// Prefer:
public class ClassName
{
}

// Over:
public class className
{
}
```

**Methods** (instance and static) should be `PascalCase`.

```csharp
// Prefer:
public void Run();

// Over:
public void run();
```

**Properties** (instance and static) should be `PascalCase`.

```csharp
// Prefer:
public string PropertyA { get; set; }
public string PropertyB => PropertyA;

// Over:
public string propertyA { get; set; }
public string propertyB => propertyA;
```

**Fields** (instance and static) should be `camelCase`.

```csharp
// Prefer:
private string field;

// Over:
private string Field;
```

**Constants** should be `PascalCase`.

```csharp
// Prefer:
private const string ConstantValue;

// Over:
private const string CONSTANT_VALUE;
```

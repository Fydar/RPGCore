## Gitmoji

[![Gitmoji](https://img.shields.io/badge/gitmoji-%20😜%20😍-FFDD67.svg)](https://gitmoji.carloscuesta.me)

This project follows commit guidelines specified by [Gitmoji](https://gitmoji.carloscuesta.me), with the exception of using the raw unicode character (✨) rather than a string (`:sparkles:`). This is to increase compatibility with Git clients.

Commit messages should target around a 50 character long headline, with any addition information following two line breaks. [Fork](https://git-fork.com/) and [GitHub](https://github.com/) uses this format when you are writing commit messages.

Feel free to submit pull requests, referencing any relevant issues and listing a brief overview of the changes in the pull request.

## Code Style

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

Use predefined type names over framework type names.

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

Use predefined type names over framework names for accessing a type's static members.

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
```

**Class** and **struct** names should be `PascalCase`.

```csharp
// Prefer:
public class ClassName
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
private string ConstantValue;

// Over:
private const string CONSTANT_VALUE;
```

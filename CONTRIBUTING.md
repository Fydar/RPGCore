# Contributing

Below are some guidelines for contributing to this project. They are mostly **suggestions** rather than rules.

Feel free to submit pull requests, referencing any relevant issues and listing a brief overview of the changes in the pull request.

## Commits

Commit messages should target around a 50 character long headline, with any addition information following two line breaks. [Fork](https://git-fork.com/) and [GitHub](https://github.com/) use this format when you are writing commit messages.

```txt
🎨 Cleanup and formatting of code

Used automatic formatting tools to cleanup and unify code style.
```

### Gitmoji

[![Gitmoji](https://img.shields.io/badge/gitmoji-%20😜%20%20😍-FFDD67.svg)](https://gitmoji.carloscuesta.me)

This project follows commit guidelines specified by [Gitmoji](https://gitmoji.carloscuesta.me), with the exception of using the raw unicode character (✨) rather than a string (`:sparkles:`).

| |  Description |
|:-:|-|
| ✨ | Introduce new features |
| 🚧 | Work in progress |
| 🎨 | Improve structure / format of the code |
| ⚡ | Improve performance |
| 🐛 | Fix a bug |
| 🔥 | Remove code or files |
| 📝 | Add or update documentation |
| ✅ | Add or update tests |
| | [*find out more...*](https://gitmoji.carloscuesta.me) |

## Software Design Patterns

This project utilises software design patterns to create clean and flexible code.

### Fluent interface

Use a [fluent interface](https://en.wikipedia.org/wiki/Fluent_interface#C#) where appropriate. Fluent interfaces can be used to replace complex constructors.

```csharp
schema = ProjectManifestBuilder.Create()
    .UseFrameworkTypes()
    .UseType(typeof(SerializerBaseObject))
    .UseType(typeof(SerializerChildObject))
    .Build();
```

More complex factories can be used to construct more complex objects. Delegates can be invoked to allow nesting of factories.

```csharp
var worldEngine = new WorldEngineFactory()
    .UseEntity(EntityTypes.Unit, options =>
    {
        options.AddComponent<TransformComponent>();
        options.AddComponent<UnitComponent>();
    })
    .UseEntity(EntityTypes.TerrainChunk, options =>
    {
        options.AddComponent<TransformComponent>();
        options.AddComponent<UnitComponent>();
    })
    .Build();
```

### Service pattern

Use the "service" patterns to allow behaviours to be abstracted to alternate implementations.

```csharp
public interface ISomethingService
{
    // Insert method declarations here
}

public class StaticSomethingService : ISomethingService
{
    // Insert method implementations here
}

public class DynamicSomethingService : ISomethingService
{
    // Insert method implementations here
}
```

### Procedure pattern

The procedure pattern involves not making changes directly to an object; instead opting for authoring a "procedure" which is applied to an object.

Procedures that can be serialized can be transported over the network and be used as [RPCs](https://en.wikipedia.org/wiki/Remote_procedure_call). Procedures can be authored ahead-of-time and either applied or rejected.

<details>
  <summary><b>Procedure pattern "world" example</b></summary>


Let's say you have an object that you want to modify. In our case it's the `World` object (as defined below).

```csharp
public class World
{
    public List<Character> Characters { get; } = new();
}

public class Character
{
    public int Health { get; set; } = 100;
}
```

We can create a set of `IWorldProcedure`s that can be used to modify the `World` object and provide them with implementations.

```csharp
public interface IWorldProcedure
{
    void ApplyToWorld(World world);
}

public class CharacterCreateProcedure : IWorldProcedure
{
    public void ApplyToWorld(World world)
    {
        world.Characters.Add(new Character());
    }
}

public class CharacterTakeDamageProcedure : IWorldProcedure
{
    public int Index { get; set; } 
    public int Damage { get; set; } 

    public void ApplyToWorld(World world)
    {
        var character = world.Characters[Index];

        character.Health -= Damage;
    }
}
```

Finally, instead of applying changes directly to the `World` object, we can author `IWorldProcedure`s and use them to modify the `World`.

```csharp
var world = new World();

var procedures = new Queue<IWorldProcedure>();

procedures.Enqueue(new CharacterCreateProcedure());
procedures.Enqueue(new CharacterTakeDamageProcedure()
{
    Index = 0,
    Damage = 10
});

// Transport procedures across the network...

while (procedures.Count > 0)
{
    var procedure = procedures.Dequeue();

    procedure.ApplyToWorld(world);
}
```

</details>

## Code Hygiene

### Encapsulation

Avoid using public fields in favour of public properties, unless you are creating highly-optimized, SIMD-optimized, or byref fields.

```csharp
// Prefer:
public string Name { get; }

// Over:
public string name;
```

### Use `DebuggerBrowsable` to hide unnecessary fields

When a field is represented by a property it will appear twice in the debugger. Using `DebuggerBrowsable` to hide unnecessary fields makes the debugger easier to read.

```csharp
[DebuggerBrowsable(DebuggerBrowsableState.Never)]
private string name;

public string Name => name;
```

### Create unit and integration tests

Code should have as much unit test and integration test coverage as reasonably possible.

Unit and integration tests are created with [NUnit](https://nunit.org/).

```csharp
using NUnit.Framework;
using ProjectNamespace;

namespace ProjectNamespace.UnitTests
{
    [TestFixture(TestOf = typeof(ProjectClass))]
    public class ProjectClassShould
    {
        [Test, Parallelizable]
        public void PassTests()
        {
            Assert.Pass();
        }
    }
}
```

## Code Style

[![Visual Studio](https://img.shields.io/static/v1?label=&message=Visual%20Studio&color=5C2D91&logo=visual-studio)](https://visualstudio.microsoft.com/)
[![Visual Studio](https://img.shields.io/static/v1?label=&message=Visual%20Studio%20Code&color=007ACC&logo=visual-studio-code)](https://code.visualstudio.com/)
[![JetBrains Rider](https://img.shields.io/static/v1?label=&message=JetBrains%20Rider&color=000000&logo=rider)](https://www.jetbrains.com/rider/)

An `.editorconfig` is included in the project that will ensure that your IDE follows the projects code style. I recommend you use an IDE that supports `.editorconfig` files such as [Visual Studio](https://visualstudio.microsoft.com/).

<details>
  <summary><b>detailed breakdown of rules</b></summary>

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

Switch statement code blocks should be indentented once.

```csharp
// Prefer:
switch (value)
{
    case 1:
    {
        Console.WriteLine("one");
        break;
    }
}

// Over:
switch (value)
{
    case 1:
        {
            Console.WriteLine("one");
            break;
        }
}
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

Use `var` over explicit type names when the type isn't a framework type.

```csharp
// Prefer:
var stream = new MemoryStream();

// Over:
MemoryStream stream = new MemoryStream();
```

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

</details>

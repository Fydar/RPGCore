## Gitmoji

[![Gitmoji](https://img.shields.io/badge/gitmoji-%20😜%20😍-FFDD67.svg)](https://gitmoji.carloscuesta.me)

This project follows commit guidelines specified by [Gitmoji](https://gitmoji.carloscuesta.me), with the exception of using the raw unicode character (✨) rather than a string (`:sparkles:`). This is to increase compatibility with Git clients.

Commit messages should target around a 50 character long headline, with any addition information following two line breaks. [Fork](https://git-fork.com/) and [GitHub](https://github.com/) uses this format when you are writing commit messages.

Feel free to submit pull requests, referencing any relevant issues and listing a brief overview of the changes in the pull request.

## Code Style

An `.editorconfig` is included in the project that will ensure that your IDE follows the projects code style. I recommend you use an IDE that supports `.editorconfig` files such as [Visual Studio](https://visualstudio.microsoft.com/).

### Spacing

Insert space between method name and its opening parenthesis for method declaration and method calls.

```csharp
private void Foo ()
{
    Bar (4);
}

private void Bar (int x)
{
    Foo ();
}
```

Insert space after keywords in control flow statements (`for`, `foreach`, `using`, `while`, `if`, e.t.c).

```csharp
for (int i; i < 10; i++)
{
}
```

### Indentation

Indentation should be denoted using tabs rather than spaces. 

### New Line

New lines should be inserted before the opening parenthesis of method blocks and control flow statements.

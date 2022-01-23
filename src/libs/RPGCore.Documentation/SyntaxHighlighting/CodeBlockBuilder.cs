using System;
using System.Collections.Generic;

namespace RPGCore.Documentation.SyntaxHighlighting;

public class CodeBlockBuilder
{
	private readonly List<CodeSpan> currentLine;
	private readonly List<CodeSpan[]> lines;

	public int Indent { get; }
	public string Name { get; }

	public CodeBlockBuilder(int indent)
	{
		currentLine = new List<CodeSpan>();
		lines = new List<CodeSpan[]>();
		Name = "";
		Indent = indent;
	}

	public CodeBlockBuilder(string name, int indent)
	{
		currentLine = new List<CodeSpan>();
		lines = new List<CodeSpan[]>();
		Name = name;
		Indent = indent;
	}

	public CodeBlock Build()
	{
		if (currentLine.Count > 0)
		{
			bool empty = true;
			foreach (var span in currentLine)
			{
				empty &= string.IsNullOrWhiteSpace(span.Content);
			}
			if (!empty)
			{
				lines.Add(currentLine.ToArray());
				currentLine.Clear();
			}
		}

		return new CodeBlock(Name, lines.ToArray());
	}

	public override string ToString()
	{
		return Name;
	}

	public void Write(ReadOnlySpan<char> value)
	{
		Write(value, null, null);
	}

	public void Write(ReadOnlySpan<char> value, string? style)
	{
		Write(value, style, null);
	}

	public void Write(ReadOnlySpan<char> value, string? style, string? linkUrl)
	{
		while (true)
		{
			int index = value.IndexOf(Environment.NewLine);
			if (index == -1)
			{
				break;
			}
			var sliced = value[..index];
			currentLine.Add(new CodeSpan(sliced.ToString(), style, linkUrl));
			WriteNewline();
			value = value[(index + Environment.NewLine.Length)..];
		}
		currentLine.Add(new CodeSpan(value.ToString(), style, linkUrl));
	}

	public void RestartLine()
	{
		currentLine.Clear();
	}

	public void WriteNewline()
	{
		var addLine = currentLine.ToArray();
		currentLine.Clear();

		lines.Add(addLine);
	}
}

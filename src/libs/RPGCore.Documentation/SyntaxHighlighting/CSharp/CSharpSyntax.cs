using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RPGCore.Documentation.SyntaxHighlighting.CSharp;

public static class CSharpSyntax
{
	private static readonly CSharpCodeStyles styles;

	static CSharpSyntax()
	{
		styles = new CSharpCodeStyles();
	}

	public static List<CodeBlock> ToCodeBlocks(string script)
	{
		var references = new List<MetadataReference>();

		var domainAssemblies = Assembly.GetEntryAssembly()!.GetReferencedAssemblies();
		foreach (var assembly in domainAssemblies)
		{
			references.Add(MetadataReference.CreateFromFile(Assembly.Load(assembly).Location));
		}
		references.Add(MetadataReference.CreateFromFile(typeof(object).Assembly.Location));

		var thisApplicationReferences =
			DependencyContext.Default.CompileLibraries
			.SelectMany(cl => cl.ResolveReferencePaths())
			.Select(asm => MetadataReference.CreateFromFile(asm))
			.ToArray();

		var scriptTree = CSharpSyntaxTree.ParseText(script);
		var compilation = CSharpCompilation.Create("ExampleCode")
			.AddReferences(thisApplicationReferences)
			.AddReferences(references)
			.AddSyntaxTrees(scriptTree);

		var semanticModel = compilation.GetSemanticModel(scriptTree);

		var diagnostics = semanticModel.GetDiagnostics();

		var builders = new List<CodeBlockBuilder>();
		var output = new List<CodeBlock>();

		var root = scriptTree.GetRoot();
		bool isFollowingNewline = false;

		WriteRecursive(root);

		foreach (var builder in builders)
		{
			output.Add(builder.Build());
		}
		return output;

		void WriteRecursive(SyntaxNode rootNode)
		{
			foreach (var nodeOrToken in rootNode.ChildNodesAndTokens())
			{
				if (styles.TryGetSyntaxStyle(nodeOrToken.Kind(), out string? style))
				{
					var span = script.AsSpan(nodeOrToken.Span.Start, nodeOrToken.Span.Length);

					RenderTrivia(nodeOrToken.GetLeadingTrivia());

					if (style == null)
					{
						Write(span);
					}
					else
					{
						WriteStyled(span, style);
					}

					RenderTrivia(nodeOrToken.GetTrailingTrivia());
				}
				else if (nodeOrToken.IsNode)
				{
					var node = nodeOrToken.AsNode();

					WriteRecursive(node!);
				}
				else if (nodeOrToken.IsToken)
				{
					var token = nodeOrToken.AsToken();

					RenderTrivia(token.LeadingTrivia);

					var span = script.AsSpan(nodeOrToken.Span.Start, nodeOrToken.Span.Length);

					if (span.ToString() == "var")
					{
						WriteStyled(span, styles.Keyword);
					}
					else
					{
						var declaredSymbol = semanticModel.GetDeclaredSymbol(rootNode);
						if (declaredSymbol != null)
						{
							WriteSymbol(declaredSymbol, span);
						}
						else
						{
							var info = semanticModel.GetSymbolInfo(rootNode);
							if (info.Symbol != null)
							{
								WriteSymbol(info.Symbol, span);
							}
							else
							{
								var typeInfo = semanticModel.GetTypeInfo(rootNode);
								if (typeInfo.Type != null)
								{
									WriteSymbol(typeInfo.Type, span);
								}
								else
								{
									Write(span);
								}
							}
						}
					}

					RenderTrivia(token.TrailingTrivia);
				}
			}
		}

		void WriteSymbol(ISymbol symbol, ReadOnlySpan<char> span)
		{
			switch (symbol.Kind)
			{
				case SymbolKind.NamedType:
				{
					string style = styles.GetStyleForTypeSymbol(symbol as ITypeSymbol);

					WriteStyled(span, style);
					break;
				}
				case SymbolKind.Method:
				{
					string style = styles.Method;
					var methodSymbol = (IMethodSymbol)symbol;
					if (methodSymbol.MethodKind == MethodKind.Constructor)
					{
						var typeSymbol = methodSymbol.ContainingType;
						style = styles.GetStyleForTypeSymbol(typeSymbol);
					}

					WriteStyled(span, style);
					break;
				}
				case SymbolKind.Parameter:
				{
					WriteStyled(span, styles.Parameter);
					break;
				}
				case SymbolKind.Local:
				{
					WriteStyled(span, styles.Local);
					break;
				}
				case SymbolKind.Field:
				{
					WriteStyled(span, styles.Field);
					break;
				}
				case SymbolKind.Property:
				{
					WriteStyled(span, styles.Property);
					break;
				}
				case SymbolKind.TypeParameter:
				{
					WriteStyled(span, styles.TypeGeneric);
					break;
				}
				case SymbolKind.Namespace:
				{
					Write(span);
					break;
				}
				default:
				{
					Write(span);
					break;
				}
			}
		}

		void RenderTrivia(SyntaxTriviaList triviaList)
		{
			SyntaxTrivia? regionDirectiveStart = null;

			foreach (var trivia in triviaList)
			{
				var triviaKind = trivia.Kind();
				var triviaSpan = script.AsSpan(trivia.Span.Start, trivia.Span.Length);

				if (triviaKind == SyntaxKind.EndRegionDirectiveTrivia)
				{
					var regionSpan = triviaSpan.TrimStart("#endregion");
					regionSpan = regionSpan.Trim();

					foreach (var builder in builders)
					{
						if (builder.Name == regionSpan.ToString())
						{
							builders.Remove(builder);
							output.Add(builder.Build());
							break;
						}
					}
				}
			}

			foreach (var trivia in triviaList)
			{
				var triviaKind = trivia.Kind();
				var triviaSpan = script.AsSpan(trivia.Span.Start, trivia.Span.Length);

				switch (triviaKind)
				{
					case SyntaxKind.WhitespaceTrivia:
					{
						if (regionDirectiveStart != null)
						{
							var regionSpan = script.AsSpan(regionDirectiveStart.Value.Span.Start, regionDirectiveStart.Value.Span.Length);

							regionSpan = regionSpan.TrimStart("#region");
							regionSpan = regionSpan.Trim();

							builders.Add(new CodeBlockBuilder(regionSpan.ToString(), Indent(triviaSpan)));
							regionDirectiveStart = null;
						}

						int indent = Indent(triviaSpan);
						if (isFollowingNewline)
						{
							foreach (var builder in builders)
							{
								string indentString = new(' ', indent - builder.Indent);
								builder.Write(indentString);
							}
						}
						else
						{
							string indentString = new(' ', indent);
							Write(indentString);
						}
						isFollowingNewline = false;
						break;
					}
					case SyntaxKind.EndOfLineTrivia:
					{
						WriteNewline();
						isFollowingNewline = true;
						break;
					}
					case SyntaxKind.RegionDirectiveTrivia:
					{
						regionDirectiveStart = trivia;

						foreach (var builder in builders)
						{
							builder.RestartLine();
						}
						isFollowingNewline = true;
						break;
					}
					case SyntaxKind.EndRegionDirectiveTrivia:
					case SyntaxKind.DefineDirectiveTrivia:
					case SyntaxKind.UndefDirectiveTrivia:
					case SyntaxKind.IfDirectiveTrivia:
					case SyntaxKind.ElifDirectiveTrivia:
					case SyntaxKind.ElseDirectiveTrivia:
					case SyntaxKind.EndIfDirectiveTrivia:
					case SyntaxKind.ErrorDirectiveTrivia:
					case SyntaxKind.WarningDirectiveTrivia:
					case SyntaxKind.LineDirectiveTrivia:
					case SyntaxKind.LoadDirectiveTrivia:
					case SyntaxKind.NullableDirectiveTrivia:
					case SyntaxKind.PragmaChecksumDirectiveTrivia:
					case SyntaxKind.PragmaWarningDirectiveTrivia:
					case SyntaxKind.ReferenceDirectiveTrivia:
					case SyntaxKind.ShebangDirectiveTrivia:
					case SyntaxKind.UsingDirective:
					case SyntaxKind.BadDirectiveTrivia:
					{
						foreach (var builder in builders)
						{
							builder.RestartLine();
						}
						isFollowingNewline = true;
						break;
					}
					default:
					{
						isFollowingNewline = false;
						if (styles.TryGetSyntaxStyle(triviaKind, out string? style))
						{
							if (style == null)
							{
								Write(triviaSpan);
							}
							else
							{
								WriteStyled(triviaSpan, style);
							}
						}
						else
						{
							Write(triviaSpan);
						}
						break;
					}
				}
			}
		}

		void Write(ReadOnlySpan<char> value)
		{
			foreach (var builder in builders)
			{
				builder.Write(value);
			}
		}

		void WriteStyled(ReadOnlySpan<char> value, string style)
		{
			foreach (var builder in builders)
			{
				builder.Write(value, style);
			}
		}

		void WriteStyledLink(ReadOnlySpan<char> value, string style, string linkUrl)
		{
			foreach (var builder in builders)
			{
				builder.Write(value, style, linkUrl);
			}
		}

		void WriteNewline()
		{
			foreach (var builder in builders)
			{
				builder.WriteNewline();
			}
		}

		int Indent(ReadOnlySpan<char> whitespace)
		{
			int indent = 0;
			foreach (char character in whitespace)
			{
				if (character == ' ')
				{
					indent += 1;
				}
				else if (character == '\t')
				{
					indent += 4;
				}
			}
			return indent;
		}
	}
}

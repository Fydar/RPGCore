using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Xml;

namespace RPGCore.Documentation.Internal
{
	public readonly struct CodeSpan
	{
		public string Content { get; }
		public string? Style { get; }
		public string? LinkURL { get; }

		public CodeSpan(string content)
		{
			Content = content;
			Style = null;
			LinkURL = null;
		}

		public CodeSpan(string content, string style)
		{
			Content = content;
			Style = style;
			LinkURL = null;
		}

		public CodeSpan(string content, string style, string linkUrl)
		{
			Content = content;
			Style = style;
			LinkURL = linkUrl;
		}
	}

	internal static class SampleParser
	{
		private class RegionBuilder
		{
			private readonly List<CodeSpan> currentLine;
			private readonly List<CodeSpan[]> lines;

			public int Indent { get; }
			public string Name { get; }

			public RegionBuilder(int indent)
			{
				currentLine = new List<CodeSpan>();
				lines = new List<CodeSpan[]>();
				Name = "";
				Indent = indent;
			}

			public RegionBuilder(string name, int indent)
			{
				currentLine = new List<CodeSpan>();
				lines = new List<CodeSpan[]>();
				Name = name;
				Indent = indent;
			}

			public SampleRegion Build()
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
					}
				}

				return new SampleRegion(Name, lines.ToArray());
			}

			public override string ToString()
			{
				return Name;
			}

			public void Write(ReadOnlySpan<char> value)
			{
				currentLine.Add(new CodeSpan(value.ToString()));
			}

			public void Write(ReadOnlySpan<char> value, string style)
			{
				currentLine.Add(new CodeSpan(value.ToString(), style));
			}

			public void Write(ReadOnlySpan<char> value, string style, string linkUrl)
			{
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

		private static readonly CodeStyles styles;

		static SampleParser()
		{
			styles = new CodeStyles();
		}

		public static List<SampleRegion> HtmlHighlight(string script)
		{
			var references = new List<MetadataReference>();

			var domainAssemblies = AppDomain.CurrentDomain.GetAssemblies();
			foreach (var assembly in domainAssemblies)
			{
				if (assembly.IsDynamic)
				{
					continue;
				}
				try
				{
					references.Add(MetadataReference.CreateFromFile(assembly.Location));
				}
				catch
				{
				}
			}

			// using var workspace = new AdhocWorkspace();
			// var solution = workspace.CurrentSolution;
			// var project = solution
			// 	.AddProject("projectName", "assemblyName", LanguageNames.CSharp)
			// 	.AddMetadataReferences(references);
			// var root = scriptTree.GetCompilationUnitRoot();

			var scriptTree = CSharpSyntaxTree.ParseText(script);

			var compilation = CSharpCompilation.Create("ExampleCode")
				.AddReferences(references)
				.AddSyntaxTrees(scriptTree);

			var semanticModel = compilation.GetSemanticModel(scriptTree);

			var builders = new List<RegionBuilder>
			{
				new RegionBuilder(0)
			};

			var output = new List<SampleRegion>();

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
					if (styles.TryGetSyntaxStyle(nodeOrToken.Kind(), out string style))
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

						WriteRecursive(node);
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

								builders.Add(new RegionBuilder(regionSpan.ToString(), Indent(triviaSpan)));
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
							if (styles.TryGetSyntaxStyle(triviaKind, out string style))
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
}

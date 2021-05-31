using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using RPGCore.Documentation.Samples.AddNodeSample;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace RPGCore.Documentation.Internal
{
	internal static class SampleParser
	{
		private class RegionBuilder
		{
			private readonly StringBuilder content;
			private readonly List<string> lines;

			public int Indent { get; }
			public string Name { get; }

			public RegionBuilder(int indent)
			{
				content = new StringBuilder();
				lines = new List<string>();
				Name = "";
				Indent = indent;
			}

			public RegionBuilder(string name, int indent)
			{
				content = new StringBuilder();
				lines = new List<string>();
				Name = name;
				Indent = indent;
			}

			public SampleRegion Build()
			{
				if (content.Length > 0)
				{
					lines.Add(content.ToString());
				}

				return new SampleRegion(Name, lines.ToArray());
			}

			public override string ToString()
			{
				return Name;
			}

			public void Write(ReadOnlySpan<char> value)
			{
				content.Append(value);
			}

			public void RestartLine()
			{
				content.Clear();
			}

			public void WriteNewline()
			{
				string addLine = content.ToString();
				content.Clear();

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

			foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
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

			var loadType = typeof(AddNode);

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
							WriteEscaped(span);
						}
						else
						{
							WriteString($"<span class=\"{style}\">");
							WriteEscaped(span);
							WriteString($"</span>");
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
									// string typeStyle = styles.GetStyleForTypeSymbol(typeInfo.Type as INamedTypeSymbol);
									// WriteString($"<span class=\"{typeStyle}\">");
									WriteEscaped(span);
									// WriteString($"</span>");
								}
								else
								{
									WriteEscaped(span);
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

						WriteString($"<span class=\"{style}\">");
						WriteEscaped(span);
						WriteString($"</span>");
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

						WriteString($"<span class=\"{style}\">");
						WriteEscaped(span);
						WriteString($"</span>");
						break;
					}
					case SymbolKind.Parameter:
					{
						WriteString($"<span class=\"{styles.Parameter}\">");
						WriteEscaped(span);
						WriteString($"</span>");
						break;
					}
					case SymbolKind.Local:
					{
						WriteString($"<span class=\"{styles.Local}\">");
						WriteEscaped(span);
						WriteString($"</span>");
						break;
					}
					case SymbolKind.Field:
					{
						WriteString($"<span class=\"{styles.Field}\">");
						WriteEscaped(span);
						WriteString($"</span>");
						break;
					}
					case SymbolKind.Property:
					{
						WriteString($"<span class=\"{styles.Property}\">");
						WriteEscaped(span);
						WriteString($"</span>");
						break;
					}
					case SymbolKind.TypeParameter:
					{
						WriteString($"<span class=\"{styles.TypeGeneric}\">");
						WriteEscaped(span);
						WriteString($"</span>");
						break;
					}
					case SymbolKind.Namespace:
					{
						WriteEscaped(span);
						break;
					}
					default:
					{
						WriteEscaped(span);
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
								WriteString(indentString);
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
									WriteEscaped(triviaSpan);
								}
								else
								{
									WriteString($"<span class=\"{style}\">");
									WriteEscaped(triviaSpan);
									WriteString($"</span>");
								}
							}
							else
							{
								WriteEscaped(triviaSpan);
							}
							break;
						}
					}
				}
			}

			string XmlEscape(string unescaped)
			{
				var doc = new XmlDocument();
				var node = doc.CreateElement("root");
				node.InnerText = unescaped;
				return node.InnerXml;
			}

			void WriteEscaped(ReadOnlySpan<char> content)
			{
				string escaped = XmlEscape(content.ToString());
				foreach (var builder in builders)
				{
					builder.Write(escaped);
				}
			}

			void WriteString(ReadOnlySpan<char> content)
			{
				foreach (var builder in builders)
				{
					builder.Write(content);
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

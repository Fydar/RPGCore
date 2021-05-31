using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace RPGCore.Documentation.Internal
{
	internal class CodeStyles
	{
		public string TypeClass = "c-class";
		public string TypeDelegate = "c-class";
		public string TypeStruct = "c-structure";
		public string TypeInterface = "c-interface";
		public string TypeEnum = "c-enum";
		public string TypeGeneric = "c-enum";

		public string TypeFramework = "c-keyword";
		public string Numeric = "c-numeric";
		public string String = "c-string";
		public string Comment = "c-comment";

		public string Local = "c-local";
		public string Parameter = "c-parameter";
		public string Field = "c-field";
		public string Property = "c-property";
		public string Method = "c-method";

		public string Keyword = "c-keyword";
		public string KeywordControl = "c-control";

		private readonly Dictionary<SyntaxKind, string?> tokenStyles;

		public bool TryGetSyntaxStyle(SyntaxKind token, out string? tokenStyle)
		{
			return tokenStyles.TryGetValue(token, out tokenStyle);
		}

		public CodeStyles()
		{
			tokenStyles = new Dictionary<SyntaxKind, string>
			{
				[SyntaxKind.NewKeyword] = Keyword,
				[SyntaxKind.VarKeyword] = Keyword,
				[SyntaxKind.InKeyword] = Keyword,
				[SyntaxKind.WhenClause] = Keyword,
				[SyntaxKind.UsingKeyword] = Keyword,

				[SyntaxKind.ReturnKeyword] = KeywordControl,
				[SyntaxKind.ForKeyword] = KeywordControl,
				[SyntaxKind.ForEachKeyword] = KeywordControl,
				[SyntaxKind.IfKeyword] = KeywordControl,
				[SyntaxKind.ElseKeyword] = KeywordControl,
				[SyntaxKind.WhileKeyword] = KeywordControl,
				[SyntaxKind.DoKeyword] = KeywordControl,
				[SyntaxKind.TryKeyword] = KeywordControl,
				[SyntaxKind.CatchKeyword] = KeywordControl,
				[SyntaxKind.FinallyKeyword] = KeywordControl,

				[SyntaxKind.VoidKeyword] = Keyword,
				[SyntaxKind.ByteKeyword] = Keyword,
				[SyntaxKind.SByteKeyword] = Keyword,
				[SyntaxKind.ShortKeyword] = Keyword,
				[SyntaxKind.UShortKeyword] = Keyword,
				[SyntaxKind.IntKeyword] = Keyword,
				[SyntaxKind.UIntKeyword] = Keyword,
				[SyntaxKind.LongKeyword] = Keyword,
				[SyntaxKind.ULongKeyword] = Keyword,
				[SyntaxKind.StringKeyword] = Keyword,
				[SyntaxKind.CharKeyword] = Keyword,
				[SyntaxKind.ObjectKeyword] = Keyword,
				[SyntaxKind.BoolKeyword] = Keyword,
				[SyntaxKind.FloatKeyword] = Keyword,
				[SyntaxKind.DoubleKeyword] = Keyword,
				[SyntaxKind.DecimalKeyword] = Keyword,

				[SyntaxKind.NullKeyword] = Keyword,
				[SyntaxKind.FalseKeyword] = Keyword,
				[SyntaxKind.TrueKeyword] = Keyword,

				[SyntaxKind.TypeOfKeyword] = Keyword,
				[SyntaxKind.SizeOfKeyword] = Keyword,
				[SyntaxKind.NameOfKeyword] = Keyword,

				[SyntaxKind.PublicKeyword] = Keyword,
				[SyntaxKind.PrivateKeyword] = Keyword,
				[SyntaxKind.ProtectedKeyword] = Keyword,
				[SyntaxKind.StaticKeyword] = Keyword,
				[SyntaxKind.InternalKeyword] = Keyword,

				[SyntaxKind.AbstractKeyword] = Keyword,
				[SyntaxKind.VirtualKeyword] = Keyword,
				[SyntaxKind.SealedKeyword] = Keyword,
				[SyntaxKind.OverrideKeyword] = Keyword,
				[SyntaxKind.BaseKeyword] = Keyword,

				[SyntaxKind.RefKeyword] = Keyword,
				[SyntaxKind.OutKeyword] = Keyword,

				[SyntaxKind.AsyncKeyword] = Keyword,

				[SyntaxKind.GetKeyword] = Keyword,
				[SyntaxKind.SetKeyword] = Keyword,
				[SyntaxKind.AddKeyword] = Keyword,
				[SyntaxKind.RemoveKeyword] = Keyword,

				[SyntaxKind.NamespaceKeyword] = Keyword,
				[SyntaxKind.ClassKeyword] = Keyword,
				[SyntaxKind.InterfaceKeyword] = Keyword,
				[SyntaxKind.RecordKeyword] = Keyword,
				[SyntaxKind.DelegateKeyword] = Keyword,
				[SyntaxKind.StructKeyword] = Keyword,
				[SyntaxKind.EnumKeyword] = Keyword,

				[SyntaxKind.LockKeyword] = Keyword,
				[SyntaxKind.CheckedKeyword] = Keyword,
				[SyntaxKind.UncheckedKeyword] = Keyword,
				[SyntaxKind.FromKeyword] = Keyword,
				[SyntaxKind.WhereKeyword] = Keyword,
				[SyntaxKind.AndKeyword] = Keyword,
				[SyntaxKind.IsKeyword] = Keyword,
				[SyntaxKind.OrKeyword] = Keyword,
				[SyntaxKind.AscendingKeyword] = Keyword,
				[SyntaxKind.DescendingKeyword] = Keyword,
				[SyntaxKind.DisableKeyword] = Keyword,

				[SyntaxKind.StringLiteralExpression] = String,
				[SyntaxKind.StringLiteralToken] = String,
				[SyntaxKind.CharacterLiteralExpression] = String,
				[SyntaxKind.CharacterLiteralToken] = String,

				[SyntaxKind.NumericLiteralExpression] = Numeric,
				[SyntaxKind.NumericLiteralToken] = Numeric,

				[SyntaxKind.DocumentationCommentExteriorTrivia] = Comment,
				[SyntaxKind.EndOfDocumentationCommentToken] = Comment,
				[SyntaxKind.MultiLineCommentTrivia] = Comment,
				[SyntaxKind.MultiLineDocumentationCommentTrivia] = Comment,
				[SyntaxKind.SingleLineCommentTrivia] = Comment,
				[SyntaxKind.SingleLineDocumentationCommentTrivia] = Comment,
				[SyntaxKind.XmlComment] = Comment,
				[SyntaxKind.XmlCommentEndToken] = Comment,
				[SyntaxKind.XmlCommentStartToken] = Comment,

				// [SyntaxKind.OpenParenToken, null,
				// [SyntaxKind.CloseParenToken, null,

				[SyntaxKind.OpenBraceToken] = null,
				[SyntaxKind.CloseBraceToken] = null,

				[SyntaxKind.OpenBracketToken] = null,
				[SyntaxKind.CloseBracketToken] = null,

				[SyntaxKind.OpenParenToken] = null,
				[SyntaxKind.CloseParenToken] = null,

				[SyntaxKind.SemicolonToken] = null,
				[SyntaxKind.ColonToken] = null,

				[SyntaxKind.EqualsEqualsToken] = null,
				[SyntaxKind.GreaterThanToken] = null,
				[SyntaxKind.GreaterThanEqualsToken] = null,
				[SyntaxKind.LessThanToken] = null,
				[SyntaxKind.LessThanOrEqualExpression] = null,

				[SyntaxKind.PlusToken] = null,
				[SyntaxKind.MinusToken] = null,
				[SyntaxKind.AsteriskToken] = null,
				[SyntaxKind.SlashToken] = null,

				[SyntaxKind.EqualsToken] = null,
				[SyntaxKind.DotToken] = null,
				[SyntaxKind.CommaToken] = null,
			};
		}
		
		public string GetStyleForTypeSymbol(ITypeSymbol typeSymbol)
		{
			var namedTypeSymbol = typeSymbol as INamedTypeSymbol;
			if (typeSymbol.TypeKind == TypeKind.Array)
			{
				typeSymbol = namedTypeSymbol.TypeArguments[0];
			}

			if (typeSymbol.BaseType?.Name == "Enum")
			{
				return TypeEnum;
			}

			switch (typeSymbol.TypeKind)
			{
				case TypeKind.Enum:
					return TypeEnum;

				case TypeKind.Interface:
					return TypeInterface;

				case TypeKind.Struct:
					return TypeStruct;

				case TypeKind.TypeParameter:
					return TypeGeneric;

				case TypeKind.Delegate:
				case TypeKind.Class:
				case TypeKind.Dynamic:
					return TypeClass;

				default:
					throw new InvalidOperationException($"Unrecognised type kind {typeSymbol.TypeKind}.");
			}
		}

		public string GetStyle(MemberInfo memberInfo)
		{
			if (memberInfo is Type type)
			{
				if (type.IsInterface)
				{
					return TypeInterface;
				}
				else if (type.IsEnum)
				{
					return TypeEnum;
				}
				else if (type.IsValueType)
				{
					return TypeStruct;
				}
				else if (type.IsGenericMethodParameter)
				{
					return TypeGeneric;
				}
				else
				{
					return TypeClass;
				}
			}
			else if (memberInfo is FieldInfo)
			{
				return Field;
			}
			else if (memberInfo is PropertyInfo)
			{
				return Property;
			}
			else if (memberInfo is MethodInfo)
			{
				return Method;
			}
			else
			{
				throw new InvalidOperationException($"{memberInfo} is not a supported type.");
			}
		}
	}
}

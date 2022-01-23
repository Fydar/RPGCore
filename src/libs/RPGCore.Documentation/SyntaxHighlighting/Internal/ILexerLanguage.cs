using System;

namespace RPGCore.Documentation.SyntaxHighlighting.Internal;

/// <summary>
/// A language used to classify spans of text.
/// </summary>
public interface ILexerLanguage
{
	/// <summary>
	/// A collection of <see cref="ITokenClassifier"/>s used to classify spans of text.
	/// </summary>
	ITokenClassifier[] Classifiers { get; }

	/// <summary>
	/// Colours associated with the <see cref="Classifiers"/> collection.
	/// </summary>
	ConsoleColor[] Colors { get; }
}

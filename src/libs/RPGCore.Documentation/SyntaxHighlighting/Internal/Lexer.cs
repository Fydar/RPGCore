using System.Collections.Generic;

namespace RPGCore.Documentation.SyntaxHighlighting.Internal
{
	/// <summary>
	/// A lexer used to classify spans of text using an <see cref="ILexerLanguage"/>.
	/// </summary>
	public class Lexer
	{
		private struct ClassifierState
		{
			public bool hasGivenUp;
			public int endIndex;
		}

		/// <summary>
		/// The <see cref="ILexerLanguage"/> that this <see cref="Lexer"/> uses to classify tokens.
		/// </summary>
		public ILexerLanguage LexerLanguage { get; }

		/// <summary>
		/// Constructs a new instance of the <see cref="Lexer"/> class.
		/// </summary>
		/// <param name="lexerLanguage">The language used to classify tokens.</param>
		public Lexer(ILexerLanguage lexerLanguage)
		{
			LexerLanguage = lexerLanguage;
		}

		/// <summary>
		/// Iterates over the classifiable spans present in the source <see cref="string"/>.
		/// </summary>
		/// <param name="source">A <see cref="string"/> to classify.</param>
		/// <returns>A collection representing every classified token in the source <see cref="string"/>.</returns>
		public IEnumerable<LexerToken> Tokenize(string source)
		{
			var classifiers = LexerLanguage.Classifiers;

			int classifiersCount = classifiers.Length;
			var classifierStates = new ClassifierState[classifiersCount];

			for (int i = 0; i < classifiersCount; i++)
			{
				var classifier = classifiers[i];
				classifierStates[i] = new ClassifierState()
				{
					endIndex = -1
				};
				classifier.Reset();
			}

			int startIndex = 0;
			for (int charIndex = 0; charIndex < source.Length + 1; charIndex++)
			{
				char c = charIndex != source.Length
					? source[charIndex]
					: ' ';

				bool anyContinuing = false;
				for (int i = 0; i < classifiersCount; i++)
				{
					var classifierState = classifierStates[i];
					if (classifierState.hasGivenUp)
					{
						continue;
					}

					var classifier = classifiers[i];
					var result = classifier.NextCharacter(c);
					switch (result.Action)
					{
						case ClassifierActionType.GiveUp:
						{
							classifierState.hasGivenUp = true;
							classifierStates[i] = classifierState;
							break;
						}
						case ClassifierActionType.TokenizeFromLast:
						{
							classifierState.endIndex = charIndex - 1;
							classifierStates[i] = classifierState;
							break;
						}
						case ClassifierActionType.TokenizeImmediately:
						{
							classifierState.endIndex = charIndex;
							classifierStates[i] = classifierState;
							break;
						}
						default:
						case ClassifierActionType.ContinueReading:
						{
							anyContinuing = true;
							break;
						}
					}
				}

				if (!anyContinuing)
				{
					int longest = -1;
					for (int i = 0; i < classifiersCount; i++)
					{
						var classifierState = classifierStates[i];
						if (classifierState.endIndex > longest)
						{
							longest = classifierState.endIndex;
						}
					}

					bool tokenized = false;
					for (int i = 0; i < classifiersCount; i++)
					{
						var classifierState = classifierStates[i];

						if (classifierState.endIndex != -1 &&
							classifierState.endIndex == longest)
						{
							int classifierEndIndex = classifierState.endIndex;

							yield return new LexerToken(
								startIndex: startIndex,
								length: classifierEndIndex - startIndex + 1,
								classifier: i
							);

							startIndex = classifierEndIndex + 1;
							charIndex = classifierEndIndex;
							tokenized = true;
							break;
						}
					}

					if (!tokenized)
					{
						if (charIndex != source.Length)
						{
							yield return new LexerToken(
								startIndex: startIndex,
								length: 1,
								classifier: -1
							);
						}

						startIndex += 1;
					}

					for (int i = 0; i < classifiersCount; i++)
					{
						var classifier = classifiers[i];
						classifierStates[i] = new ClassifierState()
						{
							endIndex = -1
						};
						classifier.Reset();
					}
				}
			}
		}
	}
}

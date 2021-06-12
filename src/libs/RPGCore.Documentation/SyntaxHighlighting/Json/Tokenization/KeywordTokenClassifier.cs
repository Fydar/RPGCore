using RPGCore.Documentation.SyntaxHighlighting.Internal;

namespace RPGCore.Documentation.SyntaxHighlighting.Json.Tokenization
{
	internal class KeywordTokenClassifier : ITokenClassifier
	{
		private int currentIndex = 0;

		public string Keyword { get; }

		public KeywordTokenClassifier(string keyword)
		{
			Keyword = keyword;
		}

		/// <inheritdoc/>
		public void Reset()
		{
			currentIndex = 0;
		}

		/// <inheritdoc/>
		public ClassifierAction NextCharacter(char nextCharacter)
		{
			if (currentIndex == Keyword.Length)
			{
				return char.IsLetterOrDigit(nextCharacter)
					? ClassifierAction.GiveUp()
					: ClassifierAction.TokenizeFromLast();
			}
			else
			{
				if (nextCharacter == Keyword[currentIndex])
				{
					currentIndex++;
					return ClassifierAction.ContinueReading();
				}
				else
				{
					return ClassifierAction.GiveUp();
				}
			}
		}
	}
}

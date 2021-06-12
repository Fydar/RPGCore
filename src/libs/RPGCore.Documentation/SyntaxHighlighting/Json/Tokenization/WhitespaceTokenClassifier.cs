using RPGCore.Documentation.SyntaxHighlighting.Internal;

namespace RPGCore.Documentation.SyntaxHighlighting.Json.Tokenization
{
	internal class WhitespaceTokenClassifier : ITokenClassifier
	{
		protected bool IsFirstCharacter { get; private set; }

		/// <inheritdoc/>
		public void Reset()
		{
			IsFirstCharacter = true;
		}

		/// <inheritdoc/>
		public ClassifierAction NextCharacter(char nextCharacter)
		{
			bool isMatched = char.IsWhiteSpace(nextCharacter)
				&& nextCharacter != '\r'
				&& nextCharacter != '\n';

			if (!IsFirstCharacter)
			{
				if (!isMatched)
				{
					return ClassifierAction.TokenizeFromLast();
				}
			}

			IsFirstCharacter = false;

			return isMatched
				? ClassifierAction.ContinueReading()
				: ClassifierAction.GiveUp();
		}
	}
}

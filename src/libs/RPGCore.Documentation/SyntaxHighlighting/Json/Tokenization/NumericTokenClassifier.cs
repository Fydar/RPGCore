using RPGCore.Documentation.SyntaxHighlighting.Internal;

namespace RPGCore.Documentation.SyntaxHighlighting.Json.Tokenization
{
	internal class NumericTokenClassifier : ITokenClassifier
	{
		private bool isFirstCharacter;

		/// <inheritdoc/>
		public void Reset()
		{
			isFirstCharacter = true;
		}

		/// <inheritdoc/>
		public ClassifierAction NextCharacter(char nextCharacter)
		{
			bool isMatched = char.IsDigit(nextCharacter)
				|| nextCharacter == '.';

			if (!isFirstCharacter)
			{
				if (!isMatched)
				{
					return ClassifierAction.TokenizeFromLast();
				}
			}
			else if (nextCharacter == '-')
			{
				return ClassifierAction.ContinueReading();
			}

			isFirstCharacter = false;

			return isMatched
				? ClassifierAction.ContinueReading()
				: ClassifierAction.GiveUp();
		}
	}
}

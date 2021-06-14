using RPGCore.Documentation.SyntaxHighlighting.Internal;

namespace RPGCore.Documentation.SyntaxHighlighting.Json.Tokenization
{
	internal class NewlineTokenClassifier : ITokenClassifier
	{
		/// <inheritdoc/>
		public void Reset()
		{
		}

		/// <inheritdoc/>
		public ClassifierAction NextCharacter(char nextCharacter)
		{
			return nextCharacter switch
			{
				'\r' => ClassifierAction.ContinueReading(),
				'\n' => ClassifierAction.TokenizeImmediately(),
				_ => ClassifierAction.GiveUp()
			};
		}
	}
}

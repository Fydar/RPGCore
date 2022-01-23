using RPGCore.Documentation.SyntaxHighlighting.Internal;

namespace RPGCore.Documentation.SyntaxHighlighting.Json.Tokenization;

internal class LineCommentTokenClassifier : ITokenClassifier
{
	private int characterIndex = 0;

	/// <inheritdoc/>
	public void Reset()
	{
		characterIndex = 0;
	}

	/// <inheritdoc/>
	public ClassifierAction NextCharacter(char nextCharacter)
	{
		if (characterIndex == 0
			|| characterIndex == 1)
		{
			characterIndex++;
			return nextCharacter == '/'
				? ClassifierAction.ContinueReading()
				: ClassifierAction.GiveUp();
		}
		else
		{
			return nextCharacter == '\n'
				|| nextCharacter == '\r'
				? ClassifierAction.TokenizeFromLast()
				: ClassifierAction.ContinueReading();
		}
	}
}

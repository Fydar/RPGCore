using RPGCore.Documentation.SyntaxHighlighting.Internal;

namespace RPGCore.Documentation.SyntaxHighlighting.Json.Tokenization;

internal class SingleCharacterTokenClassifier : ITokenClassifier
{
	public char MatchedCharacter { get; }

	public SingleCharacterTokenClassifier(char matchedCharacter)
	{
		MatchedCharacter = matchedCharacter;
	}

	/// <inheritdoc/>
	public void Reset()
	{
	}

	/// <inheritdoc/>
	public ClassifierAction NextCharacter(char nextCharacter)
	{
		bool isMatched = nextCharacter == MatchedCharacter;

		return isMatched
			? ClassifierAction.TokenizeImmediately()
			: ClassifierAction.GiveUp();
	}
}

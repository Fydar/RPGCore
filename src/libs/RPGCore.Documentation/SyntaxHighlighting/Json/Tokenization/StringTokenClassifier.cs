using RPGCore.Documentation.SyntaxHighlighting.Internal;

namespace RPGCore.Documentation.SyntaxHighlighting.Json.Tokenization;

internal class StringTokenClassifier : ITokenClassifier
{
	private bool isFirstCharacter = true;
	private bool isEscaped = false;

	/// <inheritdoc/>
	public void Reset()
	{
		isFirstCharacter = true;
	}

	/// <inheritdoc/>
	public ClassifierAction NextCharacter(char nextCharacter)
	{
		if (isFirstCharacter)
		{
			isFirstCharacter = false;

			return nextCharacter == '"'
				? ClassifierAction.ContinueReading()
				: ClassifierAction.GiveUp();
		}
		else
		{
			if (nextCharacter == '\\')
			{
				isEscaped = true;
				return ClassifierAction.ContinueReading();
			}
			else if (nextCharacter == '"')
			{
				if (isEscaped)
				{
					isEscaped = false;
					return ClassifierAction.ContinueReading();
				}
				else
				{
					return ClassifierAction.TokenizeImmediately();
				}
			}
			else
			{
				isEscaped = false;
				return ClassifierAction.ContinueReading();
			}
		}
	}
}

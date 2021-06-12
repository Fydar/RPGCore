using RPGCore.Documentation.SyntaxHighlighting.Internal;

namespace RPGCore.Documentation.SyntaxHighlighting.Json.Tokenization
{
	internal class MultiLineCommentTokenClassifier : ITokenClassifier
	{
		private enum State : byte
		{
			ExpectingStartingSlash,
			ExpectingStartingAsterisk,
			ExpectingEndingAsterisk,
			ExpectingEndingSlash,
		}

		private State state;

		/// <inheritdoc/>
		public void Reset()
		{
			state = State.ExpectingStartingSlash;
		}

		/// <inheritdoc/>
		public ClassifierAction NextCharacter(char nextCharacter)
		{
			switch (state)
			{
				case State.ExpectingStartingSlash:
				{
					if (nextCharacter == '/')
					{
						state = State.ExpectingStartingAsterisk;
						return ClassifierAction.ContinueReading();
					}
					else
					{
						return ClassifierAction.GiveUp();
					}
				}
				case State.ExpectingStartingAsterisk:
				{
					if (nextCharacter == '*')
					{
						state = State.ExpectingEndingAsterisk;
						return ClassifierAction.ContinueReading();
					}
					else
					{
						return ClassifierAction.GiveUp();
					}
				}
				case State.ExpectingEndingAsterisk:
				{
					if (nextCharacter == '*')
					{
						state = State.ExpectingEndingSlash;
					}
					return ClassifierAction.ContinueReading();
				}
				default:
				{
					if (nextCharacter == '/')
					{
						return ClassifierAction.TokenizeImmediately();
					}
					else
					{
						state = State.ExpectingStartingAsterisk;
						return ClassifierAction.ContinueReading();
					}
				}
			}
		}
	}
}

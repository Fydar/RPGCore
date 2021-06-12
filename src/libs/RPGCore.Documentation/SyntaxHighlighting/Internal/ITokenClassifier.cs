namespace RPGCore.Documentation.SyntaxHighlighting.Internal
{
	/// <summary>
	/// A classifier used to classify spans of text.
	/// </summary>
	public interface ITokenClassifier
	{
		/// <summary>
		/// Appends a new character to this <see cref="ITokenClassifier"/>.
		/// </summary>
		/// <param name="nextCharacter">The next character to attempt to classify.</param>
		/// <returns>A <see cref="ClassifierAction"/> to perform for this classifier.</returns>
		ClassifierAction NextCharacter(char nextCharacter);

		/// <summary>
		/// Resets the state of this <see cref="ITokenClassifier"/>.
		/// </summary>
		void Reset();
	}
}

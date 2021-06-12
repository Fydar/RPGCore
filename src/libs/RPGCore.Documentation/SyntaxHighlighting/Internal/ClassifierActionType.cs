namespace RPGCore.Documentation.SyntaxHighlighting.Internal
{
	/// <summary>
	/// The type of the classifier action.
	/// </summary>
	public enum ClassifierActionType : byte
	{
		/// <summary>
		/// Inform the lexer that this classifier cannot be used to match a span.
		/// </summary>
		GiveUp,

		/// <summary>
		/// Inform the lexer that this classifier needs to continue readering to determine whether the span matches.
		/// </summary>
		ContinueReading,

		/// <summary>
		/// Inform the lexer that this classifier has matched a span from the previous character in the span.
		/// </summary>
		TokenizeFromLast,

		/// <summary>
		/// Inform the lexer that this classifier has matched a span and that the current character is included in the span.
		/// </summary>
		TokenizeImmediately
	}
}

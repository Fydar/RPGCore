namespace RPGCore.Documentation.SyntaxHighlighting.Internal;

/// <summary>
/// A result used to inform the lexer of the intended classifier behaviour.
/// </summary>
public readonly ref struct ClassifierAction
{
	/// <summary>
	/// The type of this <see cref="ClassifierAction"/>.
	/// </summary>
	public ClassifierActionType Action { get; }

	private ClassifierAction(ClassifierActionType action)
	{
		Action = action;
	}

	/// <summary>
	/// Inform the lexer that this classifier cannot be used to match a span.
	/// </summary>
	/// <returns>A <see cref="ClassifierAction"/> representing the classifier result.</returns>
	public static ClassifierAction GiveUp()
	{
		return new ClassifierAction(ClassifierActionType.GiveUp);
	}

	/// <summary>
	/// Inform the lexer that this classifier needs to continue readering to determine whether the span matches.
	/// </summary>
	/// <returns>A <see cref="ClassifierAction"/> representing the classifier result.</returns>
	public static ClassifierAction ContinueReading()
	{
		return new ClassifierAction(ClassifierActionType.ContinueReading);
	}

	/// <summary>
	/// Inform the lexer that this classifier has matched a span from the previous character in the span.
	/// </summary>
	/// <returns>A <see cref="ClassifierAction"/> representing the classifier result.</returns>
	public static ClassifierAction TokenizeFromLast()
	{
		return new ClassifierAction(ClassifierActionType.TokenizeFromLast);
	}

	/// <summary>
	/// Inform the lexer that this classifier has matched a span and that the current character is included in the span.
	/// </summary>
	/// <returns>A <see cref="ClassifierAction"/> representing the classifier result.</returns>
	public static ClassifierAction TokenizeImmediately()
	{
		return new ClassifierAction(ClassifierActionType.TokenizeImmediately);
	}
}

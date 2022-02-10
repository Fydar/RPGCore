namespace RPGCore.Behaviour;

/// <summary>
/// A <see cref="Node"/> input that always evaluates to the <see cref="DefaultValue"/>.
/// </summary>
/// <typeparam name="TType">The type of this input.</typeparam>
public sealed class DefaultInput<TType> : IInput<TType>
{
	/// <summary>
	/// The default value that this <see cref="DefaultInput{TType}"/> will always evaluate to.
	/// </summary>
	public TType DefaultValue { get; set; }

	/// <summary>
	/// Creates a new instance of the <see cref="DefaultInput{TType}"/> class.
	/// </summary>
	public DefaultInput()
	{
	}

	/// <summary>
	/// Creates a new instance of the <see cref="DefaultInput{TType}"/> class.
	/// </summary>
	/// <param name="defaultValue">The default value that this <see cref="DefaultInput{TType}"/> will always evaluate to.</param>
	public DefaultInput(TType defaultValue)
	{
		DefaultValue = defaultValue;
	}
}

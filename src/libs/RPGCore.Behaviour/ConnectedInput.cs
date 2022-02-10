namespace RPGCore.Behaviour;

/// <summary>
/// A <see cref="Node"/> input that will source a its value from another <see cref="Node"/> output via a <see cref="Path"/>.
/// </summary>
/// <typeparam name="TType">The type of this input.</typeparam>
public sealed class ConnectedInput<TType> : IInput<TType>
{
	/// <summary>
	/// The path of the <see cref="Output{TType}"/> this <see cref="ConnectedInput{TType}"/> is connected to.
	/// </summary>
	public string Path { get; set; } = string.Empty;

	/// <summary>
	/// Creates a new instance of the <see cref="ConnectedInput{TType}"/> class.
	/// </summary>
	public ConnectedInput()
	{
	}

	/// <summary>
	/// Creates a new instance of the <see cref="DefaultInput{TType}"/> class.
	/// </summary>
	/// <param name="path">The path of the <see cref="Output{TType}"/> this <see cref="ConnectedInput{TType}"/> is connected to.</param>
	public ConnectedInput(string path)
	{
		Path = path;
	}
}

using RPGCore.Data.Polymorphic.Inline;

namespace RPGCore.Behaviour;

/// <summary>
/// A <see cref="Node"/> input.
/// </summary>
[SerializeBaseType]
public interface IInput
{

}

/// <summary>
/// A <see cref="Node"/> input.
/// </summary>
/// <typeparam name="TType">The type of this input.</typeparam>
/// <seealso cref="ConnectedInput{TType}"/>
/// <seealso cref="DefaultInput{TType}"/>
public interface IInput<TType> : IInput
{

}

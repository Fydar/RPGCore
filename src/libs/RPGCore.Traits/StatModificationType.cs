namespace RPGCore.Traits;

public enum StatModificationType
{
	None,

	/// <example>
	/// Example 1:
	/// My attack is 100.
	/// I add 2 <see cref="Additive"/> modifiers of +20 each.
	/// My attack is now 140.
	/// </example>
	Additive,

	/// <example>
	/// Example 1:
	/// My attack is 100.
	/// I add 2 <see cref="SumAndMultiply"/> modifiers of +10% each.
	/// My attack is now 120.
	/// 
	/// Example 2:
	/// My attack is 100.
	/// I add 2 <see cref="SumAndMultiply"/> modifiers of +100% each.
	/// My attack is now 300.
	/// </example>
	SumAndMultiply,

	/// <example>
	/// Example 1:
	/// My attack is 100.
	/// I add 2 <see cref="Multiplicative"/> modifiers of +10% each.
	/// My attack is now 121.
	/// 
	/// Example 2:
	/// My attack is 100.
	/// I add 2 <see cref="Multiplicative"/> modifiers of +100% each.
	/// My attack is now 400.
	/// </example>
	Multiplicative
}

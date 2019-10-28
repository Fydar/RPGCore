namespace RPGCore.Traits
{
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
		/// I add 2 <see cref="SimpleMultiplicative"/> modifiers of +10% each.
		/// My attack is now 120.
		/// 
		/// Example 2:
		/// My attack is 100.
		/// I add 2 <see cref="SimpleMultiplicative"/> modifiers of +100% each.
		/// My attack is now 300.
		/// </example>
		SimpleMultiplicative,

		/// <example>
		/// Example 1:
		/// My attack is 100.
		/// I add 2 <see cref="CompoundMultiplicative"/> modifiers of +10% each.
		/// My attack is now 144.
		/// 
		/// Example 2:
		/// My attack is 100.
		/// I add 2 <see cref="CompoundMultiplicative"/> modifiers of +100% each.
		/// My attack is now 400.
		/// </example>
		CompoundMultiplicative
	}
}

using System;

namespace RPGCore.Data.Polymorphic
{
	/// <summary>
	/// Evaluates a <see cref="Type"/> to determine whether it meets specified conditions.
	/// </summary>
	public interface ITypeFilter
	{
		/// <summary>
		/// Determines whether the <paramref name="type"/> should be included in the set.
		/// </summary>
		/// <param name="type">The type to evaluate.</param>
		/// <returns><c>true</c> if the <paramref name="type"/> satifies the conditions of this <see cref="ITypeFilter"/>; otherwise <c>false</c>.</returns>
		bool ShouldInclude(Type type);
	}
}

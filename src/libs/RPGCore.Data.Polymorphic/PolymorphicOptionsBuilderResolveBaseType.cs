using System;
using System.Collections.Generic;

namespace RPGCore.Data.Polymorphic
{
	/// <summary>
	/// Options for how base-types should be automatically resolved.
	/// </summary>
	public class PolymorphicOptionsBuilderResolveBaseType
	{
		internal List<Action<PolymorphicOptionsBuilderExplicitType>> identiyWith = new();

		/// <summary>
		/// A filter used to exclude types from being considered valid base-type.
		/// </summary>
		public ITypeFilter? TypeFilter { get; set; }

		/// <summary>
		/// Determines whether <see cref="object"/> should be included as a valid base type.
		/// <para>Defaults to <c>false</c>.</para>
		/// </summary>
		public bool IncludeSystemObject { get; set; } = false;

		internal PolymorphicOptionsBuilderResolveBaseType()
		{
		}

		/// <summary>
		/// Adds a callback used to assign aliases to a resolved type.
		/// </summary>
		/// <param name="options">The callback to invoke to assign aliases.</param>
		public void IdentifyWith(Action<PolymorphicOptionsBuilderExplicitType> options)
		{
			identiyWith.Add(options);
		}
	}
}

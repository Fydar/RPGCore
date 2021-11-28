namespace RPGCore.Data.Polymorphic
{
	/// <summary>
	/// Options for how base-types should be automatically resolved.
	/// </summary>
	public class PolymorphicOptionsResolveBaseType
	{
		/// <summary>
		/// A filter used to exclude types from being considered valid base-type.
		/// </summary>
		public ITypeFilter? TypeFilter { get; set; }

		/// <summary>
		/// Determines whether <see cref="object"/> should be included as a valid base type.
		/// <para>Defaults to <c>false</c>.</para>
		/// </summary>
		public bool IncludeSystemObject { get; set; } = false;

		internal PolymorphicOptionsResolveBaseType()
		{
		}
	}
}

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

		internal PolymorphicOptionsResolveBaseType()
		{
		}
	}
}

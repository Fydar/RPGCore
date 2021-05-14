namespace RPGCore.Data.Polymorphic
{
	public class PolymorphicConverterFactoryOptions
	{
		/// <summary>
		/// Determines the default type name to use when none is supplied in <see cref="SerializeTypeAttribute.NamingConvention"/>.
		/// </summary>
		public TypeName DefaultNamingConvention { get; set; } = TypeName.FullName;

		/// <summary>
		/// Determines the default type alias convention to use when none is supplied in <see cref="SerializeTypeAttribute.AliasConventions"/>.
		/// </summary>
		public TypeName DefaultAliasConventions { get; set; } = TypeName.None;

		/// <summary>
		/// Determines whether type names should be case-insensitive.
		/// </summary>
		public bool CaseInsensitive { get; set; } = true;

		internal PolymorphicConverterFactoryOptions()
		{
		}
	}
}

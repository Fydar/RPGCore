namespace RPGCore.Data.Polymorphic
{
	public class PolymorphicOptions
	{
		/// <summary>
		/// Determines the name of the field that is used to determine polymorphic types.
		/// </summary>
		public string DescriminatorName { get; set; } = "$type";

		/// <summary>
		/// Determines the default type name to use when none is supplied in <see cref="SerializeTypeAttribute.NamingConvention"/>.
		/// <para>This property has a default value of <see cref="TypeName.FullName"/>.</para>
		/// </summary>
		public TypeName DefaultNamingConvention { get; set; } = TypeName.FullName;

		/// <summary>
		/// Determines the default type alias convention to use when none is supplied in <see cref="SerializeTypeAttribute.AliasConventions"/>.
		/// <para>This property has a default value of <see cref="TypeName.None"/>.</para>
		/// </summary>
		public TypeName DefaultAliasConventions { get; set; } = TypeName.None;

		/// <summary>
		/// Determines whether type names should be case-insensitive.
		/// </summary>
		public bool CaseInsensitive { get; set; } = true;

		public PolymorphicOptions()
		{
		}
	}
}

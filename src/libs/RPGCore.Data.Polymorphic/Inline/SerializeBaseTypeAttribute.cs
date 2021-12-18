using RPGCore.Data.Polymorphic.Naming;
using System;

namespace RPGCore.Data.Polymorphic.Inline
{
	/// <summary>
	/// Denotes that a type should be considered a valid base-type.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true, Inherited = false)]
	public class SerializeBaseTypeAttribute : Attribute
	{
		/// <summary>
		/// An explicit sub-type this attribute is scoped to.
		/// </summary>
		public Type? Type { get; }

		/// <summary>
		/// Identifies a singular <see cref="ITypeNamingConvention"/> to use.
		/// </summary>
		public TypeName NamingConvention { get; }

		/// <summary>
		/// Identifies one or more <see cref="ITypeNamingConvention"/> to use.
		/// </summary>
		public TypeName AliasConventions { get; }

		/// <summary>
		/// An explicit type name for the <see cref="Type"/> specified.
		/// </summary>
		public string? TypeName { get; }

		/// <summary>
		/// An explicit collection of type aliases for the <see cref="Type"/> specified.
		/// </summary>
		public string[]? TypeAliases { get; }

		/// <summary>
		/// Indicates that the type this attribute is placed on should be considered a base-type with automatically resolved sub-types.
		/// </summary>
		/// <remarks>
		/// <list type="bullet">
		/// <item>As no explicit sub-type is provided; the sub-types for this base-type will be resolved <b>automatically</b>.</item>
		/// <item>As no naming convention is provided; all sub-types name will be resolved using <see cref="PolymorphicOptionsBuilder.DefaultNamingConvention"/>.</item>
		/// <item>As no alias conventions are provided; all sub-types aliases will be resolved using <see cref="PolymorphicOptionsBuilder.DefaultAliasConventions"/>.</item>
		/// </list>
		/// </remarks>
		public SerializeBaseTypeAttribute()
		{
		}

		/// <summary>
		/// Indicates that the type this attribute is placed on should be considered a base-type with automatically resolved sub-types.
		/// </summary>
		/// <remarks>
		/// <list type="bullet">
		/// <item>As no explicit sub-type is provided; the sub-types for this base-type will be resolved <b>automatically</b>.</item>
		/// <item>The sub-types name will be resolved using the naming convention identified by <paramref name="namingConvention"/>.</item>
		/// <item>As no alias conventions are provided; all sub-types aliases will be resolved using <see cref="PolymorphicOptionsBuilder.DefaultAliasConventions"/>.</item>
		/// </list>
		/// </remarks>
		/// <param name="namingConvention">The naming convention used to source the name for resolved sub-types.</param>
		public SerializeBaseTypeAttribute(TypeName namingConvention)
		{
			NamingConvention = namingConvention;
		}

		/// <summary>
		/// Indicates that the type this attribute is placed on should be considered a base-type with automatically resolved sub-types.
		/// </summary>
		/// <remarks>
		/// <list type="bullet">
		/// <item>As no explicit sub-type is provided; the sub-types for this base-type will be resolved <b>automatically</b>.</item>
		/// <item>The sub-types name will be resolved using the naming convention identified by <paramref name="namingConvention"/>.</item>
		/// <item>The sub-types aliases will be resolved using the alias conventions identified by <paramref name="aliasConventions"/>.</item>
		/// </list>
		/// </remarks>
		/// <param name="namingConvention">The naming convention used to source the name for resolved sub-types.</param>
		/// <param name="aliasConventions">The alias conventions used to source the aliases for resolved sub-types.</param>
		public SerializeBaseTypeAttribute(TypeName namingConvention, TypeName aliasConventions)
		{
			NamingConvention = namingConvention;
			AliasConventions = aliasConventions;
		}

		/// <summary>
		/// Indicates that the type this attribute is placed on should be considered a base-type.
		/// </summary>
		/// <remarks>
		/// <list type="bullet">
		/// <item>As no explicit sub-type is provided; the sub-types for this base-type will be resolved <b>automatically</b>.</item>
		/// <item>As no naming convention is provided; all sub-types name will be resolved using <see cref="PolymorphicOptionsBuilder.DefaultNamingConvention"/>.</item>
		/// <item>As no alias conventions are provided; all sub-types aliases will be resolved using <see cref="PolymorphicOptionsBuilder.DefaultAliasConventions"/>.</item>
		/// </list>
		/// </remarks>
		/// <param name="subType">The sub-type this base-type is associated with.</param>
		public SerializeBaseTypeAttribute(Type subType)
		{
			Type = subType;
		}

		/// <summary>
		/// Indicates that the type this attribute is placed on should be considered a base-type.
		/// </summary>
		/// <remarks>
		/// <list type="bullet">
		/// <item>As no explicit sub-type is provided; the sub-types for this base-type will be resolved <b>automatically</b>.</item>
		/// <item>The sub-types name will be resolved using the naming convention identified by <paramref name="namingConvention"/>.</item>
		/// <item>As no alias conventions are provided; all sub-types aliases will be resolved using <see cref="PolymorphicOptionsBuilder.DefaultAliasConventions"/>.</item>
		/// </list>
		/// </remarks>
		/// <param name="subType">The sub-type this base-type is associated with.</param>
		/// <param name="namingConvention">The naming convention used to source the name for resolved sub-types.</param>
		public SerializeBaseTypeAttribute(Type subType, TypeName namingConvention)
		{
			Type = subType;
			NamingConvention = namingConvention;
		}

		/// <summary>
		/// Indicates that the type this attribute is placed on should be considered a base-type.
		/// </summary>
		/// <remarks>
		/// <list type="bullet">
		/// <item>As no explicit sub-type is provided; the sub-types for this base-type will be resolved <b>automatically</b>.</item>
		/// <item>The sub-types name will be resolved using the naming convention identified by <paramref name="namingConvention"/>.</item>
		/// <item>As no alias conventions are provided; all sub-types aliases will be resolved using <see cref="PolymorphicOptionsBuilder.DefaultAliasConventions"/>.</item>
		/// </list>
		/// </remarks>
		/// <param name="subType">The sub-type this base-type is associated with.</param>
		/// <param name="namingConvention">The naming convention used to source the name for resolved sub-types.</param>
		/// <param name="typeAliases">The additional aliases for explicit sub-type.</param>
		public SerializeBaseTypeAttribute(Type subType, TypeName namingConvention, params string[] typeAliases)
		{
			Type = subType;
			NamingConvention = namingConvention;
			TypeAliases = typeAliases;
		}

		/// <summary>
		/// Indicates that the type this attribute is placed on should be considered a base-type.
		/// </summary>
		/// <remarks>
		/// <list type="bullet">
		/// <item>As no explicit sub-type is provided; the sub-types for this base-type will be resolved <b>automatically</b>.</item>
		/// <item>The sub-types name will be resolved using the naming convention identified by <paramref name="namingConvention"/>.</item>
		/// <item>The sub-types aliases will be resolved using the alias conventions identified by <paramref name="aliasConventions"/>.</item>
		/// </list>
		/// </remarks>
		/// <param name="subType">The sub-type this base-type is associated with.</param>
		/// <param name="namingConvention">The naming convention used to source the name for resolved sub-types.</param>
		/// <param name="aliasConventions">The alias conventions used to source the aliases for resolved sub-types.</param>
		public SerializeBaseTypeAttribute(Type subType, TypeName namingConvention, TypeName aliasConventions)
		{
			Type = subType;
			NamingConvention = namingConvention;
			AliasConventions = aliasConventions;
		}

		/// <summary>
		/// Indicates that the type this attribute is placed on should be considered a base-type.
		/// </summary>
		/// <remarks>
		/// <list type="bullet">
		/// <item>As no explicit sub-type is provided; the sub-types for this base-type will be resolved <b>automatically</b>.</item>
		/// <item>The sub-types name will be resolved using the naming convention identified by <paramref name="namingConvention"/>.</item>
		/// <item>The sub-types aliases will be resolved using the alias conventions identified by <paramref name="aliasConventions"/>.</item>
		/// </list>
		/// </remarks>
		/// <param name="subType">The sub-type this base-type is associated with.</param>
		/// <param name="namingConvention">The naming convention used to source the name for resolved sub-types.</param>
		/// <param name="aliasConventions">The alias conventions used to source the aliases for resolved sub-types.</param>
		/// <param name="typeAliases">The additional aliases for this sub-type.</param>
		public SerializeBaseTypeAttribute(Type subType, TypeName namingConvention, TypeName aliasConventions, params string[] typeAliases)
		{
			Type = subType;
			NamingConvention = namingConvention;
			AliasConventions = aliasConventions;
			TypeAliases = typeAliases;
		}

		/// <summary>
		/// Indicates that the type this attribute is placed on should be considered a base-type.
		/// </summary>
		/// <remarks>
		/// <list type="bullet">
		/// <item>As no explicit sub-type is provided; the sub-types for this base-type will be resolved <b>automatically</b>.</item>
		/// <item>The name of the sub-type will be <paramref name="typeName"/>.</item>
		/// <item>As no alias conventions are provided; all sub-types aliases will be resolved using <see cref="PolymorphicOptionsBuilder.DefaultAliasConventions"/>.</item>
		/// </list>
		/// </remarks>
		/// <param name="subType">The sub-type this base-type is associated with.</param>
		/// <param name="typeName">A <see cref="string"/> identifier used to identity the sub-type.</param>
		public SerializeBaseTypeAttribute(Type subType, string typeName)
		{
			Type = subType;
			TypeName = typeName;
		}

		/// <summary>
		/// Indicates that the type this attribute is placed on should be considered a base-type.
		/// </summary>
		/// <remarks>
		/// <list type="bullet">
		/// <item>As no explicit sub-type is provided; the sub-types for this base-type will be resolved <b>automatically</b>.</item>
		/// <item>The name of the sub-type will be <paramref name="typeName"/>.</item>
		/// <item>As no alias conventions are provided; all sub-types aliases will be resolved the <paramref name="additionalAliases"/></item>
		/// </list>
		/// </remarks>
		/// <param name="subType">The sub-type this base-type is associated with.</param>
		/// <param name="typeName">A <see cref="string"/> identifier used to identity the sub-type.</param>
		/// <param name="additionalAliases">The additional aliases for explicit sub-type.</param>
		public SerializeBaseTypeAttribute(Type subType, string typeName, params string[] additionalAliases)
		{
			Type = subType;
			TypeName = typeName;
			TypeAliases = additionalAliases;
		}

		/// <summary>
		/// Indicates that the type this attribute is placed on should be considered a base-type.
		/// </summary>
		/// <remarks>
		/// <list type="bullet">
		/// <item>As no explicit sub-type is provided; the sub-types for this base-type will be resolved <b>automatically</b>.</item>
		/// <item>The name of the sub-type will be <paramref name="typeName"/>.</item>
		/// <item>The sub-types aliases will be resolved using the alias conventions identified by <paramref name="aliasConventions"/>.</item>
		/// </list>
		/// </remarks>
		/// <param name="subType">The sub-type this base-type is associated with.</param>
		/// <param name="typeName">A <see cref="string"/> identifier used to identity the sub-type.</param>
		/// <param name="aliasConventions">The alias conventions used to source the aliases for resolved sub-types.</param>
		public SerializeBaseTypeAttribute(Type subType, string typeName, TypeName aliasConventions)
		{
			Type = subType;
			TypeName = typeName;
			AliasConventions = aliasConventions;
		}

		/// <summary>
		/// Indicates that the type this attribute is placed on should be considered a base-type.
		/// </summary>
		/// <remarks>
		/// <list type="bullet">
		/// <item>As no explicit sub-type is provided; the sub-types for this base-type will be resolved <b>automatically</b>.</item>
		/// <item>The name of the sub-type will be <paramref name="typeName"/>.</item>
		/// <item>The sub-types aliases will be resolved using the alias conventions identified by <paramref name="aliasConventions"/> and the <paramref name="additionalAliases"/>.</item>
		/// </list>
		/// </remarks>
		/// <param name="subType">The sub-type this base-type is associated with.</param>
		/// <param name="typeName">A <see cref="string"/> identifier used to identity the sub-type.</param>
		/// <param name="aliasConventions">The alias conventions used to source the aliases for resolved sub-types.</param>
		/// <param name="additionalAliases">The additional aliases for explicit sub-type.</param>
		public SerializeBaseTypeAttribute(Type subType, string typeName, TypeName aliasConventions, params string[] additionalAliases)
		{
			Type = subType;
			TypeName = typeName;
			AliasConventions = aliasConventions;
			TypeAliases = additionalAliases;
		}
	}
}

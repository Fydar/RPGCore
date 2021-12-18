using RPGCore.Data.Polymorphic.Naming;
using System;

namespace RPGCore.Data.Polymorphic.Inline
{
	/// <summary>
	/// Denotes that a type should be considered a valid sub-type.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
	public class SerializeSubTypeAttribute : Attribute
	{
		/// <summary>
		/// An explicit base-type this attribute is scoped to.
		/// </summary>
		public Type? ExplicitBaseType { get; }

		/// <summary>
		/// Identifies a singular <see cref="ITypeNamingConvention"/> to use.
		/// </summary>
		public TypeName NamingConvention { get; }

		/// <summary>
		/// Identifies one or more <see cref="ITypeNamingConvention"/> to use.
		/// </summary>
		public TypeName AliasConventions { get; }

		/// <summary>
		/// An explicit type name for the <see cref="ExplicitBaseType"/> specified.
		/// </summary>
		public string? TypeName { get; }

		/// <summary>
		/// An explicit collection of type aliases for the <see cref="ExplicitBaseType"/> specified.
		/// </summary>
		public string[]? TypeAliases { get; }

		/// <summary>
		/// Indicates that the type this attribute is placed on should be considered a sub-type.
		/// </summary>
		/// <remarks>
		/// <list type="bullet">
		/// <item>As no explicit base-type is provided; the base-types for this sub-type will be resolved <b>automatically</b>.</item>
		/// <item>As naming convention is provided; the sub-types name will be resolved using <see cref="PolymorphicOptionsBuilder.DefaultNamingConvention"/>.</item>
		/// <item>As alias conventions are provided; the sub-types aliases will be resolved using <see cref="PolymorphicOptionsBuilder.DefaultAliasConventions"/>.</item>
		/// </list>
		/// </remarks>
		public SerializeSubTypeAttribute()
		{
		}

		/// <summary>
		/// Indicates that the type this attribute is placed on should be considered a sub-type.
		/// </summary>
		/// <remarks>
		/// <list type="bullet">
		/// <item>As no explicit base-type is provided; the base-types for this sub-type will be resolved <b>automatically</b>.</item>
		/// <item>The sub-types name will be resolved using the naming convention identified by <paramref name="namingConvention"/>.</item>
		/// <item>As no alias conventions are provided; the sub-types aliases will be resolved using <see cref="PolymorphicOptionsBuilder.DefaultAliasConventions"/>.</item>
		/// </list>
		/// </remarks>
		/// <param name="namingConvention">The naming convention used to source the name for this sub-type.</param>
		public SerializeSubTypeAttribute(TypeName namingConvention)
		{
			NamingConvention = namingConvention;
		}

		/// <summary>
		/// Indicates that the type this attribute is placed on should be considered a sub-type.
		/// </summary>
		/// <remarks>
		/// <list type="bullet">
		/// <item>As no explicit base-type is provided; the base-types for this sub-type will be resolved <b>automatically</b>.</item>
		/// <item>The sub-types name will be resolved using the naming convention identified by <paramref name="namingConvention"/>.</item>
		/// <item>The sub-types aliases will be source from <paramref name="additionalAliases"/>.</item>
		/// </list>
		/// </remarks>
		/// <param name="namingConvention">The naming convention used to source the name for this sub-type.</param>
		/// <param name="additionalAliases">The additional aliases for this sub-type.</param>
		public SerializeSubTypeAttribute(TypeName namingConvention, string[] additionalAliases)
		{
			NamingConvention = namingConvention;
			TypeAliases = additionalAliases;
		}

		/// <summary>
		/// Indicates that the type this attribute is placed on should be considered a sub-type.
		/// </summary>
		/// <remarks>
		/// <list type="bullet">
		/// <item>As no explicit base-type is provided; the base-types for this sub-type will be resolved <b>automatically</b>.</item>
		/// <item>The sub-types name will be resolved using the naming convention identified by <paramref name="namingConvention"/>.</item>
		/// <item>The sub-types aliases will be resolved using the alias conventions identified by <paramref name="aliasConventions"/>.</item>
		/// </list>
		/// </remarks>
		/// <param name="namingConvention">The naming convention used to source the name for this sub-type.</param>
		/// <param name="aliasConventions">The alias conventions used to source the aliases for this sub-type.</param>
		public SerializeSubTypeAttribute(TypeName namingConvention, TypeName aliasConventions)
		{
			NamingConvention = namingConvention;
			AliasConventions = aliasConventions;
		}

		/// <summary>
		/// Indicates that the type this attribute is placed on should be considered a sub-type.
		/// </summary>
		/// <remarks>
		/// <list type="bullet">
		/// <item>As no explicit base-type is provided; the base-types for this sub-type will be resolved <b>automatically</b>.</item>
		/// <item>The sub-types name will be resolved using the naming convention identified by <paramref name="namingConvention"/>.</item>
		/// <item>The sub-types aliases will be resolved using the alias conventions identified by <paramref name="aliasConventions"/> and the <paramref name="additionalAliases"/> provided.</item>
		/// </list>
		/// </remarks>
		/// <param name="namingConvention">The naming convention used to source the name for this sub-type.</param>
		/// <param name="aliasConventions">The alias conventions used to source the aliases for this sub-type.</param>
		/// <param name="additionalAliases">The additional aliases for this sub-type.</param>
		public SerializeSubTypeAttribute(TypeName namingConvention, TypeName aliasConventions, params string[] additionalAliases)
		{
			NamingConvention = namingConvention;
			AliasConventions = aliasConventions;
			TypeAliases = additionalAliases;
		}

		/// <summary>
		/// Indicates that the type this attribute is placed on should be considered a sub-type.
		/// </summary>
		/// <remarks>
		/// <list type="bullet">
		/// <item>As no explicit base-type is provided; the base-types for this sub-type will be resolved <b>automatically</b>.</item>
		/// <item>The name of the type will source from <paramref name="name"/>.</item>
		/// <item>As no alias or alias conventions are provided; the sub-types aliases will be resolved using <see cref="PolymorphicOptionsBuilder.DefaultAliasConventions"/>.</item>
		/// </list>
		/// </remarks>
		/// <param name="name">The name of the sub-type.</param>
		public SerializeSubTypeAttribute(string name)
		{
			TypeName = name;
		}

		/// <summary>
		/// Indicates that the type this attribute is placed on should be considered a sub-type.
		/// </summary>
		/// <remarks>
		/// <list type="bullet">
		/// <item>As no explicit base-type is provided; the base-types for this sub-type will be resolved <b>automatically</b>.</item>
		/// <item>The name of the type will source from <paramref name="name"/>.</item>
		/// <item>The sub-types aliases will be source from <paramref name="additionalAliases"/>.</item>
		/// </list>
		/// </remarks>
		/// <param name="name">The name of the this sub-type.</param>
		/// <param name="additionalAliases">The additional aliases for this sub-type.</param>
		public SerializeSubTypeAttribute(string name, params string[] additionalAliases)
		{
			TypeName = name;
			TypeAliases = additionalAliases;
		}

		/// <summary>
		/// Indicates that the type this attribute is placed on should be considered a sub-type.
		/// </summary>
		/// <remarks>
		/// <list type="bullet">
		/// <item>As no explicit base-type is provided; the base-types for this sub-type will be resolved <b>automatically</b>.</item>
		/// <item>The name of the type will source from <paramref name="name"/>.</item>
		/// <item>The sub-types aliases will be resolved using the alias conventions identified by <paramref name="aliasConventions"/>.</item>
		/// </list>
		/// </remarks>
		/// <param name="name">The name of the this sub-type.</param>
		/// <param name="aliasConventions">The alias conventions used to source the aliases for this sub-type.</param>
		public SerializeSubTypeAttribute(string name, TypeName aliasConventions)
		{
			TypeName = name;
			AliasConventions = aliasConventions;
		}

		/// <summary>
		/// Indicates that the type this attribute is placed on should be considered a sub-type.
		/// </summary>
		/// <remarks>
		/// <list type="bullet">
		/// <item>As no explicit base-type is provided; the base-types for this sub-type will be resolved <b>automatically</b>.</item>
		/// <item>The name of the type will source from <paramref name="name"/>.</item>
		/// <item>The sub-types aliases will be resolved using the alias conventions identified by <paramref name="aliasConventions"/> and the <paramref name="additionalAliases"/> provided.</item>
		/// </list>
		/// </remarks>
		/// <param name="name">The name of the this sub-type.</param>
		/// <param name="aliasConventions">The alias conventions used to source the aliases for this sub-type.</param>
		/// <param name="additionalAliases">The additional aliases for this sub-type.</param>
		public SerializeSubTypeAttribute(string name, TypeName aliasConventions, params string[] additionalAliases)
		{
			TypeName = name;
			AliasConventions = aliasConventions;
			TypeAliases = additionalAliases;
		}


		/// <summary>
		/// Indicates that the type this attribute is placed on should be considered a sub-type.
		/// </summary>
		/// <remarks>
		/// <list type="bullet">
		/// <item>The base-type for this sub-type will be source from <paramref name="baseType"/>.</item>
		/// <item>As no name or naming convention is provided; the sub-types name will be resolved using <see cref="PolymorphicOptionsBuilder.DefaultNamingConvention"/>.</item>
		/// <item>As no alias or alias conventions are provided; the sub-types aliases will be resolved using <see cref="PolymorphicOptionsBuilder.DefaultAliasConventions"/>.</item>
		/// </list>
		/// </remarks>
		/// <param name="baseType">The base-type this sub-type is associated with.</param>
		public SerializeSubTypeAttribute(Type baseType)
		{
			ExplicitBaseType = baseType;
		}

		/// <summary>
		/// Indicates that the type this attribute is placed on should be considered a sub-type.
		/// </summary>
		/// <remarks>
		/// <list type="bullet">
		/// <item>The base-type for this sub-type will be source from <paramref name="baseType"/>.</item>
		/// <item>The sub-types name will be resolved using the naming convention identified by <paramref name="namingConvention"/>.</item>
		/// <item>As no alias or alias conventions are provided; the sub-types aliases will be resolved using <see cref="PolymorphicOptionsBuilder.DefaultAliasConventions"/>.</item>
		/// </list>
		/// </remarks>
		/// <param name="baseType">The base-type this sub-type is associated with.</param>
		/// <param name="namingConvention">The naming convention used to source the name for this sub-type.</param>
		public SerializeSubTypeAttribute(Type baseType, TypeName namingConvention)
		{
			ExplicitBaseType = baseType;
			NamingConvention = namingConvention;
		}

		/// <summary>
		/// Indicates that the type this attribute is placed on should be considered a sub-type.
		/// </summary>
		/// <remarks>
		/// <list type="bullet">
		/// <item>The base-type for this sub-type will be source from <paramref name="baseType"/>.</item>
		/// <item>The sub-types name will be resolved using the naming convention identified by <paramref name="namingConvention"/>.</item>
		/// <item>The sub-types aliases will be source from <paramref name="additionalAliases"/>.</item>
		/// </list>
		/// </remarks>
		/// <param name="baseType">The base-type this sub-type is associated with.</param>
		/// <param name="namingConvention">The naming convention used to source the name for this sub-type.</param>
		/// <param name="additionalAliases">The additional aliases for this sub-type.</param>
		public SerializeSubTypeAttribute(Type baseType, TypeName namingConvention, string[] additionalAliases)
		{
			ExplicitBaseType = baseType;
			NamingConvention = namingConvention;
			TypeAliases = additionalAliases;
		}

		/// <summary>
		/// Indicates that the type this attribute is placed on should be considered a sub-type.
		/// </summary>
		/// <remarks>
		/// <list type="bullet">
		/// <item>The base-type for this sub-type will be source from <paramref name="baseType"/>.</item>
		/// <item>The sub-types name will be resolved using the naming convention identified by <paramref name="namingConvention"/>.</item>
		/// <item>The sub-types aliases will be resolved using the alias conventions identified by <paramref name="aliasConventions"/>.</item>
		/// </list>
		/// </remarks>
		/// <param name="baseType">The base-type this sub-type is associated with.</param>
		/// <param name="namingConvention">The naming convention used to source the name for this sub-type.</param>
		/// <param name="aliasConventions">The alias conventions used to source the aliases for this sub-type.</param>
		public SerializeSubTypeAttribute(Type baseType, TypeName namingConvention, TypeName aliasConventions)
		{
			ExplicitBaseType = baseType;
			NamingConvention = namingConvention;
			AliasConventions = aliasConventions;
		}

		/// <summary>
		/// Indicates that the type this attribute is placed on should be considered a sub-type.
		/// </summary>
		/// <remarks>
		/// <list type="bullet">
		/// <item>The base-type for this sub-type will be source from <paramref name="baseType"/>.</item>
		/// <item>The sub-types name will be resolved using the naming convention identified by <paramref name="namingConvention"/>.</item>
		/// <item>The sub-types aliases will be resolved using the alias conventions identified by <paramref name="aliasConventions"/> and the <paramref name="additionalAliases"/> provided.</item>
		/// </list>
		/// </remarks>
		/// <param name="baseType">The base-type this sub-type is associated with.</param>
		/// <param name="namingConvention">The naming convention used to source the name for this sub-type.</param>
		/// <param name="aliasConventions">The alias conventions used to source the aliases for this sub-type.</param>
		/// <param name="additionalAliases">The additional aliases for this sub-type.</param>
		public SerializeSubTypeAttribute(Type baseType, TypeName namingConvention, TypeName aliasConventions, params string[] additionalAliases)
		{
			ExplicitBaseType = baseType;
			NamingConvention = namingConvention;
			AliasConventions = aliasConventions;
			TypeAliases = additionalAliases;
		}

		/// <summary>
		/// Indicates that the type this attribute is placed on should be considered a sub-type.
		/// </summary>
		/// <remarks>
		/// <list type="bullet">
		/// <item>The base-type for this sub-type will be source from <paramref name="baseType"/>.</item>
		/// <item>The name of the type will source from <paramref name="name"/>.</item>
		/// <item>As no alias or alias conventions are provided; the sub-types aliases will be resolved using <see cref="PolymorphicOptionsBuilder.DefaultAliasConventions"/>.</item>
		/// </list>
		/// </remarks>
		/// <param name="baseType">The base-type this sub-type is associated with.</param>
		/// <param name="name">The name of the sub-type.</param>
		public SerializeSubTypeAttribute(Type baseType, string name)
		{
			ExplicitBaseType = baseType;
			TypeName = name;
		}

		/// <summary>
		/// Indicates that the type this attribute is placed on should be considered a sub-type.
		/// </summary>
		/// <remarks>
		/// <list type="bullet">
		/// <item>The base-type for this sub-type will be source from <paramref name="baseType"/>.</item>
		/// <item>The name of the type will source from <paramref name="name"/>.</item>
		/// <item>The sub-types aliases will be source from <paramref name="additionalAliases"/>.</item>
		/// </list>
		/// </remarks>
		/// <param name="baseType">The base-type this sub-type is associated with.</param>
		/// <param name="name">The name of the this sub-type.</param>
		/// <param name="additionalAliases">The additional aliases for this sub-type.</param>
		public SerializeSubTypeAttribute(Type baseType, string name, params string[] additionalAliases)
		{
			ExplicitBaseType = baseType;
			TypeName = name;
			TypeAliases = additionalAliases;
		}

		/// <summary>
		/// Indicates that the type this attribute is placed on should be considered a sub-type.
		/// </summary>
		/// <remarks>
		/// <list type="bullet">
		/// <item>The base-type for this sub-type will be source from <paramref name="baseType"/>.</item>
		/// <item>The name of the type will source from <paramref name="name"/>.</item>
		/// <item>The sub-types aliases will be resolved using the alias conventions identified by <paramref name="aliasConventions"/>.</item>
		/// </list>
		/// </remarks>
		/// <param name="baseType">The base-type this sub-type is associated with.</param>
		/// <param name="name">The name of the this sub-type.</param>
		/// <param name="aliasConventions">The alias conventions used to source the aliases for this sub-type.</param>
		public SerializeSubTypeAttribute(Type baseType, string name, TypeName aliasConventions)
		{
			ExplicitBaseType = baseType;
			TypeName = name;
			AliasConventions = aliasConventions;
		}

		/// <summary>
		/// Indicates that the type this attribute is placed on should be considered a sub-type.
		/// </summary>
		/// <remarks>
		/// <list type="bullet">
		/// <item>The base-type for this sub-type will be source from <paramref name="baseType"/>.</item>
		/// <item>The name of the type will source from <paramref name="name"/>.</item>
		/// <item>The sub-types aliases will be resolved using the alias conventions identified by <paramref name="aliasConventions"/> and the <paramref name="additionalAliases"/> provided.</item>
		/// </list>
		/// </remarks>
		/// <param name="baseType">The base-type this sub-type is associated with.</param>
		/// <param name="name">The name of the this sub-type.</param>
		/// <param name="aliasConventions">The alias conventions used to source the aliases for this sub-type.</param>
		/// <param name="additionalAliases">The additional aliases for this sub-type.</param>
		public SerializeSubTypeAttribute(Type baseType, string name, TypeName aliasConventions, params string[] additionalAliases)
		{
			ExplicitBaseType = baseType;
			TypeName = name;
			AliasConventions = aliasConventions;
			TypeAliases = additionalAliases;
		}
	}
}

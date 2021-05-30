using System;

namespace RPGCore.Data.Polymorphic.Inline
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true, Inherited = false)]
	public class SerializeThisTypeAttribute : Attribute
	{
		public Type? ExplicitBaseType { get; }
		public TypeName NamingConvention { get; }
		public TypeName AliasConventions { get; }
		public string? TypeName { get; }
		public string[]? TypeAliases { get; }

		public SerializeThisTypeAttribute()
		{
		}

		public SerializeThisTypeAttribute(TypeName namingConvention)
		{
			NamingConvention = namingConvention;
		}

		public SerializeThisTypeAttribute(TypeName namingConvention, TypeName aliasConventions)
		{
			NamingConvention = namingConvention;
			AliasConventions = aliasConventions;
		}

		public SerializeThisTypeAttribute(Type type)
		{
			ExplicitBaseType = type;
		}

		public SerializeThisTypeAttribute(Type type, TypeName namingConvention)
		{
			NamingConvention = namingConvention;
			ExplicitBaseType = type;
		}

		public SerializeThisTypeAttribute(Type type, TypeName namingConvention, params string[] typeAliases)
		{
			ExplicitBaseType = type;
			NamingConvention = namingConvention;
			TypeAliases = typeAliases;
		}

		public SerializeThisTypeAttribute(Type type, TypeName namingConvention, TypeName aliasConventions)
		{
			ExplicitBaseType = type;
			NamingConvention = namingConvention;
			AliasConventions = aliasConventions;
		}

		public SerializeThisTypeAttribute(Type type, TypeName namingConvention, TypeName aliasConventions, params string[] typeAliases)
		{
			ExplicitBaseType = type;
			NamingConvention = namingConvention;
			AliasConventions = aliasConventions;
			TypeAliases = typeAliases;
		}

		public SerializeThisTypeAttribute(Type type, string typeName)
		{
			ExplicitBaseType = type;
			TypeName = typeName;
		}

		public SerializeThisTypeAttribute(Type type, string typeName, params string[] additionalAliases)
		{
			ExplicitBaseType = type;
			TypeName = typeName;
			TypeAliases = additionalAliases;
		}

		public SerializeThisTypeAttribute(Type type, string typeName, TypeName aliasConventions)
		{
			ExplicitBaseType = type;
			TypeName = typeName;
			AliasConventions = aliasConventions;
		}

		public SerializeThisTypeAttribute(Type type, string typeName, TypeName aliasConventions, params string[] additionalAliases)
		{
			ExplicitBaseType = type;
			TypeName = typeName;
			AliasConventions = aliasConventions;
			TypeAliases = additionalAliases;
		}
	}
}

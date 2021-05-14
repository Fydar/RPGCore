using System;

namespace RPGCore.Data.Polymorphic
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true, Inherited = false)]
	public class SerializeTypeAttribute : Attribute
	{
		public Type? Type { get; }
		public TypeName NamingConvention { get; }
		public TypeName AliasConventions { get; }
		public string? TypeName { get; }
		public string[]? TypeAliases { get; }

		public SerializeTypeAttribute()
		{
		}

		public SerializeTypeAttribute(TypeName namingConvention)
		{
			NamingConvention = namingConvention;
		}

		public SerializeTypeAttribute(TypeName namingConvention, TypeName aliasConventions)
		{
			NamingConvention = namingConvention;
			AliasConventions = aliasConventions;
		}

		public SerializeTypeAttribute(Type type)
		{
			Type = type;
		}

		public SerializeTypeAttribute(Type type, TypeName namingConvention)
		{
			NamingConvention = namingConvention;
			Type = type;
		}

		public SerializeTypeAttribute(Type type, TypeName namingConvention, params string[] typeAliases)
		{
			Type = type;
			NamingConvention = namingConvention;
			TypeAliases = typeAliases;
		}

		public SerializeTypeAttribute(Type type, TypeName namingConvention, TypeName aliasConventions)
		{
			Type = type;
			NamingConvention = namingConvention;
			AliasConventions = aliasConventions;
		}

		public SerializeTypeAttribute(Type type, TypeName namingConvention, TypeName aliasConventions, params string[] typeAliases)
		{
			Type = type;
			NamingConvention = namingConvention;
			AliasConventions = aliasConventions;
			TypeAliases = typeAliases;
		}

		public SerializeTypeAttribute(Type type, string typeName)
		{
			Type = type;
			TypeName = typeName;
		}

		public SerializeTypeAttribute(Type type, string typeName, params string[] additionalAliases)
		{
			Type = type;
			TypeName = typeName;
			TypeAliases = additionalAliases;
		}

		public SerializeTypeAttribute(Type type, string typeName, TypeName aliasConventions)
		{
			Type = type;
			TypeName = typeName;
			AliasConventions = aliasConventions;
		}

		public SerializeTypeAttribute(Type type, string typeName, TypeName aliasConventions, params string[] additionalAliases)
		{
			Type = type;
			TypeName = typeName;
			AliasConventions = aliasConventions;
			TypeAliases = additionalAliases;
		}
	}
}

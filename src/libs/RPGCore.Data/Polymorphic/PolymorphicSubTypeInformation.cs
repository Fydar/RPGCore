using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RPGCore.Data.Polymorphic
{
	public class PolymorphicSubTypeInformation
	{
		public Type Type { get; set; }
		public string Name { get; set; }
		public string[] Aliases { get; set; }

		public PolymorphicSubTypeInformation(Type type)
		{
			Type = type;
			Name = type.FullName;
			Aliases = Array.Empty<string>();
		}

		public bool DoesNameMatch(string name, bool caseInsentitive)
		{
			var comparison = caseInsentitive ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;

			if (string.Equals(Name, name, comparison))
			{
				return true;
			}

			foreach (string alias in Aliases)
			{
				if (string.Equals(alias, name, comparison))
				{
					return true;
				}
			}
			return false;
		}

		public static IReadOnlyList<PolymorphicSubTypeInformation> GetUserDefinedOptions(Type baseType, TypeName defaultNamingConvention, TypeName defaultAliasConvention)
		{
			static PolymorphicSubTypeInformation GetOrCreateForType(List<PolymorphicSubTypeInformation> collection, Type type)
			{
				foreach (var typeInfo in collection)
				{
					if (typeInfo.Type == type)
					{
						return typeInfo;
					}
				}
				var newTypeInfo = new PolymorphicSubTypeInformation(type);
				collection.Add(newTypeInfo);
				return newTypeInfo;
			}

			var userDefinedTypeNames = new List<PolymorphicSubTypeInformation>();

			object[] attributes = baseType.GetCustomAttributes(typeof(SerializeTypeAttribute), false);

			bool anyAttributesImplicit = false;
			foreach (object attribute in attributes)
			{
				var serializeTypeAttribute = (SerializeTypeAttribute)attribute;
				if (serializeTypeAttribute.Type == null)
				{
					anyAttributesImplicit = true;
					break;
				}
			}

			if (anyAttributesImplicit)
			{
				foreach (var assembly in GetDependentAssemblies(AppDomain.CurrentDomain, baseType.Assembly))
				{
					var types = assembly.GetTypes();
					foreach (var type in types)
					{
						if (baseType.IsAssignableFrom(type))
						{
							foreach (object attribute in attributes)
							{
								var serializeTypeAttribute = (SerializeTypeAttribute)attribute;
								if (serializeTypeAttribute.Type == null)
								{
									continue;
								}

								var polymorphicType = GetOrCreateForType(userDefinedTypeNames, serializeTypeAttribute.Type);
								AddAliasesFromAttribute(polymorphicType, serializeTypeAttribute, defaultNamingConvention, defaultAliasConvention);
							}
						}
					}
				}
			}

			foreach (object attribute in attributes)
			{
				var serializeTypeAttribute = (SerializeTypeAttribute)attribute;
				if (serializeTypeAttribute.Type == null)
				{
					continue;
				}

				var polymorphicType = GetOrCreateForType(userDefinedTypeNames, serializeTypeAttribute.Type);
				AddAliasesFromAttribute(polymorphicType, serializeTypeAttribute, defaultNamingConvention, defaultAliasConvention);
			}

			return userDefinedTypeNames;
		}

		private static IEnumerable<Assembly> GetDependentAssemblies(AppDomain appDomain, Assembly sourceAssembly)
		{
			bool Predicate(Assembly assembly)
			{
				return IsDependentAssemblies(assembly, sourceAssembly);
			}

			return appDomain.GetAssemblies().Where(Predicate);
		}

		private static bool IsDependentAssemblies(Assembly otherAssembly, Assembly sourceAssembly)
		{
			return otherAssembly == sourceAssembly
				|| otherAssembly.GetReferencedAssemblies()
					.Select(assemblyName => assemblyName.FullName)
					.Contains(sourceAssembly.FullName);
		}

		private static void AddAliasesFromAttribute(PolymorphicSubTypeInformation typeInfo, SerializeTypeAttribute attribute, TypeName defaultNamingConvention, TypeName defaultAliasConvention)
		{
			string? explicitTypeName = attribute.TypeName;
			if (string.IsNullOrEmpty(explicitTypeName))
			{
				var namingConvention = attribute.NamingConvention;
				if (namingConvention == TypeName.None)
				{
					namingConvention = defaultNamingConvention;
				}
				foreach (string? name in GetAliasesFromConvention(typeInfo, namingConvention))
				{
					typeInfo.Name = name;
					break;
				}
			}
			else
			{
				typeInfo.Name = explicitTypeName!;
			}

			var aliases = new List<string>();

			var aliasConventions = attribute.AliasConventions;
			if (aliasConventions == TypeName.None)
			{
				aliasConventions = defaultAliasConvention;
			}
			foreach (string alias in GetAliasesFromConvention(typeInfo, aliasConventions))
			{
				aliases.Add(alias);
			}

			if (attribute.TypeAliases != null)
			{
				foreach (string? alias in attribute.TypeAliases)
				{
					aliases.Add(alias);
				}
			}

			typeInfo.Aliases = aliases.ToArray();
		}

		private static IEnumerable<string> GetAliasesFromConvention(PolymorphicSubTypeInformation typeInfo, TypeName aliasConventions)
		{
			if (aliasConventions.HasFlag(TypeName.FullName))
			{
				yield return typeInfo.Type.FullName;
			}
			if (aliasConventions.HasFlag(TypeName.Name))
			{
				yield return typeInfo.Type.Name;
			}
			if (aliasConventions.HasFlag(TypeName.AssemblyQualifiedName))
			{
				var assemblyName = typeInfo.Type.Assembly.GetName();
				yield return $"{typeInfo.Type.FullName}, {assemblyName.Name}";
			}
			if (aliasConventions.HasFlag(TypeName.GUID)
				&& typeInfo.Type.GUID != new Guid())
			{
				yield return typeInfo.Type.GUID.ToString();
			}
		}
	}
}

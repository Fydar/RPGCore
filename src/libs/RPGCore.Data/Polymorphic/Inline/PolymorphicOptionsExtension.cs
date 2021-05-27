using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RPGCore.Data.Polymorphic
{


	public static class PolymorphicOptionsExtension
	{
		public static PolymorphicOptions UseInline(this PolymorphicOptions options)
		{
			return UseInlineInternal(options, new InlinePolymorphicOptions());
		}

		public static PolymorphicOptions UseInline(this PolymorphicOptions options, Action<InlinePolymorphicOptions> inlinePolymorphicOptions)
		{
			var inlinePolymorphicOptionsResult = new InlinePolymorphicOptions();

			inlinePolymorphicOptions.Invoke(inlinePolymorphicOptionsResult);

			return UseInlineInternal(options, inlinePolymorphicOptionsResult);
		}

		private static PolymorphicOptions UseInlineInternal(PolymorphicOptions options, InlinePolymorphicOptions inlinePolymorphicOptions)
		{
			var assemblies = GetDependentAssemblies(AppDomain.CurrentDomain, typeof(SerializeTypeAttribute).Assembly).ToList();

			foreach (var assembly in assemblies)
			{
				var types = assembly.GetTypes();
				foreach (var type in types)
				{
					var attributes = type.GetCustomAttributes<SerializeTypeAttribute>(false);

					foreach (var attribute in attributes)
					{
						if (attribute.Type == null)
						{
							options.UseKnownBaseType(type, options =>
							{

							});
						}
						else
						{
							options.UseKnownBaseType(type, baseType =>
							{
								baseType.KnownSubtypes.Add(attribute.Type);

								options.UseKnownSubType(attribute.Type, subType =>
								{
									AddAliasesFromAttribute(subType, attribute, inlinePolymorphicOptions);
								});
							});
						}
					}
				}
			}

			return options;
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

			var attributes = baseType.GetCustomAttributes<SerializeTypeAttribute>(false);

			bool anyAttributesImplicit = false;
			foreach (var attribute in attributes)
			{
				if (attribute.Type == null)
				{
					anyAttributesImplicit = true;
					break;
				}
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

		private static void AddAliasesFromAttribute(PolymorphicSubTypeInfo typeInfo, SerializeTypeAttribute attribute, InlinePolymorphicOptions inlinePolymorphicOptions)
		{
			string? explicitTypeName = attribute.TypeName;
			if (string.IsNullOrEmpty(explicitTypeName))
			{
				var namingConvention = attribute.NamingConvention;
				if (namingConvention == TypeName.None)
				{
					namingConvention = inlinePolymorphicOptions.DefaultNamingConvention;
				}
				foreach (string? name in GetAliasesFromConvention(typeInfo, namingConvention))
				{
					typeInfo.Descriminator = name;
					break;
				}
			}
			else
			{
				typeInfo.Descriminator = explicitTypeName!;
			}

			var aliases = new List<string>();

			var aliasConventions = attribute.AliasConventions;
			if (aliasConventions == TypeName.None)
			{
				aliasConventions = inlinePolymorphicOptions.DefaultAliasConventions;
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

			typeInfo.Aliases.AddRange(aliases);
		}

		private static IEnumerable<string> GetAliasesFromConvention(PolymorphicSubTypeInfo typeInfo, TypeName aliasConventions)
		{
			if (aliasConventions.HasFlag(TypeName.FullName))
			{
				yield return typeInfo.SubType.FullName;
			}
			if (aliasConventions.HasFlag(TypeName.Name))
			{
				yield return typeInfo.SubType.Name;
			}
			if (aliasConventions.HasFlag(TypeName.AssemblyQualifiedName))
			{
				var assemblyName = typeInfo.SubType.Assembly.GetName();
				yield return $"{typeInfo.SubType.FullName}, {assemblyName.Name}";
			}
			if (aliasConventions.HasFlag(TypeName.GUID)
				&& typeInfo.SubType.GUID != new Guid())
			{
				yield return typeInfo.SubType.GUID.ToString();
			}
		}
	}
}

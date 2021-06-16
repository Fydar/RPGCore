using RPGCore.Data.Polymorphic.Naming;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RPGCore.Data.Polymorphic.Configuration
{
	public class PolymorphicConfiguration
	{
		/// <summary>
		/// Determines the name of the field that is used to determine polymorphic types.
		/// </summary>
		public string DescriminatorName { get; set; } = "$type";

		/// <summary>
		/// Determines whether type names should be case-insensitive.
		/// </summary>
		public bool CaseInsensitive { get; set; } = true;

		internal Dictionary<Type, PolymorphicConfigurationBaseType> baseTypes = new();
		internal Dictionary<Type, List<PolymorphicConfigurationSubType>> subTypes = new();

		internal PolymorphicConfiguration(PolymorphicOptions options)
		{
			DescriminatorName = options.DescriminatorName;
			CaseInsensitive = options.CaseInsensitive;

			var searchScope = GetDependentAssemblies(AppDomain.CurrentDomain.GetAssemblies(), typeof(PolymorphicOptions).Assembly)
				.ToList();

			// Use the names from the automatically resolved sub-types.
			foreach (var knownBaseType in options.knownBaseTypes.Values)
			{
				if (knownBaseType.resolveSubTypeOptions != null)
				{
					var baseType = GetOrCreateBaseType(knownBaseType.BaseType);

					foreach (var foundSubType in FindAllSubTypes(searchScope, knownBaseType.BaseType, knownBaseType.resolveSubTypeOptions.TypeFilter))
					{
						var subType = GetOrCreateSubTypeInBase(foundSubType, baseType);

						subType.Name = knownBaseType.resolveSubTypeOptions.TypeNaming.GetNameForType(foundSubType);
						subType.Aliases = GetAllNames(foundSubType, knownBaseType.resolveSubTypeOptions.TypeAliasing);
					}
				}
			}

			// Use resolved names defined on the sub-type.
			foreach (var knownSubType in options.knownSubTypes.Values)
			{
				if (knownSubType.resolveBaseTypeOptions != null)
				{
					foreach (var foundBaseType in FindAllBaseTypes(knownSubType.SubType, knownSubType.resolveBaseTypeOptions.TypeFilter))
					{
						var baseType = GetOrCreateBaseType(foundBaseType);
						var subType = GetOrCreateSubTypeInBase(knownSubType.SubType, baseType);
					}
				}
			}

			// Use explicit names defines on the sub-type, where the base-type is defined.
			foreach (var knownSubType in options.knownSubTypes.Values)
			{
				foreach (var knownBaseType in knownSubType.knownBaseTypes.Values)
				{
					var baseType = GetOrCreateBaseType(knownBaseType.BaseType);
					var subType = GetOrCreateSubTypeInBase(knownSubType.SubType, baseType);

					if (knownSubType.Descriminator != null)
					{
						subType.Name = knownSubType.Descriminator;
					}
					else
					{
						subType.Name = options.DefaultNamingConvention.GetNameForType(knownSubType.SubType);
					}
				}
			}

			// Use explicit names defined on the sub-type.
			// TODO: Throw an exception if the type already has an explicit name.
			foreach (var knownBaseType in options.knownBaseTypes.Values)
			{
				var baseType = GetOrCreateBaseType(knownBaseType.BaseType);

				foreach (var knownSubType in knownBaseType.knownSubTypes.Values)
				{
					var subType = GetOrCreateSubTypeInBase(knownSubType.SubType, baseType);

					if (knownSubType.Descriminator != null)
					{
						subType.Name = knownSubType.Descriminator;
					}
					else
					{
						subType.Name = options.DefaultNamingConvention.GetNameForType(knownSubType.SubType);
					}
				}
			}
		}
		public bool TryGetBaseType(Type key, out PolymorphicConfigurationBaseType value)
		{
			return baseTypes.TryGetValue(key, out value);
		}

		public bool TryGetSubType(Type key, out List<PolymorphicConfigurationSubType> value)
		{
			return subTypes.TryGetValue(key, out value);
		}

		private static string[] GetAllNames(Type type, IReadOnlyList<ITypeNamingConvention> conventions)
		{
			if (conventions.Count == 0)
			{
				return Array.Empty<string>();
			}

			string[] names = new string[conventions.Count];
			for (int i = 0; i < conventions.Count; i++)
			{
				names[i] = conventions[i].GetNameForType(type);
			}
			return names;
		}

		private IEnumerable<Type> FindAllSubTypes(IEnumerable<Assembly> searchScope, Type baseType, ITypeFilter? filter)
		{
			var filteredAssemblies = GetDependentAssemblies(searchScope, baseType.Assembly);

			foreach (var assembly in filteredAssemblies)
			{
				var types = assembly.GetTypes();
				foreach (var type in types)
				{
					if (baseType.IsAssignableFrom(type)
						&& (filter == null
							|| filter.ShouldInclude(type)))
					{
						yield return type;
					}
				}
			}
		}

		private IEnumerable<Type> FindAllBaseTypes(Type type, ITypeFilter? filter)
		{
			var current = type.BaseType;
			while (current != null)
			{
				if (filter == null
					|| filter.ShouldInclude(current))
				{
					yield return current;
				}
				current = current.BaseType;
			}
			foreach (var baseInterface in type.GetInterfaces())
			{
				if (filter == null
					|| filter.ShouldInclude(baseInterface))
				{
					yield return baseInterface;
				}
			}
		}

		private PolymorphicConfigurationBaseType GetOrCreateBaseType(Type key)
		{
			if (!baseTypes.TryGetValue(key, out var value))
			{
				value = new PolymorphicConfigurationBaseType(this, key);
				baseTypes.Add(key, value);
			}
			return value;
		}

		private List<PolymorphicConfigurationSubType> GetOrCreateSubTypeCollection(Type key)
		{
			if (!subTypes.TryGetValue(key, out var value))
			{
				value = new List<PolymorphicConfigurationSubType>();
				subTypes.Add(key, value);
			}
			return value;
		}

		private PolymorphicConfigurationSubType GetOrCreateSubTypeInBase(Type key, PolymorphicConfigurationBaseType baseType)
		{
			if (!baseType.SubTypes.TryGetValue(key, out var value))
			{
				value = new PolymorphicConfigurationSubType(this, key, baseType);
				baseType.SubTypes.Add(key, value);

				var subTypeCollection = GetOrCreateSubTypeCollection(key);
				subTypeCollection.Add(value);
			}
			return value;
		}

		private static IEnumerable<Assembly> GetDependentAssemblies(IEnumerable<Assembly> assemblies, Assembly sourceAssembly)
		{
			bool Predicate(Assembly assembly)
			{
				return IsDependentAssemblies(assembly, sourceAssembly);
			}

			return assemblies.Where(Predicate);
		}

		private static bool IsDependentAssemblies(Assembly otherAssembly, Assembly sourceAssembly)
		{
			return otherAssembly == sourceAssembly
				|| otherAssembly.GetReferencedAssemblies()
					.Select(assemblyName => assemblyName.FullName)
					.Contains(sourceAssembly.FullName);
		}
	}
}

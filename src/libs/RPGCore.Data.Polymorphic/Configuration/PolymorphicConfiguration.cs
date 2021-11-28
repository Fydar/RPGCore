using RPGCore.Data.Polymorphic.Naming;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace RPGCore.Data.Polymorphic.Configuration
{
	/// <summary>
	/// A queryable configuration build from <see cref="PolymorphicOptions"/>.
	/// </summary>
	public class PolymorphicConfiguration
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal Dictionary<Type, PolymorphicConfigurationBaseType> baseTypes = new();

		internal Dictionary<Type, List<PolymorphicConfigurationSubType>> subTypes = new();

		/// <summary>
		/// Determines the name of the field that is used to determine polymorphic types.
		/// </summary>
		public string DescriminatorName { get; internal set; } = "$type";

		/// <summary>
		/// Determines whether type names should be case-insensitive.
		/// </summary>
		public bool CaseInsensitive { get; internal set; } = true;

		/// <summary>
		/// All base types recognised by this configuration.
		/// </summary>
		public IReadOnlyCollection<PolymorphicConfigurationBaseType> BaseTypes => baseTypes.Values;

		internal PolymorphicConfiguration(PolymorphicOptions options)
		{
			DescriminatorName = options.DescriminatorName;
			CaseInsensitive = options.CaseInsensitive;

			var searchScope = GetDependentAssemblies(AppDomain.CurrentDomain.GetAssemblies(), typeof(PolymorphicOptions).Assembly)
				.ToList();

			// Use the names from the automatically resolved sub-types.
			foreach (var knownBaseType in options.knownBaseTypes.Values)
			{
				foreach (var resolveSubTypeOptions in knownBaseType.resolveSubTypeOptions)
				{
					var baseType = GetOrCreateBaseType(knownBaseType.BaseType);

					foreach (var foundSubType in FindAllSubTypes(searchScope, knownBaseType.BaseType, resolveSubTypeOptions.TypeFilter))
					{
						var subType = GetOrCreateSubTypeInBase(foundSubType, baseType);

						var typeNaming = resolveSubTypeOptions.TypeNaming ?? options.DefaultNamingConvention;

						string overwritingSubTypeName = subType.Name;

						subType.Name = typeNaming.GetNameForType(foundSubType);
						subType.Aliases = GetAllNames(subType.Aliases, foundSubType, resolveSubTypeOptions.TypeAliasing, overwritingSubTypeName);
					}
				}
			}

			// Use resolved names defined on the sub-type.
			foreach (var knownSubType in options.knownSubTypes.Values)
			{
				foreach (var resolveBaseTypeOptions in knownSubType.resolveBaseTypeOptions)
				{
					foreach (var foundBaseType in FindAllBaseTypes(knownSubType.SubType, resolveBaseTypeOptions.TypeFilter))
					{
						if (!resolveBaseTypeOptions.IncludeSystemObject && foundBaseType == typeof(object))
						{
							continue;
						}

						var baseType = GetOrCreateBaseType(foundBaseType);
						var subType = GetOrCreateSubTypeInBase(knownSubType.SubType, baseType);

						subType.Name = knownSubType.Descriminator
							?? options.DefaultNamingConvention.GetNameForType(knownSubType.SubType);

						subType.Aliases = knownSubType.Aliases.ToArray();
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

					subType.Name = knownSubType.Descriminator
						?? options.DefaultNamingConvention.GetNameForType(knownSubType.SubType);

					subType.Aliases = knownSubType.Aliases.ToArray();
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

					subType.Name = knownSubType.Descriminator
						?? options.DefaultNamingConvention.GetNameForType(knownSubType.SubType);
				}
			}
		}

		/// <summary>
		/// Retrieves configuration assoociated with the <see cref="Type"/> as a base-type.
		/// </summary>
		/// <param name="key">The type to be used as a base-type.</param>
		/// <param name="value">Configuration associated with the <paramref name="key"/>.</param>
		/// <returns><c>true</c> if the <paramref name="key"/> is a valid base-type; otherwise <c>false</c>.</returns>
		public bool TryGetBaseType(Type key, out PolymorphicConfigurationBaseType value)
		{
			return baseTypes.TryGetValue(key, out value);
		}

		/// <summary>
		/// Retrieves configuration assoociated with the <see cref="Type"/> as a sub-type.
		/// </summary>
		/// <param name="key">The type to be used as a sub-type.</param>
		/// <param name="value">Configuration associated with the <paramref name="key"/>.</param>
		/// <returns><c>true</c> if the <paramref name="key"/> is a valid sub-type; otherwise <c>false</c>.</returns>
		public bool TryGetSubType(Type key, out List<PolymorphicConfigurationSubType> value)
		{
			return subTypes.TryGetValue(key, out value);
		}

		private static string[] GetAllNames(string[] baseValues, Type type, IReadOnlyList<ITypeNamingConvention> conventions)
		{
			if (conventions.Count == 0)
			{
				return Array.Empty<string>();
			}

			string[] names = new string[conventions.Count + baseValues.Length];
			for (int i = 0; i < conventions.Count; i++)
			{
				names[baseValues.Length + i] = conventions[i].GetNameForType(type);
			}
			return names;
		}

		private static string[] GetAllNames(string[] baseValues, Type type, IReadOnlyList<ITypeNamingConvention> conventions, string additionalAlias)
		{
			if (string.IsNullOrEmpty(additionalAlias))
			{
				return GetAllNames(baseValues, type, conventions);
			}
			if (conventions.Count == 0)
			{
				return new string[] { additionalAlias };
			}

			string[] names = new string[conventions.Count + baseValues.Length + 1];
			names[0] = additionalAlias;
			for (int i = 0; i < conventions.Count; i++)
			{
				names[baseValues.Length + i + 1] = conventions[i].GetNameForType(type);
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
						&& baseType != type
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
			if (!baseType.subTypes.TryGetValue(key, out var value))
			{
				value = new PolymorphicConfigurationSubType(baseType, key);
				baseType.subTypes.Add(key, value);

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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace RPGCore.Data.Polymorphic
{
	/// <summary>
	/// A queryable configuration build from <see cref="PolymorphicOptionsBuilder"/>.
	/// </summary>
	public class PolymorphicOptions
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal Dictionary<Type, PolymorphicOptionsBaseType> baseTypes = new();

		internal Dictionary<Type, List<PolymorphicOptionsSubType>> subTypes = new();

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
		public IReadOnlyCollection<PolymorphicOptionsBaseType> BaseTypes => baseTypes.Values;

		internal PolymorphicOptions(PolymorphicOptionsBuilder builder)
		{
			DescriminatorName = builder.DescriminatorName;
			CaseInsensitive = builder.CaseInsensitive;

			var searchScope = GetDependentAssemblies(AppDomain.CurrentDomain.GetAssemblies(),
				typeof(PolymorphicOptionsBuilder).Assembly)
				.ToList();

			var typeMappings = new Dictionary<Type, Dictionary<Type, PolymorphicOptionsBuilderExplicitType>>();
			PolymorphicOptionsBuilderExplicitType GetOrCreateExplicitType(Type baseType, Type subType)
			{
				if (!typeMappings!.TryGetValue(baseType, out var baseTypeMappings))
				{
					baseTypeMappings = new Dictionary<Type, PolymorphicOptionsBuilderExplicitType>();
					typeMappings[baseType] = baseTypeMappings;
				}

				if (!baseTypeMappings.TryGetValue(subType, out var explicitType))
				{
					explicitType = new PolymorphicOptionsBuilderExplicitType(baseType, subType);
					baseTypeMappings[subType] = explicitType;
				}

				return explicitType;
			}

			foreach (var knownTypeFactory in builder.knownTypeFactories)
			{
				if (knownTypeFactory.factory is Action<IPolymorphicOptionsBuilderKnownBaseType> knownBaseTypeFactory)
				{
					var knownBaseTypeOptions = new PolymorphicOptionsBuilderKnownBaseType(knownTypeFactory.type);
					knownBaseTypeFactory.Invoke(knownBaseTypeOptions);

					foreach (var configure in knownBaseTypeOptions.configures)
					{
						if (configure is PolymorphicOptionsBuilderConfigureResolveSubTypes configureResolveSubTypes)
						{
							var resolveSubTypesOptions = new PolymorphicOptionsBuilderResolveSubType();
							configureResolveSubTypes.Action?.Invoke(resolveSubTypesOptions);

							foreach (var foundSubType in FindAllSubTypes(searchScope, knownTypeFactory.type, resolveSubTypesOptions.TypeFilter))
							{
								var explicitType = GetOrCreateExplicitType(knownTypeFactory.type, foundSubType);

								if (resolveSubTypesOptions.TypeNaming != null)
								{
									explicitType.Descriminator = resolveSubTypesOptions.TypeNaming.GetNameForType(foundSubType);
								}

								foreach (var typeAlias in resolveSubTypesOptions.TypeAliasing)
								{
									explicitType.Aliases.Add(typeAlias.GetNameForType(foundSubType));
								}
							}
						}
						else if (configure is PolymorphicOptionsBuilderConfigureUseSubType configureUseSubType)
						{
							var explicitType = GetOrCreateExplicitType(knownTypeFactory.type, configureUseSubType.Type);

							configureUseSubType.Action?.Invoke(explicitType);
						}
					}
				}
				else if (knownTypeFactory.factory is Action<IPolymorphicOptionsBuilderKnownSubType> knownSubTypeFactory)
				{
					var knownSubTypeOptions = new PolymorphicOptionsBuilderKnownSubType(knownTypeFactory.type);
					knownSubTypeFactory.Invoke(knownSubTypeOptions);

					foreach (var configure in knownSubTypeOptions.configures)
					{
						if (configure is PolymorphicOptionsBuilderConfigureResolveBaseTypes configureResolveBaseTypes)
						{
							var resolveBaseTypesOptions = new PolymorphicOptionsBuilderResolveBaseType();
							configureResolveBaseTypes.Action?.Invoke(resolveBaseTypesOptions);

							foreach (var foundBaseType in FindAllBaseTypes(knownTypeFactory.type, resolveBaseTypesOptions.TypeFilter))
							{
								if (!resolveBaseTypesOptions.IncludeSystemObject && foundBaseType == typeof(object))
								{
									continue;
								}

								var explicitType = GetOrCreateExplicitType(foundBaseType, knownTypeFactory.type);

								foreach (var callback in resolveBaseTypesOptions.identiyWith)
								{
									callback.Invoke(explicitType);
								}
							}
						}
						else if (configure is PolymorphicOptionsBuilderConfigureUseBaseType configureUseBaseType)
						{
							var explicitType = GetOrCreateExplicitType(configureUseBaseType.Type, knownTypeFactory.type);

							configureUseBaseType.Action?.Invoke(explicitType);
						}
					}
				}
			}

			foreach (var typeMapping in typeMappings)
			{
				var baseType = GetOrCreateBaseType(typeMapping.Key);

				foreach (var baseTypeMapping in typeMapping.Value)
				{
					var subType = GetOrCreateSubTypeInBase(baseTypeMapping.Key, baseType);

					subType.Name = baseTypeMapping.Value.Descriminator ?? builder.DefaultNamingConvention.GetNameForType(baseTypeMapping.Key);
					subType.Aliases = baseTypeMapping.Value.Aliases.ToArray();
				}
			}
		}

		/// <summary>
		/// Retrieves configuration assoociated with the <see cref="Type"/> as a base-type.
		/// </summary>
		/// <param name="key">The type to be used as a base-type.</param>
		/// <param name="value">Options associated with the <paramref name="key"/>.</param>
		/// <returns><c>true</c> if the <paramref name="key"/> is a valid base-type; otherwise <c>false</c>.</returns>
		public bool TryGetBaseType(Type key, out PolymorphicOptionsBaseType value)
		{
			return baseTypes.TryGetValue(key, out value);
		}

		/// <summary>
		/// Retrieves configuration assoociated with the <see cref="Type"/> as a sub-type.
		/// </summary>
		/// <param name="key">The type to be used as a sub-type.</param>
		/// <param name="value">Options associated with the <paramref name="key"/>.</param>
		/// <returns><c>true</c> if the <paramref name="key"/> is a valid sub-type; otherwise <c>false</c>.</returns>
		public bool TryGetSubType(Type key, out List<PolymorphicOptionsSubType> value)
		{
			return subTypes.TryGetValue(key, out value);
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

		private PolymorphicOptionsBaseType GetOrCreateBaseType(Type key)
		{
			if (!baseTypes.TryGetValue(key, out var value))
			{
				value = new PolymorphicOptionsBaseType(this, key);
				baseTypes.Add(key, value);
			}
			return value;
		}

		private List<PolymorphicOptionsSubType> GetOrCreateSubTypeCollection(Type key)
		{
			if (!subTypes.TryGetValue(key, out var value))
			{
				value = new List<PolymorphicOptionsSubType>();
				subTypes.Add(key, value);
			}
			return value;
		}

		private PolymorphicOptionsSubType GetOrCreateSubTypeInBase(Type key, PolymorphicOptionsBaseType baseType)
		{
			if (!baseType.subTypes.TryGetValue(key, out var value))
			{
				value = new PolymorphicOptionsSubType(baseType, key);
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

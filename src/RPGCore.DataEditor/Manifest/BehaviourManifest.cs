using Newtonsoft.Json;
using RPGCore.Behaviour;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RPGCore.DataEditor.Manifest
{
	public sealed class BehaviourManifest
	{
		private static readonly Type[] frameworkTypes = new[]
		{
			typeof(bool),
			typeof(string),
			typeof(int),
			typeof(byte),
			typeof(long),
			typeof(short),
			typeof(uint),
			typeof(ulong),
			typeof(ushort),
			typeof(sbyte),
			typeof(char),
			typeof(float),
			typeof(double),
			typeof(decimal),
		};

		public TypeManifest Types;

		private static IEnumerable<Assembly> GetDependentAssemblies(AppDomain appDomain, Assembly analyzedAssembly)
		{
			return appDomain.GetAssemblies()
				.Where(assembly => assembly == analyzedAssembly || GetNamesOfAssembliesReferencedBy(assembly)
					.Contains(analyzedAssembly.FullName));
		}

		public static IEnumerable<string> GetNamesOfAssembliesReferencedBy(Assembly assembly)
		{
			return assembly.GetReferencedAssemblies()
				.Select(assemblyName => assemblyName.FullName);
		}

		public static BehaviourManifest CreateFromAppDomain(AppDomain appDomain)
		{
			var manifest = new BehaviourManifest();

			var objectTypes = new Dictionary<string, TypeInformation>();
			foreach (var type in frameworkTypes)
			{
				objectTypes.Add(type.Name, TypeInformation.Construct(type));
			}

			var nodeTypes = new Dictionary<string, NodeInformation>();
			foreach (var assembly in GetDependentAssemblies(appDomain, typeof(NodeTemplate).Assembly))
			{
				Type[] types;
				try
				{
					types = assembly.GetTypes();
				}
				catch
				{
					continue;
				}

				foreach (var type in types)
				{
					ConstructType(type, objectTypes);

					if (type.IsAbstract)
					{
						continue;
					}

					if (typeof(NodeTemplate).IsAssignableFrom(type))
					{
						nodeTypes.Add(type.FullName, NodeInformation.Construct(type));
					}
				}
			}

			manifest.Types = new TypeManifest()
			{
				ObjectTypes = objectTypes,
				NodeTypes = nodeTypes,
			};

			return manifest;
		}

		private static void ConstructType(Type type, Dictionary<string, TypeInformation> objectTypes)
		{
			if (!type.IsAbstract)
			{
				var typeAttributes = type.GetCustomAttribute(typeof(EditorTypeAttribute));
				if (typeAttributes != null)
				{
					objectTypes.Add(type.Name, TypeInformation.Construct(type));
				}
			}
		}

		private BehaviourManifest()
		{
		}

		/// <inheritdoc/>
		public override string ToString()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented);
		}

		public TypeInformation GetTypeInformation(string type)
		{
			if (Types == null
				|| string.IsNullOrEmpty(type))
			{
				return null;
			}

			int arrayIndex = type.LastIndexOf('[');
			string lookupType = arrayIndex == -1
				? type
				: type.Substring(0, arrayIndex);

			if (Types.ObjectTypes != null)
			{
				if (Types.ObjectTypes.TryGetValue(lookupType, out var objectType))
				{
					return objectType;
				}
			}
			if (Types.NodeTypes != null)
			{
				if (Types.NodeTypes.TryGetValue(lookupType, out var nodeType))
				{
					return nodeType;
				}
			}
			return null;
		}
	}
}

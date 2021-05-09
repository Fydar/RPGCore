using RPGCore.DataEditor.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RPGCore.DataEditor.Manifest
{
	/// <summary>
	/// A collection of extensions for adding support for user .NET objects.
	/// </summary>
	public static class ProjectManifestBuilderAppDomainTypeExtensions
	{
		/// <summary>
		/// Iterates each <see cref="Assembly"/> in the <see cref="AppDomain"/> that is dependenat on another assembly and adds all editable types to the <see cref="ProjectManifestBuilder"/>.
		/// </summary>
		/// <param name="builder">The <see cref="ProjectManifestBuilder"/> to add types to.</param>
		/// <param name="appDomain">The <see cref="AppDomain"/> that will be iterated.</param>
		/// <param name="dependantAssembly">An <see cref="Assembly"/> that will be used to filter out unnessesary assemblies.</param>
		/// <returns>Returns the current instance of the <see cref="ProjectManifestBuilder"/>.</returns>
		public static ProjectManifestBuilder UseAllTypesFromAppDomain(this ProjectManifestBuilder builder, AppDomain appDomain, Assembly dependantAssembly)
		{
			foreach (var assembly in GetDependentAssemblies(appDomain, dependantAssembly))
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
					if (type.IsAbstract)
					{
						continue;
					}

					builder.UseType(type);
				}
			}
			return builder;
		}

		public static ProjectManifestBuilder UseType(this ProjectManifestBuilder builder, Type type)
		{
			var typeAttributes = type.GetCustomAttribute(typeof(EditorTypeAttribute));
			if (typeAttributes != null)
			{
				builder.AddType(BuiltInTypes.Construct(type));
			}
			/*else if (typeof(NodeTemplate).IsAssignableFrom(type))
			{
				builder.AddNodeType(SchemaNode.Construct(type));
			}*/
			return builder;
		}

		private static IEnumerable<Assembly> GetDependentAssemblies(AppDomain appDomain, Assembly analyzedAssembly)
		{
			bool Predicate(Assembly assembly)
			{
				return assembly == analyzedAssembly
					|| assembly.GetReferencedAssemblies()
						.Select(assemblyName => assemblyName.FullName)
						.Contains(analyzedAssembly.FullName);
			}

			return appDomain.GetAssemblies().Where(Predicate);
		}
	}
}

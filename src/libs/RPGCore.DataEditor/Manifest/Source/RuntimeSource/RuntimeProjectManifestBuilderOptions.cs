using RPGCore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RPGCore.DataEditor.Manifest.Source.RuntimeSource
{
	public class RuntimeProjectManifestBuilderOptions
	{
		internal static readonly Type[] frameworkTypes = new[]
		{
			typeof(string),
			typeof(bool),
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

		private readonly IRuntimeTypeConverter runtimeTypeConverter;
		private readonly ProjectManifestBuilder builder;

		internal RuntimeProjectManifestBuilderOptions(IRuntimeTypeConverter runtimeTypeConverter, ProjectManifestBuilder builder)
		{
			this.runtimeTypeConverter = runtimeTypeConverter;
			this.builder = builder;
		}

		/// <summary>
		/// Iterates each <see cref="Assembly"/> in the <see cref="AppDomain"/> that is dependenat on another assembly and adds all editable types to the <see cref="ProjectManifestBuilder"/>.
		/// </summary>
		/// <param name="appDomain">The <see cref="AppDomain"/> that will be iterated.</param>
		/// <param name="dependantAssembly">An <see cref="Assembly"/> that will be used to filter out unnessesary assemblies.</param>
		/// <returns>Returns the current instance of the <see cref="ProjectManifestBuilder"/>.</returns>
		public RuntimeProjectManifestBuilderOptions UseAllTypesFromAppDomain(AppDomain appDomain, Assembly dependantAssembly)
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

				UseAllValidTypes(builder, types);
			}
			return this;
		}

		/// <summary>
		/// Iterates each <see cref="Assembly"/> in the <see cref="AppDomain"/> that is dependenat on another assembly and adds all editable types to the <see cref="ProjectManifestBuilder"/>.
		/// </summary>
		/// <param name="assembly">An <see cref="Assembly"/> to source types from.</param>
		/// <returns>Returns the current instance of the <see cref="ProjectManifestBuilder"/>.</returns>
		public RuntimeProjectManifestBuilderOptions UseAllTypesFromAssembly(Assembly assembly)
		{
			var types = assembly.GetTypes();
			UseAllValidTypes(builder, types);

			return this;
		}

		/// <summary>
		/// Adds types to the project manifest for all framework types.
		/// </summary>
		/// <returns>Returns the current instance of the <see cref="ProjectManifestBuilder"/>.</returns>
		public RuntimeProjectManifestBuilderOptions UseFrameworkTypes()
		{
			foreach (var type in frameworkTypes)
			{
				var schemaType = runtimeTypeConverter.Convert(type);
				builder.AddType(schemaType);
			}
			return this;
		}

		/// <summary>
		/// Adds a <see cref="Type"/> as a type to the <see cref="ProjectManifestBuilder"/>.
		/// </summary>
		/// <param name="type"></param>
		/// <returns>Returns the current instance of the <see cref="ProjectManifestBuilder"/>.</returns>
		public RuntimeProjectManifestBuilderOptions UseType(Type type)
		{
			var typeAttributes = type.GetCustomAttribute(typeof(EditableTypeAttribute));
			if (typeAttributes == null)
			{
				throw new InvalidOperationException($"Type \"{type.FullName}\" does not contain the [EditorType] attribute.");
			}

			var schemaType = runtimeTypeConverter.Convert(type);
			builder.AddType(schemaType);

			return this;
		}

		private void UseAllValidTypes(ProjectManifestBuilder builder, Type[] types)
		{
			foreach (var type in types)
			{
				var typeAttributes = type.GetCustomAttribute(typeof(EditableTypeAttribute));
				if (typeAttributes != null)
				{
					builder.AddType(runtimeTypeConverter.Convert(type));
				}
			}
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
	}
}

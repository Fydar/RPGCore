using RPGCore.Data.Polymorphic;
using RPGCore.Data.Polymorphic.Inline;
using RPGCore.Data.Polymorphic.Naming;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RPGCore.Behaviour;

public static class PolymorphicOptionsBuilderExtensions
{
	private struct OutputMap
	{
		public string Name { get; set; }
		public Type OutputDataType { get; set; }
	}

	public static PolymorphicOptionsBuilder UseGraphSerialization(this PolymorphicOptionsBuilder polymorphicOptions)
	{
		var assemblies = GetDependentAssemblies(AppDomain.CurrentDomain, typeof(SerializeBaseTypeAttribute).Assembly).ToList();

		var outputs = new List<OutputMap>();
		var inputs = new HashSet<Type>();

		foreach (var assembly in assemblies)
		{
			var types = assembly.GetTypes();
			foreach (var type in types)
			{
				var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);

				foreach (var property in properties)
				{
					if (property.PropertyType.IsGenericType)
					{
						var genericDefinition = property.PropertyType.GetGenericTypeDefinition();
						if (genericDefinition == typeof(Output<>))
						{
							var outputType = property.PropertyType.GetGenericArguments()[0];
							var outputDataTypeGenericDefinition = genericDefinition.GetNestedType("OutputData");
							var outputDataType = outputDataTypeGenericDefinition.MakeGenericType(outputType);

							outputs.Add(new OutputMap()
							{
								Name = GenericNames.ToGenericName(outputType),
								OutputDataType = outputDataType
							});
						}
						else if (genericDefinition == typeof(IInput<>))
						{
							var inputType = property.PropertyType.GetGenericArguments()[0];

							inputs.Add(inputType);
						}
					}

					var serialiseBaseTypeAttributes = type.GetCustomAttributes<SerializeBaseTypeAttribute>(false);
				}
			}
		}

		foreach (var output in outputs)
		{
			polymorphicOptions.UseKnownBaseType(typeof(IOutputData), baseType =>
			{
				baseType.UseSubType(output.OutputDataType, subType =>
				{
					subType.Descriminator = output.Name;
				});
			});
		}

		foreach (var input in inputs)
		{
			var inputDataType = typeof(IInput<>).MakeGenericType(input);
			var connectedDataType = typeof(ConnectedInput<>).MakeGenericType(input);
			var defaultDataType = typeof(DefaultInput<>).MakeGenericType(input);

			polymorphicOptions.UseKnownBaseType(inputDataType, baseType =>
			{
				baseType.UseSubType(connectedDataType, subType =>
				{
					subType.Descriminator = "connected";
				});

				baseType.UseSubType(defaultDataType, subType =>
				{
					subType.Descriminator = GenericNames.ToGenericName(input);
				});
			});
		}

		return polymorphicOptions;
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

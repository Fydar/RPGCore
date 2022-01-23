using RPGCore.Data.Polymorphic.Naming;
using RPGCore.Data.Polymorphic.SystemTextJson;
using System.Text.Json;

namespace RPGCore.Documentation.Samples.RPGCore.Data;

public class PolymorphicFluentBaseAutoResolve
{
	#region types
	public interface IProcedure
	{
	}

	public class CreateProcedure : IProcedure
	{
	}

	public class UpdateProcedure : IProcedure
	{
		public int Identifier { get; set; }
	}

	public class RemoveProcedure : UpdateProcedure
	{
		public float Delay { get; set; }
	}
	#endregion types

	[PresentOutput(OutputFormat.Json, "output")]
	public static string Result()
	{
		#region configure
		var options = new JsonSerializerOptions()
			.UsePolymorphicSerialization(options =>
			{
				options.UseKnownBaseType(typeof(IProcedure), baseType =>
				{
					baseType.UseResolvedSubTypes(resolve =>
					{
						resolve.TypeNaming = TypeNameNamingConvention.Instance;
					});
				});
			});
		#endregion configure

		options.WriteIndented = true;

		#region serialize
		IProcedure procedure = new RemoveProcedure()
		{
			Delay = 0.5f,
			Identifier = 4
		};
		return JsonSerializer.Serialize(procedure, options);
		#endregion serialize
	}
}

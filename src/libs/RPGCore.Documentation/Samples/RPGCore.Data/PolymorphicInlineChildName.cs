using RPGCore.Data.Polymorphic.Inline;
using RPGCore.Data.Polymorphic.SystemTextJson;
using System.Text.Json;

namespace RPGCore.Documentation.Samples.RPGCore.Data;

public class PolymorphicInlineChildName
{
	#region types
	public interface IProcedure
	{
	}

	[SerializeSubType("create")]
	public class CreateProcedure : IProcedure
	{
	}

	[SerializeSubType("update")]
	public class UpdateProcedure : IProcedure
	{
		public int Identifier { get; set; }
	}

	[SerializeSubType("remove")]
	public class RemoveProcedure : UpdateProcedure
	{
		public float Delay { get; set; }
	}
	#endregion types

	[PresentOutput(OutputFormat.Json, "output")]
	public static string Result()
	{
		var options = new JsonSerializerOptions()
			.UsePolymorphicSerialization(options =>
			{
				options.UseInline();
			});

		options.WriteIndented = true;

		IProcedure procedure = new RemoveProcedure()
		{
			Delay = 0.5f,
			Identifier = 4
		};
		return JsonSerializer.Serialize(procedure, options);
	}
}

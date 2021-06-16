using RPGCore.Data.Polymorphic.Inline;
using RPGCore.Data.Polymorphic.SystemTextJson;
using System.Text.Json;

namespace RPGCore.Documentation.Samples.RPGCore.Data
{
	public class PolymorphicInlineBaseTypeName
	{
		#region types
		[SerializeType(TypeName.Name)]
		public interface IProcedure
		{
		}
		#endregion types

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
}

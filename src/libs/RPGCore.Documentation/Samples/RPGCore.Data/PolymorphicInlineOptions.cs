using RPGCore.Data.Polymorphic.Inline;
using RPGCore.Data.SystemTextJson.Polymorphic;
using System.Text.Json;
using static RPGCore.Documentation.Samples.RPGCore.Data.PolymorphicInlineBaseDefault;

namespace RPGCore.Documentation.Samples.RPGCore.Data
{
	public class PolymorphicInlineOptions
	{
		public static string Result()
		{
			#region serialize
			var options = new JsonSerializerOptions()
				.UsePolymorphicSerialization(options =>
				{
					options.UseInline();
				});

			IProcedure procedure = new RemoveProcedure()
			{
				Delay = 0.5f,
				Identifier = 4
			};
			return JsonSerializer.Serialize(procedure, options);
			#endregion serialize
		}
	}
}

using RPGCore.Data.Polymorphic.Inline;
using RPGCore.Data.SystemTextJson.Polymorphic;
using System.Text.Json;

namespace RPGCore.Documentation.Samples.RPGCore.Data
{
	public class PolymorphicInlineSample3
	{
		#region types
		[SerializeType(typeof(CreateProcedure), "create")]
		[SerializeType(typeof(UpdateProcedure), "update")]
		[SerializeType(typeof(RemoveProcedure), "remove")]
		public interface IProcedure
		{
		}

		public class CreateProcedure : IProcedure
		{
		}

		public class UpdateProcedure : IProcedure
		{
		}

		public class RemoveProcedure : IProcedure
		{
		}
		#endregion types

		[DocumentationMethod("serialize")]
		public static string Result()
		{
			#region serialize
			var options = new JsonSerializerOptions()
				.UsePolymorphicSerialization(options =>
				{
					options.UseInline();
				});

			IProcedure procedure = new CreateProcedure();
			return JsonSerializer.Serialize(procedure, options);
			#endregion serialize
		}
	}
}

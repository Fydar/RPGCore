using RPGCore.Data.Polymorphic.Inline;
using RPGCore.Data.SystemTextJson.Polymorphic;
using System.Text.Json;

namespace RPGCore.Documentation.Samples.RPGCore.Data
{
	public class PolymorphicInlineSample5
	{
		#region types
		public interface IProcedure
		{
		}

		[SerializeThisType("create")]
		public class CreateProcedure : IProcedure
		{
		}

		[SerializeThisType("update")]
		public class UpdateProcedure : IProcedure
		{
		}

		[SerializeThisType("remove")]
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

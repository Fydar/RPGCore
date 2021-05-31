using RPGCore.Data.Polymorphic.Inline;
using RPGCore.Data.SystemTextJson.Polymorphic;
using System.Text.Json;

namespace RPGCore.Documentation.Samples.RPGCore.Data
{
	public class PolymorphicInlineSample4
	{
		#region types
		public interface IProcedure
		{
		}

		[SerializeThisType(typeof(IProcedure), "create")]
		public class CreateProcedure : IProcedure
		{
		}

		[SerializeThisType(typeof(IProcedure), "update")]
		public class UpdateProcedure : IProcedure
		{
		}

		[SerializeThisType(typeof(IProcedure), "remove")]
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

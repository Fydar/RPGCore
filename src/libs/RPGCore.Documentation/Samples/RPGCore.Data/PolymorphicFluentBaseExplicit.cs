using RPGCore.Data.SystemTextJson.Polymorphic;
using System.Text.Json;

namespace RPGCore.Documentation.Samples.RPGCore.Data
{
	public class PolymorphicFluentBaseExplicit
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
		}

		public class RemoveProcedure : IProcedure
		{
		}
		#endregion types

		public static void Result()
		{
			#region configure
			var options = new JsonSerializerOptions()
				.UsePolymorphicSerialization(options =>
				{
					options.UseKnownBaseType(typeof(IProcedure), baseType =>
					{
						baseType.UseSubType(typeof(CreateProcedure), subType =>
						{
							subType.Descriminator = "create";
						});

						baseType.UseSubType(typeof(UpdateProcedure), subType =>
						{
							subType.Descriminator = "update";
						});

						baseType.UseSubType(typeof(RemoveProcedure), subType =>
						{
							subType.Descriminator = "remove";
						});
					});
				});
			#endregion configure
		}
	}
}

using RPGCore.Data.Polymorphic.Naming;
using RPGCore.Data.SystemTextJson.Polymorphic;
using System.Text.Json;

namespace RPGCore.Documentation.Samples.RPGCore.Data
{
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
						baseType.ResolveSubTypesAutomatically(resolve =>
						{
							resolve.TypeNaming = TypeNameNamingConvention.Instance;
						});
					});
				});
			#endregion configure
		}
	}
}

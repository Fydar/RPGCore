using RPGCore.Data.Polymorphic.Inline;
using RPGCore.Data.Polymorphic.SystemTextJson;
using System.Text.Json;

namespace RPGCore.Documentation.Samples.RPGCore.Data.Polymorphic.SystemTextJson;

public class Extensions
{
	public static void Run()
	{
		#region use
		// Configure JsonSerializerOptions to use RPGCore.Data.Polymorphic
		var serializer = new JsonSerializerOptions()
			.UsePolymorphicSerialization(options =>
			{
				options.UseInline();
			});
		#endregion use
	}
}

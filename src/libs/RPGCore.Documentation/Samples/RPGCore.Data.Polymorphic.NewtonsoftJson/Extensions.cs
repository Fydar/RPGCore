using Newtonsoft.Json;
using RPGCore.Data.Polymorphic.Inline;
using RPGCore.Data.Polymorphic.NewtonsoftJson;
using RPGCore.Data.Polymorphic.SystemTextJson;

namespace RPGCore.Documentation.Samples.RPGCore.Data.Polymorphic.NewtonsoftJson;

public class Extensions
{
	public static void Run()
	{
		#region use
		// Configure JsonSerializerSettings to use RPGCore.Data.Polymorphic
		var options = new JsonSerializerSettings();

		options.UsePolymorphicSerialization(options =>
		{
			options.UseInline();
		});

		var serializer = JsonSerializer.Create(options);
		#endregion use
	}
}

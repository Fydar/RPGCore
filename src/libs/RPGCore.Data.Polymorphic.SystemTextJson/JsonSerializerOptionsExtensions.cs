using RPGCore.Data.Polymorphic.SystemTextJson.Internal;
using System;
using System.Text.Json;

namespace RPGCore.Data.Polymorphic.SystemTextJson
{
	public static class JsonSerializerOptionsExtensions
	{
		public static JsonSerializerOptions UsePolymorphicSerialization(this JsonSerializerOptions jsonSerializerOptions)
		{
			var options = new PolymorphicOptions();
			jsonSerializerOptions.Converters.Add(new PolymorphicConverterFactory(options.Build()));
			return jsonSerializerOptions;
		}

		public static JsonSerializerOptions UsePolymorphicSerialization(this JsonSerializerOptions jsonSerializerOptions, Action<PolymorphicOptions> options)
		{
			var optionsResult = new PolymorphicOptions();
			options.Invoke(optionsResult);
			jsonSerializerOptions.Converters.Add(new PolymorphicConverterFactory(optionsResult.Build()));
			return jsonSerializerOptions;
		}
	}
}

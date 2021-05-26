using RPGCore.Data.Polymorphic;
using RPGCore.Data.SystemTextJson.Polymorphic.Internal;
using System;
using System.Text.Json;

namespace RPGCore.Data.SystemTextJson.Polymorphic
{
	public static class JsonSerializerOptionsExtensions
	{
		public static JsonSerializerOptions UsePolymorphicSerialization(this JsonSerializerOptions jsonSerializerOptions)
		{
			var options = new PolymorphicOptions();
			jsonSerializerOptions.Converters.Add(new PolymorphicConverterFactory(options));
			return jsonSerializerOptions;
		}

		public static JsonSerializerOptions UsePolymorphicSerialization(this JsonSerializerOptions jsonSerializerOptions, Action<PolymorphicOptions> options)
		{
			var optionsResult = new PolymorphicOptions();
			options.Invoke(optionsResult);
			jsonSerializerOptions.Converters.Add(new PolymorphicConverterFactory(optionsResult));
			return jsonSerializerOptions;
		}
	}
}

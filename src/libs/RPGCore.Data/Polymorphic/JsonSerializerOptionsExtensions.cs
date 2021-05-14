using System;
using System.Text.Json;

namespace RPGCore.Data.Polymorphic
{
	public static class JsonSerializerOptionsExtensions
	{
		public static JsonSerializerOptions UsePolymorphicSerialization(this JsonSerializerOptions jsonSerializerOptions)
		{
			var options = new PolymorphicConverterFactoryOptions();
			jsonSerializerOptions.Converters.Add(new PolymorphicConverterFactory(options));
			return jsonSerializerOptions;
		}

		public static JsonSerializerOptions UsePolymorphicSerialization(this JsonSerializerOptions jsonSerializerOptions, Action<PolymorphicConverterFactoryOptions> options)
		{
			var optionsResult = new PolymorphicConverterFactoryOptions();
			options.Invoke(optionsResult);
			jsonSerializerOptions.Converters.Add(new PolymorphicConverterFactory(optionsResult));
			return jsonSerializerOptions;
		}
	}
}

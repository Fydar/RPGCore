using Newtonsoft.Json;
using RPGCore.Data.Polymorphic.NewtonsoftJson.Internal;
using System;

namespace RPGCore.Data.Polymorphic.NewtonsoftJson
{
	public static class JsonSerializerSettingsExtensions
	{
		public static JsonSerializerSettings UsePolymorphicSerialization(this JsonSerializerSettings jsonSerializerOptions)
		{
			var options = new PolymorphicOptions();
			jsonSerializerOptions.Converters.Add(new PolymorphicConverter(options.Build()));
			return jsonSerializerOptions;
		}

		public static JsonSerializerSettings UsePolymorphicSerialization(this JsonSerializerSettings jsonSerializerOptions, Action<PolymorphicOptions> options)
		{
			var optionsResult = new PolymorphicOptions();
			options.Invoke(optionsResult);
			jsonSerializerOptions.Converters.Add(new PolymorphicConverter(optionsResult.Build()));
			return jsonSerializerOptions;
		}
	}
}

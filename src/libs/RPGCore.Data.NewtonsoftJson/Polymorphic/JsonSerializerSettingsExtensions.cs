using Newtonsoft.Json;
using RPGCore.Data.NewtonsoftJson.Polymorphic.Internal;
using RPGCore.Data.Polymorphic;
using System;

namespace RPGCore.Data.NewtonsoftJson.Polymorphic
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

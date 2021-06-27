using Newtonsoft.Json;
using RPGCore.Data.Polymorphic.NewtonsoftJson.Internal;
using System;

namespace RPGCore.Data.Polymorphic.NewtonsoftJson
{
	/// <summary>
	/// Extensions for enabling support for RPGCore.Data.Polymorphic with Newtonsoft.Json.
	/// </summary>
	public static class JsonSerializerSettingsExtensions
	{
		/// <summary>
		/// Adds nessessary converters to <see cref="JsonSerializerSettings"/> to enable support for RPGCore.Data.Polymorphic.
		/// </summary>
		/// <param name="jsonSerializerSettings">The serializer settings to configure.</param>
		/// <returns>The current instance of the <see cref="JsonSerializerSettings"/>.</returns>
		public static JsonSerializerSettings UsePolymorphicSerialization(this JsonSerializerSettings jsonSerializerSettings)
		{
			var options = new PolymorphicOptions();
			jsonSerializerSettings.Converters.Add(new PolymorphicConverter(options.Build()));
			return jsonSerializerSettings;
		}

		/// <summary>
		/// Adds nessessary converters to <see cref="JsonSerializerSettings"/> to enable support for RPGCore.Data.Polymorphic.
		/// </summary>
		/// <param name="jsonSerializerSettings">The serializer settings to configure.</param>
		/// <param name="options">Options used to configure RPGCore.Data.Polymorphic.</param>
		/// <returns>The current instance of the <see cref="JsonSerializerSettings"/>.</returns>
		public static JsonSerializerSettings UsePolymorphicSerialization(this JsonSerializerSettings jsonSerializerSettings, Action<PolymorphicOptions> options)
		{
			var optionsResult = new PolymorphicOptions();
			options.Invoke(optionsResult);
			jsonSerializerSettings.Converters.Add(new PolymorphicConverter(optionsResult.Build()));
			return jsonSerializerSettings;
		}
	}
}

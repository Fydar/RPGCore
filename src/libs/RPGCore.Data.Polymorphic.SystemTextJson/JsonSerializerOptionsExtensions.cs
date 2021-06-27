using RPGCore.Data.Polymorphic.SystemTextJson.Internal;
using System;
using System.Text.Json;

namespace RPGCore.Data.Polymorphic.SystemTextJson
{
	/// <summary>
	/// Extensions for enabling support for RPGCore.Data.Polymorphic with System.Text.Json.
	/// </summary>
	public static class JsonSerializerOptionsExtensions
	{
		/// <summary>
		/// Adds nessessary converters to <see cref="JsonSerializerOptions"/> to enable support for RPGCore.Data.Polymorphic.
		/// </summary>
		/// <param name="jsonSerializerOptions">The serializer settings to configure.</param>
		/// <returns>The current instance of the <see cref="JsonSerializerOptions"/>.</returns>
		public static JsonSerializerOptions UsePolymorphicSerialization(this JsonSerializerOptions jsonSerializerOptions)
		{
			var options = new PolymorphicOptions();
			jsonSerializerOptions.Converters.Add(new PolymorphicConverterFactory(options.Build()));
			return jsonSerializerOptions;
		}

		/// <summary>
		/// Adds nessessary converters to <see cref="JsonSerializerOptions"/> to enable support for RPGCore.Data.Polymorphic.
		/// </summary>
		/// <param name="jsonSerializerOptions">The serializer settings to configure.</param>
		/// <param name="options">Options used to configure RPGCore.Data.Polymorphic.</param>
		/// <returns>The current instance of the <see cref="JsonSerializerOptions"/>.</returns>
		public static JsonSerializerOptions UsePolymorphicSerialization(this JsonSerializerOptions jsonSerializerOptions, Action<PolymorphicOptions> options)
		{
			var optionsResult = new PolymorphicOptions();
			options.Invoke(optionsResult);
			jsonSerializerOptions.Converters.Add(new PolymorphicConverterFactory(optionsResult.Build()));
			return jsonSerializerOptions;
		}
	}
}

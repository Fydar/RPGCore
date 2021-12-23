using RPGCore.Data.Polymorphic.Configuration;
using System;
using System.Globalization;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RPGCore.Data.Polymorphic.SystemTextJson.Internal
{
	/// <inheritdoc/>
	internal class PolymorphicConverterFactory : JsonConverterFactory
	{
		private readonly PolymorphicConfiguration configuration;

		internal PolymorphicConverterFactory(PolymorphicConfiguration configuration)
		{
			this.configuration = configuration;
		}

		/// <inheritdoc/>
		public override bool CanConvert(Type typeToConvert)
		{
			return configuration.TryGetBaseType(typeToConvert, out _);
		}

		/// <inheritdoc/>
		public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
		{
			var converterType = typeof(PolymorphicConverter<>).MakeGenericType(typeToConvert);

			return (JsonConverter)Activator.CreateInstance(
				converterType,
				BindingFlags.Instance | BindingFlags.NonPublic,
				null,
				new object[] { configuration, typeToConvert },
				CultureInfo.CurrentCulture)!;
		}
	}
}

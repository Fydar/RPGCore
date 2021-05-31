using RPGCore.Data.Polymorphic.Configuration;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RPGCore.Data.SystemTextJson.Polymorphic.Internal
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
			return new PolymorphicConverter(configuration, typeToConvert);
		}
	}
}

using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RPGCore.Data.Polymorphic.SystemTextJson.Internal
{
	/// <inheritdoc/>
	internal class PolymorphicConverterFactory : JsonConverterFactory
	{
		private readonly PolymorphicOptions options;

		internal PolymorphicConverterFactory(PolymorphicOptions options)
		{
			this.options = options;
		}

		/// <inheritdoc/>
		public override bool CanConvert(Type typeToConvert)
		{
			return options.TryGetBaseType(typeToConvert, out _);
		}

		/// <inheritdoc/>
		public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
		{
			return new PolymorphicConverter(this.options, typeToConvert);
		}
	}
}

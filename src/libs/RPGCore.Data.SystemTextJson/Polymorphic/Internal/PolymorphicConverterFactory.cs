using RPGCore.Data.Polymorphic;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RPGCore.Data.SystemTextJson.Polymorphic.Internal
{
	/// <inheritdoc/>
	internal class PolymorphicConverterFactory : JsonConverterFactory
	{
		private readonly PolymorphicOptions polymorphicConverterFactoryOptions;

		internal PolymorphicConverterFactory(PolymorphicOptions polymorphicConverterFactoryOptions)
		{
			this.polymorphicConverterFactoryOptions = polymorphicConverterFactoryOptions;
		}

		/// <inheritdoc/>
		public override bool CanConvert(Type typeToConvert)
		{
			object[] attributes = typeToConvert.GetCustomAttributes(typeof(SerializeTypeAttribute), false);

			return attributes.Length != 0;
		}

		/// <inheritdoc/>
		public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
		{
			return new PolymorphicConverter(polymorphicConverterFactoryOptions, typeToConvert);
		}
	}
}

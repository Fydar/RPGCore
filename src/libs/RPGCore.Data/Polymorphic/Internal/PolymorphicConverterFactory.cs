using RPGCore.Data.Polymorphic.Internal;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RPGCore.Data.Polymorphic
{
	/// <inheritdoc/>
	internal class PolymorphicConverterFactory : JsonConverterFactory
	{
		private readonly PolymorphicConverterFactoryOptions polymorphicConverterFactoryOptions;

		internal PolymorphicConverterFactory(PolymorphicConverterFactoryOptions polymorphicConverterFactoryOptions)
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

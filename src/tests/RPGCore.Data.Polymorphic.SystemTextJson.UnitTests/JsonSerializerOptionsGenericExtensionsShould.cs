using NUnit.Framework;
using RPGCore.Data.Polymorphic.Inline;
using RPGCore.Data.Polymorphic.SystemTextJson.UnitTests.Utility;
using System;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace RPGCore.Data.Polymorphic.SystemTextJson.UnitTests;

[TestFixture(TestOf = typeof(JsonSerializerOptionsExtensions))]
public class JsonSerializerOptionsGenericExtensionsShould
{
	[SerializeBaseType(typeof(ValueHolder<int>), TypeName.Name)]
	[SerializeBaseType(typeof(ValueHolder<float>), TypeName.Name)]
	[SerializeBaseType(typeof(ValueHolder<string>), TypeName.Name)]
	public interface IValueHolder
	{
	}

	public struct ValueHolder<T> : IValueHolder
		where T : notnull
	{
		public T Value { get; set; }
	}

	[Test, Parallelizable]
	public void SerializeGenericFloatValueHolder()
	{
		var options = new JsonSerializerOptions()
		{
			WriteIndented = true,
			Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
		};

		options.UsePolymorphicSerialization(options =>
		{
			options.UseInline();
		});

		IValueHolder original = new ValueHolder<float>()
		{
			Value = 10.0f
		};

		string serialized = JsonSerializer.Serialize(original, options);

		object? deserialized = JsonSerializer.Deserialize(serialized, typeof(IValueHolder), options);

		AssertUtility.AssertThatTypeIsEqualTo(deserialized, out IValueHolder deserializedProcedure);
		string reserialized = JsonSerializer.Serialize(deserializedProcedure, options);

		Console.WriteLine(serialized);
		Console.WriteLine(reserialized);

		Assert.That(serialized, Is.EqualTo(reserialized));
	}

	[Test, Parallelizable]
	public void SerializeGenericIntValueHolder()
	{
		var options = new JsonSerializerOptions()
		{
			WriteIndented = true,
			Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
		};

		options.UsePolymorphicSerialization(options =>
		{
			options.UseInline();
		});

		IValueHolder original = new ValueHolder<int>()
		{
			Value = 15
		};

		string serialized = JsonSerializer.Serialize(original, options);

		object? deserialized = JsonSerializer.Deserialize(serialized, typeof(IValueHolder), options);

		AssertUtility.AssertThatTypeIsEqualTo(deserialized, out IValueHolder deserializedProcedure);
		string reserialized = JsonSerializer.Serialize(deserializedProcedure, options);

		Console.WriteLine(serialized);
		Console.WriteLine(reserialized);

		Assert.That(serialized, Is.EqualTo(reserialized));
	}

	[Test, Parallelizable]
	public void SerializeGenericStringValueHolder()
	{
		var options = new JsonSerializerOptions()
		{
			WriteIndented = true,
			Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
		};

		options.UsePolymorphicSerialization(options =>
		{
			options.UseInline();
		});

		IValueHolder original = new ValueHolder<string>()
		{
			Value = "string value"
		};

		string serialized = JsonSerializer.Serialize(original, options);

		object? deserialized = JsonSerializer.Deserialize(serialized, typeof(IValueHolder), options);

		AssertUtility.AssertThatTypeIsEqualTo(deserialized, out IValueHolder deserializedProcedure);
		string reserialized = JsonSerializer.Serialize(deserializedProcedure, options);

		Console.WriteLine(serialized);
		Console.WriteLine(reserialized);

		Assert.That(serialized, Is.EqualTo(reserialized));
	}
}

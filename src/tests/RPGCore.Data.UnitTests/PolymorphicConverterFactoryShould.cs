using NUnit.Framework;
using RPGCore.Data.Polymorphic;
using RPGCore.Data.UnitTests.Utility;
using System;
using System.Text.Json;

namespace RPGCore.Data.UnitTests
{
	[TestFixture(TestOf = typeof(JsonSerializerOptionsExtensions))]
	public class PolymorphicConverterFactoryShould
	{
		public class Container
		{
			public IProcedure? RunOnStart { get; set; }
		}

		[SerializeType(typeof(CreateProcedure), TypeName.Name)]
		[SerializeType(typeof(UpdateProcedure), TypeName.Name)]
		[SerializeType(typeof(SomethingProcedure), TypeName.Name)]
		public interface IProcedure
		{
		}

		public class CreateProcedure : IProcedure
		{
			public string CreateString { get; set; } = "";
		}

		public class UpdateProcedure : IProcedure
		{
			public string UpdateString { get; set; } = "";
		}

		public class SomethingProcedure : CreateProcedure
		{
			public string SomethingString { get; set; } = "";
		}


		[Test, Parallelizable]
		public void ShouldSerializedPolymorphicTypeAtRoot()
		{
			var options = new JsonSerializerOptions()
			{
				WriteIndented = true,
			};

			options.UsePolymorphicSerialization();

			IProcedure original = new SomethingProcedure();

			string serialized = JsonSerializer.Serialize(original, options);
			object? deserialized = JsonSerializer.Deserialize(serialized, typeof(IProcedure), options);

			AssertUtility.AssertThatTypeIsEqualTo(deserialized, out IProcedure deserializedProcedure);
			string reserialized = JsonSerializer.Serialize(deserializedProcedure, options);

			Console.WriteLine(serialized);
			Console.WriteLine(reserialized);

			Assert.That(serialized, Is.EqualTo(reserialized));
		}


		[Test, Parallelizable]
		public void ShouldSerializedPolymorphicTypeInObject()
		{
			var options = new JsonSerializerOptions()
			{
				WriteIndented = true,
			};
			options.UsePolymorphicSerialization();

			var original = new Container()
			{
				RunOnStart = new SomethingProcedure()
			};

			string serialized = JsonSerializer.Serialize(original, options);
			object? deserialized = JsonSerializer.Deserialize(serialized, typeof(Container), options);
			string reserialized = JsonSerializer.Serialize(deserialized, options);

			Console.WriteLine(serialized);
			Console.WriteLine(reserialized);

			Assert.That(serialized, Is.EqualTo(reserialized));
		}
	}
}

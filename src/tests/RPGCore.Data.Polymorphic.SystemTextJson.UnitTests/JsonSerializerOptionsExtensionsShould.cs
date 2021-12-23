using NUnit.Framework;
using RPGCore.Data.Polymorphic.Inline;
using RPGCore.Data.Polymorphic.SystemTextJson.UnitTests.Utility;
using System;
using System.Text.Json;

namespace RPGCore.Data.Polymorphic.SystemTextJson.UnitTests
{
	[TestFixture(TestOf = typeof(JsonSerializerOptionsExtensions))]
	public class JsonSerializerOptionsExtensionsShould
	{
		public class Container
		{
			public IProcedure? RunOnStart { get; set; }
		}

		[SerializeBaseType(typeof(CreateProcedure), TypeName.Name)]
		[SerializeBaseType(typeof(UpdateProcedure), TypeName.Name)]
		[SerializeBaseType(typeof(SomethingProcedure), TypeName.Name)]
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
		public void SerializeInterfaceAtRoot()
		{
			var options = new JsonSerializerOptions()
			{
				WriteIndented = true,
			};

			options.UsePolymorphicSerialization(options =>
			{
				options.UseInline();
			});

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
		public void SerializeInterfaceArray()
		{
			var options = new JsonSerializerOptions()
			{
				WriteIndented = true,
			};

			options.UsePolymorphicSerialization(options =>
			{
				options.UseInline();
			});

			var original = new IProcedure[]
			{
				new SomethingProcedure(),
				new UpdateProcedure()
			};

			string serialized = JsonSerializer.Serialize(original, options);
			object? deserialized = JsonSerializer.Deserialize(serialized, typeof(IProcedure[]), options);

			AssertUtility.AssertThatTypeIsEqualTo(deserialized, out IProcedure deserializedProcedure);
			string reserialized = JsonSerializer.Serialize(deserializedProcedure, options);

			Console.WriteLine(serialized);
			Console.WriteLine(reserialized);

			Assert.That(serialized, Is.EqualTo(reserialized));
		}


		[Test, Parallelizable]
		public void SerializeInterfaceInObject()
		{
			var options = new JsonSerializerOptions()
			{
				WriteIndented = true,
			};

			options.UsePolymorphicSerialization(options =>
			{
				options.UseInline();
			});

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

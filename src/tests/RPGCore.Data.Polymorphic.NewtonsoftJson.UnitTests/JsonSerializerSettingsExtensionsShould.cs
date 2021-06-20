using Newtonsoft.Json;
using NUnit.Framework;
using RPGCore.Data.Polymorphic.Inline;
using RPGCore.Data.Polymorphic.NewtonsoftJson.UnitTests.Utility;
using System;

namespace RPGCore.Data.Polymorphic.NewtonsoftJson.UnitTests
{
	[TestFixture(TestOf = typeof(JsonSerializerSettingsExtensions))]
	public class JsonSerializerSettingsExtensionsShould
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
		public void SerializeInterfaceAtRoot()
		{
			var options = new JsonSerializerSettings();

			options.UsePolymorphicSerialization(options =>
			{
				options.UseInline();
			});

			IProcedure original = new SomethingProcedure();
			string serialized = JsonConvert.SerializeObject(original, typeof(IProcedure), Formatting.Indented, options);

			Console.WriteLine(serialized);

			object? deserialized = JsonConvert.DeserializeObject(serialized, typeof(IProcedure), options);

			AssertUtility.AssertThatTypeIsEqualTo(deserialized, out IProcedure deserializedProcedure);

			string reserialized = JsonConvert.SerializeObject(deserializedProcedure, typeof(IProcedure), Formatting.Indented, options);

			Console.WriteLine(reserialized);

			Assert.That(serialized, Is.EqualTo(reserialized));
		}

		[Test, Parallelizable]
		public void SerializeInterfaceInObject()
		{
			var options = new JsonSerializerSettings();

			options.UsePolymorphicSerialization(options =>
			{
				options.UseInline();
			});

			var original = new Container()
			{
				RunOnStart = new SomethingProcedure()
			};

			string serialized = JsonConvert.SerializeObject(original, typeof(Container), Formatting.Indented, options);

			Console.WriteLine(serialized);

			object? deserialized = JsonConvert.DeserializeObject(serialized, typeof(Container), options);

			AssertUtility.AssertThatTypeIsEqualTo(deserialized, out Container deserializedProcedure);

			string reserialized = JsonConvert.SerializeObject(deserializedProcedure, typeof(Container), Formatting.Indented, options);

			Console.WriteLine(reserialized);

			Assert.That(serialized, Is.EqualTo(reserialized));
		}
	}
}

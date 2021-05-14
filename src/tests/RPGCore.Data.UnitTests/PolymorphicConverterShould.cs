using NUnit.Framework;
using RPGCore.Data.Polymorphic;
using System;
using System.Text.Json;

namespace RPGCore.Data.UnitTests
{
	[TestFixture(TestOf = typeof(PolymorphicConverter))]
	public class PolymorphicConverterShould
	{
		public class Container
		{
			[SerializeType]
			public IProcedure? RunOnStart { get; set; }
		}

		[SerializeType(typeof(CreateProcedure))]
		[SerializeType(typeof(UpdateProcedure))]
		[SerializeType(typeof(SomethingProcedure))]
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
		public void FromStringShould()
		{
			var options = new JsonSerializerOptions()
			{
				WriteIndented = true
			};

			options.Converters.Add(new PolymorphicConverter());



			var original = new Container()
			{
				RunOnStart = new SomethingProcedure()
			};

			string serialized = JsonSerializer.Serialize(original, options);
			Console.WriteLine(serialized);

			object? deserialized = JsonSerializer.Deserialize(serialized, typeof(Container), options);


			string reserialized = JsonSerializer.Serialize(deserialized, options);
			Console.WriteLine(reserialized);


			Assert.That(serialized, Is.EqualTo(reserialized));
		}
	}
}

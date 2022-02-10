using NUnit.Framework;
using RPGCore.Data.Polymorphic.Inline;
using RPGCore.Data.Polymorphic.SystemTextJson;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace RPGCore.Behaviour.UnitTests;

[TestFixture(TestOf = typeof(Graph))]
public class GraphShould
{
	[Test, Parallelizable]
	public void SerializeAndDeserialize()
	{
		var jsonSerializerOptions = new JsonSerializerOptions()
			.UsePolymorphicSerialization(options =>
			{
				options.UseGraphSerialization();
				options.UseInline();
			});
		jsonSerializerOptions.WriteIndented = true;
		jsonSerializerOptions.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;

		var graph = new Graph(new Node[]
		{
			new AddNode()
			{
				Id = LocalId.NewShortId().ToString()
			},
			new IterateNode()
			{
				Id = LocalId.NewShortId().ToString(),
				Graph = new Graph(new Node[]
				{
					new AddNode()
					{
						Id = LocalId.NewShortId().ToString()
					},
					new AddNode()
					{
						Id = LocalId.NewShortId().ToString()
					}
				})
			}
		});

		string serializedGraph = JsonSerializer.Serialize(graph, jsonSerializerOptions);

		TestContext.Out.WriteLine(serializedGraph);

		var deserializedGraph = JsonSerializer.Deserialize<Graph>(serializedGraph, jsonSerializerOptions);

		string reserializedGraph = JsonSerializer.Serialize(deserializedGraph, jsonSerializerOptions);

		Assert.That(serializedGraph, Is.EqualTo(reserializedGraph));
	}
}

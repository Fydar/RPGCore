using NUnit.Framework;
using RPGCore.Data.Polymorphic.Inline;
using RPGCore.Data.Polymorphic.SystemTextJson;
using System.Collections.Generic;
using System.Text.Json;

namespace RPGCore.Behaviour.UnitTests;

[TestFixture(TestOf = typeof(GraphEngine))]
public class GraphRuntimeShould
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

		var graphEngine = new GraphEngine();

		string firstNode = LocalId.NewShortId().ToString();

		var swordItemGraph = new Graph(new Node[]
		{
			new AddNode() {
				Id = firstNode,
				ValueA = new DefaultInput<int>(15),
				ValueB = new DefaultInput<int>(25),
				Output = new Output<int>()
			},
			new DurabilityNode() {
				Id = LocalId.NewShortId().ToString(),
				BaseDurability = new ConnectedInput<int>($"{firstNode}.Output"),
			},
			new IterateNode() {
				Id = LocalId.NewShortId().ToString(),
				Graph = new Graph(new Node[]
				{
					new AddNode() {
						Id = LocalId.NewShortId().ToString()
					},
					new AddNode() {
						Id = LocalId.NewShortId().ToString()
					}
				})
			}
		});

		string serializedGraph = JsonSerializer.Serialize(swordItemGraph, jsonSerializerOptions);
		TestContext.Out.WriteLine(serializedGraph);

		var mainModule = GraphEngineModule.Create()
			.UseGraph("graph-1", swordItemGraph)
			.Build();

		graphEngine.LoadModule(mainModule);
		var weaponGraph = mainModule.Graphs["graph-1"];

		var graphDefinition = weaponGraph.CreateDefinition();

		var graphRuntimeData = graphDefinition.CreateRuntimeData();
		//  {
		//  	var subRuntimeDefinition = graphDefinition.CreateRuntimeData();
		//  	{
		//  		subRuntimeDefinition.Nodes["123"] = new IterateNode.IterateNodeData()
		//  		{
		//  			Instances = new List<GraphRuntimeData>()
		//  			{
		//  
		//  			}
		//  		};
		//  		subRuntimeDefinition.Outputs["123"] = new Output<int>.OutputData()
		//  		{
		//  			Value = 10
		//  		};
		//  	}
		//  	graphRuntimeData.Nodes["123"] = new IterateNode.IterateNodeData()
		//  	{
		//  		Instances = new List<GraphRuntimeData>()
		//  		{
		//  			subRuntimeDefinition
		//  		}
		//  	};
		//  }

		var graphRuntime = graphEngine.CreateGraphRuntime(graphDefinition, graphRuntimeData);

		graphRuntime.Enable();
		var durabilityNode = graphRuntime.GetNode<DurabilityNode>();
		if (durabilityNode != null)
		{
			durabilityNode.Value.UseOutput(durabilityNode.Value.Node.CurrentDurability, out var output);
			output.Value += 5;
		}
		graphRuntime.Disable();

		string serializedGraphRunner = JsonSerializer.Serialize(graphRuntimeData, jsonSerializerOptions);

		var deserializedGraphRunner = JsonSerializer.Deserialize<GraphRuntimeData>(serializedGraphRunner, jsonSerializerOptions);

		string reserializedGraphRunner = JsonSerializer.Serialize(deserializedGraphRunner, jsonSerializerOptions);

		Assert.That(serializedGraphRunner, Is.EqualTo(reserializedGraphRunner));

		TestContext.Out.WriteLine(reserializedGraphRunner);
	}
}

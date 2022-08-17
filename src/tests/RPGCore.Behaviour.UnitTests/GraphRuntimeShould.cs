using NUnit.Framework;
using RPGCore.Behaviour.Fluent;
using RPGCore.Behaviour.UnitTests.Nodes;
using RPGCore.Data.Polymorphic.Inline;
using RPGCore.Data.Polymorphic.SystemTextJson;
using System.Text.Json;

namespace RPGCore.Behaviour.UnitTests;

[TestFixture(TestOf = typeof(BehaviourEngine))]
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

		var graph1 = Graph.Create()
			.AddNode<AddNode>(node =>
			{
				node.ValueA = Input.Default(5);
				node.ValueB = Input.Default(5);
			})
			.AddNode<AddNode>(node =>
			{
				node.ValueA = Input.Default(15);
				node.ValueB = Input.Default(25);
			}, out var addNode)
			.AddNode<DurabilityNode>(node =>
			{
				node.BaseDurability = Input.Connected<int>(addNode, "Output");
			})
			.AddNode<IterateNode>(node =>
			{
				node.Iterations = Input.Default(3);

				node.Graph = Graph.Create()
					.AddNode<AddNode>(node =>
					{
						node.ValueA = Input.Default(10);
						node.ValueB = Input.Default(15);
					}, out var childAddNode)
					.AddNode<AddNode>(node =>
					{
						node.ValueA = Input.Connected<int>(childAddNode, "Output");
						node.ValueB = Input.Default(30);
					})
					.Build();
			})
			.Build();

		var mainModule = BehaviourEngineModule.Create()
			.UseGraph("graph-1", graph1)
			.Build();

		var behaviourEngine = new BehaviourEngine();
		behaviourEngine.LoadModule(mainModule);

		var weaponGraph = mainModule.Graphs["graph-1"];

		string serializedGraph = JsonSerializer.Serialize(weaponGraph, jsonSerializerOptions);

		TestContext.Out.WriteLine("[========[ RuntimeGraphData ]========]");
		TestContext.Out.WriteLine(serializedGraph);

		var graphDefinition = weaponGraph.CreateDefinition();
		var graphEngine = graphDefinition.CreateEngine();

		for (int i = 0; i < graphEngine.Nodes.Count; i++)
		{
			var node = graphEngine.Nodes[i];

			for (int j = 0; j < node.Outputs.Count; j++)
			{
				var output = node.Outputs[j];

				TestContext.Out.WriteLine(output.Name);

				for (int k = 0; k < output.ConnectedInputs.Count; k++)
				{
					var connectedInput = output.ConnectedInputs[k];
				}
			}
		}

		var graphRuntimeData = graphEngine.CreateInstanceData();
		var graphRuntime = behaviourEngine.CreateGraphRuntime(graphEngine, graphRuntimeData);

		using (var graphMutation = graphRuntime.Mutate())
		{
			graphMutation.Enable();
		}

		using (var graphMutation = graphRuntime.Mutate())
		{
			if (graphMutation.TryGetNode<DurabilityNode>(out var durabilityNode))
			{
				durabilityNode.OpenOutput(durabilityNode.Node.CurrentDurability, out var output);
				output.Value += 5;
			}
		}

		using (var graphMutation = graphRuntime.Mutate())
		{
			graphMutation.Disable();
		}

		string serializedGraphRuntimeData = JsonSerializer.Serialize(graphRuntimeData, jsonSerializerOptions);

		TestContext.Out.WriteLine(" ");
		TestContext.Out.WriteLine("[========[ RuntimeGraphData ]========]");
		TestContext.Out.WriteLine(serializedGraphRuntimeData);

		var deserializedGraphRuntimeData = JsonSerializer.Deserialize<GraphInstanceData>(serializedGraphRuntimeData, jsonSerializerOptions);
		string reserializedGraphRuntimeData = JsonSerializer.Serialize(deserializedGraphRuntimeData, jsonSerializerOptions);

		Assert.That(serializedGraphRuntimeData, Is.EqualTo(reserializedGraphRuntimeData));
	}
}

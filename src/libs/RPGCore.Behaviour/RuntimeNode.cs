using System;
using System.Collections.Generic;

namespace RPGCore.Behaviour;

public struct RuntimeNode
{
	public GraphRuntime GraphRuntime { get; }
	public Node Node { get; }

	public RuntimeNode(
		GraphRuntime graphRuntime,
		Node node)
	{
		GraphRuntime = graphRuntime;
		Node = node;
	}
}

public struct RuntimeNode<TNode>
	where TNode : Node
{
	public GraphRuntime GraphRuntime { get; }
	public TNode Node { get; }

	public RuntimeNode(
		GraphRuntime graphRuntime,
		TNode node)
	{
		GraphRuntime = graphRuntime;
		Node = node;
	}

	public void UseInput<TType>(
		IInput<TType> inputSocket,
		out GraphRuntimeInput<TType> socket)
	{
		socket = default;

		if (inputSocket is ConnectedInput<TType> connectedInput)
		{
			string outputIdentifier = connectedInput.Path.ToString();
			var outputPath = new LocalPropertyId(outputIdentifier);

			ref var nodeData = ref GraphRuntime.GraphRuntimeData.GetNode(outputPath.TargetIdentifier.ToString());

			if (nodeData.Outputs == null)
			{
				nodeData.Outputs = new Dictionary<string, IOutputData>();
			}

			if (nodeData.Outputs.TryGetValue(outputPath.PropertyPath[0], out var outputData))
			{
				var castedOutputData = outputData as Output<TType>.OutputData;
				socket = new GraphRuntimeInput<TType>(GraphRuntime.GraphRuntimeData, inputSocket, castedOutputData.Value, false);
			}
			else
			{
				throw new InvalidOperationException("Failed to locate Output that the input is connected to.");
			}
		}
		else if (inputSocket is DefaultInput<TType> defaultInput)
		{
			socket = new GraphRuntimeInput<TType>(GraphRuntime.GraphRuntimeData, inputSocket, defaultInput.DefaultValue, false);
		}
	}

	public void UseOutput<TType>(Output<TType>? outputSocket, out GraphRuntimeOutput<TType> socket)
	{
		socket = default;

		NodeDefinition? nodeDefinition = null;
		foreach (var searchNodeDefinition in GraphRuntime.GraphDefinition.NodeDefinitions)
		{
			if (searchNodeDefinition.Node == Node)
			{
				nodeDefinition = searchNodeDefinition;
				break;
			}
		}
		if (nodeDefinition == null)
		{
			throw new InvalidOperationException("");
		}

		NodeOutputDefinition? outputDefinition = null;
		foreach (var output in nodeDefinition.Outputs)
		{
			if (output.output == outputSocket)
			{
				outputDefinition = output;
				break;
			}
		}

		ref var nodeData = ref GraphRuntime.GraphRuntimeData.GetNode(Node.Id);

		if (nodeData.Outputs == null)
		{
			nodeData.Outputs = new Dictionary<string, IOutputData>();
		}

		if (nodeData.Outputs.TryGetValue(outputDefinition.Name, out var outputData))
		{
			var castedOutputData = outputData as Output<TType>.OutputData;
			socket = new GraphRuntimeOutput<TType>(castedOutputData);
		}
		else
		{
			var castedOutputData = new Output<TType>.OutputData();
			nodeData.Outputs.Add(outputDefinition.Name, castedOutputData);

			socket = new GraphRuntimeOutput<TType>(castedOutputData);
		}
	}

	public ref TComponent GetComponent<TComponent>()
		where TComponent : struct, IRuntimeNodeComponent
	{
		ref var nodeData = ref GraphRuntime.GraphRuntimeData.GetNode(Node.Id);

		return ref nodeData.GetComponent<TComponent>();
	}
}

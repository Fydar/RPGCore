using System;

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

			if (GraphRuntime.Data.Outputs.TryGetValue(outputIdentifier, out var outputData))
			{
				var castedOutputData = outputData as Output<TType>.OutputData;
				socket = new GraphRuntimeInput<TType>(GraphRuntime.Data, inputSocket, castedOutputData.Value, false);
			}
			else
			{
				var castedOutputData = new Output<TType>.OutputData();
				GraphRuntime.Data.Outputs.Add(outputIdentifier, castedOutputData);

				socket = new GraphRuntimeInput<TType>(GraphRuntime.Data, inputSocket, castedOutputData.Value, false);
			}
		}
		else if (inputSocket is DefaultInput<TType> defaultInput)
		{
			socket = new GraphRuntimeInput<TType>(GraphRuntime.Data, inputSocket, defaultInput.DefaultValue, false);
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

		string outputIdentifier = $"{Node.Id}.{outputDefinition.Name}";

		if (GraphRuntime.Data.Outputs.TryGetValue(outputIdentifier, out var outputData))
		{
			var castedOutputData = outputData as Output<TType>.OutputData;
			socket = new GraphRuntimeOutput<TType>(castedOutputData);
		}
		else
		{
			var castedOutputData = new Output<TType>.OutputData();
			GraphRuntime.Data.Outputs.Add(outputIdentifier, castedOutputData);

			socket = new GraphRuntimeOutput<TType>(castedOutputData);
		}
	}

	public ref TNodeData GetNodeInstanceData<TNodeData>()
		where TNodeData : struct, INodeData
	{
		if (!GraphRuntime.Data.Nodes.TryGetValue(Node.Id, out var nodeData))
		{
			nodeData = new TNodeData();
			GraphRuntime.Data.Nodes.Add(Node.Id, nodeData);
		}

		var array = new TNodeData[1];

		return ref array[0];
	}
}

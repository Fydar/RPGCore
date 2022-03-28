using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RPGCore.Behaviour;

public class NodeRuntimeData
{
	[JsonIgnore]
	internal Array[] componentPools;

	[JsonPropertyOrder(-1)]
	public string Id { get; set; }

	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public Dictionary<string, IOutputData>? Outputs { get; set; }

	public NodeRuntimeData()
	{
		componentPools = Array.Empty<Array>();
		Id = string.Empty;
		Outputs = null;
	}

	[JsonInclude]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public IRuntimeNodeComponent[]? Components
	{
		get
		{
			if (componentPools == null || componentPools.Length == 0)
			{
				return null;
			}
			var data = new IRuntimeNodeComponent[componentPools.Length];
			for (int i = 0; i < data.Length; i++)
			{
				data[i] = componentPools[i].GetValue(0) as IRuntimeNodeComponent;
			}
			return data;
		}
		set
		{
			if (value == null)
			{
				componentPools = Array.Empty<Array>();
				return;
			}
			componentPools = new Array[value.Length];
			for (int i = 0; i < value.Length; i++)
			{
				var component = value[i];

				var componentPool = Array.CreateInstance(component.GetType(), 1);
				componentPool.SetValue(component, 0);
				componentPools[i] = componentPool;
			}
		}
	}

	public ref TComponent GetComponent<TComponent>()
		where TComponent : struct, IRuntimeNodeComponent
	{
		for (int i = 0; i < componentPools.Length; i++)
		{
			var componentPool = componentPools[i];

			if (componentPool is TComponent[] components)
			{
				return ref components[0];
			}
		}
		throw new InvalidOperationException($"Unable to find node runtime data with type {typeof(TComponent).Name}.");
	}
}

public sealed class GraphRuntimeData
{
	public NodeRuntimeData[] Nodes { get; set; } = Array.Empty<NodeRuntimeData>();

	public ref NodeRuntimeData GetNode(string id)
	{
		for (int i = 0; i < Nodes.Length; i++)
		{
			ref var node = ref Nodes[i];
			if (node.Id == id)
			{
				return ref node;
			}
		}
		throw new InvalidOperationException($"Unable to find node runtime data with id {id}.");
	}

	public bool ContainsNode(string id)
	{
		for (int i = 0; i < Nodes.Length; i++)
		{
			ref var node = ref Nodes[i];
			if (node.Id == id)
			{
				return true;
			}
		}
		return false;
	}
}

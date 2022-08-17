using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RPGCore.Behaviour;

public class GraphInstanceDataNode
{
	[JsonIgnore]
	internal Array[] componentPools;

	/// <summary>
	/// The identifier for the node this data represents.
	/// </summary>
	[JsonPropertyOrder(-1)]
	public string Id { get; set; }

	/// <summary>
	/// Data for all outputs associated with this node.
	/// </summary>
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public Dictionary<string, IOutputData>? Outputs { get; set; }

	/// <summary>
	/// Creates a new instance of the <see cref="GraphInstanceDataNode"/> class.
	/// </summary>
	public GraphInstanceDataNode()
	{
		componentPools = Array.Empty<Array>();
		Id = string.Empty;
		Outputs = null;
	}

	[JsonInclude]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public INodeComponent[]? Components
	{
		get
		{
			if (componentPools == null || componentPools.Length == 0)
			{
				return null;
			}
			var data = new INodeComponent[componentPools.Length];
			for (int i = 0; i < data.Length; i++)
			{
				data[i] = (INodeComponent)componentPools[i].GetValue(0);
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
		where TComponent : struct, INodeComponent
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

	public GraphInstanceOutput<TType> GetOrCreateOutput<TType>(GraphEngineNodeOutput graphEngineNodeOutput)
	{
		if (Outputs == null)
		{
			Outputs = new Dictionary<string, IOutputData>();
		}

		Output<TType>.OutputData castedOutputData;
		if (Outputs.TryGetValue(graphEngineNodeOutput.Name, out var outputData))
		{
			castedOutputData = (Output<TType>.OutputData)outputData;
		}
		else
		{
			castedOutputData = new Output<TType>.OutputData();
			Outputs.Add(graphEngineNodeOutput.Name, castedOutputData);
		}

		return new GraphInstanceOutput<TType>(castedOutputData);
	}
}

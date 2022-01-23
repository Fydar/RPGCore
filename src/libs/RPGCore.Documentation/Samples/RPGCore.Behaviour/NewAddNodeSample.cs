namespace RPGCore.Documentation.Samples.RPGCore.Behaviour.NewAddNodeSample;

public class NodeTemplate
{
	public class Instance
	{

	}

	public virtual void OnInputChanged(GraphInstance graphInstance)
	{

	}
}
public class GraphInstance
{
	public void UseInput<TType>(InputSocket<TType> inputSocket, out InputSocketInstance<TType> socket)
	{
		socket = default;
	}
	public void UseOutput<TType>(OutputSocket<TType> outputSocket, out OutputSocketInstance<TType> socket)
	{
		socket = default;
	}
}
public struct InputSocket<TType>
{

}
public struct InputSocketInstance<TType>
{
	public float Value { get; set; }
}
public struct OutputSocket<TType>
{

}
public struct OutputSocketInstance<TType>
{
	public float Value { get; set; }
}

#region default
public class AddNode : NodeTemplate
{
	public InputSocket<float> ValueA;
	public InputSocket<float> ValueB;

	public OutputSocket<float> Output;

	public override void OnInputChanged(GraphInstance graphInstance)
	{
		graphInstance.UseInput(ValueA, out var valueA);
		graphInstance.UseInput(ValueB, out var valueB);
		graphInstance.UseOutput(Output, out var output);

		output.Value = valueA.Value + valueB.Value;
	}
}
#endregion default

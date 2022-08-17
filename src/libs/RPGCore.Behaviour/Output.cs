using System.Text.Json.Serialization;

namespace RPGCore.Behaviour;

public sealed class Output<TType> : IOutput
{
	public class OutputData : IOutputData
	{
		[JsonPropertyName("value")]
		public TType Value { get; set; }

		[JsonIgnore]
		public bool HasChanged { get; set; }
	}
}

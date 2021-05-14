using Newtonsoft.Json;
using RPGCore.Data;

namespace RPGCore.Demo.BoardGame.Models
{
	[EditableType]
	public class ResourceTemplate : IResourceModel
	{
		[JsonIgnore]
		public string Identifier { get; set; }

		public string DisplayName { get; set; }
		public VoxelColour Colour { get; set; }
	}
}

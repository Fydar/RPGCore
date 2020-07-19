using Newtonsoft.Json;
using RPGCore.Behaviour;

namespace RPGCore.Demo.BoardGame.Models
{
	[EditorType]
	public class ResourceTemplate : IResourceModel
	{
		[JsonIgnore]
		public string Identifier { get; set; }

		public string DisplayName { get; set; }
		public VoxelColour Colour { get; set; }
	}
}

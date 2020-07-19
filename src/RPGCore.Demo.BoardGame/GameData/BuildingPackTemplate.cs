using Newtonsoft.Json;
using RPGCore.Behaviour;

namespace RPGCore.Demo.BoardGame.Models
{
	[EditorType]
	public class BuildingPackTemplate : IResourceModel
	{
		[JsonIgnore]
		public string Identifier { get; set; }

		public string DisplayName { get; set; }
	}
}

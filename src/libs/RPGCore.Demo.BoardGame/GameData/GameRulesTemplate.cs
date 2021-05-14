using Newtonsoft.Json;
using RPGCore.Data;
using System.Collections.Generic;

namespace RPGCore.Demo.BoardGame.Models
{
	[EditableType]
	public class GameRulesTemplate : IResourceModel
	{
		[JsonIgnore]
		public string Identifier { get; set; }

		public List<string> SharedCards { get; set; }
		public List<string> PlayerCards { get; set; }

		public int ResourceTurnCarryover { get; set; }
	}
}

using RPGCore.Behaviour;

namespace RPGCore.Items
{
	public abstract class ItemTemplate
	{
		public string DisplayName { get; set; }
		public string Description { get; set; }
		public int Weight { get; set; }

		public SerializedGraph Behaviour { get; set; }
	}
}

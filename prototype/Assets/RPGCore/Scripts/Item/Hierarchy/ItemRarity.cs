using UnityEngine;

namespace RPGCore
{
	[CreateAssetMenu (menuName = "RPGCore/Item Rarity")]
	public class ItemRarity : ScriptableObject
	{
		public Color NameColour;
		public Sprite SlotImage;

		[Header ("Tooltip")]
		public Color HeaderText;
		public Color HeaderBackground;
	}
}

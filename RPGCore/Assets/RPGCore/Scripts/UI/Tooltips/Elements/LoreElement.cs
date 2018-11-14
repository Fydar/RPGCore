using UnityEngine;
using UnityEngine.UI;

namespace RPGCore.Tooltips
{
	public class LoreElement : TooltipElement, ITooltipTarget<ItemSurrogate>
	{
		[SerializeField]
		private Text itemDescription = null;

		public void Render (ItemSurrogate target)
		{
			string description = "";

			description += target.Description.ToString ();

			itemDescription.text = description;
		}
	}
}
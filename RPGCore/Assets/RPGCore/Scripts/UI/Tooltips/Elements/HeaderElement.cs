using RPGCore.Inventories;
using RPGCore.Stats;
using UnityEngine;
using UnityEngine.UI;

namespace RPGCore.Tooltips
{
	public class HeaderElement : TooltipElement, ITooltipTarget<ItemSurrogate>, ITooltipTarget<StatInstance>,
		ITooltipTarget<Buff>
	{
		[SerializeField] private Text itemName;
		[SerializeField] private Image headerBackground;

		[Space]
		[SerializeField] private Color defaultBackgroundColor;
		[SerializeField] private Color defaultTextColor;

		public void Render (ItemSlot target)
		{
			Render (target.Item);
		}

		public void Render (ItemSurrogate target)
		{
			itemName.text = target.FullName;

			if (target.template.Rarity != null)
			{
				headerBackground.color = target.template.Rarity.HeaderBackground;
				itemName.color = target.template.Rarity.HeaderText;
			}
			else
			{
				ResetColors ();
			}
		}

		public void Render (StatInstance target)
		{
			itemName.text = target.Info.Name;

			ResetColors ();
		}

		public void Render (Buff target)
		{
			itemName.text = target.buffTemplate.Name;

			ResetColors ();
		}

		private void ResetColors ()
		{
			headerBackground.color = defaultBackgroundColor;
			itemName.color = defaultTextColor;
		}
	}
}

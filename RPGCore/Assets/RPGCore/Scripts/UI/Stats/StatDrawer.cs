using RPGCore.Tooltips;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RPGCore.Stats
{
	public class StatDrawer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
	{
		public Text NameText;
		public Text ValueText;

		StatInstance info;

		public void Setup (StatInstance _info)
		{
			info = _info;

			if (info.Info == null)
			{
				Destroy (gameObject);
				return;
			}

			OnStatChanged ();
			info.OnValueChanged += OnStatChanged;
		}

		void OnStatChanged ()
		{
			NameText.text = info.Info.Name;
			ValueText.text = info.Info.RenderValue (info.Value);
		}

		public void OnPointerEnter (PointerEventData eventData)
		{
			TooltipManager.instance.StatTooltip (GetComponent<RectTransform> (), info);
		}

		public void OnPointerExit (PointerEventData eventData)
		{
			TooltipManager.instance.Hide ();
		}
	}
}
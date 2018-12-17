using UnityEngine;
using UnityEngine.UI;

namespace RPGCore.UI
{
	public class UIKeyValue : MonoBehaviour
	{
		public Text Key;
		public Text Value;

		[Space]

		[SerializeField] private Color defaultStatColor;
		[SerializeField] private Color enchantedStatColor;

		public void Setup(string key, string value, bool enchant = false)
		{
			Key.gameObject.SetActive (!string.IsNullOrEmpty (key));
			Value.gameObject.SetActive (!string.IsNullOrEmpty (value));

			Key.text = key;
			Value.text = value;

			Value.color = enchant ? enchantedStatColor : defaultStatColor;
		}
	}
}

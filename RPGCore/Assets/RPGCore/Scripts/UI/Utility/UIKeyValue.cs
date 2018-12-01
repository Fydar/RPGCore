using UnityEngine;
using UnityEngine.UI;

namespace RPGCore.UI
{
	public class UIKeyValue : MonoBehaviour
	{
		public Text Key;
		public Text Value;

		public void Setup(string key, string value)
		{
			Key.gameObject.SetActive (!string.IsNullOrEmpty (key));
			Value.gameObject.SetActive (!string.IsNullOrEmpty (value));

			Key.text = key;
			Value.text = value;
		}
	}
}

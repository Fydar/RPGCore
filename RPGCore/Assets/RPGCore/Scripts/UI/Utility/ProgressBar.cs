using UnityEngine;
using UnityEngine.UI;

namespace RPGCore.UI
{
	public class ProgressBar : MonoBehaviour
	{
		public Image Bar;

		[Header ("Labels")]
		public Text StartLabel;
		public Text EndLabel;
		public Text CurrentLabel;

		public void SetFill (float amount)
		{
			Bar.fillAmount = amount;
		}

		public void SetLabels (string start, string end, string current = null)
		{
			if (StartLabel != null)
			{
				StartLabel.text = start;
				StartLabel.gameObject.SetActive (true);
			}
			if (EndLabel != null)
			{
				EndLabel.text = end;
				EndLabel.gameObject.SetActive (true);
			}
			if (CurrentLabel != null && !string.IsNullOrEmpty (current))
			{
				CurrentLabel.text = current;
				CurrentLabel.gameObject.SetActive (true);
			}
		}

		public void ClearLabels ()
		{
			StartLabel.text = "";
			EndLabel.text = "";

			if (StartLabel != null)
				StartLabel.gameObject.SetActive (false);

			if (EndLabel != null)
				EndLabel.gameObject.SetActive (false);

			if (CurrentLabel != null)
				CurrentLabel.gameObject.SetActive (false);
		}
	}
}
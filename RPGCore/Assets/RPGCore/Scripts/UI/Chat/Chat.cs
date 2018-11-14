using RPGCore.Utility.InspectorLog;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace RPGCore.UI
{
	[Serializable] public class TextPool : UIPool<Text> { }

	public class Chat : MonoBehaviour
	{
		public static Chat Instance;

		public InspectorLog EditorLog;

		public RectTransform Holder;
		public TextPool TextElements;

		private void Awake ()
		{
			Instance = this;

			TextElements.Flush ();
		}

		public void Log (string text)
		{
			Text textElement = TextElements.Grab (Holder);

			textElement.text = text;
			EditorLog.Log (text);
		}
	}
}
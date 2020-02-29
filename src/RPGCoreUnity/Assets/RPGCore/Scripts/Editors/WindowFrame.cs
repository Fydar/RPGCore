using UnityEditor;
using UnityEngine;

namespace RPGCore.Unity.Editors
{
	public abstract class WindowFrame
	{
		public Rect Position;
		public EditorWindow Window;

		public abstract void OnEnable();
		public abstract void OnGUI();
	}
}

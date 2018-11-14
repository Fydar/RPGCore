using UnityEditor;
using UnityEngine;

namespace RPGCore.Behaviour.Editor
{
	public class BehaviourGUIStyles
	{
		private static BehaviourGUIStyles instance;

		public static BehaviourGUIStyles Instance
		{
			get
			{
				if (instance == null)
					instance = new BehaviourGUIStyles ();

				return instance;
			}
		}

		public GUIStyle settingsStyle;
		public GUIStyle backgroundTextStyle;
		public GUIStyle informationTextStyle;
		public GUIStyle EmptyTextStyle;
		public GUIStyle HelpHeaderTextStyle;

		public BehaviourGUIStyles ()
		{
			settingsStyle = GUI.skin.GetStyle ("PaneOptions");

			backgroundTextStyle = new GUIStyle (EditorStyles.centeredGreyMiniLabel);
			backgroundTextStyle.fontSize = 40;

			informationTextStyle = new GUIStyle (EditorStyles.centeredGreyMiniLabel);
			informationTextStyle.fontSize = 16;
			informationTextStyle.normal.textColor = new Color (0.25f, 0.25f, 0.25f);

			EmptyTextStyle = new GUIStyle (EditorStyles.centeredGreyMiniLabel);
			EmptyTextStyle.fontSize = 16;
			EmptyTextStyle.normal.textColor = new Color (0.25f, 0.25f, 0.25f);

			HelpHeaderTextStyle = new GUIStyle (EditorStyles.label);
			HelpHeaderTextStyle.alignment = TextAnchor.UpperCenter;
		}
	}
}
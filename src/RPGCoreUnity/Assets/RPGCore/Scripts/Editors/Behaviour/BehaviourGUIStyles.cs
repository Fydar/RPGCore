#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace RPGCoreUnity.Editors
{
	public class BehaviourGUIStyles
	{
		private static BehaviourGUIStyles instance;

		public static BehaviourGUIStyles Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new BehaviourGUIStyles();
				}

				return instance;
			}
		}

		public GUIStyle settingsStyle;
		public GUIStyle backgroundTextStyle;
		public GUIStyle informationTextStyle;
		public GUIStyle EmptyTextStyle;
		public GUIStyle HelpHeaderTextStyle;

		public BehaviourGUIStyles()
		{
			settingsStyle = GUI.skin.GetStyle("PaneOptions");

			backgroundTextStyle = new GUIStyle(EditorStyles.centeredGreyMiniLabel)
			{
				fontSize = 40
			};

			informationTextStyle = new GUIStyle(EditorStyles.centeredGreyMiniLabel)
			{
				fontSize = 16
			};
			informationTextStyle.normal.textColor = new Color(0.25f, 0.25f, 0.25f);

			EmptyTextStyle = new GUIStyle(EditorStyles.centeredGreyMiniLabel)
			{
				fontSize = 16
			};
			EmptyTextStyle.normal.textColor = new Color(0.25f, 0.25f, 0.25f);

			HelpHeaderTextStyle = new GUIStyle(EditorStyles.label)
			{
				alignment = TextAnchor.UpperCenter
			};
		}
	}
}
#endif

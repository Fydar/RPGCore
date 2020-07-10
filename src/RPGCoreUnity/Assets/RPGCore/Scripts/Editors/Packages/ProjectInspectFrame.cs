using RPGCore.Packages;
using UnityEditor;
using UnityEngine;

namespace RPGCore.Unity.Editors
{
	public class ProjectInspectFrame : WindowFrame
	{
		public ProjectExplorer Explorer { get; private set; }

		public ProjectInspectFrame(ProjectExplorer explorer)
		{
			Explorer = explorer;
		}

		public override void OnEnable()
		{
		}

		public override void OnGUI()
		{
			GUILayout.BeginArea(Position);

			EditorGUILayout.LabelField(Explorer.Definition.Properties.Name);
			EditorGUILayout.LabelField(Explorer.Definition.Properties.Version);

			GUILayout.EndArea();
		}
	}
}

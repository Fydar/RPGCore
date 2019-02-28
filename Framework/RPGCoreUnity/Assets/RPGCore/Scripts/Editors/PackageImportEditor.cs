using UnityEditor;
using UnityEngine;

namespace RPGCore.Unity.Editors
{
	[CustomEditor(typeof(PackageImport))]
	public class PackageImportEditor : Editor
	{
		public override void OnInspectorGUI ()
		{
			DrawDefaultInspector ();

			var packageImport = (PackageImport)target;

			GUILayout.Space (8);

			EditorGUILayout.BeginVertical (EditorStyles.helpBox);
			foreach (var folder in packageImport.Explorer.Folders)
			{
				EditorGUILayout.LabelField (folder.ToString (), EditorStyles.boldLabel);

				EditorGUI.indentLevel++;
				foreach (var asset in folder.Assets)
				{
					EditorGUILayout.LabelField (asset.Name);
				}
				EditorGUI.indentLevel--;
			}
			EditorGUILayout.EndVertical ();
		}
	}
}

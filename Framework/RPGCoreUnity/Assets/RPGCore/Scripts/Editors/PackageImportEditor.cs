using UnityEditor;
using UnityEngine;

namespace RPGCore.Unity.Editors
{
	[CustomEditor(typeof(PackageImport))]
	public class PackageImportEditor : Editor
	{
		Texture2D Spritesheet;
		
		public override void OnInspectorGUI ()
		{
			DrawDefaultInspector ();

			var packageImport = (PackageImport)target;

			GUILayout.Space (8);

			EditorGUILayout.BeginVertical (EditorStyles.helpBox);
			foreach (var asset in packageImport.Explorer.Assets)
			{
				EditorGUILayout.LabelField (asset.ToString (), EditorStyles.boldLabel);

				EditorGUI.indentLevel++;
				foreach (var resource in asset.Resources)
				{
					EditorGUILayout.LabelField (resource.Name);
				}
				EditorGUI.indentLevel--;
			}
			EditorGUILayout.EndVertical ();
			
			if (Spritesheet != null)
			{
				var rect = GUILayoutUtility.GetRect (0, 250);
				var centerRect = new Rect (rect.center.x - (rect.height * 0.5f), rect.y, rect.height, rect.height);
				GUI.DrawTexture(centerRect, Spritesheet);
			}

			if (GUILayout.Button ("Reload"))
			{
				packageImport.Reload ();
			}
		}
	}
}

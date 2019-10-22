using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace RPGCore.Unity.Editors
{
	[CustomEditor (typeof (ProjectImport))]
	public class ProjectImportEditor : Editor
	{
		private Texture2D Spritesheet;

		public override void OnInspectorGUI ()
		{
			DrawDefaultInspector ();

			var projectImport = (ProjectImport)target;

			GUILayout.Space (8);

			EditorGUILayout.BeginVertical (EditorStyles.helpBox);
			foreach (var resource in projectImport.Explorer.Resources)
			{
				EditorGUILayout.LabelField (resource.Name);
			}

			if (Spritesheet != null)
			{
				var rect = GUILayoutUtility.GetRect (0, 250);
				var centerRect = new Rect (rect.center.x - (rect.height * 0.5f), rect.y, rect.height, rect.height);
				GUI.DrawTexture (centerRect, Spritesheet);
			}

			if (GUILayout.Button ("Reload"))
			{
				projectImport.Reload ();
			}

			if (GUILayout.Button ("Create"))
			{
				string path = AssetDatabase.GetAssetPath (target);
				path = path.Substring (0, path.LastIndexOf ('/'));
				Debug.Log (path);

				/*foreach (var resource in projectImport.Explorer.Resources)
				{
					string assetFolder = path + "/" + resource.Name;
					if (!AssetDatabase.IsValidFolder (assetFolder))
					{
						assetFolder = AssetDatabase.CreateFolder (path, resource.Name);
						assetFolder = AssetDatabase.GUIDToAssetPath (assetFolder);
					}

					string assetImportPath = assetFolder + "/" + resource.Name + ".asset";

					var assetImport = AssetDatabase.LoadAssetAtPath<AssetImport> (assetImportPath);
					if (assetImport == null)
					{
						assetImport = CreateInstance<AssetImport> ();
						AssetDatabase.CreateAsset (assetImport, assetImportPath);
					}
					
					AssetImporter.GetAtPath (assetImportPath).SetAssetBundleNameAndVariant (target.name, "");

					var assetIcon = resource.FindIcon().LoadImage ();

					AssetDatabase.AddObjectToAsset (assetIcon, assetImport);
					AssetDatabase.ImportAsset (AssetDatabase.GetAssetPath (assetIcon));

				} */
			}

			if (GUILayout.Button ("Export"))
			{
				string path = AssetDatabase.GetAssetPath (target);
				path = path.Substring (0, path.LastIndexOf ('/'));
				string exportDirectory = path + "/" + target.name + DateTime.Now.ToString ("ddmmyy");
				if (!Directory.Exists (exportDirectory))
				{
					Directory.CreateDirectory (exportDirectory);
				}
				Debug.Log (exportDirectory);

				var manifest = BuildPipeline.BuildAssetBundles (exportDirectory, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
			}
		}
	}
}

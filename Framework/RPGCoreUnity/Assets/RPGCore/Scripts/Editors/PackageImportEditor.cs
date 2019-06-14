using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace RPGCore.Unity.Editors
{
	[CustomEditor(typeof(ProjectImport))]
	public class ProjectImportEditor : Editor
	{
		Texture2D Spritesheet;
		
		public override void OnInspectorGUI ()
		{
			DrawDefaultInspector ();

			var projectImport = (ProjectImport)target;

			GUILayout.Space (8);

			EditorGUILayout.BeginVertical (EditorStyles.helpBox);
			foreach (var asset in projectImport.Explorer.Assets)
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
				projectImport.Reload ();
			}

			if (GUILayout.Button ("Create"))
			{
				string path = AssetDatabase.GetAssetPath (target);
				path = path.Substring (0, path.LastIndexOf ('/'));
				Debug.Log (path);

				foreach (var asset in projectImport.Explorer.Assets)
				{
					string assetFolder = path + "/" + asset.Name;
					if (!AssetDatabase.IsValidFolder (assetFolder))
					{
						assetFolder = AssetDatabase.CreateFolder (path, asset.Name);
						assetFolder = AssetDatabase.GUIDToAssetPath (assetFolder);
					}

					string assetImportPath = assetFolder + "/" + asset.Name + ".asset";

					var assetImport = AssetDatabase.LoadAssetAtPath<AssetImport> (assetImportPath);
					if (assetImport == null)
					{
						assetImport = CreateInstance<AssetImport> ();
						AssetDatabase.CreateAsset (assetImport, assetImportPath);
					}
					
					AssetImporter.GetAtPath (assetImportPath).SetAssetBundleNameAndVariant (target.name, "");

					var assetIcon = asset.FindIcon().LoadImage ();

					AssetDatabase.AddObjectToAsset (assetIcon, assetImport);
					AssetDatabase.ImportAsset (AssetDatabase.GetAssetPath (assetIcon));

				}
			}

			if (GUILayout.Button ("Export"))
			{
				string path = AssetDatabase.GetAssetPath (target);
				path = path.Substring (0, path.LastIndexOf ('/'));
				string exportDirectory = path + "/" + target.name + DateTime.Now.ToString("ddmmyy");
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

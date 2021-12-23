using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace RPGCore.Unity.Editors
{
	[CustomEditor(typeof(ProjectImport))]
	public class ProjectImportEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();

			var projectImport = (ProjectImport)target;

			GUILayout.Space(8);

			EditorGUILayout.BeginVertical(EditorStyles.helpBox);
			if (projectImport.SourceFiles?.Resources != null)
			{
				foreach (var resource in projectImport.SourceFiles.Resources)
				{
					EditorGUILayout.LabelField(resource.FullName);
				}
			}
			EditorGUILayout.EndVertical();

			if (GUILayout.Button("Reload"))
			{
				projectImport.Reload();
			}

			if (GUILayout.Button("Export"))
			{
				string path = AssetDatabase.GetAssetPath(target);
				path = path.Substring(0, path.LastIndexOf('/'));
				string exportDirectory = path + "/" + target.name + DateTime.Now.ToString("ddmmyy");
				if (!Directory.Exists(exportDirectory))
				{
					Directory.CreateDirectory(exportDirectory);
				}
				Debug.Log(exportDirectory);

				var manifest = BuildPipeline.BuildAssetBundles(exportDirectory, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
			}
		}
	}
}

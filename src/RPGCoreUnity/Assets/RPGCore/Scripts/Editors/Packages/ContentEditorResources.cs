#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

namespace RPGCore.Behaviour.Editor
{
	public class ContentEditorResources : ScriptableObject
	{
		private static ContentEditorResources instance;

		[Header("Window")]
		public Texture2D LightThemeIcon;
		public Texture2D DarkThemeIcon;
		public Texture2D WindowBackground;

		[Header("Icons")]
		public Texture2D ProjectIcon;
		public Texture2D PackageIcon;
		public Texture2D DependanciesIcon;
		public Texture2D ProjectDependancyIcon;
		public Texture2D ManifestDependancyIcon;

		public Texture2D FolderIcon;
		public Texture2D DocumentIcon;

		public static ContentEditorResources Instance
		{
			get
			{
				if (instance == null || instance.Equals(null))
				{
					var newInstance = CreateInstance<ContentEditorResources>();
					string path = AssetDatabase.GetAssetPath(MonoScript.FromScriptableObject(newInstance));
					string dir = Path.GetDirectoryName(path);
					instance = AssetDatabase.LoadAssetAtPath<ContentEditorResources>(Path.Combine(dir, typeof(ContentEditorResources).Name + ".asset"));

					if (instance == null)
					{
						instance = newInstance;
						AssetDatabase.CreateAsset(instance, Path.Combine(dir, typeof(ContentEditorResources).Name + ".asset"));
					}
				}

				return instance;
			}
		}
	}
}
#endif

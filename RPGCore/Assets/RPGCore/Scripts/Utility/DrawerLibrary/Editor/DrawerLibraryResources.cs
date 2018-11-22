using System.IO;
using UnityEditor;
using UnityEngine;

namespace RPGCore.Utility
{
	public class DrawerLibraryResources : ScriptableObject
	{
		private static DrawerLibraryResources instance;

		[ErrorIfNull]
		public Texture2D CheckIcon;
		[ErrorIfNull]
		public Texture2D WarningIcon;
		[ErrorIfNull]
		public Texture2D ErrorIcon;

		public static DrawerLibraryResources Instance
		{
			get
			{
				if (instance == null || instance.Equals (null))
				{
					DrawerLibraryResources newInstance = CreateInstance<DrawerLibraryResources> ();
					var path = AssetDatabase.GetAssetPath (MonoScript.FromScriptableObject (newInstance));
					var dir = Path.GetDirectoryName (path);
					instance = AssetDatabase.LoadAssetAtPath<DrawerLibraryResources> (Path.Combine (dir, typeof (DrawerLibraryResources).Name + ".asset"));

					if (instance == null)
					{
						instance = newInstance;
						AssetDatabase.CreateAsset (instance, Path.Combine (dir, typeof (DrawerLibraryResources).Name + ".asset"));
					}
				}

				return instance;
			}
		}
	}
}
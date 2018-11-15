using System.IO;
using UnityEditor;
using UnityEngine;

namespace RPGCore.Behaviour.Editor
{
	public class BehaviourGraphResources : ScriptableObject
	{
		private static BehaviourGraphResources instance;

		[Header ("Window")]
		public Texture2D LightThemeIcon;
		public Texture2D DarkThemeIcon;
		public Texture2D WindowBackground;

		[Header ("Connections")]
		public Texture2D DefaultTrail;
		public Texture2D SmallConnection;

		[Header ("Nodes")]
		public Texture2D AndNodeGraphic;
		public Texture2D OrNodeGraphic;
		public Texture2D NotNodeGraphic;

		[Space]

		public GUIStyle NodeStyle;

		public static BehaviourGraphResources Instance
		{
			get
			{
				if (instance == null || instance.Equals (null))
				{
					BehaviourGraphResources newInstance = CreateInstance<BehaviourGraphResources> ();
					var path = AssetDatabase.GetAssetPath (MonoScript.FromScriptableObject (newInstance));
					var dir = Path.GetDirectoryName (path);
					instance = AssetDatabase.LoadAssetAtPath<BehaviourGraphResources> (Path.Combine (dir, typeof (BehaviourGraphResources).Name + ".asset"));

					if (instance == null)
					{
						instance = newInstance;
						AssetDatabase.CreateAsset (instance, Path.Combine (dir, typeof (BehaviourGraphResources).Name + ".asset"));
					}
				}

				return instance;
			}
		}
	}
}
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RPGCore.Utility
{
	public abstract class StaticDatabase : ScriptableObject
	{
		public virtual void Awake () { }
	}

	public abstract class StaticDatabase<T> : StaticDatabase
		where T : StaticDatabase
	{
		public static T Instance
		{
			get
			{
				return StaticDatabaseCollection.Get<T> ();
			}
		}

#if UNITY_EDITOR
		[CustomEditor (typeof (StaticDatabase), true)]
		public class StaticDatabaseEditor : Editor
		{
			public override void OnInspectorGUI ()
			{
				UpdatePreloadedAssets ();
				DrawDefaultInspector ();
			}

			protected void UpdatePreloadedAssets ()
			{
				StaticDatabaseCollection singleton = StaticDatabaseCollection.Singleton;

				StaticDatabase database = (StaticDatabase)target;

				if (!singleton.DatabaseObjects.Contains (database))
				{
					singleton.DatabaseObjects.Add (database);
				}

				EditorUtility.SetDirty (singleton);
			}
		}
#endif
	}
}
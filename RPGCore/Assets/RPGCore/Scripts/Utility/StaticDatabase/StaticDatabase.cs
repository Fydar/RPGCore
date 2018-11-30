using UnityEngine;

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
	}
}


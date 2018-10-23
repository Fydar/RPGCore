using UnityEngine;
using System;
using System.Collections.Generic;

namespace RPGCore.Utility
{
	[CreateAssetMenu (menuName = "RPGCore/Utility/StaticDatabase")]
	public class StaticDatabaseCollection : ScriptableObject, ISerializationCallbackReceiver
	{
		public static StaticDatabaseCollection Singleton
		{
			get
			{
				if (singleton == null)
					singleton = Resources.Load<StaticDatabaseCollection> ("StaticDatabase");

				return singleton;
			}
		}

		private static StaticDatabaseCollection singleton;

		public List<StaticDatabase> DatabaseObjects;

		protected Dictionary<Type, StaticDatabase> Databases = new Dictionary<Type, StaticDatabase> ();

		public static T Get<T> ()
			where T : StaticDatabase
		{
			return (T)Get (typeof (T));
		}

		public static StaticDatabase Get (Type type)
		{
			StaticDatabase cachedDatabase;
			Singleton.Databases.TryGetValue (type, out cachedDatabase);

			return cachedDatabase;
		}

		void ISerializationCallbackReceiver.OnBeforeSerialize () { }

		void ISerializationCallbackReceiver.OnAfterDeserialize ()
		{
			foreach (StaticDatabase database in DatabaseObjects)
			{
				if (database == null)
					continue;

				if (!Databases.ContainsKey (database.GetType ()))
					Databases.Add (database.GetType (), database);
			}

			foreach (StaticDatabase database in Databases.Values)
			{
				if (database == null)
					continue;
				database.Awake ();
			}
		}
	}
}
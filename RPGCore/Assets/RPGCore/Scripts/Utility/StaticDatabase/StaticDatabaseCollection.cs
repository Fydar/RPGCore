using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPGCore.Utility
{
	[CreateAssetMenu (menuName = "RPGCore/Utility/StaticDatabase")]
	public sealed class StaticDatabaseCollection : ScriptableObject, ISerializationCallbackReceiver
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

		public List<StaticDatabase> DatabaseObjects;

		private static StaticDatabaseCollection singleton;
		private readonly Dictionary<Type, StaticDatabase> databases = new Dictionary<Type, StaticDatabase> ();

		public static T Get<T> ()
			where T : StaticDatabase
		{
			return (T)Get (typeof (T));
		}

		public static StaticDatabase Get (Type type)
		{
			StaticDatabase cachedDatabase;
			Singleton.databases.TryGetValue (type, out cachedDatabase);

			return cachedDatabase;
		}

		void ISerializationCallbackReceiver.OnBeforeSerialize () { }

		void ISerializationCallbackReceiver.OnAfterDeserialize ()
		{
			foreach (StaticDatabase database in DatabaseObjects)
			{
				if (database == null)
					continue;

				if (!databases.ContainsKey (database.GetType ()))
					databases.Add (database.GetType (), database);
			}

			foreach (StaticDatabase database in databases.Values)
			{
				if (database == null)
					continue;
				database.Awake ();
			}
		}
	}
}


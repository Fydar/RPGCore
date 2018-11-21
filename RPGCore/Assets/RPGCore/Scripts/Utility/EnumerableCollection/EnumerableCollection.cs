using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Serialization;

namespace RPGCore
{
	public abstract class EnumerableCollection
	{
		protected static Dictionary<Type, CollectionInformation> ReflectionCache =
			new Dictionary<Type, CollectionInformation> ();

		protected int IndexOf (string fieldName)
		{
			CollectionInformation information = ReflectionInformation ();

			for (int i = 0; i < information.fieldNames.Length; i++)
			{
				if (information.fieldNames[i] == fieldName)
					return i;
			}
			Debug.LogError ("\"" + fieldName + "\" is not a member of " + GetType ().Name + ".");
			return -1;
		}

		protected CollectionInformation ReflectionInformation ()
		{
			return GetReflectionInformation (GetType ());
		}

		public static CollectionInformation GetReflectionInformation (Type type)
		{
			Type collectionType = EnumerableCollection.BaseCollectionType (type);
			CollectionInformation information;

			bool result = ReflectionCache.TryGetValue (collectionType, out information);

			if (!result)
			{
				information = new CollectionInformation (collectionType);

				ReflectionCache.Add (collectionType, information);
			}
			return information;
		}

		public static Type BaseCollectionType (Type type)
		{
			Type currentType = type;
			while (true)
			{
				if (currentType == null)
					return null;

				Type[] types = currentType.GetGenericArguments ();

				if (types.Length != 1)
				{
					currentType = currentType.BaseType;
					continue;
				}

				return currentType.GetGenericTypeDefinition ();
			}
		}
	}

	public class EnumerableCollection<T> : EnumerableCollection, IEnumerable<T>, ISerializationCallbackReceiver
	{
		[SerializeField, HideInInspector]
		private string[] fieldDirectories;

		[SerializeField, HideInInspector]
		private T[] fieldValues;

		[NonSerialized]
		private T[] arrayCache = null;

		public T this[CollectionEntry entry]
		{
			get
			{
				if (arrayCache == null)
					Collect ();

				if (entry.entryIndex == -1)
					entry.entryIndex = IndexOf (entry.Field);

				if (entry.entryIndex == -1)
				{
					Debug.LogError ("EnumerableCollection not working");
					return default (T);
				}

				return arrayCache[entry.entryIndex];
			}
		}

		private void Collect ()
		{
			List<T> foundObjects = new List<T> ();
			CollectionInformation information = ReflectionInformation ();

			foreach (FieldInfo info in information.directFields)
			{
				FieldInfo collectionInfo = GetType ().GetField (info.Name);

				if (collectionInfo.FieldType == typeof (T))
				{
					foundObjects.Add ((T)collectionInfo.GetValue (this));
				}
				else if (typeof (EnumerableCollection).IsAssignableFrom (info.FieldType))
				{
					var collection = FieldGetOrCreate<EnumerableCollection> (collectionInfo, this);

					CollectionInformation childInformation = GetReflectionInformation (collection.GetType ());

					foreach (FieldInfo childInfo in childInformation.directFields)
					{
						FieldInfo childCollectionInfo = collection.GetType ().GetField (childInfo.Name);

						if (childCollectionInfo.FieldType == typeof (T))
						{
							foundObjects.Add ((T)childCollectionInfo.GetValue (collection));
						}
					}
				}
			}

			arrayCache = foundObjects.ToArray ();
		}

		void ISerializationCallbackReceiver.OnBeforeSerialize ()
		{
			Type thisType = GetType ();

			List<string> directories = new List<string> ();
			List<T> values = new List<T> ();
			CollectionInformation information = ReflectionInformation ();

			foreach (FieldInfo info in information.directFields)
			{
				if (!typeof (EnumerableCollection).IsAssignableFrom (info.FieldType))
					continue;

				// Find all nested EnumerableCollections

				FieldInfo childCollectionValue = thisType.GetField (info.Name);

				var collection = FieldGetOrCreate<EnumerableCollection<T>> (childCollectionValue, this);

				string parent = info.Name + "/";

				CollectionInformation childInformation = collection.ReflectionInformation ();

				foreach (FieldInfo childInfo in childInformation.directFields)
				{
					FieldInfo collectionChildInfo = collection.GetType ().GetField (childInfo.Name);

					if (collectionChildInfo.FieldType == typeof (T))
					{
						directories.Add (parent + childInfo.Name);
						values.Add ((T)collectionChildInfo.GetValue (collection));
					}
				}
			}

			fieldDirectories = directories.ToArray ();
			fieldValues = values.ToArray ();
		}

		void ISerializationCallbackReceiver.OnAfterDeserialize ()
		{
			Type thisType = GetType ();

			if (fieldDirectories == null || fieldValues == null)
				return;

			for (int i = 0; i < fieldDirectories.Length; i++)
			{
				string directory = fieldDirectories[i];

				int sperator = directory.IndexOf ('/');
				string fieldName = directory.Substring (0, sperator);

				FieldInfo thisField = GetSerializationField (thisType, fieldName);

				if (thisField == null)
					continue;

				object obj = FieldGetOrCreate (thisField, this);

				string childName = directory.Substring (sperator + 1);
				FieldInfo childField = GetSerializationField (obj.GetType (), childName);
				if (childField == null)
					continue;

				T value = fieldValues[i];
				childField.SetValue (obj, value);
			}
		}

		public IEnumerator<T> GetEnumerator ()
		{
			if (arrayCache == null)
				Collect ();

			return ((IEnumerable<T>)arrayCache).GetEnumerator ();
		}

		IEnumerator IEnumerable.GetEnumerator ()
		{
			if (arrayCache == null)
				Collect ();

			return arrayCache.GetEnumerator ();
		}

		private static V FieldGetOrCreate<V> (FieldInfo field, object instance)
		{
			return (V)FieldGetOrCreate (field, instance);
		}

		private static object FieldGetOrCreate (FieldInfo field, object instance)
		{
			object obj = field.GetValue (instance);

			if (obj == null)
			{
				obj = Activator.CreateInstance (field.FieldType);
				field.SetValue (instance, obj);
			}

			return obj;
		}

		private static FieldInfo GetSerializationField (Type type, string name)
		{
			FieldInfo thisField = type.GetField (name);

			if (thisField != null)
				return thisField;

			// What we serialized got renamed and no longer exists.
			FieldInfo[] fields = type.GetFields (BindingFlags.Public | BindingFlags.Instance);
			foreach (FieldInfo field in fields)
			{
				var attributes = field.GetCustomAttributes (typeof(FormerlySerializedAsAttribute), true);

				foreach (var attribute in attributes)
				{
					var serializationAttribute = (FormerlySerializedAsAttribute)attribute;

					if (serializationAttribute.oldName == name)
					{
						return field;
					}
				}
			}
			return null;
		}
	}
}
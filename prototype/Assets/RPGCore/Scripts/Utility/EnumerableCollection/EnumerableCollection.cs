﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Serialization;

namespace RPGCore
{
	public abstract class EnumerableCollection
	{
		public struct Route
		{
			public FieldInfo Member;
			public object Target;

			public Route(FieldInfo member, object target)
			{
				Member = member;
				Target = target;
			}
		}

		protected static Dictionary<Type, CollectionInformation> ReflectionCache =
			new Dictionary<Type, CollectionInformation>();

		protected int IndexOf(string fieldName)
		{
			return ReflectionInformation().IndexOf(fieldName);
		}

		protected CollectionInformation ReflectionInformation()
		{
			return GetReflectionInformation(GetType());
		}

		public static CollectionInformation GetReflectionInformation(Type type)
		{
			var collectionType = BaseCollectionType(type);
			CollectionInformation information;

			bool result = ReflectionCache.TryGetValue(collectionType, out information);

			if (!result)
			{
				information = new CollectionInformation(collectionType);

				ReflectionCache.Add(collectionType, information);
			}
			return information;
		}

		public static Type BaseCollectionType(Type type)
		{
			var currentType = type;
			while (true)
			{
				if (currentType == null)
				{
					return null;
				}

				var types = currentType.GetGenericArguments();

				if (types.Length != 1)
				{
					currentType = currentType.BaseType;
					continue;
				}

				return currentType.GetGenericTypeDefinition();
			}
		}

		public abstract IEnumerable<Route> FindAllRoutes();
	}

	public class EnumerableCollection<T> : EnumerableCollection, IEnumerable<T>, ISerializationCallbackReceiver
	{
		[SerializeField, HideInInspector]
		private string[] fieldDirectories;

		[SerializeField, HideInInspector]
		private T[] fieldValues;

		[NonSerialized]
		private T[] arrayCache;

		public T this[CollectionEntry entry]
		{
			get
			{
				if (arrayCache == null)
				{
					Collect();
				}

				return arrayCache[entry.Index];
			}
		}

		private void Collect()
		{
			var foundObjects = new List<T>();

			foreach (var route in FindAllRoutes())
			{
				foundObjects.Add((T)FieldGetOrCreate(route.Member, route.Target));
			}

			arrayCache = foundObjects.ToArray();
		}

		public override IEnumerable<Route> FindAllRoutes()
		{
			var information = ReflectionInformation();

			foreach (var info in information.directFields)
			{
				var collectionInfo = GetType().GetField(info.Name);

				if (collectionInfo.FieldType == typeof(T))
				{
					yield return new Route(collectionInfo, this);
				}
				else if (typeof(EnumerableCollection).IsAssignableFrom(info.FieldType))
				{
					var collection = FieldGetOrCreate<EnumerableCollection>(collectionInfo, this);

					foreach (var route in collection.FindAllRoutes())
					{
						yield return route;
					}
				}
			}
		}

		public void OnBeforeSerialize()
		{
			var thisType = GetType();

			var directories = new List<string>();
			var values = new List<T>();
			var information = ReflectionInformation();

			foreach (var info in information.directFields)
			{
				if (!typeof(EnumerableCollection).IsAssignableFrom(info.FieldType))
				{
					continue;
				}

				// Find all nested EnumerableCollections

				var childCollectionValue = thisType.GetField(info.Name);

				var collection = FieldGetOrCreate<EnumerableCollection<T>>(childCollectionValue, this);

				string parent = info.Name + "/";

				var childInformation = collection.ReflectionInformation();

				foreach (var childInfo in childInformation.directFields)
				{
					var collectionChildInfo = collection.GetType().GetField(childInfo.Name);

					if (collectionChildInfo.FieldType == typeof(T))
					{
						directories.Add(parent + childInfo.Name);
						values.Add((T)collectionChildInfo.GetValue(collection));
					}
				}
			}

			fieldDirectories = directories.ToArray();
			fieldValues = values.ToArray();
		}

		public void OnAfterDeserialize()
		{
			var thisType = GetType();

			if (fieldDirectories == null || fieldValues == null)
			{
				return;
			}

			for (int i = 0; i < fieldDirectories.Length; i++)
			{
				string directory = fieldDirectories[i];

				int sperator = directory.IndexOf('/');
				string fieldName = directory.Substring(0, sperator);

				var thisField = GetSerializationField(thisType, fieldName);

				if (thisField == null)
				{
					continue;
				}

				object obj = FieldGetOrCreate(thisField, this);

				string childName = directory.Substring(sperator + 1);
				var childField = GetSerializationField(obj.GetType(), childName);
				if (childField == null)
				{
					continue;
				}

				var value = fieldValues[i];
				childField.SetValue(obj, value);
			}
		}

		public IEnumerator<T> GetEnumerator()
		{
			if (arrayCache == null)
			{
				Collect();
			}

			return ((IEnumerable<T>)arrayCache).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			if (arrayCache == null)
			{
				Collect();
			}

			return arrayCache.GetEnumerator();
		}

		private static V FieldGetOrCreate<V>(FieldInfo field, object instance)
		{
			return (V)FieldGetOrCreate(field, instance);
		}

		private static object FieldGetOrCreate(FieldInfo field, object instance)
		{
			object obj = field.GetValue(instance);

			if (obj == null)
			{
				obj = Activator.CreateInstance(field.FieldType);
				field.SetValue(instance, obj);
			}

			return obj;
		}

		private static FieldInfo GetSerializationField(Type type, string name)
		{
			var thisField = type.GetField(name);

			if (thisField != null)
			{
				return thisField;
			}

			// What we serialized got renamed and no longer exists.
			var fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);
			foreach (var field in fields)
			{
				object[] attributes = field.GetCustomAttributes(typeof(FormerlySerializedAsAttribute), true);

				foreach (object attribute in attributes)
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


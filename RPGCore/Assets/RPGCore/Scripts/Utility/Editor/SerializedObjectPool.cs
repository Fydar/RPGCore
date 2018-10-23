using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RPGCore.Utility.Editors
{
	public static class SerializedObjectPool
	{
		public static Dictionary<Object, SerializedObject> Pool =
			new Dictionary<Object, SerializedObject> ();


		public static SerializedObject Grab (Object target)
		{
			SerializedObject serializedObject;
			bool result = Pool.TryGetValue (target, out serializedObject);

			if (result)
			{
				if (serializedObject == null ||
					serializedObject.targetObject == null ||
					serializedObject.targetObject != target)
				{
					result = false;
					Pool.Remove (target);
				}
			}
			if (!result)
			{
				serializedObject = new SerializedObject (target);
				Pool.Add (target, serializedObject);
			}

			return serializedObject;
		}
	}
}

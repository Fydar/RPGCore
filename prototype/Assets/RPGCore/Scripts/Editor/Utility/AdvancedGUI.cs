#if UNITY_EDITOR
using System.Reflection;
using UnityEditor;

namespace RPGCore.Utility.Editors
{
	public static class AdvancedGUI
	{
		public static object GetTargetObjectOfProperty (SerializedProperty prop)
		{
			string path = prop.propertyPath.Replace (".Array.data[", "[");
			object obj = prop.serializedObject.targetObject;
			string[] elements = path.Split ('.');

			foreach (string element in elements)
			{
				if (element.Contains ("["))
				{
					string elementName = element.Substring (0, element.IndexOf ("[", System.StringComparison.Ordinal));
					int index = System.Convert.ToInt32 (element.Substring (element.IndexOf ("[", System.StringComparison.Ordinal)).Replace ("[", "").Replace ("]", ""));
					obj = GetValue_Imp (obj, elementName, index);
				}
				else
				{
					obj = GetMemberValue (obj, element);
				}
			}
			return obj;
		}

		private static object GetMemberValue (object source, string name)
		{
			if (source == null)
				return null;
			var type = source.GetType ();

			while (type != null)
			{
				var f = type.GetField (name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
				if (f != null)
					return f.GetValue (source);

				var p = type.GetProperty (name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
				if (p != null)
					return p.GetValue (source, null);

				type = type.BaseType;
			}
			return null;
		}

		private static object GetValue_Imp (object source, string name, int index)
		{
			var enumerable = GetMemberValue (source, name) as System.Collections.IEnumerable;

			if (enumerable == null)
				return null;

			var enm = enumerable.GetEnumerator ();

			for (int i = 0; i <= index; i++)
			{
				if (!enm.MoveNext ())
					return null;
			}
			return enm.Current;
		}
	}
}
#endif

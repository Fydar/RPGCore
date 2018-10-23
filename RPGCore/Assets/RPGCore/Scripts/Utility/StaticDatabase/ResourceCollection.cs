using UnityEngine;

namespace RPGCore.Utility
{
	public class ResourceCollection<T> : ScriptableObject
	where T : ResourceCollection<T>
	{
		private static T instance;
		public static T Instance
		{
			get
			{
				if (instance == null)
				{
					T[] loadedResources = Resources.LoadAll<T> ("");

					if (loadedResources == null || loadedResources.Length == 0)
					{
						Debug.LogError ("No \"" + typeof (T).Name + "\" found in the Resources directories");
					}
					else if (loadedResources.Length != 1)
					{
						Debug.LogError ("Multiple \"" + typeof (T).Name + " found in the Resources directories");
						instance = loadedResources[0];
					}
					else
					{
						instance = loadedResources[0];
					}
				}

				return instance;
			}
		}
	}
}
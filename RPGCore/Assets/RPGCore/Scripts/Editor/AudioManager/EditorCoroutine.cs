#if UNITY_EDITOR
using System.Collections;
using UnityEditor;

namespace RPGCore.Audio.Editors
{
	public class EditorCoroutine
	{
		public static EditorCoroutine Start (IEnumerator _routine)
		{
			EditorCoroutine coroutine = new EditorCoroutine (_routine);
			coroutine.StartInternal ();
			return coroutine;
		}

		readonly IEnumerator routine;
		EditorCoroutine (IEnumerator _routine)
		{
			routine = _routine;
		}

		void StartInternal ()
		{
			EditorApplication.update += Update;
		}
		public void StopInternal ()
		{
			EditorApplication.update -= Update;
		}

		void Update ()
		{
			if (!routine.MoveNext ())
			{
				StopInternal ();
			}
		}
	}
}
#endif

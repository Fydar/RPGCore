#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace RPGCore.Audio.Editors
{
	[CustomEditor (typeof (SfxGroup))]
	public class SfxGroupEditor : Editor
	{
		public string lastPlayed = "";

		[OnOpenAsset]
		public static bool OpenAsset (int instanceID, int line)
		{
			var asset = EditorUtility.InstanceIDToObject (instanceID);

			if (typeof (SfxGroup).IsAssignableFrom (asset.GetType ()))
			{
				var group = (SfxGroup)asset;
				PreviewGroup (group);
				return true;
			}
			return false;
		}

		public override void OnInspectorGUI ()
		{
			var group = (SfxGroup)target;

			DrawDefaultInspector ();

			if (GUILayout.Button ("Play Clips"))
			{
				lastPlayed = PreviewGroup (group).name;
			}
			EditorGUILayout.LabelField (lastPlayed);
		}

		public static AudioClip PreviewGroup (SfxGroup group)
		{
			var clip = group.GetClip ();
			PlayClip (clip, group);
			return clip;
		}

		public static void PlayClip (AudioClip clip, SfxGroup group)
		{
			var go = EditorUtility.CreateGameObjectWithHideFlags ("PLAY_AUDIO_TEMP", HideFlags.HideAndDontSave);

			var source = go.AddComponent<AudioSource> ();
			source.clip = clip;
			source.volume = UnityEngine.Random.Range (group.VolumeRange.x, group.VolumeRange.y);
			source.pitch = UnityEngine.Random.Range (group.PitchRange.x, group.PitchRange.y);
			source.Play ();
		}
	}
}
#endif

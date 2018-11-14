using System.Collections;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace RPGCore.Audio.Editors
{
	[CustomEditor (typeof (LoopGroup))]
	public class LoopGroupEditor : Editor
	{
		public string lastPlayed = "";

		[OnOpenAsset]
		public static bool OpenAsset (int instanceID, int line)
		{
			var asset = EditorUtility.InstanceIDToObject (instanceID);

			if (typeof (LoopGroup).IsAssignableFrom (asset.GetType ()))
			{
				LoopGroup group = (LoopGroup)asset;
				PreviewGroup (group);
				return true;
			}
			return false;
		}

		public override void OnInspectorGUI ()
		{
			LoopGroup group = (LoopGroup)target;

			DrawDefaultInspector ();

			if (GUILayout.Button ("Play Clips"))
			{
				lastPlayed = PreviewGroup (group).name;
			}
			EditorGUILayout.LabelField (lastPlayed);
		}

		public static AudioClip PreviewGroup (LoopGroup group)
		{
			AudioClip clip = group.LoopedAudio;
			PlayClip (clip, group);
			return clip;
		}

		public static void PlayClip (AudioClip clip, LoopGroup group)
		{
			var go = EditorUtility.CreateGameObjectWithHideFlags ("PLAY_AUDIO_TEMP", HideFlags.HideAndDontSave);

			AudioSource source = go.AddComponent<AudioSource> ();
			source.clip = clip;
			source.volume = UnityEngine.Random.Range (group.VolumeRange.x, group.VolumeRange.y);
			source.pitch = UnityEngine.Random.Range (group.PitchRange.x, group.PitchRange.y);
			source.Play ();
			source.loop = true;

			EditorCoroutine.Start (TurnOffAfter (source, group));
		}

		private static IEnumerator TurnOffAfter (AudioSource source, LoopGroup group)
		{
			float startTime = Time.realtimeSinceStartup + 1.5f;

			while (startTime > Time.realtimeSinceStartup)
			{
				source.volume = Mathf.Lerp (group.VolumeRange.x, group.VolumeRange.y,
					Mathf.PerlinNoise (0.313f, Time.realtimeSinceStartup * group.PerlinSpeed));

				source.pitch = Mathf.Lerp (group.PitchRange.x, group.PitchRange.y,
					Mathf.PerlinNoise (0.83f, Time.realtimeSinceStartup * group.PerlinSpeed));

				yield return null;
			}

			source.Stop ();

			DestroyImmediate (source.gameObject);
		}
	}
}
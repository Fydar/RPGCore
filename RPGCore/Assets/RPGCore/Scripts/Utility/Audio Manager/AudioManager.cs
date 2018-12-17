using System.Collections;
using UnityEngine;

namespace RPGCore.Audio
{
	public class AudioManager : MonoBehaviour
	{
		private static AudioManager instance;

		public MusicGroup Music;
		public AudioSourcePool Pool;

		[RuntimeInitializeOnLoadMethod (RuntimeInitializeLoadType.BeforeSceneLoad)]
		static void OnRuntimeMethodLoad ()
		{
			GameObject singleton = new GameObject ("Audio Manager", typeof (AudioManager));
			DontDestroyOnLoad (singleton);
		}

		private void Awake ()
		{
			instance = this;
			Pool = new AudioSourcePool { PrewarmAmount = 5 };
			Pool.Initialise (gameObject);
		}

		private void Start ()
		{
			if (Music != null)
				PlayMusic (Music);
		}

		public void PlayClip (SfxGroup group)
		{
			AudioSource source = Pool.Grab ();

			source.clip = group.GetClip ();
			source.volume = Random.Range (group.VolumeRange.x, group.VolumeRange.y);
			source.pitch = Random.Range (group.PitchRange.x, group.PitchRange.y);
			source.Play ();
			StartCoroutine (ReturnToPool (source));
		}

		public void PlayClip (LoopGroup group, EffectFader fader)
		{
			AudioSource source = Pool.Grab ();

			source.clip = group.LoopedAudio;

			source.pitch = group.PitchRange.x;
			source.volume = group.VolumeRange.x;
			source.loop = true;

			source.Play ();
			StartCoroutine (ManageLoop (source, group, fader));
		}

		public void PlayMusic (MusicGroup group)
		{
			AudioSource source = Pool.Grab ();

			source.clip = group.Music[0];
			source.volume = group.Volume;
			source.priority = 1024;
			source.loop = true;
			source.Play ();
		}

		IEnumerator ReturnToPool (AudioSource source)
		{
			yield return new WaitForSeconds (source.clip.length / source.pitch);
			source.Stop ();
			Pool.Return (source);
		}

		IEnumerator ManageLoop (AudioSource source, LoopGroup group, EffectFader fader)
		{
			while (true)
			{
				fader.Update (Time.deltaTime);
				source.volume = fader.Value * group.VolumeRange.x;
				yield return null;
			}
		}

		public static void Play (SfxGroup group)
		{
			instance.PlayClip (group);
		}

		public static void Play (LoopGroup group, EffectFader fader)
		{
			instance.PlayClip (group, fader);
		}
	}
}

using UnityEngine;

namespace RPGCore.Audio
{
	[CreateAssetMenu (menuName = "RPGCore/Audio/Music Group")]
	public class MusicGroup : ScriptableObject
	{
		public AudioClip[] Music;

		public float Volume = 1.0f;

		public void Play ()
		{

		}
	}
}


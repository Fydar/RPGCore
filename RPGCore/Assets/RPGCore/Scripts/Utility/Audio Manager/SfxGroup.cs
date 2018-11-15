using UnityEngine;

namespace RPGCore.Audio
{
	[CreateAssetMenu (menuName = "RPGCore/Audio/Sfx Group")]
	public class SfxGroup : ScriptableObject
	{
		public AudioClip[] Clips;

		public Vector2 VolumeRange;
		public Vector2 PitchRange;

		public AudioClip GetClip ()
		{
			return Clips[Random.Range (0, Clips.Length)];
		}

		public void Play ()
		{

		}
	}
}
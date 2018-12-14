using UnityEngine;

namespace RPGCore.Audio
{
	[CreateAssetMenu (menuName = "RPGCore/Audio/Loop Group")]
	public class LoopGroup : ScriptableObject
	{
		public AudioClip LoopedAudio;

		public Vector2 VolumeRange;
		public Vector2 PitchRange;
		public float PerlinSpeed = 5.0f;

		public void Play ()
		{

		}
	}
}


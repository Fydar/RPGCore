using UnityEngine;
using RPGCore.Utility;

namespace RPGCore
{
	[CreateAssetMenu]
	public class BehaviourGraphResources : ResourceCollection<BehaviourGraphResources>
	{
		public Texture2D DefaultTrail;
		public Texture2D SmallConnection;
		public Texture2D WindowIcon;
		public Texture2D WindowBackground;

		[Space]

#if UNITY_EDITOR
		public GUIStyle NodeStyle;
#endif
	}
}
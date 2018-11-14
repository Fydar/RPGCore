using RPGCore.Behaviour;
using UnityEngine;

namespace RPGCore
{
	[CreateAssetMenu (menuName = "RPGCore/Buff")]
	public class BuffTemplate : BehaviourGraph
	{
		public string Name;
		[TextArea (3, 6)]
		public string Description;

#if ASSET_ICONS
		[AssetIcon]
#endif
		public Sprite Icon;

		public bool isDebuff = false;
	}
}
using RPGCore.Behaviour;
using System.Collections.Generic;
using UnityEngine;

namespace RPGCore
{
	[CreateAssetMenu (menuName = "RPGCore/Buff")]
	public class BuffTemplate : ScriptableObject, IBehaviourGraph
	{
		public string Name;
		[TextArea (3, 6)]
		public string Description;

#if ASSET_ICONS
		[AssetIcon]
#endif
		public Sprite Icon;

		public BuffType Type;

		[UnityEngine.Serialization.FormerlySerializedAs ("Nodes")]
		[SerializeField, HideInInspector] private List<BehaviourNode> nodes;

		public List<BehaviourNode> AllNodes
		{
			get
			{
				return nodes;
			}
			set
			{
				AllNodes = value;
			}
		}
	}
}

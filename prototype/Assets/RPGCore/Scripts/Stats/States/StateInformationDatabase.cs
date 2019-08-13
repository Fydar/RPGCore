using RPGCore.Utility;
using UnityEngine;

namespace RPGCore.Stats
{
	[CreateAssetMenu (menuName = "RPGCore/State/Database")]
	public class StateInformationDatabase : StaticDatabase<StateInformationDatabase>
	{
		public StateInformationCollection StateInfos;
	}
}


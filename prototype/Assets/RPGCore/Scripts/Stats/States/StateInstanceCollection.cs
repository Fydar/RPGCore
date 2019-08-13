using System;
using System.Collections.Generic;

namespace RPGCore.Stats
{
	[Serializable]
	public class StateInstanceCollection : StateCollection<StateInstance>
	{
		public void SetupReferences ()
		{
			IEnumerator<StateInstance> stats = GetEnumerator ();
			IEnumerator<StateInformation> info = StateInformationDatabase.Instance.StateInfos.GetEnumerator ();

			while (stats.MoveNext ())
			{
				info.MoveNext ();

				stats.Current.Info = info.Current;
			}
		}
	}
}


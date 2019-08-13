using System;

namespace RPGCore.Stats
{
	[Serializable]
	public class StateInstanceCollection : StateCollection<StateInstance>
	{
		public void SetupReferences ()
		{
			var stats = GetEnumerator ();
			var info = StateInformationDatabase.Instance.StateInfos.GetEnumerator ();

			while (stats.MoveNext ())
			{
				info.MoveNext ();

				stats.Current.Info = info.Current;
			}
		}
	}
}


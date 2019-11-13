using System;

namespace RPGCore.Stats
{
	[Serializable]
	public class ArmourStatInstanceCollection : ArmourStatCollection<StatInstance>
	{
		public void SetupReferences()
		{
			var ArmourStats = GetEnumerator();
			var info = ArmourStatInformationDatabase.Instance.ArmourStatInfos.GetEnumerator();

			while (ArmourStats.MoveNext())
			{
				info.MoveNext();

				if (info.Current == null)
				{
					continue;
				}

				ArmourStats.Current.Info = info.Current;
			}
		}
	}
}

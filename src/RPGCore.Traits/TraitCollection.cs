using System.Collections.Generic;

namespace RPGCore.Traits
{
	/// <summary>
	/// Contains a collection for all stats belonging to a player or an item.
	/// </summary>
	public class TraitCollection
	{
		public Dictionary<StatIdentifier, StatInstance> Stats;
		public Dictionary<StateIdentifier, StateInstance> States;

		public StatInstance this[StatIdentifier identifier]
		{
			get
			{
				if (Stats == null)
				{
					return null;
				}

				return Stats[identifier];
			}
		}

		public StateInstance this[StateIdentifier identifier]
		{
			get
			{
				if (Stats == null)
				{
					return null;
				}

				return States[identifier];
			}
		}
	}
}

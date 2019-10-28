using System.Collections.Generic;

namespace RPGCore.Traits
{
	/// <summary>
	/// Consolidates the definitons of traits with their templates sourced from data.
	/// </summary>
	public class TraitContext
	{
		public Dictionary<StatIdentifier, StatTemplate> Stats;
		public Dictionary<StateIdentifier, StateTemplate> States;

		public StatTemplate this[StatIdentifier identifier]
		{
			get
			{
				if (Stats == null)
					return null;

				return Stats[identifier];
			}
		}

		public StateTemplate this[StateIdentifier identifier]
		{
			get
			{
				if (States == null)
					return null;

				return States[identifier];
			}
		}
	}
}

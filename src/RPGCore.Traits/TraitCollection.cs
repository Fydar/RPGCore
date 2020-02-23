using Newtonsoft.Json;
using RPGCore.Behaviour;

namespace RPGCore.Traits
{
	/// <summary>
	/// Contains a collection for all stats belonging to a player or an item.
	/// </summary>
	[JsonObject]
	public class TraitCollection
	{
		[JsonProperty]
		public EventCollection<StatIdentifier, StatInstance> Stats;

		[JsonProperty]
		public EventCollection<StateIdentifier, StateInstance> States;

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

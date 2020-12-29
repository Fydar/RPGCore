using Newtonsoft.Json;
using RPGCore.Behaviour;
using RPGCore.Events;

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

		private readonly TraitContext traitContext;

		public StatInstance this[StatIdentifier identifier]
		{
			get
			{
				if (Stats == null)
				{
					return null;
				}

				if (!Stats.TryGetValue(identifier, out var instance))
				{
					var template = traitContext.Stats[identifier];
					instance = template.CreateInstance(identifier);
					Stats.Add(identifier, instance);
				}
				return instance;
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

				if (!States.TryGetValue(identifier, out var instance))
				{
					var template = traitContext.States[identifier];
					instance = template.CreateInstance(identifier);
					States.Add(identifier, instance);
				}
				return instance;
			}
		}

		public TraitCollection(TraitContext traitContext)
		{
			this.traitContext = traitContext;

			Stats = new EventCollection<StatIdentifier, StatInstance>();
			States = new EventCollection<StateIdentifier, StateInstance>();
		}
	}
}

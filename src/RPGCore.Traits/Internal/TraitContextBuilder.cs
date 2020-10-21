using System.Collections.Generic;

namespace RPGCore.Traits.Internal
{
	internal class TraitContextBuilder : ITraitContextBuilder
	{
		private readonly List<StatIdentifier> statIdentifiers;
		private readonly List<StateIdentifier> stateIdentifiers;
		private readonly Dictionary<StatIdentifier, StatTemplate> statMapping;
		private readonly Dictionary<StateIdentifier, StateTemplate> stateMapping;

		internal TraitContextBuilder()
		{
			statIdentifiers = new List<StatIdentifier>();
			stateIdentifiers = new List<StateIdentifier>();
			statMapping = new Dictionary<StatIdentifier, StatTemplate>();
			stateMapping = new Dictionary<StateIdentifier, StateTemplate>();
		}

		public TraitContext Build()
		{
			return new TraitContext(
				statMapping,
				stateMapping
			);
		}

		public ITraitContextBuilder UseTraits(IReadOnlyDictionary<StatIdentifier, StatTemplate> map)
		{
			foreach (var mapping in map)
			{
				statMapping.Add(mapping.Key, mapping.Value);
			}
			return this;
		}

		public ITraitContextBuilder UseTraits(IReadOnlyDictionary<StateIdentifier, StateTemplate> map)
		{
			foreach (var mapping in map)
			{
				stateMapping.Add(mapping.Key, mapping.Value);
			}
			return this;
		}

		public ITraitContextBuilder UseTrait(StatIdentifier statIdentifier, StatTemplate statTemplate)
		{
			statIdentifiers.Add(statIdentifier);
			statMapping.Add(statIdentifier, statTemplate);
			return this;
		}

		public ITraitContextBuilder UseTrait(StateIdentifier stateIdentifier, StateTemplate stateTemplate)
		{
			stateIdentifiers.Add(stateIdentifier);
			stateMapping.Add(stateIdentifier, stateTemplate);
			return this;
		}
	}
}

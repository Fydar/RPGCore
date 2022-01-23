using System.Collections.Generic;

namespace RPGCore.Traits;

public interface ITraitContextBuilder
{
	TraitContext Build();
	ITraitContextBuilder UseTrait(StatIdentifier statIdentifier, StatTemplate statTemplate);
	ITraitContextBuilder UseTrait(StateIdentifier stateIdentifier, StateTemplate stateTemplate);
	ITraitContextBuilder UseTraits(IReadOnlyDictionary<StateIdentifier, StateTemplate> map);
	ITraitContextBuilder UseTraits(IReadOnlyDictionary<StatIdentifier, StatTemplate> map);
}

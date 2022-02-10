using System.Collections.Generic;

namespace RPGCore.Traits;

/// <summary>
/// A builder for constructing instances of <see cref="TraitContext"/>.
/// </summary>
public interface ITraitContextBuilder
{
	/// <summary>
	/// Constructs a <see cref="TraitContext"/> from the current instance of this builder.
	/// </summary>
	/// <returns>A new <see cref="TraitContext"/> constructed from the current state of this builder.</returns>
	TraitContext Build();

	/// <summary>
	/// Instructs the builder to include a stat, identified by <paramref name="statIdentifier"/>.
	/// </summary>
	/// <param name="statIdentifier">An identifier for the new stat to include.</param>
	/// <param name="statTemplate">Template data for the new stat to include.</param>
	/// <returns>The current instance of this builder.</returns>
	ITraitContextBuilder UseTrait(StatIdentifier statIdentifier, StatTemplate statTemplate);

	/// <summary>
	/// Instructs the builder to include a state, identified by <paramref name="stateIdentifier"/>.
	/// </summary>
	/// <param name="stateIdentifier">An identifier for the new state to include.</param>
	/// <param name="stateTemplate">Template data for the new state to include.</param>
	/// <returns>The current instance of this builder.</returns>
	ITraitContextBuilder UseTrait(StateIdentifier stateIdentifier, StateTemplate stateTemplate);

	/// <summary>
	/// Instructs the builder to include a collection of states.
	/// </summary>
	/// <param name="map">A map of the state identifiers to templates to include.</param>
	/// <returns>The current instance of this builder.</returns>
	ITraitContextBuilder UseTraits(IReadOnlyDictionary<StateIdentifier, StateTemplate> map);

	/// <summary>
	/// Instructs the builder to include a collection of stats.
	/// </summary>
	/// <param name="map">A map of the stat identifiers to templates to include.</param>
	/// <returns>The current instance of this builder.</returns>
	ITraitContextBuilder UseTraits(IReadOnlyDictionary<StatIdentifier, StatTemplate> map);
}

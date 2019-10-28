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
	}
}

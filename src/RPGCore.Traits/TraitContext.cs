using RPGCore.Traits.Internal;
using System;
using System.Collections.Generic;

namespace RPGCore.Traits
{
	/// <summary>
	/// Consolidates the definitons of traits with their templates sourced from data.
	/// </summary>
	public class TraitContext
	{
		public IReadOnlyDictionary<StatIdentifier, StatTemplate> Stats { get; }
		public IReadOnlyDictionary<StateIdentifier, StateTemplate> States { get; }

		public TraitContext(
			IReadOnlyDictionary<StatIdentifier, StatTemplate> stats,
			IReadOnlyDictionary<StateIdentifier, StateTemplate> states)
		{
			Stats = stats;
			States = states;
		}

		public StatTemplate this[StatIdentifier identifier]
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

		public StateTemplate this[StateIdentifier identifier]
		{
			get
			{
				if (States == null)
				{
					return null;
				}

				return States[identifier];
			}
		}

		public IEnumerable<string> ValidateAgainstStaticShortcuts(Type type)
		{
			foreach (var stat in TraitIdentifierTemplateUtilities.GetLogicalMembers<StatIdentifier>(type))
			{
				if (!Stats.ContainsKey(stat))
				{
					yield return $"Could not locate a template for the defined identifier {stat}";
				}
			}
			foreach (var state in TraitIdentifierTemplateUtilities.GetLogicalMembers<StateIdentifier>(type))
			{
				if (!States.ContainsKey(state))
				{
					yield return $"Could not locate a template for the defined identifier {state}";
				}
			}
		}

		public IEnumerable<string> ValidateAgainstShortcuts<T>(T shortcuts)
			where T : ITraitIdentifierStructure
		{
			foreach (var stat in TraitIdentifierTemplateUtilities.GetLogicalMembers<StatIdentifier>(shortcuts))
			{
				if (!Stats.ContainsKey(stat))
				{
					yield return $"Could not locate a template for the defined identifier {stat}";
				}
			}
			foreach (var state in TraitIdentifierTemplateUtilities.GetLogicalMembers<StateIdentifier>(shortcuts))
			{
				if (!States.ContainsKey(state))
				{
					yield return $"Could not locate a template for the defined identifier {state}";
				}
			}
		}

		public static ITraitContextBuilder Create()
		{
			return new TraitContextBuilder();
		}
	}
}

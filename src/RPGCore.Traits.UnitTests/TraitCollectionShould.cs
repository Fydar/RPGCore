using NUnit.Framework;
using System.Collections.Generic;

namespace RPGCore.Traits.UnitTests
{
	public class TraitCollectionShould
	{
		[Test]
		public void FindAllFields ()
		{
			var context = new TraitContext ()
			{
				Stats = new Dictionary<StatIdentifier, StatTemplate> ()
				{
					[CharacterTraitDefinition.MaxHealth] = new StatTemplate ()
					{
						Name = "Max Health",
						MaxValue = 1000
					},
					[CharacterTraitDefinition.MaxMana] = new StatTemplate ()
					{
						Name = "Max Mana",
						MaxValue = 1000
					}
				},
				States = new Dictionary<StateIdentifier, StateTemplate>()
				{
					[CharacterTraitDefinition.CurrentHealth] = new StateTemplate ()
					{
						Name = "Current Health",
						MaxValue = 1000
					}
				}
			};
		}
	}
}

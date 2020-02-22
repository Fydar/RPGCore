using NUnit.Framework;
using System.Collections.Generic;

namespace RPGCore.Traits.UnitTests
{
	[TestFixture(TestOf = typeof(TraitCollection))]
	public class TraitCollectionShould
	{
		[Test, Parallelizable]
		public void ContextualInformation()
		{
			var context = new TraitContext()
			{
				Stats = new Dictionary<StatIdentifier, StatTemplate>()
				{
					[CharacterTrait.MaxHealth] = new StatTemplate()
					{
						Name = "Max Health",
						MaxValue = 1000
					},
					[CharacterTrait.MaxMana] = new StatTemplate()
					{
						Name = "Max Mana",
						MaxValue = 1000
					}
				},
				States = new Dictionary<StateIdentifier, StateTemplate>()
				{
					[CharacterTrait.CurrentHealth] = new StateTemplate()
					{
						Name = "Current Health",
						MaxValue = 1000
					}
				}
			};

			var characterTraits = new TraitCollection();
		}

		[Test, Parallelizable]
		public void FindAllFields()
		{
			var characterTraits = new TraitCollection();

			foreach (var stat in CharacterTrait.AllStats)
			{
				var statInstance = characterTraits[stat];

				TestContext.Error.WriteLine(stat);
			}
		}
	}
}

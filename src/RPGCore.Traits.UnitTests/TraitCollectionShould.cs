using NUnit.Framework;
using RPGCore.Traits.UnitTests.Shared;
using System.Collections.Generic;

namespace RPGCore.Traits.UnitTests
{
	[TestFixture(TestOf = typeof(TraitCollection))]
	public class TraitCollectionShould
	{
		private readonly TraitContext context = new TraitContext()
		{
			Stats = new Dictionary<StatIdentifier, StatTemplate>()
			{
				[StaticTraits.Health.Maximum] = new StatTemplate()
				{
					Name = "Max Health",
					MaxValue = 1000,
					MinValue = 0
				},
				[StaticTraits.Health.RegenDelay] = new StatTemplate()
				{
					Name = "Regen Delay",
					MaxValue = 60,
					MinValue = 0
				},
				[StaticTraits.Health.RegenRate] = new StatTemplate()
				{
					Name = "Regen Rate",
					MaxValue = 1000,
					MinValue = 0
				},
				[StaticTraits.Mana.Maximum] = new StatTemplate()
				{
					Name = "Max Mana",
					MaxValue = 1000,
					MinValue = 0
				},
				[StaticTraits.Mana.RegenDelay] = new StatTemplate()
				{
					Name = "Mana Regen Delay",
					MaxValue = 60,
					MinValue = 0
				},
				[StaticTraits.Mana.RegenRate] = new StatTemplate()
				{
					Name = "Mana Regen Rate",
					MaxValue = 1000,
					MinValue = 0
				},
				[StaticTraits.Stamina.Maximum] = new StatTemplate()
				{
					Name = "Stamina Max Mana",
					MaxValue = 1000,
					MinValue = 0
				},
				[StaticTraits.Stamina.RegenDelay] = new StatTemplate()
				{
					Name = "StaminaRegen Delay",
					MaxValue = 60,
					MinValue = 0
				},
				[StaticTraits.Stamina.RegenRate] = new StatTemplate()
				{
					Name = "StaminaRegen Rate",
					MaxValue = 1000,
					MinValue = 0
				},
			},
			States = new Dictionary<StateIdentifier, StateTemplate>()
			{
				[StaticTraits.Health.Current] = new StateTemplate()
				{
					Name = "Current Health",
					MaxValue = 1000,
					MinValue = 0
				},
				[StaticTraits.Health.RegenCooldown] = new StateTemplate()
				{
					Name = "Health Regen Cooldown",
					MaxValue = 1000,
					MinValue = 0
				},
				[StaticTraits.Mana.Current] = new StateTemplate()
				{
					Name = "Current Mana",
					MaxValue = 1000,
					MinValue = 0
				},
				[StaticTraits.Mana.RegenCooldown] = new StateTemplate()
				{
					Name = "Mana Regen Cooldown",
					MaxValue = 1000,
					MinValue = 0
				},
				[StaticTraits.Stamina.Current] = new StateTemplate()
				{
					Name = "Current Stamina",
					MaxValue = 1000,
					MinValue = 0
				},
				[StaticTraits.Stamina.RegenCooldown] = new StateTemplate()
				{
					Name = "Stamina Regen Cooldown",
					MaxValue = 1000,
					MinValue = 0
				},
			}
		};

		[Test, Parallelizable]
		public void ContextualInformation()
		{
			var characterTraits = new TraitCollection(context);
		}

		[Test, Parallelizable]
		public void FindAllFields()
		{
			var characterTraits = new TraitCollection(context);

			foreach (var stat in StaticTraits.AllStats)
			{
				var statInstance = characterTraits[stat];

				TestContext.Error.WriteLine($"{stat}:   {statInstance.Value}  ({statInstance.Template.MinValue} - {statInstance.Template.MaxValue})");
			}
		}
	}
}

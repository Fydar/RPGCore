using NUnit.Framework;
using RPGCore.Events;

namespace RPGCore.Traits.UnitTests;

[TestFixture(TestOf = typeof(StatInstance))]
public class StatInstanceShould
{
	[Test, Parallelizable]
	public void UpdateFromModifierChanges()
	{
		StatIdentifier genericStat = "generic_stat";

		var template = new StatTemplate()
		{
			Name = "Generic Stat",
			MaxValue = 1000
		};
		var instance = template.CreateInstance(genericStat);

		var static5 = new EventField<float>(5);
		var changeFrom5to10 = new EventField<float>(5);

		Assert.AreEqual(0.0f, instance.Value);

		var modifier = new StatModifier(StatModificationPhase.Typical, StatModificationType.Additive, static5);
		instance.AddModifier(modifier);

		Assert.AreEqual(5.0f, instance.Value);

		var anotherModifier = new StatModifier(StatModificationPhase.Typical, StatModificationType.Additive, changeFrom5to10);
		instance.AddModifier(anotherModifier);

		Assert.AreEqual(10.0f, instance.Value);

		changeFrom5to10.Value = 10.0f;

		Assert.AreEqual(15.0f, instance.Value);
	}

	[Test, Parallelizable]
	public void UnsubscribeModifierWhenRemoved()
	{
		StatIdentifier genericStat = "generic_stat";

		var template = new StatTemplate()
		{
			Name = "Generic Stat",
			MaxValue = 1000
		};
		var instance = template.CreateInstance(genericStat);

		var changingModifierValue = new EventField<float>(5);

		Assert.AreEqual(0.0f, instance.Value);

		var modifier = new StatModifier(StatModificationPhase.Typical, StatModificationType.Additive, changingModifierValue);
		instance.AddModifier(modifier);

		Assert.AreEqual(5.0f, instance.Value);

		changingModifierValue.Value = 10.0f;

		Assert.AreEqual(10.0f, instance.Value);

		instance.RemoveModifier(modifier);

		Assert.AreEqual(0.0f, instance.Value);

		changingModifierValue.Value = 15.0f;

		Assert.AreEqual(0.0f, instance.Value);
	}
}

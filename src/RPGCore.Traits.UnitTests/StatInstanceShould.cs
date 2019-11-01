using NUnit.Framework;
using RPGCore.Behaviour;

namespace RPGCore.Traits.UnitTests
{
	public class StatInstanceShould
	{
		[Test]
		public void UpdateFromModifierChanges ()
		{
			var template = new StatTemplate ()
			{
				Name = "Current Health",
				MaxValue = 1000
			};
			var instance = template.CreateInstance (CharacterTrait.MaxMana);

			var static5 = new EventField<float> (5);
			var changeFrom5to10 = new EventField<float> (5);

			Assert.AreEqual (0.0f, instance.Value);

			var modifier = new StatModifier (StatModificationPhase.Typical, StatModificationType.Additive, static5);
			instance.AddModifier (modifier);

			Assert.AreEqual (5.0f, instance.Value);

			var anotherModifier = new StatModifier (StatModificationPhase.Typical, StatModificationType.Additive, changeFrom5to10);
			instance.AddModifier (anotherModifier);

			Assert.AreEqual (10.0f, instance.Value);

			changeFrom5to10.Value = 10.0f;

			Assert.AreEqual (15.0f, instance.Value);
		}

		[Test]
		public void UnsubscribeModifierWhenRemoved ()
		{
			var template = new StatTemplate ()
			{
				Name = "Current Health",
				MaxValue = 1000
			};
			var instance = template.CreateInstance (CharacterTrait.MaxMana);

			var changingModifierValue = new EventField<float> (5);

			Assert.AreEqual (0.0f, instance.Value);

			var modifier = new StatModifier (StatModificationPhase.Typical, StatModificationType.Additive, changingModifierValue);
			instance.AddModifier (modifier);

			Assert.AreEqual (5.0f, instance.Value);

			changingModifierValue.Value = 10.0f;

			Assert.AreEqual (10.0f, instance.Value);

			instance.RemoveModifier (modifier);

			Assert.AreEqual (0.0f, instance.Value);

			changingModifierValue.Value = 15.0f;

			Assert.AreEqual (0.0f, instance.Value);
		}
	}
}

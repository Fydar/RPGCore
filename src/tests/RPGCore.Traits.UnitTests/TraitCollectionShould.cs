using NUnit.Framework;

namespace RPGCore.Traits.UnitTests
{
	[TestFixture(TestOf = typeof(TraitCollection))]
	public class TraitCollectionShould
	{
		[Test, Parallelizable]
		public void ContextualInformation()
		{
			var charStatA = (StatIdentifier)"char_statA";

			var context = TraitContext.Create()
				.UseTrait(charStatA, new StatTemplate()
				{
					Name = "Character Stat A",
					MinValue = 2,
					MaxValue = 12,
				})
				.Build();

			var characterTraits = new TraitCollection(context);

			var charStatAInstance = characterTraits[charStatA];

		}
	}
}

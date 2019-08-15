using NUnit.Framework;

namespace RPGCore.Traits.UnitTests
{
	public class TraitCollectionShould
	{
		[Test]
		public void FindAllFields ()
		{
			var traitTemplates = new CharacterTrait<StatTemplate, StateTemplate> ();

			foreach (var state in traitTemplates.States)
			{
				TestContext.WriteLine (state);
			}

			foreach (var stat in traitTemplates.Stats)
			{
				TestContext.WriteLine (stat);
			}
		}
	}
}

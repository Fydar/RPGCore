using NUnit.Framework;

namespace RPGCore.Traits.UnitTests
{
	public class TraitCollectionShould
	{
		[Test]
		public void FindAllFields ()
		{
			var traitTemplates = new CharacterTrait<StatTemplate, StateTemplate> ();
			var traitInstances = new CharacterTrait<StatInstance, StateInstance> ();

			traitInstances.SetTemplate(traitTemplates);
		}
	}
}

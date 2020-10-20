namespace RPGCore.Traits.UnitTests.Shared
{
	public static class StaticTraits
	{
		public static ResourceTraitTemplate Health = new ResourceTraitTemplate("character_health");
		public static ResourceTraitTemplate Stamina = new ResourceTraitTemplate("character_stamina");
		public static ResourceTraitTemplate Mana = new ResourceTraitTemplate("character_mana");

		public static ElementalTraitsTemplate[] Elemental = new ElementalTraitsTemplate[]
		{
			new ElementalTraitsTemplate("character_fire")
		};

		public static StatIdentifier[] AllStats = TraitIdentifiers.AllStats(typeof(StaticTraits));
		public static StateIdentifier[] AllStates = TraitIdentifiers.AllStates(typeof(StaticTraits));
	}
}

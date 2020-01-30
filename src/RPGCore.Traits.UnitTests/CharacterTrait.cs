namespace RPGCore.Traits.UnitTests
{
	public static class CharacterTrait
	{
		public static StateIdentifier CurrentHealth = "character_currenthealth";
		public static StatIdentifier MaxHealth = "character_maxhealth";

		public static StatIdentifier MaxMana = "character_maxmana";

		public static StatIdentifier[] AllStats = TraitIdentifiers.AllStats(typeof(CharacterTrait));
		public static StateIdentifier[] AllStates = TraitIdentifiers.AllStates(typeof(CharacterTrait));
	}
}

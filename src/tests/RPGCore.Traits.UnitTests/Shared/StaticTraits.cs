namespace RPGCore.Traits.UnitTests.Shared;

public static class StaticTraits
{
	public static ResourceTraitTemplate Health { get; } = new("character_health");
	public static ResourceTraitTemplate Stamina { get; } = new("character_stamina");
	public static ResourceTraitTemplate Mana { get; } = new("character_mana");

	public static ElementalTraitsTemplate[] Elemental { get; } = new ElementalTraitsTemplate[]
	{
		new ElementalTraitsTemplate("character_fire")
	};
}

namespace RPGCore.Traits.UnitTests.Shared;

public class ElementalTraitsTemplate : ITraitIdentifierStructure
{
	public StatIdentifier Proficiency { get; }
	public StatIdentifier Resistance { get; }

	public StateIdentifier Shielding { get; }

	public ElementalTraitsTemplate(string elementalIdentifier)
	{
		Proficiency = elementalIdentifier + "_proficiency";
		Resistance = elementalIdentifier + "_resistance";

		Shielding = elementalIdentifier + "_shielding";
	}
}

namespace RPGCore.Traits.UnitTests.Shared;

public class ResourceTraitTemplate : ITraitIdentifierStructure
{
	public StatIdentifier Maximum { get; }
	public StatIdentifier RegenDelay { get; }
	public StatIdentifier RegenRate { get; }

	public StateIdentifier Current { get; }
	public StateIdentifier RegenCooldown { get; }

	public ResourceTraitTemplate(string resourceIdentifier)
	{
		Maximum = resourceIdentifier + "_max";
		RegenDelay = resourceIdentifier + "_regendelay";
		RegenRate = resourceIdentifier + "_regenrate";

		Current = resourceIdentifier + "_current";
		RegenCooldown = resourceIdentifier + "_regencooldown";
	}
}

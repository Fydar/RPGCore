using RPGCore.Events;

namespace RPGCore.Demo.Inventory;

public class DemoPlayer
{
	public EventField<int> Health;

	public DemoPlayer()
	{
		Health = new EventField<int>
		{
			Value = 100
		};
	}

	/// <inheritdoc/>
	public override string ToString()
	{
		return "Actor";
	}
}

using RPGCore.Demo.Inventory;

namespace RPGCore.Runner;

public sealed class Simulator
{
	public Simulation simulation;

	public Simulator()
	{
		simulation = new Simulation();
	}

	public void Start()
	{
		simulation.Start();
	}

	public void Update()
	{
	}
}

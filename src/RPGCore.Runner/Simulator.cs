using RPGCore.Behaviour;
using RPGCore.Demo;

namespace RPGCore.Runner
{
	public sealed class Simulator
	{
		public Simulation simulation;

		public Simulator ()
		{
			simulation = new Simulation ();
		}

		public void Start ()
		{
			simulation.Start ();
		}

		public void Update ()
		{

		}
	}
}

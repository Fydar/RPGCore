﻿using RPGCore.Behaviour;

namespace RPGCore.Runner
{
	public class Simulator
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
using System.Collections.Generic;

namespace RPGCore.Behaviour
{
	public class DemoPlayer
	{
		public List<INodeInstance> behaviours;

		public EventField<int> Health;

		public DemoPlayer ()
		{
			behaviours = new List<INodeInstance> ();
			Health = new EventField<int>
			{
				Value = 100
			};
		}

		public override string ToString ()
		{
			return "Actor";
		}
	}
}

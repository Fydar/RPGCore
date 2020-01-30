using System.Collections.Generic;
using RPGCore.Traits;

namespace RPGCore.Behaviour
{
	public class DemoPlayer
	{
		public TraitCollection Traits;

		public List<INodeInstance> behaviours;
		public EventField<int> Health;

		public DemoPlayer()
		{
			behaviours = new List<INodeInstance>();
			Health = new EventField<int>
			{
				Value = 100
			};
		}

		public override string ToString()
		{
			return "Actor";
		}
	}
}

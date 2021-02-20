using RPGCore.Behaviour;
using RPGCore.Events;
using RPGCore.Traits;
using System.Collections.Generic;

namespace RPGCore.Demo.Inventory
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

		/// <inheritdoc/>
		public override string ToString()
		{
			return "Actor";
		}
	}
}

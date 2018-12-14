using System.Collections.Generic;

namespace Behaviour
{
	public class Actor
	{
		public List<INodeInstance> behaviours;

		public EventField<int> Health;

		public Actor()
		{
			behaviours = new List<INodeInstance>();
			Health = new EventField<int>();
			Health.Value = 100;
		}

		public override string ToString()
		{
			return "Actor";
		}
	}
}

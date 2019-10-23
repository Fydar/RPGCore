using System.Collections.Generic;

namespace RPGCore.Behaviour
{
	public interface IGraphInstance : IBehaviour
	{
		INodeInstance this[LocalId id] { get; }
		void Setup (Actor target);
		InputSource GetSource<T> (Input<T> input);
		IEnumerable<OutputSource> GetSource<T> (Output<T> output);
	}
}

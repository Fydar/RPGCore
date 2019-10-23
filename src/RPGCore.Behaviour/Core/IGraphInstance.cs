using System.Collections.Generic;

namespace RPGCore.Behaviour
{
	public interface IGraphInstance : IBehaviour
	{
		INodeInstance this[LocalId id] { get; }

		void Setup (Actor target);

		T GetNodeInstance<T> () where T : INodeInstance;
		IEnumerable<T> GetNodeInstances<T> () where T : INodeInstance;
		InputSource GetSource<T> (Input<T> input);
		IEnumerable<OutputSource> GetSource<T> (Output<T> output);
	}
}

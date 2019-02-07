namespace Behaviour
{
	public interface INodeInstance
	{
		void Setup(IGraphInstance graph, Node parent, Actor target);
		void Remove();
	}
}

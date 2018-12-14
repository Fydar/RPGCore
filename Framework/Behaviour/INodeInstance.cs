namespace Behaviour
{
	public interface INodeInstance
	{
		void Setup(GraphInstance graph, Node parent, Actor target);
		void Remove();
	}
}

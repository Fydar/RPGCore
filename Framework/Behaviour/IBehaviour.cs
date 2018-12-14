namespace Behaviour
{
	public interface IBehaviour
	{
		INodeInstance GetNode<T>();
		void Remove();
	}
}

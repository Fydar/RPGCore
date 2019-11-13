namespace RPGCore.Behaviour
{
	public interface INodeSerialization
	{
		string Serialize(IBehaviourContext context);
		void Deserialize(IBehaviourContext context, string data);
	}
}

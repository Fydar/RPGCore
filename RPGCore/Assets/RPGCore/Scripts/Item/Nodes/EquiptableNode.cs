using RPGCore.Behaviour;
using RPGCore.Behaviour.Connections;

namespace RPGCore
{
	[NodeInformation ("Item/Equiptable Input", "Input")]
	public class EquiptableNode : BehaviourNode
	{
		public Slot slot;

		public BoolOutput Equipped;

		protected override void OnSetup (IBehaviourContext context)
		{
			/*ItemSurrogate item = (ItemSurrogate)context;
			ConnectionEntry<bool> equippedOutput = Equipped.GetEntry (context);

			equippedOutput.Value = item.Equipped.Value;

			item.Equipped.onChanged += () => 
			{
				equippedOutput.Value = character.Equipped.Value;
			};*/
		}

		protected override void OnRemove (IBehaviourContext character)
		{

		}
	}
}
using RPGCore.Behaviour;
using RPGCore.Behaviour.Connections;

namespace RPGCore
{
	[NodeInformation ("Buff/Set Stack Size")]
	public class SetStackSizeNode : BehaviourNode
	{
		public EventInput Set;
		public IntInput Size;

		protected override void OnSetup (IBehaviourContext context)
		{
			EventEntry setInput = Set[context];
			ConnectionEntry<int> sizeInput = Size[context];

			setInput.OnEventFired += () =>
			{
				Buff buff = (Buff)context;

				buff.BaseStackSize.Value = 0;
			};
		}

		protected override void OnRemove (IBehaviourContext context)
		{

		}
	}
}


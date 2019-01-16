using RPGCore.Behaviour;
using RPGCore.Behaviour.Connections;

namespace RPGCore
{
	[NodeInformation ("Item/Charges Input", "Input", OnlyOne = true)]
	public class ChargesInputNode : BehaviourNode, IInputNode<ItemSurrogate>
	{
		public IntOutput Charges;
		public EventInput ConsumeCharge;
		public EventOutput OnConsumeCharge;

		protected override void OnSetup (IBehaviourContext context)
		{
			
		}

		protected override void OnRemove (IBehaviourContext context)
		{

		}

		public void SetTarget (IBehaviourContext context, ItemSurrogate target)
		{
			var chargesData = target.data.dataCollection.AssureElement ("Charges");

			//chargesData.SetValue (0);
		}
	}
}

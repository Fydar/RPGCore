using RPGCore.Behaviour;
using RPGCore.Behaviour.Connections;
using UnityEngine;

namespace RPGCore
{
	[NodeInformation ("Item/Charges Input", "Input", OnlyOne = true)]
	public class ChargesInputNode : BehaviourNode, IInputNode<ItemSurrogate>, INodeSerialization
	{
		public struct InstanceData
		{
			public int Charges;
		}

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
			
		}
		
		public string Serialize (IBehaviourContext context)
		{
			var chargesOutput = Charges[context];

			return JsonUtility.ToJson(new InstanceData() { Charges = chargesOutput.Value });
		}

		public void Deserialize (IBehaviourContext context, string data)
		{
			var chargesOutput = Charges[context];
			var instanceData = JsonUtility.FromJson<InstanceData>(data);

			chargesOutput.Value = instanceData.Charges;
		}
	}
}

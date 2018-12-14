using RPGCore.Behaviour;
using RPGCore.Behaviour.Connections;
using RPGCore.Stats;
using System;
using System.Collections.Generic;

namespace RPGCore
{
	[NodeInformation ("Item/Armour Input", "Input", OnlyOne = true)]
	public class ArmourInputNode : BehaviourNode, IInputNode<ItemSurrogate>
	{
		public FloatInput Armour;

		[NonSerialized]
		public Dictionary<IBehaviourContext, ArmourStatInstanceCollection> StatsMapping = new Dictionary<IBehaviourContext, ArmourStatInstanceCollection> ();
		
		public ArmourStatInstanceCollection GetStats (IBehaviourContext context)
		{
			ArmourStatInstanceCollection statsCollection;
			if (!StatsMapping.TryGetValue (context, out statsCollection))
			{
				ConnectionEntry<float> armourInput = Armour[context];

				statsCollection = new ArmourStatInstanceCollection ();
				StatsMapping[context] = statsCollection;
				statsCollection.GetEnumerator ();

				var attackModifier = statsCollection.Armour.AddFlatModifier (armourInput.Value);
				armourInput.OnAfterChanged += () => attackModifier.Value = armourInput.Value;
			}
			return statsCollection;
		}

		public StatInstance GetStat (IBehaviourContext context, CollectionEntry entry)
		{
			ArmourStatInstanceCollection statsCollection = GetStats (context);

			if (entry.entryIndex == -1)
			{
				var temp = ArmourStatInformationDatabase.Instance.ArmourStatInfos[entry];
			}
			
			StatInstance stat = null;

			if (entry.entryIndex == 0)
				stat = statsCollection.Armour;

			return stat;
		}

		protected override void OnSetup (IBehaviourContext context)
		{
			
		}

		protected override void OnRemove (IBehaviourContext context)
		{

		}

		public void SetTarget (IBehaviourContext context, ItemSurrogate target)
		{

		}
	}
}

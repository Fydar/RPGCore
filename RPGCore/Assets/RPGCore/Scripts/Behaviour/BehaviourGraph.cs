using System.Collections.Generic;
using UnityEngine;

namespace RPGCore.Behaviour
{
	public abstract class BehaviourGraph : ScriptableObject
	{
		[HideInInspector]
		public List<BehaviourNode> Nodes = new List<BehaviourNode> ();

		public T GetNode<T> ()
			where T : BehaviourNode
		{
			foreach (BehaviourNode node in Nodes)
			{
				if (typeof (T).IsAssignableFrom (node.GetType ()))
				{
					return (T)node;
				}
			}
			return null;
		}

		public T[] GetNodes<T> ()
			where T : BehaviourNode
		{
			List<T> foundNodes = new List<T> ();

			foreach (BehaviourNode node in Nodes)
			{
				if (typeof (T).IsAssignableFrom (node.GetType ()))
				{
					foundNodes.Add ((T)node);
				}
			}
			return foundNodes.ToArray ();
		}

		public void SetupGraph (IBehaviourContext context)
		{
			foreach (BehaviourNode node in Nodes)
			{
				node.SetupContext (context);
			}
		}

		public void RemoveGraph (IBehaviourContext context)
		{
			foreach (BehaviourNode node in Nodes)
			{
				BehaviourNode behaviourNode = node;

				behaviourNode.RemoveContext (context);

				foreach (OutputSocket data in node.Outputs)
				{
					if (data == null)
						continue;

					data.RemoveContext (context);
				}

				foreach (InputSocket data in node.Inputs)
				{
					if (data == null)
						continue;

					data.RemoveContext (context);
				}
			}
		}
	}
}
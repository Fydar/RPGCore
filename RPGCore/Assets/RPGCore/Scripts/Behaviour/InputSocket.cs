using System;
using UnityEngine;

namespace RPGCore.Behaviour
{
	[Serializable]
	public abstract class InputSocket : Socket
	{
		private const int socketSize = 16;

		[SerializeField]
		public BehaviourNode SourceNode;
		[SerializeField]
		public string SourceField;

		[NonSerialized]
		private OutputSocket sourceSocket;

		public OutputSocket SourceSocket
		{
			get
			{
				if (sourceSocket == null)
				{

				}
				if (SourceNode == null)
				{
					return null;
				}

				var tempA = SourceNode.Inputs;
				var tempB = SourceNode.Outputs;

				sourceSocket = SourceNode.GetOutput (SourceField);

				return sourceSocket;
			}
		}

		public override void RemoveContext (IBehaviourContext context)
		{

		}

		public virtual object GetConnectionObject (IBehaviourContext context)
		{
			return null;
		}

		public virtual void AfterContentChanged ()
		{

		}

#if UNITY_EDITOR
		public Rect SocketRect
		{
			get
			{
				return new Rect (drawRect.xMin - socketSize - 4, drawRect.y, socketSize, socketSize);
			}
		}
#endif
	}
}
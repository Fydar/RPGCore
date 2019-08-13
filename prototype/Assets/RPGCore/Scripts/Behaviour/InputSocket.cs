using System;
using UnityEngine;

namespace RPGCore.Behaviour
{
	public abstract class InputSocket : Socket
	{
		private const int socketSize = 16;

		[SerializeField]
		public BehaviourNode SourceNode;
		[SerializeField, UnityEngine.Serialization.FormerlySerializedAs ("SourceField")]
		public string SourcePath;

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

				var tempA = SourceNode.InputSockets;
				var tempB = SourceNode.OutputSockets;

				sourceSocket = SourceNode.GetOutput (SourcePath);

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


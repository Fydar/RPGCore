using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;

namespace RPGCore
{
	[UnityEditor.CustomPropertyDrawer (typeof (InputSocket))]
	public partial class InputDrawer { }
#endif

	[Serializable]
	public class InputSocket : Socket
	{
		public const int socketSize = 16;

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

		public override void RemoveContext (IBehaviourContext character)
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
		public Rect socketRect
		{
			get
			{
				return new Rect (drawRect.xMin - socketSize - 4, drawRect.y, socketSize, socketSize);
			}
		}
#endif
	}
}
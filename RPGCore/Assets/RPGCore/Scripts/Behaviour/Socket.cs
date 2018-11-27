using UnityEngine;
using System;
using RPGCore.Behaviour.Editor;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RPGCore.Behaviour
{
	public abstract class Socket
	{
		[NonSerialized] public BehaviourNode ParentNode;
		[NonSerialized] public string SocketName;
		public Rect drawRect;

		public abstract void RemoveContext (IBehaviourContext context);

		public virtual ConnectionEntry GetBaseEntry (IBehaviourContext context)
		{
			return null;
		}

#if UNITY_EDITOR
		public virtual void DrawConnection (Vector3 start, Vector3 end, Vector3 startDir, Vector3 endDir)
		{
			float distance = Vector3.Distance (start, end);
			Vector3 startTan = start + (startDir * distance * 0.5f);
			Vector3 endTan = end + (endDir * distance * 0.5f);

			Color connectionColour = new Color (1.0f, 1.0f, 1.0f) * Color.Lerp (GUI.color, Color.white, 0.5f);
			Handles.DrawBezier (start, end, startTan, endTan, connectionColour,
				BehaviourGraphResources.Instance.SmallConnection, 10);
		}

		public virtual void DrawSocket (Rect rect)
		{
			EditorGUI.HelpBox (rect, "", MessageType.None);
		}
#endif
	}
}
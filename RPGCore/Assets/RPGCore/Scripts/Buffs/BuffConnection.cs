using UnityEngine;
using System;
using RPGCore.Behaviour;
using RPGCore.Behaviour.Editor;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RPGCore
{
	public class BuffConnection : Connection<Buff, BuffConnection, ConnectionEntry<Buff>>
	{
#if UNITY_EDITOR
		public override void DrawConnection (Vector3 start, Vector3 end, Vector3 startDir, Vector3 endDir)
		{
			float distance = Vector3.Distance (start, end);
			Vector3 startTan = start + (startDir * distance * 0.5f);
			Vector3 endTan = end + (endDir * distance * 0.5f);

			Color connectionColour = new Color (1.0f, 0.85f, 0.8f) * Color.Lerp (GUI.color, Color.white, 0.5f);
			Handles.DrawBezier (start, end, startTan, endTan, connectionColour,
				BehaviourGraphResources.Instance.DefaultTrail, 14);
		}
#endif
	}

	[Serializable]
	public class BuffInput : BuffConnection.Input
	{
#if UNITY_EDITOR
		public override void DrawSocket (Rect rect)
		{
			GUI.color = new Color (1.0f, 0.8f, 0.7f) * Color.Lerp (GUI.color, Color.white, 0.5f);
			base.DrawSocket (rect);
			GUI.color = Color.white;
		}
#endif
	}

	[Serializable]
	public class BuffOutput : BuffConnection.Output
	{
#if UNITY_EDITOR
		public override void DrawSocket (Rect rect)
		{
			GUI.color = new Color (1.0f, 0.8f, 0.7f) * Color.Lerp (GUI.color, Color.white, 0.5f);
			base.DrawSocket (rect);
			GUI.color = Color.white;
		}
#endif
	}

	[Serializable]
	public class BuffListOutput : BuffConnection.ListOutput
	{
#if UNITY_EDITOR
		public override void DrawSocket (Rect rect)
		{
			GUI.color = new Color (1.0f, 0.8f, 0.7f) * Color.Lerp (GUI.color, Color.white, 0.5f);
			base.DrawSocket (rect);
			GUI.color = Color.white;
		}
#endif
	}
}
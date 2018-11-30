using UnityEngine;
using System;
using RPGCore.Behaviour.Editor;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RPGCore.Behaviour.Connections
{
	[ConnectionInformation ("Float", "A decimal number")]
	public class FloatConnection : Connection<float, FloatConnection, ConnectionEntry<float>>
	{
		public static Color SocketColour = new Color (0.8f, 0.8f, 0.8f);

#if UNITY_EDITOR
		public override void DrawConnection (Vector3 start, Vector3 end, Vector3 startDir, Vector3 endDir)
		{
			float distance = Vector3.Distance (start, end);
			Vector3 startTan = start + (startDir * distance * 0.5f);
			Vector3 endTan = end + (endDir * distance * 0.5f);

			Color connectionColour = new Color (0.9f, 0.9f, 0.9f) * Color.Lerp (GUI.color, Color.white, 0.5f);
			Handles.DrawBezier (start, end, startTan, endTan, connectionColour,
				BehaviourGraphResources.Instance.SmallConnection, 10);
		}
#endif


	}

	[Serializable]
	public class FloatInput : FloatConnection.Input
	{
#if UNITY_EDITOR
		public override void DrawSocket (Rect rect)
		{
			GUI.color = new Color (0.8f, 0.8f, 0.8f) * Color.Lerp (GUI.color, Color.white, 0.5f);
			base.DrawSocket (rect);
			GUI.color = Color.white;
		}
#endif
	}

	[Serializable]
	public class FloatOutput : FloatConnection.Output
	{
#if UNITY_EDITOR
		public override void DrawSocket (Rect rect)
		{
			GUI.color = new Color (0.8f, 0.8f, 0.8f) * Color.Lerp (GUI.color, Color.white, 0.5f);
			base.DrawSocket (rect);
			GUI.color = Color.white;
		}
#endif
	}
}


using UnityEngine;
using System;
using RPGCore.Behaviour.Editor;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RPGCore.Behaviour.Connections
{
	[ConnectionInformation ("Int", "Convertable to an integer")]
	public class IntEntry : ConnectionEntry<int>, ISocketConvertable<float>
	{
		public static Color SocketColour = new Color (1.0f, 1.0f, 1.0f);

		float ISocketConvertable<float>.Convert
		{
			get
			{
				return Value;
			}
		}
	}

	public class IntConnection : Connection<int, IntConnection, IntEntry>
	{
#if UNITY_EDITOR
		public override void DrawConnection (Vector3 start, Vector3 end, Vector3 startDir, Vector3 endDir)
		{
			float distance = Vector3.Distance (start, end);
			Vector3 startTan = start + (startDir * distance * 0.5f);
			Vector3 endTan = end + (endDir * distance * 0.5f);

			Color connectionColour = new Color (1.0f, 1.0f, 1.0f) * Color.Lerp (GUI.color, Color.white, 0.5f);
			Handles.DrawBezier (start, end, startTan, endTan, connectionColour,
				BehaviourGraphResources.Instance.SmallConnection, 10);
		}
#endif
	}

	[Serializable]
	public class IntInput : IntConnection.Input
	{
#if UNITY_EDITOR
		public override void DrawSocket (Rect rect)
		{
			GUI.color = new Color (1.0f, 1.0f, 1.0f) * Color.Lerp (GUI.color, Color.white, 0.5f);
			base.DrawSocket (rect);
			GUI.color = Color.white;
		}
#endif
	}

	[Serializable]
	public class IntOutput : IntConnection.Output
	{
#if UNITY_EDITOR
		public override void DrawSocket (Rect rect)
		{
			GUI.color = new Color (1.0f, 1.0f, 1.0f) * Color.Lerp (GUI.color, Color.white, 0.5f);
			base.DrawSocket (rect);
			GUI.color = Color.white;
		}
#endif
	}
}


using UnityEngine;
using System;
using RPGCore.Behaviour;
using RPGCore.Behaviour.Editor;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RPGCore
{
	[ConnectionInformation ("Item", "Represents an item")]
	public class ItemConnection : Connection<ItemSurrogate, ItemConnection, ConnectionEntry<ItemSurrogate>>
	{
		public static Color SocketColour = new Color (0.65f, 0.65f, 1.0f);

#if UNITY_EDITOR
		public override void DrawConnection (Vector3 start, Vector3 end, Vector3 startDir, Vector3 endDir)
		{
			float strength = Vector3.Distance (start, end) * 0.5f;
			Vector3 startTan = start + (startDir * strength);
			Vector3 endTan = end + (endDir * strength);

			Color connectionColour = new Color (0.75f, 0.85f, 1.0f);
			Handles.DrawBezier (start, end, startTan, endTan, connectionColour,
				BehaviourGraphResources.Instance.DefaultTrail, 14);
		}
#endif
	}

	[Serializable]
	public class ItemInput : ItemConnection.Input
	{
#if UNITY_EDITOR
		public override void DrawSocket (Rect rect)
		{
			GUI.color = ItemConnection.SocketColour;
			base.DrawSocket (rect);
			GUI.color = Color.white;
		}
#endif
	}

	[Serializable]
	public class ItemOutput : ItemConnection.Output
	{
#if UNITY_EDITOR
		public override void DrawSocket (Rect rect)
		{
			GUI.color = ItemConnection.SocketColour;
			base.DrawSocket (rect);
			GUI.color = Color.white;
		}
#endif
	}

	[Serializable]
	public class ItemListOutput : ItemConnection.ListOutput
	{
#if UNITY_EDITOR
		public override void DrawSocket (Rect rect)
		{
			GUI.color = ItemConnection.SocketColour;
			base.DrawSocket (rect);
			GUI.color = Color.white;
		}
#endif
	}
}

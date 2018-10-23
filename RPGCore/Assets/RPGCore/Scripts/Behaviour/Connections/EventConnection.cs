using UnityEngine;
using System;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;

namespace RPGCore
{
	[CustomPropertyDrawer (typeof (EventInput))]
	public class EventInputDrawer : PropertyDrawer
	{
		public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
		{
			return EditorGUIUtility.singleLineHeight;
		}

		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
		{
			SerializedProperty sourceProperty = property.FindPropertyRelative ("SourceNode");
			Rect inputNoteRect = EditorGUI.PrefixLabel (position, label);

			if (sourceProperty.objectReferenceValue != null)
			{
				if (label != GUIContent.none)
					EditorGUI.LabelField (inputNoteRect, "Input", EditorStyles.centeredGreyMiniLabel);
			}
			else
			{
				EditorGUI.LabelField (inputNoteRect, "Never", EditorStyles.centeredGreyMiniLabel);
			}
			property.FindPropertyRelative ("drawRect").rectValue = position;
		}
	}

	[CustomPropertyDrawer (typeof (EventOutput))]
	public partial class EventOutputDrawer : PropertyDrawer
	{
		public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
		{
			return EditorGUIUtility.singleLineHeight;
		}

		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
		{
			property.FindPropertyRelative ("drawRect").rectValue = position;

			EditorGUI.LabelField (position, label);
		}
	}
#endif

	[ConnectionInformation ("Event", "Used to fire effects")]
	public class EventConnection
	{
		public static Color SocketColour = new Color (0.6f, 0.85f, 0.6f);
	}

	public class EventEntry
	{
		public Action OnEventFired;

		public void Invoke ()
		{
			if (OnEventFired != null)
				OnEventFired ();
		}
	}

	[Serializable]
	public class EventInput : InputSocket
	{
		public Dictionary<IBehaviourContext, EventEntry> ContextCahce;

		public EventEntry defaultEntry = new EventEntry ();

		public EventEntry this[IBehaviourContext context]
		{
			get
			{
				return GetEntry (context);
			}
		}

		public EventEntry GetEntry (IBehaviourContext character)
		{
			if (this.SourceNode == null)
				return defaultEntry;

			EventEntry foundEntry;

			if (ContextCahce == null)
				ContextCahce = new Dictionary<IBehaviourContext, EventEntry> ();

			bool result = ContextCahce.TryGetValue (character, out foundEntry);

			EventOutput sourceOutput = (EventOutput)SourceSocket;

			EventEntry connectionEntry = sourceOutput.GetEntry (character);

			if (!result)
			{
				foundEntry = new EventEntry ();

				connectionEntry.OnEventFired += () =>
				{
					if (foundEntry != null)
						foundEntry.OnEventFired ();
				};

				ContextCahce.Add (character, foundEntry);
			}

			return foundEntry;
		}

		public override object GetConnectionObject (IBehaviourContext context)
		{
			return GetEntry (context);
		}

		public override void RemoveContext (IBehaviourContext character)
		{
			if (ContextCahce == null)
				return;

			ContextCahce.Remove (character);
		}

#if UNITY_EDITOR
		public override void DrawConnection (Vector3 start, Vector3 end, Vector3 startDir, Vector3 endDir)
		{
			float distance = Vector3.Distance (start, end);
			Vector3 startTan = start + (startDir * distance * 0.5f);
			Vector3 endTan = end + (endDir * distance * 0.5f);

			Color connectionColour = new Color (0.9f, 1.0f, 0.9f) * Color.Lerp (GUI.color, Color.white, 0.5f);
			Handles.DrawBezier (start, end, startTan, endTan, connectionColour,
				BehaviourGraphResources.Instance.DefaultTrail, 14);
		}

		public override void DrawSocket (Rect rect)
		{
			GUI.color = new Color (0.6f, 0.85f, 0.6f) * Color.Lerp (GUI.color, Color.white, 0.5f);
			base.DrawSocket (rect);
			GUI.color = Color.white;
		}
#endif
	}

	[Serializable]
	public class EventOutput : OutputSocket
	{
		private Dictionary<IBehaviourContext, EventEntry> contextCahce;

		public EventEntry this[IBehaviourContext context]
		{
			get
			{
				return GetEntry (context);
			}
		}

		public EventEntry GetEntry (IBehaviourContext character)
		{
			if (contextCahce == null)
				contextCahce = new Dictionary<IBehaviourContext, EventEntry> ();

			EventEntry foundEntry;

			bool result = contextCahce.TryGetValue (character, out foundEntry);

			if (!result)
			{
				foundEntry = new EventEntry ();

				contextCahce.Add (character, foundEntry);
			}

			return foundEntry;
		}

		public override void RemoveContext (IBehaviourContext character)
		{
			if (contextCahce == null)
				return;

			contextCahce.Remove (character);
		}

#if UNITY_EDITOR
		public override void DrawConnection (Vector3 start, Vector3 end, Vector3 startDir, Vector3 endDir)
		{
			float distance = Vector3.Distance (start, end);
			Vector3 startTan = start + (startDir * distance * 0.75f);
			Vector3 endTan = end + (endDir * distance * 0.75f);

			Color connectionColour = new Color (0.9f, 1.0f, 0.9f) * Color.Lerp (GUI.color, Color.white, 0.5f);
			Handles.DrawBezier (start, end, startTan, endTan, connectionColour,
				BehaviourGraphResources.Instance.DefaultTrail, 14);
		}

		public override void DrawSocket (Rect rect)
		{
			GUI.color = new Color (0.6f, 0.85f, 0.6f) * Color.Lerp (GUI.color, Color.white, 0.5f);
			base.DrawSocket (rect);
			GUI.color = Color.white;
		}
#endif
	}
}
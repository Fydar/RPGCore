using System;
using UnityEngine;
using RPGCore.Behaviour.Connections;
using RPGCore.Behaviour.Editor;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RPGCore.Behaviour.Logic
{
	[NodeInformation ("Logic/Not")]
	public class NotNode : BehaviourNode
	{
		public BoolInput ValueA = new BoolInput ();
		public BoolOutput Output = new BoolOutput ();

		protected override void OnSetup (IBehaviourContext context)
		{
			ConnectionEntry<bool> valueAInput = ValueA[context];
			ConnectionEntry<bool> output = Output[context];

			Action updateHandler = () =>
			{
				output.Value = !valueAInput.Value;
			};

			valueAInput.OnAfterChanged += updateHandler;

			updateHandler ();
		}

		protected override void OnRemove (IBehaviourContext context)
		{

		}

#if UNITY_EDITOR
		public override Vector2 GetDiamentions ()
		{
			return new Vector2 (70, 38);
		}

		public override void DrawGUI (SerializedObject serializedObject, Rect position)
		{
			SerializedProperty valueAProperty = serializedObject.FindProperty ("ValueA");
			SerializedProperty outputProperty = serializedObject.FindProperty ("Output");

			GUI.DrawTexture (position, BehaviourGraphResources.Instance.NotNodeGraphic, ScaleMode.ScaleToFit, true);

			Rect valueARect = new Rect (position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
			Rect valueBRect = new Rect (position.x, valueARect.yMax + EditorGUIUtility.standardVerticalSpacing,
				position.width, valueARect.height);
			Rect outputRect = new Rect (position.x, position.y + (EditorGUIUtility.singleLineHeight * 0.5f),
								  position.width, EditorGUIUtility.singleLineHeight);

			EditorGUI.PropertyField (valueARect, valueAProperty, GUIContent.none);
			EditorGUI.PropertyField (outputRect, outputProperty, GUIContent.none);

			serializedObject.ApplyModifiedProperties ();
		}
#endif
	}
}


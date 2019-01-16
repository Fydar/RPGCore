using System;
using UnityEngine;
using RPGCore.Behaviour.Connections;

#if UNITY_EDITOR
using UnityEditor;
using RPGCore.Behaviour.Editor;
#endif

namespace RPGCore.Behaviour.Logic
{
	[NodeInformation ("Logic/Or")]
	public class OrNode : BehaviourNode
	{
		public BoolInput ValueA = new BoolInput ();
		public BoolInput ValueB = new BoolInput ();
		public BoolOutput Output = new BoolOutput ();

		protected override void OnSetup (IBehaviourContext context)
		{
			ConnectionEntry<bool> valueAInput = ValueA[context];
			ConnectionEntry<bool> valueBInput = ValueB[context];
			ConnectionEntry<bool> output = Output[context];

			Action updateHandler = () =>
			{
				output.Value = valueAInput.Value | valueBInput.Value;
			};

			valueAInput.OnAfterChanged += updateHandler;
			valueBInput.OnAfterChanged += updateHandler;

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
			SerializedProperty valueBProperty = serializedObject.FindProperty ("ValueB");
			SerializedProperty outputProperty = serializedObject.FindProperty ("Output");

			GUI.DrawTexture (position, BehaviourGraphResources.Instance.AndNodeGraphic, ScaleMode.ScaleToFit, true);

			Rect valueARect = new Rect (position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
			Rect valueBRect = new Rect (position.x, valueARect.yMax + EditorGUIUtility.standardVerticalSpacing,
				position.width, valueARect.height);
			Rect outputRect = new Rect (position.x, position.y + (EditorGUIUtility.singleLineHeight * 0.5f),
								  position.width, EditorGUIUtility.singleLineHeight);

			EditorGUI.PropertyField (valueARect, valueAProperty, GUIContent.none);
			EditorGUI.PropertyField (valueBRect, valueBProperty, GUIContent.none);
			EditorGUI.PropertyField (outputRect, outputProperty, GUIContent.none);

			serializedObject.ApplyModifiedProperties ();
		}
#endif
	}
}

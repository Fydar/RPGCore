using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RPGCore
{
	[NodeInformation ("Logic/And")]
	public class AndNode : BehaviourNode
	{
		public BoolInput ValueA = new BoolInput ();
		public BoolInput ValueB = new BoolInput ();
		public BoolOutput Output = new BoolOutput ();

		protected override void OnSetup (IBehaviourContext character)
		{
			ConnectionEntry<bool> valueAInput = ValueA.GetEntry (character);
			ConnectionEntry<bool> valueBInput = ValueB.GetEntry (character);
			ConnectionEntry<bool> output = Output.GetEntry (character);

			Action updateHandler = () =>
			{
				output.Value = (valueAInput.Value == true) ? valueBInput.Value == true : false;
			};

			valueAInput.OnAfterChanged += updateHandler;
			valueBInput.OnAfterChanged += updateHandler;

			updateHandler ();
		}

		protected override void OnRemove (IBehaviourContext character)
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

			GUI.DrawTexture (position, Resources.Load<Texture2D> ("Symbol_AND"), ScaleMode.ScaleToFit, true);

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
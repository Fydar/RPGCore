using System;
using UnityEngine;
using RPGCore.Behaviour.Connections;

#if UNITY_EDITOR
using UnityEditor;
using RPGCore.Behaviour.Editor;
#endif

namespace RPGCore.Behaviour.Logic
{
	[NodeInformation("Logic/Not")]
	public class NotNode : BehaviourNode
	{
		public BoolInput ValueA = new BoolInput();
		public BoolOutput Output = new BoolOutput();

		protected override void OnSetup(IBehaviourContext context)
		{
			var valueAInput = ValueA[context];
			var output = Output[context];

			Action updateHandler = () =>
			{
				output.Value = !valueAInput.Value;
			};

			valueAInput.OnAfterChanged += updateHandler;

			updateHandler();
		}

		protected override void OnRemove(IBehaviourContext context)
		{
		}

#if UNITY_EDITOR
		public override Vector2 GetDiamentions()
		{
			return new Vector2(70, 38);
		}

		public override void DrawGUI(SerializedObject serializedObject, Rect position)
		{
			var valueAProperty = serializedObject.FindProperty("ValueA");
			var outputProperty = serializedObject.FindProperty("Output");

			GUI.DrawTexture(position, BehaviourGraphResources.Instance.NotNodeGraphic, ScaleMode.ScaleToFit, true);

			var valueARect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
			var outputRect = new Rect(position.x, position.y + (EditorGUIUtility.singleLineHeight * 0.5f),
								  position.width, EditorGUIUtility.singleLineHeight);

			EditorGUI.PropertyField(valueARect, valueAProperty, GUIContent.none);
			EditorGUI.PropertyField(outputRect, outputProperty, GUIContent.none);

			serializedObject.ApplyModifiedProperties();
		}
#endif
	}
}


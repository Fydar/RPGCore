using RPGCore.Behaviour;
using RPGCore.Behaviour.Editor;
using RPGCore.DataEditor;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPGCoreUnity.Editors
{
	[Serializable]
	public class BehaviourEditorView
	{
		public enum ControlMode
		{
			None,
			NodeDragging,
			ViewDragging,
			CreatingConnection
		}

		[SerializeField] private ControlMode currentMode;
		[SerializeField] private Vector2 panPosition;
		[SerializeField] private HashSet<string> selection;
		[SerializeField] private LocalPropertyId connectionOutput;
		[SerializeField] private bool isOutputSocket;

		public LocalPropertyId ConnectionOutput => connectionOutput;

		public EditorField ConnectionInput { get; set; }
		public string ConnectionInputNodeId { get; set; }
		public EditorSession Session { get; private set; }
		public EditorObject EditorObject { get; private set; }

		public bool IsOutputSocket => isOutputSocket;

		public ControlMode CurrentMode
		{
			get
			{
				return currentMode;
			}
			set
			{
				currentMode = value;
			}
		}

		public string DescribeCurrentAction
		{
			get
			{
				switch (currentMode)
				{
					case ControlMode.NodeDragging:

						return Selection.Count == 1
							? $"Dragging nodes {string.Join(", ", Selection)}"
							: $"Dragging node {string.Join(", ", Selection)}";

					case ControlMode.ViewDragging:
						return $"Panning to {panPosition}";

					case ControlMode.CreatingConnection:
						return IsOutputSocket
							? $"Creating connection from {connectionOutput}"
							: $"Creating connection to {connectionOutput}";

					default:
						return "None";
				}
			}
		}

		public HashSet<string> Selection
		{
			get
			{
				if (selection == null)
				{
					selection = new HashSet<string>();
				}
				return selection;
			}
		}

		public Vector2 PanPosition
		{
			get
			{
				return panPosition;
			}
			set
			{
				panPosition = value;
			}
		}

		public void BeginSession(EditorSession session, EditorObject editorObject)
		{
			Session = session;
			EditorObject = editorObject;
		}

		public void BeginConnectionFromOutput(LocalPropertyId connectionStart)
		{
			currentMode = ControlMode.CreatingConnection;
			connectionOutput = connectionStart;
			isOutputSocket = true;
		}

		public void BeginConnectionFromInput(EditorField connectionEnd, string connectionInputNodeId)
		{
			ConnectionInput = connectionEnd;
			ConnectionInputNodeId = connectionInputNodeId;

			currentMode = ControlMode.CreatingConnection;
			isOutputSocket = false;
		}
	}
}

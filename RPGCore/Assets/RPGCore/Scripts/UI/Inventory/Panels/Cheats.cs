using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

namespace RPGCore.Inventories
{
	public class Cheats : MonoBehaviour
	{
		public RPGCharacter Character;

		[Space]

		public GameObject ButtonPrefab;
		public RectTransform ParentHolder;

		[Space]
		public ItemGenerator[] CheatItems;

		private void Start ()
		{
			for (int i = 0; i < CheatItems.Length; i++)
			{
				ItemGenerator generator = CheatItems[i];

				GameObject clone = Instantiate (ButtonPrefab, Vector3.zero, Quaternion.identity, ParentHolder) as GameObject;
				clone.transform.localScale = Vector3.one;
				clone.transform.localRotation = Quaternion.identity;
				clone.GetComponent<RectTransform> ().anchoredPosition3D = Vector3.zero;

				int cheatIndex = i;

				Button button = clone.GetComponent<Button> ();
				button.onClick.AddListener (new UnityAction (() =>
				{
					CheatItem (cheatIndex);
				}));

				ItemRenderer slotRenderer = clone.GetComponent<ItemRenderer> ();

				slotRenderer.RenderGenerator (generator);
			}
		}

		public void CheatItem (int index)
		{
			if (index >= CheatItems.Length)
			{
				throw new System.ArgumentOutOfRangeException ("index", "The index provided is out of range.");
			}

			ItemGenerator generator = CheatItems[index];

			ItemSurrogate item = generator.Generate ();

			Character.inventory.Add (item);
		}
	}

#if UNITY_EDITOR
	[CustomEditor (typeof (Cheats))]
	public class CheatsEditor : Editor
	{
		private ReorderableList list;

		private void OnEnable ()
		{
			Cheats cheats = (Cheats)target;

			list = new ReorderableList (serializedObject, serializedObject.FindProperty ("CheatItems"));

			list.drawHeaderCallback += (rect) =>
			{
				EditorGUI.LabelField (rect, "Cheat Items", EditorStyles.boldLabel);
			};

			list.drawElementCallback += (Rect rect, int index, bool isActive, bool isFocused) =>
			{
				var element = list.serializedProperty.GetArrayElementAtIndex (index);
				rect.y += 2;
				EditorGUI.PropertyField (
					new Rect (rect.x, rect.y, rect.width, rect.height),
					element, GUIContent.none);
			};

			list.elementHeightCallback += (index) =>
			{
				var element = list.serializedProperty.GetArrayElementAtIndex (index);

				return EditorGUI.GetPropertyHeight (element) + 4;
			};
		}

		public override void OnInspectorGUI ()
		{
			DrawDefaultInspector (); ;

			serializedObject.Update ();
			list.DoLayoutList ();
			serializedObject.ApplyModifiedProperties ();
		}
	}
#endif
}


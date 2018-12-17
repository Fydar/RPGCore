using System;
using System.Collections.Generic;

using UnityEngine;
using RPGCore.Inventories;
using RPGCore.Tables;
using RPGCore.Stats;
using RPGCore.Quests;
using RPGCore.World;
using RPGCore.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RPGCore
{
	public class RPGCharacter : MonoBehaviour
	{
		public static List<RPGCharacter> All = new List<RPGCharacter> ();

		public event Action<RPGCharacter> OnHit;
		public event Action OnDeath;

		public Loottable Loot;

		[Header ("Inventory")]
		public int startingSlots;
		public ItemGenerator[] startingLoot;

		[Header ("Stats")]
		public StateInstanceCollection States;
		public StatInstanceCollection Stats;

		[Header ("Buffs")]
		public BuffCollection Buffs;

		[Header ("Quests")]
		public List<Quest> Quests;

		[NonSerialized]
		public Inventory inventory;

		[NonSerialized]
		public SlottedInventory equipment;

		[NonSerialized]
		public int teamID = 1;

		private void Awake ()
		{
			All.Add (this);
			Buffs = new BuffCollection (this);

			Stats.SetupReferences ();
			States.SetupReferences ();

			List<ItemSlot> slots = new List<ItemSlot> ();

			foreach (EquipmentInformation info in EquipmentInformationDatabase.Instance.EquipmentInfos)
			{
				slots.Add (info.GenerateSlot (this));
			}

			equipment = new SlottedInventory (this, slots.ToArray ());

			inventory = new SlottedInventory (this, startingSlots,
				new ItemSlotBehaviour[] { });

			if (startingLoot != null)
			{
				for (int i = 0; i < startingLoot.Length; i++)
				{
					var generatedItem = startingLoot[i].Generate ();
					Debug.Log(generatedItem);
					inventory.Add (generatedItem);
				}
			}
		}

		private void Start ()
		{
			States.CurrentHealth.Value = Stats.MaxHealth.Value;
			States.CurrentMana.Value = Stats.MaxMana.Value;

			Chat.Instance.Log (name + " joined the game.");
		}

		private void FixedUpdate ()
		{
			float deltaTime = Time.fixedDeltaTime;

			Buffs.Update (deltaTime);

			if (States.HealthDelayRemaining.Value >= 0.0f)
			{
				States.HealthDelayRemaining.Value -= deltaTime;
			}
			else
			{
				if (States.CurrentHealth.Value < Stats.MaxHealth.Value)
				{
					Heal (Stats.HealthRegeneration.Value * deltaTime);
				}
			}

			if (States.ManaDelayRemaining.Value >= 0.0f)
			{
				States.ManaDelayRemaining.Value -= deltaTime;
			}
			else
			{
				RestoreMana (Stats.ManaRegeneration.Value * deltaTime);
			}
		}

		public bool HasEnoughMana (float amount)
		{
			return States.CurrentMana.Value - amount > -Stats.ManaBurnoutDebt.Value;
		}

		public void TakeDamage (int damage)
		{
			Chat.Instance.Log (name + " taking " + damage + " damage.");
			States.CurrentHealth.Value -= damage;

			States.HealthDelayRemaining.Value = Mathf.Max (States.HealthDelayRemaining.Value, Stats.HealthRegenerationDelay.Value);

			if (States.CurrentHealth.Value <= 0)
			{
				Kill ();
			}
		}

		public void Heal (float health)
		{
			if (health >= 1)
			{
				Chat.Instance.Log (name + " healing " + health.ToString ("0") + " health.");
			}

			States.CurrentHealth.Value = Mathf.Min (
				States.CurrentHealth.Value + health, Stats.MaxHealth.Value);
		}

		public void DrainMana (float amount)
		{
			States.CurrentMana.Value -= amount;

			States.ManaDelayRemaining.Value = Mathf.Max (States.ManaDelayRemaining.Value, Stats.ManaRegenerationDelay.Value);

			if (States.CurrentMana.Value <= 0.0f)
				Burnout ();
		}

		public void RestoreMana (float mana)
		{
			States.CurrentMana.Value = Mathf.Min (
				States.CurrentMana.Value + mana, Stats.MaxMana.Value);
		}

		public void Kill ()
		{
			Chat.Instance.Log (name + " died.");

			if (OnDeath != null)
				OnDeath ();

			States.CurrentMana.Value = 0;

			gameObject.SetActive (false);

			foreach (ItemSurrogate loot in Loot.Select ())
			{
				if (loot != null)
					ItemDropper.DropItem (transform.position, loot);
			}
		}

		public void Burnout ()
		{
			States.CurrentMana.Value = 0.0f;
			States.ManaDelayRemaining.Value = Mathf.Max (States.ManaDelayRemaining.Value, Stats.ManaBurnoutDelay.Value);
		}
	}

#if UNITY_EDITOR
	[CustomEditor (typeof (RPGCharacter), true)]
	public class RPGCharacterDrawer : Editor
	{
		private void OnEnable ()
		{
			RPGCharacter character = (RPGCharacter)target;
			character.Stats.SetupReferences ();
			character.States.SetupReferences ();
		}

		public override void OnInspectorGUI ()
		{
			DrawDefaultInspector ();

			RPGCharacter character = (RPGCharacter)target;

			if (character.Buffs != null && character.Buffs.Count != 0)
			{
				foreach (Buff buff in character.Buffs)
				{
					if (buff == null)
						continue;

					EditorGUI.indentLevel++;
					EditorGUILayout.LabelField (buff.StackSize.Value.ToString () + "   " + buff.buffTemplate.name, EditorStyles.boldLabel);

					Rect iconRect = GUILayoutUtility.GetLastRect ();
					iconRect = EditorGUI.IndentedRect (iconRect);
					iconRect.xMax = iconRect.xMin + iconRect.height;
					iconRect.x -= iconRect.height;

					DrawCustomIcon (iconRect, buff.buffTemplate.Icon);

					EditorGUI.indentLevel++;
					foreach (BuffClock clock in buff.Clocks)
					{
						Rect rect = GUILayoutUtility.GetRect (0, 16);

						rect = EditorGUI.IndentedRect (rect);

						if (typeof (BuffClockDecaying).IsAssignableFrom (clock.GetType ()))
						{
							BuffClockDecaying decayingClock = (BuffClockDecaying)clock;

							EditorGUI.ProgressBar (rect, decayingClock.TimeRemaining / decayingClock.Duration, decayingClock.StackSize.Value.ToString ());
						}
						else if (typeof (BuffClockFixed).IsAssignableFrom (clock.GetType ()))
						{
							BuffClockFixed fixedClock = (BuffClockFixed)clock;

							EditorGUI.ProgressBar (rect, 1.0f, fixedClock.StackSize.Value.ToString ());
						}
					}

					EditorGUI.indentLevel--;
					EditorGUI.indentLevel--;
				}

				GUILayout.Space (20);
			}
		}

		private void Update ()
		{
			RPGCharacter character = (RPGCharacter)target;

			if (character.Buffs != null && character.Buffs.Count != 0)
				Repaint ();
		}

		private static void DrawCustomIcon (Rect rect, Sprite sprite)
		{
			Texture2D textureIcon;
			Rect textureRect;

			textureIcon = sprite.texture;
			textureRect = sprite.rect;

			DrawCustomIcon (rect, textureIcon, textureRect);
		}

		private static void DrawCustomIcon (Rect rect, Texture texture)
		{
			Rect textureRect;

			textureRect = new Rect (0.0f, 0.0f, texture.width, texture.height);

			DrawCustomIcon (rect, texture, textureRect);
		}

		private static void DrawCustomIcon (Rect rect, Texture texture, Rect textureRect)
		{
			Rect normalisedTextureRect = RemapRect (textureRect, texture.width, texture.height);

			GUI.DrawTextureWithTexCoords (rect, texture, normalisedTextureRect, true);
		}

		private static Rect RemapRect (Rect rect, float width, float height)
		{
			Rect remapedRect = new Rect (rect);

			remapedRect.x /= width;
			remapedRect.width /= width;

			remapedRect.y /= height;
			remapedRect.height /= height;

			return remapedRect;
		}
	}
#endif
}


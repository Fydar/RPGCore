using UnityEngine;
using System.Collections.Generic;
using RPGCore.Audio;
using RPGCore.Tables;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RPGCore
{
	[CreateAssetMenu (menuName = "RPGCore/Item")]
	public class ProceduralItemTemplate : ItemTemplate
	{
		[Header ("Render")]
		[SerializeField] private GameObject renderPrefab;
		[SerializeField] private float renderScale = 1.0f;

		[Header ("General")]
		[SerializeField] private string itemName = "New Item";

		[TextArea (3, 7)]
		[SerializeField] private string description = "New Item Description";

		[Space]

		[UnityEngine.Serialization.FormerlySerializedAs ("StartDrag")]
		[SerializeField] private SfxGroup startDrag;
		[UnityEngine.Serialization.FormerlySerializedAs ("EndDrag")]
		[SerializeField] private SfxGroup endDrag;

		[Space]

		[SerializeField] private EnchantmentSelector prefix;
		[SerializeField] private EnchantmentSelector suffix;

		[SerializeField] private int maxStack = 1;

		[Space]

		[SerializeField] private ItemRarity rarity;


		[HideInInspector] [SerializeField] private Sprite icon;
		[HideInInspector] [SerializeField] private int weight = 1;

		public override SfxGroup StartDrag
		{
			get
			{
				return startDrag;
			}
		}

		public override SfxGroup EndDrag
		{
			get
			{
				return endDrag;
			}
		}

		public override ItemRarity Rarity
		{
			get
			{
				return rarity;
			}
		}

		public override GameObject RenderPrefab
		{
			get
			{
				return renderPrefab;
			}
		}

		public override float RenderScale
		{
			get
			{
				return renderScale;
			}
		}

#if UNITY_EDITOR
#if ASSET_ICONS
		[AssetIcon]
#endif
		public Sprite DrawBackground ()
		{
			if (rarity == null)
				return null;
			return rarity.SlotImage;
		}
#if ASSET_ICONS
		[AssetIcon ("Camera Direction: Default; IsOrthographic: true; Width: 85%; Height: 85%")]
#endif
		public GameObject DrawIcon ()
		{
			return renderPrefab;
		}
#endif

		public override ItemSurrogate GenerateItem (ItemData data)
		{
			ItemSurrogate newItem = new ItemSurrogate ();
			newItem.data = data;
			newItem.template = this;

			if (prefix != null)
			{
				EnchantmentTemplate template = prefix.GetEnchantment ();

				if (template != null)
				{
					newItem.Prefix = new Enchantment (template);
				}
			}

			if (suffix != null)
			{
				EnchantmentTemplate template = suffix.GetEnchantment ();

				if (template != null)
				{
					newItem.Suffix = new Enchantment (template);
				}
			}


			SetupGraph (newItem);

			ItemInputNode node = GetNode<ItemInputNode> ();

			newItem.owner.onChanged += () =>
			{
				if (node == null)
					return;

				node.Owner[newItem].Value = newItem.owner.Value;
			};

			node.StackSize[newItem].Value = newItem.Quantity;
			newItem.data.quantity.onChanged += () =>
			{
				node.StackSize[newItem].Value = newItem.Quantity;
			};

			return newItem;
		}

		protected override string GetItemBaseName ()
		{
			return itemName;
		}

		protected override string GetItemDescription ()
		{
			return description;
		}

		protected override int GetItemWeight ()
		{
			return weight;
		}

		protected override int GetItemMaxStack ()
		{
			return maxStack;
		}

		protected override IEnumerable<float[]> GetItemPositiveOverrides ()
		{
			yield return null;
		}

		protected override IEnumerable<float[]> GetItemNegativeOverrides ()
		{
			yield return null;
		}

		protected override Sprite GetItemIcon ()
		{
			return icon;
		}
	}

#if UNITY_EDITOR
	[CustomEditor (typeof (ProceduralItemTemplate))]
	[CanEditMultipleObjects]
	public class ProceduralItemTemplateEditor : Editor
	{
		public override void OnInspectorGUI ()
		{
			ProceduralItemTemplate itemTemplate = (ProceduralItemTemplate)target;

			/*GUILayout.Space (10);
			Rect rect = GUILayoutUtility.GetRect (0, 175);
			GUILayout.Space (10);
			*/

			DrawDefaultInspector ();

			serializedObject.ApplyModifiedProperties ();
		}
	}
#endif
}
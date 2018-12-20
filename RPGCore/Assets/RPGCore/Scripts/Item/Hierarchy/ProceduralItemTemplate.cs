using UnityEngine;
using System.Collections.Generic;
using RPGCore.Audio;
using RPGCore.Behaviour;

#if UNITY_EDITOR
using UnityEditor;
using RPGCore.Utility.Editors;
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

		[SerializeField] private SfxGroup startDrag;
		[SerializeField] private SfxGroup endDrag;

		[Space]

		[SerializeField] private EnchantmentSelector prefix;
		[SerializeField] private EnchantmentSelector suffix;

		[SerializeField] private int maxStack = 1;

		[Space]

		[SerializeField] private ItemRarity rarity;

		[HideInInspector] [SerializeField] private Sprite icon;
		[HideInInspector] [SerializeField] private int weight = 1;

		[UnityEngine.Serialization.FormerlySerializedAs ("Nodes")]
		[SerializeField] private List<BehaviourNode> nodes;

		public override List<BehaviourNode> AllNodes
		{
			get
			{
				return nodes;
			}
			set
			{
				AllNodes = value;
			}
		}

		public override string BaseName
		{
			get
			{
				return itemName;
			}
		}

		public override string Description
		{
			get
			{
				return description;
			}
		}

		public override Sprite Icon
		{
			get
			{
				return icon;
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

		public override ItemRarity Rarity
		{
			get
			{
				return rarity;
			}
		}

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

		public override int Weight
		{
			get
			{
				return weight;
			}
		}

		public override int StackSize
		{
			get
			{
				return maxStack;
			}
		}

		public override IEnumerable<float[]> PositiveOverrides
		{
			get
			{
				return null;
			}
		}

		public override IEnumerable<float[]> NegativeOverrides
		{
			get
			{
				return null;
			}
		}

		public override ItemSurrogate GenerateItem (ItemData data)
		{
			ItemSurrogate newItem = new ItemSurrogate
			{
				data = data,
				template = this
			};

			this.SetupGraph (newItem);

			IInputNode<ItemSurrogate>[] itemNodes = this.GetNodes<IInputNode<ItemSurrogate>> ();
			foreach (var itemNode in itemNodes)
			{
				itemNode.SetTarget (newItem, newItem);
			}

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

			return newItem;
		}
	}

#if UNITY_EDITOR && ASSET_ICONS
		[AssetIcon]
		Sprite DrawBackground ()
		{
			if (rarity == null)
				return null;
			return rarity.SlotImage;
		}
		[AssetIcon ("Camera Direction: Default; IsOrthographic: true; Width: 85%; Height: 85%")]
		GameObject DrawIcon ()
		{
			return renderPrefab;
		}
#endif

#if UNITY_EDITOR && !ASSET_ICONS
	[CustomEditor (typeof (ProceduralItemTemplate))]
	[CanEditMultipleObjects]
	class ItemTemplateEditor : Editor
	{
		public override Texture2D RenderStaticPreview (string assetPath, Object[] subAssets, int width, int height)
		{
			if (((ItemTemplate)target).RenderPrefab == null)
				return null;

			RuntimePreviewGenerator.OrthographicMode = true;
			RuntimePreviewGenerator.BackgroundColor = new Color32 (19, 23, 26, 255);
			RuntimePreviewGenerator.Padding = 0.1f;

			return RuntimePreviewGenerator.GenerateModel (((ItemTemplate)target).RenderPrefab.transform, width, height);
		}
	}
#endif
}

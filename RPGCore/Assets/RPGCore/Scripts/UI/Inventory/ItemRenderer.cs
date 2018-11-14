using RPGCore.Tooltips;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace RPGCore.Inventories
{
	public class ItemRenderer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
	{
		public bool DisplayTooltip = true;

		[SerializeField] private Color normalColour = Color.white;
		[SerializeField] private Color fadedColour = Color.grey;

		[Header ("Setup")]
		[SerializeField] private Image slotImage;
		[SerializeField] private Image slotDecoration;
		[SerializeField] private Image iconImage;
		[SerializeField] private Image equippedIcon;
		[SerializeField] private Image selectedIcon;
		[SerializeField] private Image newIcon;
		[SerializeField] private Text nameText;
		[Space]
		[SerializeField] private Text quantityText;
		[SerializeField] private bool HideQuantityOnOne = true;

		private bool hovered = false;
		private bool faded = false;


		private ItemSlot _lastSlot = null;
		private ItemSurrogate lastItem = null;
		private ItemTemplate lastTemplate = null;

		//3d Render Stuff
		public int id = 0;
		public int Height = 0;
		private GameObject currentObject;
		private RectTransform rectTransform;
		private int lastWidth = -1;
		private int lastHeight = -1;

		private Sprite defaultSlotSprite;

		public bool Hovered
		{
			get
			{
				return hovered;
			}
		}

		public bool Faded
		{
			get
			{
				return faded;
			}
			set
			{
				faded = value;

				if (faded)
				{
					iconImage.color = fadedColour;

					if (slotImage != null)
						slotImage.color = fadedColour;
				}
				else
				{
					iconImage.color = normalColour;

					if (slotImage != null)
						slotImage.color = normalColour;
				}
			}
		}

		private ItemSlot lastSlot
		{
			get
			{
				return _lastSlot;
			}
			set
			{
				if (_lastSlot != null && _lastSlot.Item != null)
				{
					_lastSlot.Item.OnReferenceChanged -= OnReferenceChangedCallback;
				}

				_lastSlot = value;

				if (_lastSlot != null)
					lastItem = _lastSlot.Item;
				else
					lastItem = null;

				if (lastItem != null)
					lastTemplate = lastItem.Template;
				else
					lastTemplate = null;

				if (_lastSlot != null && _lastSlot.Item != null)
				{
					_lastSlot.Item.OnReferenceChanged += OnReferenceChangedCallback;
				}

				OnReferenceChangedCallback ();
			}
		}

		public void RenderGenerator (ItemGenerator generator)
		{
			if (generator == null)
			{
				RenderEmpty ();
				return;
			}

			RenderTemplate (generator.RewardTemplate);

			if (quantityText != null)
			{
				quantityText.gameObject.SetActive (true);
				if (generator.MinCount == generator.MaxCount)
				{
					if (generator.MinCount != 1)
						quantityText.text = generator.MinCount.ToString ();
					else
						quantityText.gameObject.SetActive (false);
				}
				else
				{
					quantityText.text = generator.MinCount.ToString () + "-" + generator.MaxCount.ToString ();
				}
			}

			Faded = false;
		}

		public void RenderSlot (ItemSlot slot)
		{
			if (slot == null)
			{
				RenderEmpty ();
				return;
			}

			Slot_Pair pairSlot = slot.GetSlotBehaviour<Slot_Pair> ();
			if (pairSlot != null && pairSlot.pair.Item != null)
			{
				if (pairSlot.pair.Item.EquiptableSlot == Slot.TwoHanded)
				{
					RenderSlot (pairSlot.pair);

					Faded = true;

					return;
				}
			}

			lastSlot = slot;

			RenderItem (slot.Item);

			if (slot.Item == null || slot.Item.template == null)
			{
				if (slotDecoration != null && slot.SlotDecoration != null)
				{
					slotDecoration.gameObject.SetActive (true);
					slotDecoration.sprite = slot.SlotDecoration;
				}
			}

			Faded = slot.IsDefault && slot.Item != null;
		}

		public void RenderItem (ItemSurrogate item)
		{
			if (item == null)
			{
				RenderEmpty ();
				return;
			}

			lastItem = item;
			lastTemplate = item.template;


			if (rectTransform == null)
				rectTransform = GetComponent<RectTransform> ();

			if (Hovered && DisplayTooltip)
				TooltipManager.instance.ItemTooltip (rectTransform, lastSlot);

			RenderTemplate (item.Template);

			if (quantityText != null)
			{
				if (HideQuantityOnOne && item.Quantity == 1)
				{
					quantityText.gameObject.SetActive (false);
				}
				else
				{
					quantityText.gameObject.SetActive (true);
					quantityText.text = item.Quantity.ToString ();
				}
			}

			if (nameText != null)
			{
				nameText.gameObject.SetActive (true);
				nameText.text = item.BaseName;
			}

			Faded = true;
		}

		public void RenderTemplate (ItemTemplate template)
		{
			if (template == null)
			{
				RenderEmpty ();
				return;
			}
			lastTemplate = template;

			iconImage.gameObject.SetActive (true);
			iconImage.sprite = template.Icon;

			if (template.RenderPrefab != null)
				RenderPrefab (template.RenderPrefab);

			if (nameText != null)
			{
				nameText.gameObject.SetActive (true);
				nameText.text = template.BaseName;
			}

			if (slotImage != null)
				slotImage.sprite = template.Rarity.SlotImage;

			if (slotDecoration != null)
				slotDecoration.gameObject.SetActive (false);
		}

		public void RenderEmpty ()
		{
			if (slotImage != null)
				slotImage.sprite = defaultSlotSprite;

			iconImage.gameObject.SetActive (false);

			if (currentObject != null)
				Destroy (currentObject);

			if (quantityText != null)
				quantityText.gameObject.SetActive (false);

			if (nameText != null)
				nameText.gameObject.SetActive (false);

			if (slotDecoration != null)
				slotDecoration.gameObject.SetActive (false);

			Faded = false;

			if (Hovered)
				TooltipManager.instance.Hide ();

			lastSlot = null;
			lastItem = null;
			lastTemplate = null;
		}

		public void Unhover ()
		{
			if (!Hovered)
				return;

			hovered = false;

			if (selectedIcon != null)
				selectedIcon.gameObject.SetActive (false);

			if (lastSlot == null)
				return;

			//if (TooltipManager.instance.CurrentItem == lastSlot.Item)
			TooltipManager.instance.Hide ();
		}

		public void Hover ()
		{
			if (Hovered)
				return;

			hovered = true;

			if (selectedIcon != null)
				selectedIcon.gameObject.SetActive (true);

			if (lastSlot == null)
				return;

			if (DisplayTooltip)
				TooltipManager.instance.ItemTooltip (rectTransform, lastSlot);
		}

		public void ShowEquipped ()
		{
			if (equippedIcon != null)
				equippedIcon.gameObject.SetActive (true);
		}

		public void HideEquipped ()
		{
			if (equippedIcon != null)
				equippedIcon.gameObject.SetActive (false);
		}

		private void Awake ()
		{
			if (slotImage != null)
				defaultSlotSprite = slotImage.sprite;

			if (slotDecoration != null)
				slotDecoration.gameObject.SetActive (false);
		}

		private void Update ()
		{
			if (transform.hasChanged || lastWidth != Screen.width || lastHeight != Screen.height)
			{
				UpdatePosition ();

				lastWidth = Screen.width;
				lastHeight = Screen.height;

				transform.hasChanged = false;
			}
		}

		private void OnEnable ()
		{
			Unhover ();

			if (lastItem != null)
			{
				if (currentObject != null)
					return;

				RenderPrefab (lastItem.Template.RenderPrefab);
			}
			else
			{
				UpdatePosition ();
			}
		}

		private void OnDisable ()
		{
			Unhover ();

			if (currentObject != null)
				Destroy (currentObject);

			if (lastSlot == null)
				return;

			//if (TooltipManager.instance.CurrentItem == lastSlot.Item)
			TooltipManager.instance.Hide ();
		}

		void IPointerEnterHandler.OnPointerEnter (PointerEventData eventData)
		{
			Hover ();
		}

		void IPointerExitHandler.OnPointerExit (PointerEventData eventData)
		{
			Unhover ();
		}

		private void RenderPrefab (GameObject prefab)
		{
			if (currentObject != null)
				Destroy (currentObject);

			ItemRendererLayer renderLayer = ItemRendererLayer.Get (id);

			currentObject = Instantiate (prefab, renderLayer.objectHolder.transform) as GameObject;
			currentObject.layer = renderLayer.gameObject.layer;

			MeshRenderer renderer = currentObject.GetComponent<MeshRenderer> ();
			renderer.receiveShadows = false;
			renderer.shadowCastingMode = ShadowCastingMode.Off;
			renderer.lightProbeUsage = LightProbeUsage.Off;
			renderer.allowOcclusionWhenDynamic = false;

			currentObject.transform.position += new Vector3 (3000, 4000, 4000);

			UpdatePosition ();
		}

		private void UpdatePosition ()
		{
			if (currentObject == null)
				return;

			if (rectTransform == null)
				rectTransform = GetComponent<RectTransform> ();

			if (lastTemplate == null)
				return;

			ItemRendererLayer renderLayer = ItemRendererLayer.Get (id);
			MeshFilter meshFilter = currentObject.GetComponent<MeshFilter> ();

			Vector3 rectTransformCenter = rectTransform.position;

			Vector2 point = new Vector2 (rectTransformCenter.x, rectTransformCenter.y);

			Rect screenRect = RectTransformUtility.PixelAdjustRect (rectTransform, null);

			Ray ray = renderLayer.layerCamera.ScreenPointToRay (point);

			float maxSize = Mathf.Max (meshFilter.mesh.bounds.size.x, meshFilter.mesh.bounds.size.y,
				meshFilter.mesh.bounds.size.z) / lastTemplate.RenderScale;

			float heightPerc = ((screenRect.height / Screen.height) * ((float)Screen.height / 2560.0f)) *
				renderLayer.layerCamera.orthographicSize * 3.0f;

			float newScale = heightPerc / maxSize;

			currentObject.transform.position = ray.GetPoint (20 - (Height * 2));
			currentObject.transform.position -= meshFilter.mesh.bounds.center * newScale;
			currentObject.transform.localScale = new Vector3 (newScale, newScale, newScale);
		}

		private void OnReferenceChangedCallback ()
		{
			if (lastSlot == null || lastSlot.Item == null)
			{
				if (equippedIcon != null)
					equippedIcon.gameObject.SetActive (false);
				return;
			}

			//if (equippedIcon != null)
			//	equippedIcon.gameObject.SetActive (lastSlot.Item.IsEquipted);
		}
	}
}
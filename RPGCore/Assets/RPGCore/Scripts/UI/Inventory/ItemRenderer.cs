using RPGCore.Tooltips;
using RPGCore.Utility;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace RPGCore.Inventories
{
	public class ItemRenderer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
	{
		public bool DisplayTooltip = true;

		[SerializeField] private AnimationCurve bounceCurve = new AnimationCurve (new Keyframe[] { new Keyframe (0.0f, 1.0f), new Keyframe (1.0f, 0.0f) });

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
		[SerializeField] private int id = 0;
		[SerializeField] private int Height = 0;
		private GameObject currentObject;
		private RectTransform rectTransform;
		private int lastWidth = -1;
		private int lastHeight = -1;
		private Coroutine lastBounceRoutine;

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

		private ItemSlot LastSlot
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

			LastSlot = slot;

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
				TooltipManager.instance.ItemTooltip (rectTransform, LastSlot);

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

			PlayBounce ();
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

			LastSlot = null;
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

			if (LastSlot == null)
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

			if (LastSlot == null)
				return;

			if (DisplayTooltip)
				TooltipManager.instance.ItemTooltip (rectTransform, LastSlot);
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

			if (LastSlot == null)
				return;

			//if (TooltipManager.instance.CurrentItem == lastSlot.Item)
			TooltipManager.instance.Hide ();
		}

		public void OnPointerEnter (PointerEventData eventData)
		{
			Hover ();
		}

		public void OnPointerExit (PointerEventData eventData)
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

			MeshRenderer itemMesh = currentObject.GetComponent<MeshRenderer> ();
			itemMesh.receiveShadows = false;
			itemMesh.shadowCastingMode = ShadowCastingMode.Off;
			itemMesh.lightProbeUsage = LightProbeUsage.Off;
			itemMesh.allowOcclusionWhenDynamic = false;

			currentObject.transform.position += new Vector3 (3000, 4000, 4000);

			UpdatePosition ();
		}

		private void UpdatePosition (float scaleMultiplier = 1.0f)
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

			float newScale = (heightPerc / maxSize) * scaleMultiplier;

			currentObject.transform.position = ray.GetPoint (20 - (Height * 2));
			currentObject.transform.position -= meshFilter.mesh.bounds.center * newScale;
			currentObject.transform.localScale = new Vector3 (newScale, newScale, newScale);
		}

		private void OnReferenceChangedCallback ()
		{
			if (LastSlot == null || LastSlot.Item == null)
			{
				if (equippedIcon != null)
					equippedIcon.gameObject.SetActive (false);
				return;
			}

			//if (equippedIcon != null)
			//	equippedIcon.gameObject.SetActive (lastSlot.Item.IsEquipted);
		}

		private void PlayBounce ()
		{
			if (isActiveAndEnabled)
			{
				if (lastBounceRoutine != null)
					StopCoroutine (lastBounceRoutine);

				lastBounceRoutine = StartCoroutine (BounceCoroutine ());
			}
		}

		private IEnumerator BounceCoroutine ()
		{
			TimedLoop loop = new TimedLoop (0.25f);

			foreach (float time in loop)
			{
				if (currentObject == null)
					yield break;

				float scale = 1.0f + bounceCurve.Evaluate (time);
				UpdatePosition (scale);
				yield return null;
			}
		}
	}
}

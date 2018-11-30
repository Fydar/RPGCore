using RPGCore.Audio;
using RPGCore.Inventories;
using RPGCore.Stats;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPGCore.Tooltips
{
	public class TooltipManager : MonoBehaviour
	{
		public static TooltipManager instance;

		public TooltipPositioner DefaultPositioning;
		public float FadeSpeed = 2.0f;

		[Space]
		public CanvasMouse mouse;
		public RawImage fadeRenderLayer;
		public CanvasGroup fader;
		public RectTransform holder;

		private RectTransform currentTransform;
		private TooltipElement[] elements;
		private object renderingTarget;

		private IInterpolator fade;

		private void Awake ()
		{
			instance = this;

			fade = new LinearInterpolator (FadeSpeed);
		}

		private void Start ()
		{
			elements = GetChildElements (holder);
		}

		private void Update ()
		{
			bool display = renderingTarget != null &&
				!Input.GetMouseButton (0) &&
				ItemSlotDragAndDrop.instance.dragState != ItemSlotDragAndDrop.DragState.Dragging;

			fade.TargetValue = display ? 1.0f : 0.0f;
			fade.Update (Time.deltaTime);
			fader.alpha = fade.Value;
			fadeRenderLayer.color = new Color (1.0f, 1.0f, 1.0f, fade.Value);

			Vector2 normalised = new Vector2 (Input.mousePosition.x / Screen.width,
											 Input.mousePosition.y / Screen.height);

			Vector2 newPivot = DefaultPositioning.pivot;
			Vector2 modifiedAnchor = DefaultPositioning.anchorPivot;
			if (normalised.x < DefaultPositioning.flipPoint.x)
			{
				newPivot = new Vector2 (1.0f - newPivot.x, newPivot.y);
				modifiedAnchor = new Vector2 (1.0f - modifiedAnchor.x, modifiedAnchor.y);
			}
			if (normalised.y < DefaultPositioning.flipPoint.y)
			{
				newPivot = new Vector2 (newPivot.x, 1.0f - newPivot.y);
				modifiedAnchor = new Vector2 (modifiedAnchor.x, 1.0f - modifiedAnchor.y);
			}

			//holder.position = mouse.ScreenToCanvas (Input.mousePosition);
			if (currentTransform != null)
			{
				holder.pivot = newPivot;
				holder.pivot = newPivot;

				Vector3[] corners = new Vector3[4];
				currentTransform.GetWorldCorners (corners);
				Vector3 topLeft = corners[0];
				Vector3 bottomRight = corners[3];

				holder.position = new Vector3 (
					Mathf.Lerp (topLeft.x, bottomRight.x, modifiedAnchor.x),
					Mathf.Lerp (topLeft.y, bottomRight.y, modifiedAnchor.y), 0);
			}
		}

		public void BuffTooltip (RectTransform target, Buff buff)
		{
			currentTransform = target;
			// fade.Value = 0.0f;

			RenderTarget (buff);
		}

		public void StatTooltip (RectTransform target, StatInstance stat)
		{
			currentTransform = target;
			// fade.Value = 0.0f;

			RenderTarget (stat);
		}

		public void ItemTooltip (RectTransform target, ItemSurrogate item)
		{
			currentTransform = target;

			RenderTarget (item);
		}

		public void ItemTooltip (RectTransform target, ItemSlot slot)
		{
			currentTransform = target;

			RenderTarget (slot, slot.Item);
		}

		public void RenderTarget<T> (T target)
		{
			renderingTarget = target;

			for (int i = 0; i < elements.Length; i++)
			{
				TooltipElement element = elements[i];

				if (typeof (ITooltipTarget<T>).IsAssignableFrom (element.GetType ()))
				{
					ITooltipTarget<T> tooltipTarget = (ITooltipTarget<T>)element;

					element.gameObject.SetActive (true);
					tooltipTarget.Render (target);
				}
				else
				{
					element.gameObject.SetActive (false);
				}
			}
		}

		public void RenderTarget<A, B> (A target, B fallback)
		{
			renderingTarget = target;

			for (int i = 0; i < elements.Length; i++)
			{
				TooltipElement element = elements[i];

				if (typeof (ITooltipTarget<A>).IsAssignableFrom (element.GetType ()))
				{
					ITooltipTarget<A> tooltipTarget = (ITooltipTarget<A>)element;

					tooltipTarget.Render (target);
					element.gameObject.SetActive (true);
				}
				else if (typeof (ITooltipTarget<B>).IsAssignableFrom (element.GetType ()))
				{
					ITooltipTarget<B> tooltipTarget = (ITooltipTarget<B>)element;

					tooltipTarget.Render (fallback);
					element.gameObject.SetActive (true);
				}
				else
				{
					element.gameObject.SetActive (false);
				}
			}
		}

		public void Hide ()
		{
			currentTransform = null;
			renderingTarget = null;

			// ResetElements ();
		}

		private void ResetElements ()
		{
			for (int i = 0; i < elements.Length; i++)
			{
				TooltipElement element = elements[i];

				element.gameObject.SetActive (false);
			}
		}

		private static TooltipElement[] GetChildElements (Transform target)
		{
			List<TooltipElement> tooltipElements = new List<TooltipElement> ();

			for (int i = 0; i < target.childCount; i++)
			{
				TooltipElement element = target.GetChild (i).GetComponent<TooltipElement> ();

				if (element != null)
				{
					tooltipElements.Add (element);
				}
			}

			return tooltipElements.ToArray ();
		}
	}
}


﻿using RPGCore.Audio;
using RPGCore.Inventories;
using RPGCore.Stats;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPGCore.Tooltips
{
	public class TooltipManager : MonoBehaviour
	{
		public enum PositingMode
		{
			Mouse,
			Target
		}

		public static TooltipManager instance;

		public PositingMode Mode;

		[Space]
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

		private void Awake()
		{
			instance = this;

			fade = new LinearInterpolator(FadeSpeed);
		}

		private void Start()
		{
			elements = GetChildElements(holder);
		}

		private void Update()
		{
			bool display = renderingTarget != null &&
				!Input.GetMouseButton(0) &&
				ItemSlotDragAndDrop.instance.dragState != ItemSlotDragAndDrop.DragState.Dragging;

			fade.TargetValue = display ? 1.0f : 0.0f;
			fade.Update(Time.deltaTime);
			fader.alpha = fade.Value;
			fadeRenderLayer.color = new Color(1.0f, 1.0f, 1.0f, fade.Value);

			var normalised = new Vector2(Input.mousePosition.x / Screen.width,
											 Input.mousePosition.y / Screen.height);

			var newPivot = DefaultPositioning.pivot;
			var modifiedAnchor = DefaultPositioning.anchorPivot;
			if (normalised.x < DefaultPositioning.flipPoint.x)
			{
				newPivot = new Vector2(1.0f - newPivot.x, newPivot.y);
				modifiedAnchor = new Vector2(1.0f - modifiedAnchor.x, modifiedAnchor.y);
			}
			if (normalised.y < DefaultPositioning.flipPoint.y)
			{
				newPivot = new Vector2(newPivot.x, 1.0f - newPivot.y);
				modifiedAnchor = new Vector2(modifiedAnchor.x, 1.0f - modifiedAnchor.y);
			}

			if (Mode == PositingMode.Mouse)
			{
				holder.pivot = newPivot;
				holder.position = mouse.ScreenToCanvas(Input.mousePosition);
			}
			else if (Mode == PositingMode.Target)
			{
				if (currentTransform != null)
				{
					holder.pivot = newPivot;

					var corners = new Vector3[4];
					currentTransform.GetWorldCorners(corners);
					var topLeft = corners[0];
					var bottomRight = corners[3];

					holder.position = new Vector3(
						Mathf.Lerp(topLeft.x, bottomRight.x, modifiedAnchor.x),
						Mathf.Lerp(topLeft.y, bottomRight.y, modifiedAnchor.y), 0);
				}
			}
		}

		public void BuffTooltip(RectTransform target, Buff buff)
		{
			currentTransform = target;
			// fade.Value = 0.0f;

			RenderTarget(buff);
		}

		public void StatTooltip(RectTransform target, StatInstance stat)
		{
			currentTransform = target;
			// fade.Value = 0.0f;

			RenderTarget(stat);
		}

		public void ItemTooltip(RectTransform target, ItemSurrogate item)
		{
			currentTransform = target;

			RenderTarget(item);
		}

		public void ItemTooltip(RectTransform target, ItemSlot slot)
		{
			currentTransform = target;

			RenderTarget(slot, slot.Item);
		}

		public void RenderTarget<T>(T target)
		{
			renderingTarget = target;

			for (int i = 0; i < elements.Length; i++)
			{
				var element = elements[i];

				if (typeof(ITooltipTarget<T>).IsAssignableFrom(element.GetType()))
				{
					var tooltipTarget = (ITooltipTarget<T>)element;

					element.gameObject.SetActive(true);
					tooltipTarget.Render(target);
				}
				else
				{
					element.gameObject.SetActive(false);
				}
			}
		}

		public void RenderTarget<A, B>(A target, B fallback)
		{
			renderingTarget = target;

			for (int i = 0; i < elements.Length; i++)
			{
				var element = elements[i];

				if (typeof(ITooltipTarget<A>).IsAssignableFrom(element.GetType()))
				{
					var tooltipTarget = (ITooltipTarget<A>)element;

					tooltipTarget.Render(target);
					element.gameObject.SetActive(true);
				}
				else if (typeof(ITooltipTarget<B>).IsAssignableFrom(element.GetType()))
				{
					var tooltipTarget = (ITooltipTarget<B>)element;

					tooltipTarget.Render(fallback);
					element.gameObject.SetActive(true);
				}
				else
				{
					element.gameObject.SetActive(false);
				}
			}
		}

		public void Hide()
		{
			currentTransform = null;
			renderingTarget = null;

			// ResetElements ();
		}

		private void ResetElements()
		{
			for (int i = 0; i < elements.Length; i++)
			{
				var element = elements[i];

				element.gameObject.SetActive(false);
			}
		}

		private static TooltipElement[] GetChildElements(Transform target)
		{
			var tooltipElements = new List<TooltipElement>();

			for (int i = 0; i < target.childCount; i++)
			{
				var element = target.GetChild(i).GetComponent<TooltipElement>();

				if (element != null)
				{
					tooltipElements.Add(element);
				}
			}

			return tooltipElements.ToArray();
		}
	}
}


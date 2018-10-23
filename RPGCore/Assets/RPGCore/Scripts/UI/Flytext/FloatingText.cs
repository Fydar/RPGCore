using UnityEngine;
using UnityEngine.UI;

namespace RPGCore.UI
{
	public class FloatingText : MonoBehaviour
	{
		[Header ("Start")]
		public Vector2 MinOffset = new Vector2 (-35.0f, -10.0f);
		public Vector2 MaxOffset = new Vector2 (35.0f, 10.0f);

		[Space]

		public Vector2 MinVelocity = new Vector2 (-1.0f, 1.5f);
		public Vector2 MaxVelocity = new Vector2 (1.0f, 3.0f);

		public float GravityScale = 9.0f;
		public float Lifetime = 0.6f;
		public float Drag = 0.05f;

		[Header ("Display")]
		public Color HealColor = new Color (0.35f, 0.75f, 0.35f);
		public Color DamageColor = new Color (0.75f, 0.35f, 0.35f);
		public Color BuffColor = new Color (1.0f, 0.75f, 0.35f);

		[SerializeField]
		private Text damageText;

		private Vector2 Velocity = Vector2.zero;
		private RectTransform rectTransform;
		private Transform target;
		private Vector2 offset;

		private void Awake ()
		{
			rectTransform = GetComponent<RectTransform> ();
		}

		private void Start ()
		{
			Velocity = new Vector2 (Random.Range (MinVelocity.x, MaxVelocity.x), Random.Range (MinVelocity.y, MaxVelocity.y));

			Destroy (gameObject, Lifetime);
		}

		private void Update ()
		{
			offset += Velocity;
			Velocity = Vector2.Lerp (Velocity, Vector2.zero, Drag * Time.unscaledDeltaTime);
			Velocity -= new Vector2 (0, GravityScale * Time.unscaledDeltaTime);

			Vector2 screenPosition = Camera.main.WorldToScreenPoint (target.position);
			rectTransform.position = screenPosition;

			rectTransform.anchoredPosition += offset;
		}

		public void SetText (Transform _target, int healthChange)
		{
			target = _target;

			if (healthChange < 0)
			{
				damageText.color = HealColor;
			}
			else
			{
				damageText.color = DamageColor;
			}

			healthChange = Mathf.Abs (healthChange);
			damageText.text = healthChange.ToString ();

			offset = new Vector2 (Random.Range (MinOffset.x, MaxOffset.x), Random.Range (MinOffset.y, MaxOffset.y));

			Vector2 screenPosition = Camera.main.WorldToScreenPoint (target.position);
			rectTransform.position = screenPosition;

			rectTransform.anchoredPosition += offset;
		}

		public void SetText (Transform _target, string text)
		{
			target = _target;
			damageText.text = text;

			damageText.color = BuffColor;

			offset = new Vector2 (Random.Range (MinOffset.x, MaxOffset.x), Random.Range (MinOffset.y, MaxOffset.y));

			Vector2 screenPosition = Camera.main.WorldToScreenPoint (target.position);
			rectTransform.position = screenPosition;

			rectTransform.anchoredPosition += offset;
		}
	}
}
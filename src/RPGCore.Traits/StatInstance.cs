using RPGCore.Behaviour;
using System;
using System.Collections.Generic;

namespace RPGCore.Traits
{
	public class StatInstance : IReadOnlyEventField<float>, IEventFieldHandler
	{
		public StatIdentifier Identifier { get; set; }

		public StatTemplate Template { get; internal set; }

		public EventFieldHandlerCollection Handlers { get; set; }

		public List<StatModifier> BaseAdditiveModifiers;
		public List<StatModifier> BaseSimpleModifiers;
		public List<StatModifier> BaseCompoundModifiers;

		public List<StatModifier> AdditiveModifiers;
		public List<StatModifier> SimpleModifiers;
		public List<StatModifier> CompoundModifiers;

		public float Value { get; private set; }

		public StatInstance()
		{
			Handlers = new EventFieldHandlerCollection (this);
		}

		public override string ToString()
		{
			return $"{nameof (StatInstance)}({Identifier.ToString ()})";
		}

		private float CalculateValue()
		{
			float currentValue = 0.0f;
			if (BaseAdditiveModifiers != null)
			{
				foreach (var modifier in BaseAdditiveModifiers)
				{
					currentValue += modifier.CurrentValue.Value;
				}
			}
			if (BaseSimpleModifiers != null)
			{
				foreach (var modifier in BaseSimpleModifiers)
				{
					currentValue += modifier.CurrentValue.Value;
				}
			}
			if (BaseCompoundModifiers != null)
			{
				foreach (var modifier in BaseCompoundModifiers)
				{
					currentValue += modifier.CurrentValue.Value;
				}
			}

			if (AdditiveModifiers != null)
			{
				foreach (var modifier in AdditiveModifiers)
				{
					currentValue += modifier.CurrentValue.Value;
				}
			}
			if (SimpleModifiers != null)
			{
				foreach (var modifier in SimpleModifiers)
				{
					currentValue += modifier.CurrentValue.Value;
				}
			}
			if (CompoundModifiers != null)
			{
				foreach (var modifier in CompoundModifiers)
				{
					currentValue += modifier.CurrentValue.Value;
				}
			}
			return currentValue;
		}

		public void AddModifier(StatModifier modifier)
		{
			Handlers.InvokeBeforeChanged ();

			switch (modifier.Phase)
			{
				case StatModificationPhase.Base:
					switch (modifier.Type)
					{
						case StatModificationType.Additive:
							if (BaseAdditiveModifiers == null)
							{
								BaseAdditiveModifiers = new List<StatModifier> ();
							}
							BaseAdditiveModifiers.Add (modifier);
							break;
						case StatModificationType.SimpleMultiplicative:
							if (BaseSimpleModifiers == null)
							{
								BaseSimpleModifiers = new List<StatModifier> ();
							}
							BaseSimpleModifiers.Add (modifier);
							break;

						case StatModificationType.CompoundMultiplicative:
							if (BaseCompoundModifiers == null)
							{
								BaseCompoundModifiers = new List<StatModifier> ();
							}
							BaseCompoundModifiers.Add (modifier);
							break;

						case StatModificationType.None:
							throw new InvalidOperationException ($"\"{modifier.Type}\" is not a valid stat modification type.");
					}
					break;

				case StatModificationPhase.Typical:
					switch (modifier.Type)
					{
						case StatModificationType.Additive:
							if (AdditiveModifiers == null)
							{
								AdditiveModifiers = new List<StatModifier> ();
							}
							AdditiveModifiers.Add (modifier);
							break;
						case StatModificationType.SimpleMultiplicative:
							if (SimpleModifiers == null)
							{
								SimpleModifiers = new List<StatModifier> ();
							}
							SimpleModifiers.Add (modifier);
							break;

						case StatModificationType.CompoundMultiplicative:
							if (CompoundModifiers == null)
							{
								CompoundModifiers = new List<StatModifier> ();
							}
							CompoundModifiers.Add (modifier);
							break;

						case StatModificationType.None:
							throw new InvalidOperationException ($"\"{modifier.Type}\" is not a valid stat modification type.");
					}
					break;

				case StatModificationPhase.None:
					throw new InvalidOperationException ($"\"{modifier.Phase}\" is not a valid stat modification phase.");
			}
			Handlers.InvokeAfterChanged ();

			modifier.CurrentValue.Handlers[this].Add (this);

			Value = CalculateValue ();
		}

		public bool RemoveModifier(StatModifier modifier)
		{
			Handlers.InvokeBeforeChanged ();

			bool result;
			switch (modifier.Phase)
			{
				case StatModificationPhase.Base:
					switch (modifier.Type)
					{
						case StatModificationType.Additive:
							if (BaseAdditiveModifiers == null)
							{
								BaseAdditiveModifiers = new List<StatModifier> ();
							}
							result = BaseAdditiveModifiers.Remove (modifier);
							break;
						case StatModificationType.SimpleMultiplicative:
							if (BaseSimpleModifiers == null)
							{
								BaseSimpleModifiers = new List<StatModifier> ();
							}
							result = BaseSimpleModifiers.Remove (modifier);
							break;

						case StatModificationType.CompoundMultiplicative:
							if (BaseCompoundModifiers == null)
							{
								BaseCompoundModifiers = new List<StatModifier> ();
							}
							result = BaseCompoundModifiers.Remove (modifier);
							break;

						default:
						case StatModificationType.None:
							throw new InvalidOperationException ($"\"{modifier.Type}\" is not a valid stat modification type.");
					}
					break;

				case StatModificationPhase.Typical:
					switch (modifier.Type)
					{
						case StatModificationType.Additive:
							if (AdditiveModifiers == null)
							{
								AdditiveModifiers = new List<StatModifier> ();
							}
							result = AdditiveModifiers.Remove (modifier);
							break;
						case StatModificationType.SimpleMultiplicative:
							if (SimpleModifiers == null)
							{
								SimpleModifiers = new List<StatModifier> ();
							}
							result = SimpleModifiers.Remove (modifier);
							break;

						case StatModificationType.CompoundMultiplicative:
							if (CompoundModifiers == null)
							{
								CompoundModifiers = new List<StatModifier> ();
							}
							result = CompoundModifiers.Remove (modifier);
							break;

						default:
						case StatModificationType.None:
							throw new InvalidOperationException ($"\"{modifier.Type}\" is not a valid stat modification type.");
					}
					break;

				default:
				case StatModificationPhase.None:
					throw new InvalidOperationException ($"\"{modifier.Phase}\" is not a valid stat modification phase.");
			}

			Handlers.InvokeAfterChanged ();

			Value = CalculateValue ();

			return result;
		}

		public void OnBeforeChanged()
		{
		}

		public void OnAfterChanged()
		{
			Value = CalculateValue ();
		}
	}
}

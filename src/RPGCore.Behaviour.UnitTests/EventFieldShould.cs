using NUnit.Framework;
using System.Collections.Generic;

namespace RPGCore.Behaviour.UnitTests
{
	public class EventFieldShould
	{
		public class TestPlayer
		{
			public EventField<TestWeapon> Mainhand;
		}
		public class TestWeapon
		{
			public EventField<int> Damage;
		}

		public class AddToListEventHandler<T> : IEventFieldHandler
		{
			private readonly EventField<T> source;
			private readonly List<T> target;

			public AddToListEventHandler (EventField<T> source, List<T> target)
			{
				this.source = source;
				this.target = target;
			}

			public void Dispose ()
			{

			}

			public void OnAfterChanged ()
			{
				target.Add (source.Value);
			}

			public void OnBeforeChanged ()
			{

			}
		}

		[SetUp]
		public void Setup ()
		{
		}

		[Test]
		public void FireEvents ()
		{
			var target = new EventField<TestPlayer> ();

			var mainhand = target.Watch ((e) => e?.Mainhand);
			var mainhandDamage = mainhand.Watch ((e) => e?.Damage);

			var damages = new List<int> ();

			mainhandDamage.Handlers[this] += new AddToListEventHandler<int> (mainhandDamage, damages);

			target.Value = new TestPlayer ()
			{
				Mainhand = new EventField<TestWeapon> ()
				{
					Value = new TestWeapon ()
					{
						Damage = new EventField<int> ()
						{
							Value = 10
						}
					}
				}
			};

			target.Value = new TestPlayer ()
			{
				Mainhand = new EventField<TestWeapon> ()
				{
					Value = new TestWeapon ()
					{
						Damage = new EventField<int> ()
						{
							Value = 20
						}
					}
				}
			};

			target.Value.Mainhand.Value = null;

			target.Value.Mainhand.Value = new TestWeapon ()
			{
				Damage = new EventField<int> ()
				{
					Value = 25
				}
			};

			target.Value.Mainhand.Value.Damage.Value = 15;

			mainhand.Dispose ();
			mainhandDamage.Dispose ();

			target.Value.Mainhand.Value.Damage.Value = 5;

			int[] expectedValues = { 10, 20, 0, 25, 15 };
			Assert.AreEqual (expectedValues.Length, damages.Count);
			for (int i = 0; i < damages.Count; i++)
			{
				int damage = damages[i];
				Assert.AreEqual (expectedValues[i], damage);
			}
		}
	}
}

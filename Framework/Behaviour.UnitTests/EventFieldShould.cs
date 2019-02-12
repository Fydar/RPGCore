using NUnit.Framework;
using Behaviour;
using System;
using System.Collections.Generic;

namespace Behaviour.UnitTests
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
			private EventField<T> source;
			private List<T> target;

			public AddToListEventHandler(EventField<T> source, List<T> target)
			{
				this.source = source;
				this.target = target;
			}

            public void Dispose()
            {
                
            }

            public void OnAfterChanged()
            {
				target.Add(source.Value);
            }

            public void OnBeforeChanged()
            {
                
            }
        }

        [SetUp]
		public void Setup ()
		{
		}

		[Test]
		public void Test1 ()
		{
            var target = new EventField<TestPlayer>();

            var mainhand = target.Watch((e) => e?.Mainhand);
            var mainhandDamage = mainhand.Watch((e) => e?.Damage);

			mainhandDamage.OnAfterChanged += () => { Console.WriteLine($"Tracking Mainhand Damage: {mainhandDamage.Value}"); };

			target.Value = new TestPlayer()
            {
                Mainhand = new EventField<TestWeapon>()
                {
                    Value = new TestWeapon()
                    {
                        Damage = new EventField<int>()
                        {
                            Value = 10
                        }
                    }
                }
            };

            mainhand.Dispose();
            mainhandDamage.Dispose();

            target.Value = new TestPlayer()
            {
                Mainhand = new EventField<TestWeapon>()
                {
                    Value = new TestWeapon()
                    {
                        Damage = new EventField<int>()
                        {
                            Value = 20
                        }
                    }
                }
            };

			target.Value.Mainhand.Value = new TestWeapon()
			{
				Damage = new EventField<int>()
				{
					Value = 25
				}
			};

			target.Value.Mainhand.Value.Damage.Value = 15;

			target.Value.Mainhand.Value = null;
		}
    }
}
using System;

namespace Behaviour
{
    public static class BubbleTest
    {
        public static void Test()
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

    public class TestPlayer
    {
        public EventField<TestWeapon> Mainhand;
    }
    public class TestWeapon
    {
        public EventField<int> Damage;
    }
}
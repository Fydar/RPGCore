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

            Console.WriteLine(mainhandDamage.Value);

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
            
            Console.WriteLine(mainhandDamage.Value);

            target.Value.Mainhand.Value = new TestWeapon()
            {
                Damage = new EventField<int>()
                {
                    Value = 25
                }
            };
            
            Console.WriteLine(mainhandDamage.Value);

            target.Value = null;
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
using System;
using System.Numerics;

namespace RpgCombat.Iteration05
{
    public class Prop : ITargetable
    {
        public Prop(double health)
        {
            if (health < 0)
            {
                throw new InvalidOperationException("Props cannot have negative health");
            }
            
            Health = health;
        }

        public double Health { get; private set; }
        public Vector2 Position { get; init; } = Vector2.Zero;
        public bool IsDestroyed => Health == 0;

        public void ReceiveDamage(double amount)
        {
            Health = Math.Max(0, Health - amount);
        }

        public void ReceiveHealing(double amount) => throw new InvalidOperationException("Props cannot be healed");
    }
}
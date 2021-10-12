using System;
using System.Numerics;

namespace RpgCombat
{
    public enum PropStatus
    {
        Normal,
        Destroyed,
    }

    public class Prop : ITarget
    {
        public Prop(double health, Vector2 position = default)
        {
            Health = Math.Max(0, health);
            Position = position;
        }

        public double Health { get; private set; }
        public Vector2 Position { get; }

        public PropStatus Status => Health switch
        {
            <= 0 => PropStatus.Destroyed,
            _ => PropStatus.Normal,
        };

        void ITarget.ReceiveDamage(Damage damage)
        {
            if (Status == PropStatus.Destroyed)
            {
                throw new InvalidOperationException("Cannot damage destroyed prop");
            }

            Health -= Math.Min(Health, damage.Amount);
        }
    }
}
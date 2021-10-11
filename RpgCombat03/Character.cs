using System;
using System.Numerics;

namespace RpgCombat03
{
    public class Character
    {
        private const int MaxHealth = 1000;

        public CharacterStatus Status { get; set; } = CharacterStatus.Alive;
        public CharacterType Type { get; set; } = CharacterType.Melee;
        public double Health { get; set; } = MaxHealth;
        public int Level { get; set; } = 1;
        public Vector2 Position { get; set; } = Vector2.Zero;

        public float Range => Type switch
        {
            CharacterType.Melee => 2,
            CharacterType.Ranged => 20,
            _ => throw new ArgumentOutOfRangeException(),
        };

        public void Damage(Character other, double amount)
        {
            if (other == this)
            {
                throw new InvalidTargetException("Characters cannot damage themselves");
            }

            if ((other.Position - Position).Length() > Range)
            {
                throw new TargetOutOfRangeException();
            }

            if (other.Level >= Level + 5)
            {
                amount *= 0.5;
            }
            else if (other.Level + 5 <= Level)
            {
                amount *= 1.5;
            }

            other.Health = Math.Max(0.0, other.Health - amount);

            if (other.Health == 0)
            {
                other.Status = CharacterStatus.Dead;
            }
        }

        public void Heal(Character other, int amount)
        {
            if (other != this)
            {
                throw new InvalidTargetException("Cannot heal other characters");
            }

            if (Status == CharacterStatus.Dead)
            {
                throw new InvalidOperationException("Cannot heal dead characters");
            }

            Health = Math.Min(MaxHealth, Health + amount);
        }
    }

    public enum CharacterStatus
    {
        Alive,
        Dead,
    }

    public enum CharacterType
    {
        Melee,
        Ranged
    }
}
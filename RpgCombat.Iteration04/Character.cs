using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace RpgCombat.Iteration04
{
    public class Character
    {
        private const int MaxHealth = 1000;

        public CharacterStatus Status { get; set; } = CharacterStatus.Alive;
        public CharacterType Type { get; init; } = CharacterType.Melee;
        public double Health { get; set; } = MaxHealth;
        public int Level { get; init; } = 1;
        public Vector2 Position { get; init; } = Vector2.Zero;
        public IReadOnlyCollection<string> Factions { get; set; } = new List<string>();

        public float Range => Type switch
        {
            CharacterType.Melee => 2,
            CharacterType.Ranged => 20,
            _ => throw new ArgumentOutOfRangeException(),
        };

        public void Damage(Character other, double amount)
        {
            if (IsAlly(other))
            {
                throw new InvalidTargetException("Characters cannot damage allies");
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
            if (!IsAlly(other))
            {
                throw new InvalidTargetException("Cannot heal non-ally characters");
            }

            if (other.Status == CharacterStatus.Dead)
            {
                throw new InvalidOperationException("Cannot heal dead characters");
            }

            other.Health = Math.Min(MaxHealth, other.Health + amount);
        }

        public void JoinFaction(string faction)
        {
            Factions = Factions.Append(faction).Distinct().ToList();
        }

        public void LeaveFaction(string faction)
        {
            Factions = Factions.Where(f => f != faction).ToList();
        }

        private bool IsAlly(Character other)
        {
            return other == this || other.Factions.Intersect(Factions).Any();
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
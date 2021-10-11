using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace RpgCombat.Iteration05
{
    public class Character : ITargetable
    {
        private const int MaxHealth = 1000;
        private double _health = MaxHealth;
        private readonly int _level = 1;
        private readonly HashSet<Faction> _factions = new();

        public CharacterStatus Status => Health > 0 ? CharacterStatus.Alive : CharacterStatus.Dead;
        public CharacterType Type { get; init; } = CharacterType.Melee;
        public IReadOnlyCollection<Faction> Factions => _factions;

        public double Health
        {
            get => _health;
            init
            {
                _health = value switch
                {
                    < 0 => throw new InvalidOperationException("Characters cannot have negative health"),
                    > MaxHealth => throw new InvalidOperationException(
                        $"Characters cannot have more than {MaxHealth} health"),
                    _ => value
                };
            }
        }

        public int Level
        {
            get => _level;
            init
            {
                if (value < 1)
                {
                    throw new InvalidOperationException("Minimum character level is 1");
                }

                _level = value;
            }
        }

        public Vector2 Position { get; init; } = Vector2.Zero;

        public float Range => Type switch
        {
            CharacterType.Melee => 2,
            CharacterType.Ranged => 20,
            _ => throw new ArgumentOutOfRangeException(),
        };

        public void Damage(ITargetable target, double amount)
        {
            if (target == this)
            {
                throw new InvalidTargetException("Characters cannot damage themselves");
            }

            if (_factions.Any(f => f.Characters.Contains(target)))
            {
                throw new InvalidTargetException("Characters cannot damage allies");
            }

            if ((target.Position - Position).Length() > Range)
            {
                throw new TargetOutOfRangeException();
            }

            if (target is Character other)
            {
                if (other.Level >= Level + 5)
                {
                    amount *= 0.5;
                }
                else if (other.Level + 5 <= Level)
                {
                    amount *= 1.5;
                }
            }

            target.ReceiveDamage(amount);
        }

        public void Heal(ITargetable target, int amount)
        {
            if (target != this && !_factions.Any(f => f.Characters.Contains(target)))
            {
                throw new InvalidTargetException("Cannot heal characters except self and allies");
            }

            target.ReceiveHealing(amount);
        }

        public void ReceiveDamage(double amount)
        {
            _health = Math.Max(0, Health - amount);
        }

        public void ReceiveHealing(double amount)
        {
            if (Status == CharacterStatus.Dead)
            {
                throw new InvalidOperationException("Dead characters cannot be healed");
            }

            _health = Math.Min(MaxHealth, Health + amount);
        }

        public void JoinFaction(Faction faction)
        {
            faction.AddCharacter(this);
            _factions.Add(faction);
        }

        public void LeaveFaction(Faction faction)
        {
            faction.RemoveCharacter(this);
            _factions.Remove(faction);
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
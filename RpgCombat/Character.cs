using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace RpgCombat
{
    public enum CharacterType
    {
        Melee,
        Ranged,
    }

    public enum CharacterStatus
    {
        Alive,
        Dead,
    }

    public class Character : ITarget
    {
        private const double MaximumHealth = 1000;
        private const int MinimumLevel = 1;
        private const CharacterType DefaultCharacterType = CharacterType.Melee;

        private readonly HashSet<Faction> _factions = new();

        public Character() : this(DefaultCharacterType)
        {
        }

        public Character(CharacterType type)
        {
            Type = type;
            Health = MaximumHealth;
            Level = MinimumLevel;
            Position = Vector2.Zero;
        }

        public double Health { get; internal set; }
        public Vector2 Position { get; internal set; }
        public int Level { get; internal set; }
        public CharacterType Type { get; }
        public IReadOnlyCollection<Faction> Factions => _factions;

        public CharacterStatus Status => Health switch
        {
            <= 0 => CharacterStatus.Dead,
            _ => CharacterStatus.Alive
        };

        private CharacterBehavior Behavior => Status switch
        {
            CharacterStatus.Alive => new AliveBehavior(),
            CharacterStatus.Dead => new DeadBehavior(),
            _ => throw new ArgumentOutOfRangeException()
        };

        public void Damage(ITarget target, double amount) => Behavior.Damage(this, target, amount);
        public void Heal(Character target, double amount) => Behavior.Heal(this, target, amount);

        void ITarget.ReceiveDamage(Damage damage) => Behavior.ReceiveDamage(this, damage);
        private void ReceiveHealing(double amount) => Behavior.ReceiveHealing(this, amount);

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

        private bool IsInRange(IEntity entity)
        {
            var distance = this.DistanceTo(entity);

            return Type switch
            {
                CharacterType.Melee => distance <= 2,
                CharacterType.Ranged => distance <= 20,
                _ => throw new ArgumentOutOfRangeException(),
            };
        }

        private bool IsAlly(ITarget target)
        {
            return target is Character character && Factions.Any(f => f.Characters.Contains(character));
        }

        private abstract class CharacterBehavior
        {
            public abstract void Damage(Character character, ITarget target, double amount);
            public abstract void ReceiveDamage(Character character, Damage damage);
            public abstract void Heal(Character character, Character target, double amount);
            public abstract void ReceiveHealing(Character character, double amount);
        }

        private class AliveBehavior : CharacterBehavior
        {
            public override void Damage(Character character, ITarget target, double amount)
            {
                if (character == target || character.IsAlly(target))
                {
                    throw new InvalidOperationException("Invalid target");
                }

                if (!character.IsInRange(target))
                {
                    throw new InvalidOperationException("Target out of range");
                }

                target.ReceiveDamage(new Damage(amount, character.Level));
            }

            public override void ReceiveDamage(Character character, Damage damage)
            {
                var multiplier = (damage.AttackerLevel - character.Level) switch
                {
                    <= -5 => 0.5,
                    >= 5 => 1.5,
                    _ => 1,
                };

                var amount = damage.Amount * multiplier;
                var maximumDamage = character.Health;

                character.Health -= Math.Min(maximumDamage, amount);
            }

            public override void Heal(Character character, Character target, double amount)
            {
                if (character != target && !character.IsAlly(target))
                {
                    throw new InvalidOperationException("Invalid target");
                }

                if (!character.IsInRange(target))
                {
                    throw new InvalidOperationException("Target out of range");
                }

                target.ReceiveHealing(amount);
            }

            public override void ReceiveHealing(Character character, double amount)
            {
                var maximumHealingAmount = MaximumHealth - character.Health;

                character.Health += Math.Min(maximumHealingAmount, amount);
            }
        }

        private class DeadBehavior : CharacterBehavior
        {
            public override void Damage(Character character, ITarget target, double amount)
            {
                throw new InvalidOperationException("Character is dead");
            }

            public override void ReceiveDamage(Character character, Damage damage)
            {
                throw new InvalidOperationException("Character is dead");
            }

            public override void Heal(Character character, Character target, double amount)
            {
                throw new InvalidOperationException("Character is dead");
            }

            public override void ReceiveHealing(Character character, double amount)
            {
                throw new InvalidOperationException("Character is dead");
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace RpgCombat
{
    public enum CharacterClass
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
        private const CharacterClass DefaultClass = CharacterClass.Melee;

        private readonly HashSet<Faction> _factions = new();

        /// <summary>
        /// Create a new character with the default class.
        /// </summary>
        public Character() : this(DefaultClass)
        {
        }

        /// <summary>
        /// Create a new character with the given class.
        /// </summary>
        /// <param name="class">The class of the character</param>
        public Character(CharacterClass @class)
        {
            Class = @class;
            Health = MaximumHealth;
            Level = MinimumLevel;
            Position = Vector2.Zero;
        }

        /// <summary>
        /// The current health of the character.
        /// </summary>
        public double Health { get; internal set; }
        
        /// <summary>
        /// The current position of the character.
        /// </summary>
        public Vector2 Position { get; internal set; }
        
        /// <summary>
        /// The current level of the character.
        /// </summary>
        public int Level { get; internal set; }
        
        /// <summary>
        /// The current class of the character.
        /// </summary>
        public CharacterClass Class { get; }
        
        /// <summary>
        /// The factions this character belongs to.
        /// </summary>
        public IReadOnlyCollection<Faction> Factions => _factions;

        /// <summary>
        /// The current status of the character.
        /// </summary>
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

        /// <summary>
        /// Deal damage to a target.
        /// </summary>
        /// <param name="target">The target to damage</param>
        /// <param name="amount">The amount of damage to deal</param>
        public void Damage(ITarget target, double amount) => Behavior.Damage(this, target, amount);
        
        /// <summary>
        /// Heal a character.
        /// </summary>
        /// <param name="character">The character to heal</param>
        /// <param name="amount">The amount of health to heal</param>
        public void Heal(Character character, double amount) => Behavior.Heal(this, character, amount);

        void ITarget.ReceiveDamage(Damage damage) => Behavior.ReceiveDamage(this, damage);
        private void ReceiveHealing(double amount) => Behavior.ReceiveHealing(this, amount);

        /// <summary>
        /// Join a faction.
        /// </summary>
        /// <param name="faction">The faction to join</param>
        public void JoinFaction(Faction faction)
        {
            faction.AddCharacter(this);
            _factions.Add(faction);
        }

        /// <summary>
        /// Leave a faction.
        /// </summary>
        /// <param name="faction">The faction to leave</param>
        public void LeaveFaction(Faction faction)
        {
            faction.RemoveCharacter(this);
            _factions.Remove(faction);
        }

        private bool IsInRange(IEntity entity)
        {
            var distance = this.DistanceTo(entity);

            return Class switch
            {
                CharacterClass.Melee => distance <= 2,
                CharacterClass.Ranged => distance <= 20,
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
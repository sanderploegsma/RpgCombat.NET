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
        private CharacterState _currentState;

        public Character() : this(DefaultCharacterType)
        {
        }

        public Character(CharacterType type)
        {
            Type = type;
            TransitionToState(new AliveState());
        }

        internal Character(
            CharacterType type = DefaultCharacterType, 
            double health = MaximumHealth,
            int level = MinimumLevel, 
            Vector2 position = default, 
            params Faction[] factions)
        {
            Type = type;
            Health = Math.Min(health, MaximumHealth);
            Level = Math.Max(level, MinimumLevel);
            Position = position;

            foreach (var faction in factions)
            {
                JoinFaction(faction);
            }
            
            TransitionToState(new AliveState());
        }

        public double Health { get; private set; } = MaximumHealth;
        public Vector2 Position { get; private set; }
        public int Level { get; private set; } = MinimumLevel;
        public IReadOnlyCollection<Faction> Factions => _factions;
        public CharacterType Type { get; }
        public CharacterStatus Status { get; private set; }

        public void Damage(ITarget target, double amount) => _currentState.Damage(this, target, amount);
        public void Heal(Character target, double amount) => _currentState.Heal(this, target, amount);

        void ITarget.ReceiveDamage(Damage damage) => _currentState.ReceiveDamage(this, damage);
        private void ReceiveHealing(double amount) => _currentState.ReceiveHealing(this, amount);
        
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

        private void TransitionToState(CharacterState state)
        {
            _currentState = state;
            _currentState.ActivateState(this);
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

        private abstract class CharacterState
        {
            public abstract void ActivateState(Character character);

            public abstract void Damage(Character character, ITarget target, double amount);
            public abstract void ReceiveDamage(Character character, Damage damage);
            public abstract void Heal(Character character, Character target, double amount);
            public abstract void ReceiveHealing(Character character, double amount);
        }

        private class AliveState : CharacterState
        {
            public override void ActivateState(Character character)
            {
                if (character.Health > 0)
                {
                    character.Status = CharacterStatus.Alive;
                }
                else
                {
                    character.TransitionToState(new DeadState());
                }
            }

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

                character.Health -= damage.Amount * multiplier;

                if (character.Health <= 0)
                {
                    character.TransitionToState(new DeadState());
                }
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
                character.Health = Math.Min(MaximumHealth, character.Health + amount);
            }
        }

        private class DeadState : CharacterState
        {
            public override void ActivateState(Character character)
            {
                character.Status = CharacterStatus.Dead;
                character.Health = 0;
            }

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
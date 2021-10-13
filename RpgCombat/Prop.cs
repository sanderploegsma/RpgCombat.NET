using System;
using System.Numerics;

namespace RpgCombat
{
    public enum PropStatus
    {
        Normal,
        Destroyed,
    }

    /// <summary>
    /// A prop is a static entity in the game that has Health and can be damaged.
    /// If the prop's Health reaches 0 it is Destroyed.
    /// </summary>
    public class Prop : ITarget
    {
        /// <summary>
        /// Create a new prop with the given amount of health and an optional position.
        /// </summary>
        /// <param name="name">The name of the prop</param>
        /// <param name="health">The health of the prop (minimum: 0)</param>
        /// <param name="position">The position of the prop. Will be set to the default if not provided.</param>
        public Prop(string name, double health, Vector2 position = default)
        {
            Name = name;
            Health = Math.Max(0, health);
            Position = position;
        }
        
        /// <summary>
        /// The name of the prop.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The current health of the prop.
        /// </summary>
        public double Health { get; private set; }
        
        /// <summary>
        /// The position of the prop.
        /// </summary>
        public Vector2 Position { get; }

        /// <summary>
        /// The current status of the prop.
        /// </summary>
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

        public override string ToString() => $"{Name} (Health:{Health}, Position:{Position})";
    }
}
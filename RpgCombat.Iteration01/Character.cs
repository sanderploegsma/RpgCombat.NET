using System;

namespace RpgCombat.Iteration01
{
    public class Character
    {
        private const int MaxHealth = 1000;

        public CharacterStatus Status { get; set; } = CharacterStatus.Alive;
        public int Health { get; set; } = MaxHealth;
        public int Level { get; set; } = 1;

        public void Damage(Character other, int amount)
        {
            other.Health = Math.Max(0, other.Health - amount);

            if (other.Health == 0)
            {
                other.Status = CharacterStatus.Dead;
            }
        }

        public void Heal(Character other, int amount)
        {
            if (other.Status == CharacterStatus.Dead)
            {
                throw new InvalidOperationException("Cannot heal dead characters");
            }

            other.Health = Math.Min(MaxHealth, other.Health + amount);
        }
    }

    public enum CharacterStatus
    {
        Alive,
        Dead,
    }
}
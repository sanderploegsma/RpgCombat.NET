using System.Numerics;

namespace RpgCombat.Iteration05
{
    public interface ITargetable
    {
        public double Health { get; }
        public Vector2 Position { get; }

        public void ReceiveDamage(double amount);

        public void ReceiveHealing(double amount);
    }
}
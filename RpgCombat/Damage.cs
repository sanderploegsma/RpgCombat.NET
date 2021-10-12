namespace RpgCombat
{
    internal class Damage
    {
        public Damage(double amount, int attackerLevel)
        {
            Amount = amount;
            AttackerLevel = attackerLevel;
        }

        public double Amount { get; }
        public int AttackerLevel { get; }
    }
}
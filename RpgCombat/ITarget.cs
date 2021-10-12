namespace RpgCombat
{
    public interface ITarget : IEntity
    {
        public double Health { get; }

        internal void ReceiveDamage(Damage damage);
    }
}
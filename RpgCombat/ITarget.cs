namespace RpgCombat
{
    /// <summary>
    /// Any entity in the game that has Health can be damaged. 
    /// </summary>
    public interface ITarget : IEntity
    {
        /// <summary>
        /// The current health of the target.
        /// </summary>
        public double Health { get; }

        internal void ReceiveDamage(Damage damage);
    }
}
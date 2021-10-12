using System.Numerics;

namespace RpgCombat
{
    /// <summary>
    /// Any entity in the game.
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// The position of the entity within the game.
        /// </summary>
        public Vector2 Position { get; }
    }

    internal static class EntityExtensions
    {
        public static float DistanceTo(this IEntity self, IEntity other) => (self.Position - other.Position).Length();
    }
}
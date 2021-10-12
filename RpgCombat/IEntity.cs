using System.Numerics;

namespace RpgCombat
{
    public interface IEntity
    {
        public Vector2 Position { get; }
    }

    internal static class EntityExtensions
    {
        public static float DistanceTo(this IEntity self, IEntity other) => (self.Position - other.Position).Length();
    }
}
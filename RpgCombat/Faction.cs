using System.Collections.Generic;

namespace RpgCombat
{
    /// <summary>
    /// A faction is a group of characters. All characters in the same faction are considered allies.
    /// </summary>
    public class Faction
    {
        private readonly HashSet<Character> _characters = new();

        /// <summary>
        /// Create a new faction with the given name.
        /// </summary>
        /// <param name="name">The name of the faction</param>
        public Faction(string name)
        {
            Name = name;
        }

        /// <summary>
        /// The name of the faction.
        /// </summary>
        public string Name { get; }
        
        /// <summary>
        /// The characters that belong to this faction.
        /// </summary>
        public IReadOnlyCollection<Character> Characters => _characters;

        internal void AddCharacter(Character character)
        {
            _characters.Add(character);
        }

        internal void RemoveCharacter(Character character)
        {
            _characters.Remove(character);
        }

        public override string ToString() => Name;
    }
}
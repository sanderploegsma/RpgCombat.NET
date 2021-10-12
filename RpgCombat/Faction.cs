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
    }
}
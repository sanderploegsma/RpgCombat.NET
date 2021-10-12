using System.Collections.Generic;

namespace RpgCombat
{
    public class Faction
    {
        private readonly HashSet<Character> _characters = new();
        
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
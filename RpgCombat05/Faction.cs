using System.Collections.Generic;

namespace RpgCombat05
{
    public class Faction
    {
        private readonly HashSet<Character> _characters;

        public Faction()
        {
            _characters = new HashSet<Character>();
        }

        public IReadOnlyCollection<Character> Characters => _characters;

        public void AddCharacter(Character character)
        {
            _characters.Add(character);
        }

        public void RemoveCharacter(Character character)
        {
            _characters.Remove(character);
        }
    }
}
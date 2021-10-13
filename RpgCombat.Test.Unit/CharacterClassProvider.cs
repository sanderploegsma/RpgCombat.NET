using System;
using System.Collections;
using System.Linq;

namespace RpgCombat.Test.Unit
{
    public class CharacterClassProvider : IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            return Enum.GetValues<CharacterClass>()
                .Select(characterClass => new object[] { characterClass })
                .GetEnumerator();
        }
    }
}
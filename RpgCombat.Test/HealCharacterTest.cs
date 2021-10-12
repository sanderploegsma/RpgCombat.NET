using System;
using Xunit;

namespace RpgCombat.Test
{
    public class HealCharacterTest
    {
        [Fact]
        public void HealingIncreasesHealthOfCharacter()
        {
            var character = new Character { Health = 100 };

            character.Heal(character, 100);

            Assert.Equal(200, character.Health);
        }

        [Fact]
        public void CharacterHealthDoesNotExceed1000WhenHealed()
        {
            var character = new Character { Health = 999 };

            character.Heal(character, 100);

            Assert.Equal(1000, character.Health);
        }

        [Fact]
        public void CharactersCannotHealOthers()
        {
            var character = new Character();
            var target = new Character { Health = 999 };

            Assert.Throws<InvalidOperationException>(() => character.Heal(target, 100));
        }

        [Fact]
        public void CharactersCanHealAllies()
        {
            var faction = new Faction();
            var character = new Character();
            var ally = new Character { Health = 999 };

            character.JoinFaction(faction);
            ally.JoinFaction(faction);

            character.Heal(ally, 100);

            Assert.Equal(1000, ally.Health);
        }

        [Fact]
        public void CharactersCannotHealDeadAllies()
        {
            var faction = new Faction();
            var character = new Character();
            var ally = new Character { Health = 0 };

            character.JoinFaction(faction);
            ally.JoinFaction(faction);

            Assert.Throws<InvalidOperationException>(() => character.Heal(ally, 100));
        }
    }
}
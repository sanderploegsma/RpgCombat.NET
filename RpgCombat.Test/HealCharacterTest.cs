using System;
using Xunit;

namespace RpgCombat.Test
{
    public class HealCharacterTest
    {
        [Fact]
        public void HealingIncreasesHealthOfCharacter()
        {
            var character = new Character(health: 100);

            character.Heal(character, 100);
            
            Assert.Equal(200, character.Health);
        }

        [Fact]
        public void CharacterHealthDoesNotExceed1000WhenHealed()
        {
            var character = new Character(health: 999);

            character.Heal(character, 100);
            
            Assert.Equal(1000, character.Health);
        }
        
        [Fact]
        public void CharactersCannotHealOthers()
        {
            var source = new Character();
            var target = new Character(health: 999);

            Assert.Throws<InvalidOperationException>(() => source.Heal(target, 100));
        }
        
        [Fact]
        public void CharactersCanHealAllies()
        {
            var faction = new Faction();
            var source = new Character(factions: faction);
            var target = new Character(health: 999, factions: faction);

            source.Heal(target, 100);
            
            Assert.Equal(1000, target.Health);
        }
        
        [Fact]
        public void CharactersCannotHealDeadAllies()
        {
            var faction = new Faction();
            var source = new Character(factions: faction);
            var target = new Character(health: 0, factions: faction);

            Assert.Throws<InvalidOperationException>(() => source.Heal(target, 100));
        }
    }
}
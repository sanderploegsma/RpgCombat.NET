using System;
using System.Collections.Generic;
using Xunit;

namespace RpgCombat.Iteration04.Test
{
    public class HealCharacterTest
    {
        [Fact]
        public void DeadCharactersCannotBeHealed()
        {
            var character = new Character
            {
                Status = CharacterStatus.Dead
            };

            Assert.Throws<InvalidOperationException>(() => character.Heal(character, 100));
        }

        [Fact]
        public void HealingIncreasesHealthOfCharacter()
        {
            var character = new Character
            {
                Health = 100
            };

            character.Heal(character, 100);
            
            Assert.Equal(200, character.Health);
        }

        [Fact]
        public void CharacterHealthDoesNotExceed1000WhenHealed()
        {
            var character = new Character
            {
                Health = 999
            };

            character.Heal(character, 100);
            
            Assert.Equal(1000, character.Health);
        }
        
        [Fact]
        public void CharactersCannotHealOthers()
        {
            var source = new Character();
            var target = new Character
            {
                Health = 999
            };

            Assert.Throws<InvalidTargetException>(() => source.Heal(target, 100));
        }
        
        [Fact]
        public void CharactersCanHealAllies()
        {
            var source = new Character
            {
                Factions = new List<string> { "Red Bulls" }
            };
            
            var target = new Character
            {
                Factions = new List<string> { "Red Bulls" },
                Health = 999,
            };

            source.Heal(target, 100);
            
            Assert.Equal(1000, target.Health);
        }
    }
}
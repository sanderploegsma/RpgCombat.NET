using System;
using Xunit;

namespace RpgCombat.Iteration01.Test
{
    public class HealCharacterTest
    {
        [Fact]
        public void DeadCharactersCannotBeHealed()
        {
            var source = new Character();
            var target = new Character
            {
                Status = CharacterStatus.Dead
            };

            Assert.Throws<InvalidOperationException>(() => source.Heal(target, 100));
        }

        [Fact]
        public void HealingIncreasesHealthOfTargetCharacter()
        {
            var source = new Character();
            var target = new Character
            {
                Health = 100
            };

            source.Heal(target, 100);
            
            Assert.Equal(200, target.Health);
        }

        [Fact]
        public void TargetCharacterHealthDoesNotExceed1000WhenHealed()
        {
            var source = new Character();
            var target = new Character
            {
                Health = 999
            };

            source.Heal(target, 100);
            
            Assert.Equal(1000, target.Health);
        }
    }
}
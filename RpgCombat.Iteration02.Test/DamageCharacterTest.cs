using System;
using Xunit;

namespace RpgCombat.Iteration02.Test
{
    public class DamageCharacterTest
    {
        [Fact]
        public void DamageIsSubtractedFromHealthOfVictim()
        {
            var attacker = new Character();
            var victim = new Character();
            
            attacker.Damage(victim, 100);
            
            Assert.Equal(900, victim.Health);
        }
        
        [Fact]
        public void VictimRemainsAliveWhenDamageDoesNotExceedHealth()
        {
            var attacker = new Character();
            var victim = new Character();
            
            attacker.Damage(victim, 100);
            
            Assert.Equal(CharacterStatus.Alive, victim.Status);
        }

        [Fact]
        public void VictimDiesWhenDamageExceedsHealth()
        {
            var attacker = new Character();
            var victim = new Character();
            
            attacker.Damage(victim, 9999);
            
            Assert.Equal(CharacterStatus.Dead, victim.Status);
            Assert.Equal(0, victim.Health);
        }

        [Fact]
        public void CharacterCannotDamageHimself()
        {
            var character = new Character();

            Assert.Throws<InvalidOperationException>(() => character.Damage(character, 100));
        }

        [Fact]
        public void DamageIsReducedWhenVictimIsMuchHigherLevel()
        {
            var attacker = new Character();
            var victim = new Character()
            {
                Level = 6,
            };
            
            attacker.Damage(victim, 100);
            
            Assert.Equal(950, victim.Health);
        }

        [Fact]
        public void DamageIsIncreasedWhenAttackerIsMuchHigherLevel()
        {
            var attacker = new Character()
            {
                Level = 6,
            };
            var victim = new Character();
            
            attacker.Damage(victim, 100);
            
            Assert.Equal(850, victim.Health);
        }
    }
}
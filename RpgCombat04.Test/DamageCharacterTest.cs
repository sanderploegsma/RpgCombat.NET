using System;
using System.Collections.Generic;
using System.Numerics;
using Xunit;

namespace RpgCombat04.Test
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

            Assert.Throws<InvalidTargetException>(() => character.Damage(character, 100));
        }

        [Fact]
        public void DamageIsReducedWhenVictimIsMuchHigherLevel()
        {
            var attacker = new Character();
            var victim = new Character
            {
                Level = 6,
            };
            
            attacker.Damage(victim, 100);
            
            Assert.Equal(950, victim.Health);
        }

        [Fact]
        public void DamageIsIncreasedWhenAttackerIsMuchHigherLevel()
        {
            var attacker = new Character
            {
                Level = 6,
            };
            var victim = new Character();
            
            attacker.Damage(victim, 100);
            
            Assert.Equal(850, victim.Health);
        }
        
        [Theory]
        [InlineData(CharacterType.Melee, 1, 1)]
        [InlineData(CharacterType.Ranged, 10, 10)]
        public void CanDamageWithinRange(CharacterType type, float x, float y)
        {
            var attacker = new Character
            {
                Type = type,
            };
            var victim = new Character
            {
                Position = new Vector2(x, y),
            };

            attacker.Damage(victim, 100);
            
            Assert.Equal(900, victim.Health);
        }

        [Theory]
        [InlineData(CharacterType.Melee, 2, 2)]
        [InlineData(CharacterType.Ranged, 15, 15)]
        public void CannotDamageOutOfRange(CharacterType type, float x, float y)
        {
            var attacker = new Character
            {
                Type = type,
            };
            var victim = new Character
            {
                Position = new Vector2(x, y),
            };

            Assert.Throws<TargetOutOfRangeException>(() => attacker.Damage(victim, 100));
        }

        [Fact]
        public void CharactersCannotDamageAllies()
        {
            var attacker = new Character
            {
                Factions = new List<string> { "Red Bulls" }
            };
            
            var victim = new Character
            {
                Factions = new List<string> { "Red Bulls" }
            };

            Assert.Throws<InvalidTargetException>(() => attacker.Damage(victim, 100));
        }
    }
}
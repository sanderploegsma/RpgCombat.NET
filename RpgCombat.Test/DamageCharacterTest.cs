using System;
using System.Numerics;
using Xunit;

namespace RpgCombat.Test
{
    public class DamageCharacterTest
    {
        [Fact]
        public void DamageIsSubtractedFromHealthOfVictim()
        {
            var character = new Character();
            var target = new Character();

            character.Damage(target, 100);

            Assert.Equal(900, target.Health);
        }

        [Fact]
        public void VictimRemainsAliveWhenDamageDoesNotExceedHealth()
        {
            var character = new Character();
            var target = new Character();

            character.Damage(target, 100);

            Assert.Equal(CharacterStatus.Alive, target.Status);
        }

        [Fact]
        public void VictimDiesWhenDamageExceedsHealth()
        {
            var character = new Character();
            var target = new Character();

            character.Damage(target, 9999);

            Assert.Equal(CharacterStatus.Dead, target.Status);
            Assert.Equal(0, target.Health);
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
            var character = new Character();
            var target = new Character { Level = 6 };

            character.Damage(target, 100);

            Assert.Equal(950, target.Health);
        }

        [Fact]
        public void DamageIsIncreasedWhenAttackerIsMuchHigherLevel()
        {
            var character = new Character { Level = 6 };
            var target = new Character();

            character.Damage(target, 100);

            Assert.Equal(850, target.Health);
        }

        [Theory]
        [InlineData(CharacterClass.Melee, 1, 1)]
        [InlineData(CharacterClass.Ranged, 10, 10)]
        public void CanDamageWithinRange(CharacterClass @class, float x, float y)
        {
            var character = new Character(@class);
            var target = new Character { Position = new Vector2(x, y) };

            character.Damage(target, 100);

            Assert.Equal(900, target.Health);
        }

        [Theory]
        [InlineData(CharacterClass.Melee, 2, 2)]
        [InlineData(CharacterClass.Ranged, 15, 15)]
        public void CannotDamageOutOfRange(CharacterClass @class, float x, float y)
        {
            var character = new Character(@class);
            var target = new Character { Position = new Vector2(x, y) };

            Assert.Throws<InvalidOperationException>(() => character.Damage(target, 100));
        }

        [Fact]
        public void CharactersCannotDamageAllies()
        {
            var faction = new Faction();
            var character = new Character();
            var ally = new Character();

            character.JoinFaction(faction);
            ally.JoinFaction(faction);

            Assert.Throws<InvalidOperationException>(() => character.Damage(ally, 100));
        }

        [Fact]
        public void DeadCharactersCannotBeDamaged()
        {
            var character = new Character();
            var target = new Character { Health = 0 };

            Assert.Throws<InvalidOperationException>(() => character.Damage(target, 100));
        }
    }
}
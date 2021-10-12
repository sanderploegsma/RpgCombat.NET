﻿using System;
using System.Numerics;
using Xunit;

namespace RpgCombat.Test
{
    public class DamagePropTest
    {
        [Theory]
        [InlineData(CharacterClass.Melee, 1, 1)]
        [InlineData(CharacterClass.Ranged, 10, 10)]
        public void CharacterCanDamagePropWithinRange(CharacterClass @class, float x, float y)
        {
            var character = new Character(@class);
            var prop = new Prop(2000, new Vector2(x, y));

            character.Damage(prop, 100);
            
            Assert.Equal(1900, prop.Health);
        }
        
        [Theory]
        [InlineData(CharacterClass.Melee, 2,2)]
        [InlineData(CharacterClass.Ranged, 15, 15)]
        public void CharacterCannotDamagePropOutsideRange(CharacterClass @class, float x, float y)
        {
            var character = new Character(@class);
            var prop = new Prop(2000, new Vector2(x, y));

            Assert.Throws<InvalidOperationException>(() => character.Damage(prop, 100));
        }
        
        [Fact]
        public void PropIsDestroyedWhenDamageExceedsHealth()
        {
            var character = new Character();
            var prop = new Prop(1000);
            
            character.Damage(prop, 9999);
            
            Assert.Equal(0, prop.Health);
            Assert.Equal(PropStatus.Destroyed, prop.Status);
        }

        [Fact]
        public void DestroyedPropsCannotBeDamaged()
        {
            var character = new Character();
            var prop = new Prop(0);

            Assert.Throws<InvalidOperationException>(() => character.Damage(prop, 100));
        }
    }
}
using System;
using System.Numerics;
using Xunit;

namespace RpgCombat05.Test
{
    public class DamagePropTest
    {
        [Theory]
        [InlineData(CharacterType.Melee, 1, 1)]
        [InlineData(CharacterType.Ranged, 10, 10)]
        public void CharacterCanDamagePropWithinRange(CharacterType type, float x, float y)
        {
            var character = new Character
            {
                Type = type,
            };
            var prop = new Prop(2000)
            {
                Position = new Vector2(x, y),
            };

            character.Damage(prop, 100);
            
            Assert.Equal(1900, prop.Health);
        }
        
        [Theory]
        [InlineData(CharacterType.Melee, 2,2)]
        [InlineData(CharacterType.Ranged, 15, 15)]
        public void CharacterCannotDamagePropOutsideRange(CharacterType type, float x, float y)
        {
            var character = new Character
            {
                Type = type,
            };
            var prop = new Prop(2000)
            {
                Position = new Vector2(x, y),
            };

            Assert.Throws<TargetOutOfRangeException>(() => character.Damage(prop, 100));
        }
        
        [Fact]
        public void PropIsDestroyedWhenDamageExceedsHealth()
        {
            var character = new Character();
            var prop = new Prop(1000);
            
            character.Damage(prop, 9999);
            
            Assert.Equal(0, prop.Health);
            Assert.True(prop.IsDestroyed);
        }

        [Fact]
        public void PropsCannotBeHealed()
        {
            var character = new Character();
            var prop = new Prop(1000);

            Assert.ThrowsAny<InvalidOperationException>(() => character.Heal(prop, 100));
        }
    }
}
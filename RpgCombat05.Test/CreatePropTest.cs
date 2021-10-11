using System;
using Xunit;

namespace RpgCombat05.Test
{
    public class CreatePropTest
    {
        [Fact]
        public void NewPropsCanHaveArbitraryHealth()
        {
            var prop = new Prop(2000);
            
            Assert.Equal(2000, prop.Health);
        }

        [Fact]
        public void NewPropsWithZeroHealthAreDestroyed()
        {
            var prop = new Prop(0);
            
            Assert.True(prop.IsDestroyed);
        }

        [Fact]
        public void CannotCreatePropWithNegativeHealth()
        {
            Assert.Throws<InvalidOperationException>(() => new Prop(-10));
        }
    }
}
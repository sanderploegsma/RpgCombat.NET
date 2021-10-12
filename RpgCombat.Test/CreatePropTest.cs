using Xunit;

namespace RpgCombat.Test
{
    public class CreatePropTest
    {
        [Fact]
        public void NewPropsCanHaveArbitraryHealth()
        {
            var prop = new Prop(2000);
            
            Assert.Equal(2000, prop.Health);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1000)]
        public void NewPropsWithZeroOrNegativeHealthAreImmediatelyDestroyed(double health)
        {
            var prop = new Prop(health);
            
            Assert.Equal(0, prop.Health);
            Assert.Equal(PropStatus.Destroyed, prop.Status);
        }
    }
}
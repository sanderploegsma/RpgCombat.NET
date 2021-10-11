using Xunit;

namespace RpgCombat.Iteration01.Test
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
    }
}
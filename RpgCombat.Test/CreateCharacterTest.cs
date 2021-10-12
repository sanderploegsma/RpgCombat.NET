using Xunit;

namespace RpgCombat.Test
{
    public class CreateCharacterTest
    {
        [Fact]
        public void NewCharactersHave1000Health()
        {
            var character = new Character();
            Assert.Equal(1000, character.Health);
        }

        [Fact]
        public void NewCharactersAreAlive()
        {
            var character = new Character();
            Assert.Equal(CharacterStatus.Alive, character.Status);
        }

        [Fact]
        public void NewCharactersAreLevel1()
        {
            var character = new Character();
            Assert.Equal(1, character.Level);
        }
        
        [Fact]
        public void NewCharactersBelongToNoFaction()
        {
            var character = new Character();
            Assert.Empty(character.Factions);
        }
    }
}
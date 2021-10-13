using NUnit.Framework;

namespace RpgCombat.Test.Unit
{
    [TestFixtureSource(typeof(CharacterClassProvider))]
    public class NewCharacterTests
    {
        private readonly CharacterClass _characterClass;

        public NewCharacterTests(CharacterClass characterClass)
        {
            _characterClass = characterClass;
        }

        [Test]
        public void NewCharactersHave1000Health()
        {
            var character = new Character(_characterClass);
            Assert.That(character.Health, Is.EqualTo(1000));
        }

        [Test]
        public void NewCharactersAreAlive()
        {
            var character = new Character(_characterClass);
            Assert.That(character.Status, Is.EqualTo(CharacterStatus.Alive));
        }

        [Test]
        public void NewCharactersAreLevel1()
        {
            var character = new Character(_characterClass);
            Assert.That(character.Level, Is.EqualTo(1));
        }

        [Test]
        public void NewCharactersBelongToNoFaction()
        {
            var character = new Character(_characterClass);
            Assert.That(character.Factions, Is.Empty);
        }
    }
}
using NUnit.Framework;

namespace RpgCombat.Test.Unit
{
    [TestFixture]
    public class FactionTests
    {
        [Test]
        public void CharactersAreAddedToFactionWhenTheyJoin()
        {
            var faction = new Faction(TestContext.CurrentContext.Random.GetString());
            Assert.That(faction.Characters, Is.Empty);

            var character1 = new Character("Character 1");
            character1.JoinFaction(faction);
            Assert.That(faction.Characters, Has.Exactly(1).Items);
            Assert.That(faction.Characters, Contains.Item(character1));

            var character2 = new Character("Character 2");
            character2.JoinFaction(faction);
            Assert.That(faction.Characters, Has.Exactly(2).Items);
            Assert.That(faction.Characters, Contains.Item(character1).And.Contains(character2));
        }
        
        [Test]
        public void CharactersAreRemovedFromFactionWhenTheyLeave()
        {
            var faction = new Faction(TestContext.CurrentContext.Random.GetString());
            var character1 = new Character("Character 1");
            var character2 = new Character("Character 2");
            character1.JoinFaction(faction);
            character2.JoinFaction(faction);
            Assert.That(faction.Characters, Has.Exactly(2).Items);
            Assert.That(faction.Characters, Contains.Item(character1).And.Contains(character2));
            
            character1.LeaveFaction(faction);
            Assert.That(faction.Characters, Has.Exactly(1).Items);
            Assert.That(faction.Characters, Contains.Item(character2));
            
            character2.LeaveFaction(faction);
            Assert.That(faction.Characters, Is.Empty);
        }
    }
}
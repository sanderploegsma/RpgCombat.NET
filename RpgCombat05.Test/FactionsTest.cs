using System.Collections.Generic;
using Xunit;

namespace RpgCombat05.Test
{
    public class FactionsTest
    {
        [Fact]
        public void CharactersCanJoinAFaction()
        {
            var faction = new Faction();
            var character = new Character();

            character.JoinFaction(faction);

            Assert.Contains(character, faction.Characters);
            Assert.Contains(faction, character.Factions);
        }

        [Fact]
        public void CharactersCanBelongToMultipleFactions()
        {
            var faction1 = new Faction();
            var faction2 = new Faction();
            var character = new Character();
            
            character.JoinFaction(faction1);
            character.JoinFaction(faction2);

            Assert.Contains(character, faction1.Characters);
            Assert.Contains(character, faction2.Characters);
            Assert.Contains(faction1, character.Factions);
            Assert.Contains(faction2, character.Factions);
        }

        [Fact]
        public void CharactersCannotBelongToFactionTwice()
        {
            var faction = new Faction();
            var character = new Character();

            character.JoinFaction(faction);
            character.JoinFaction(faction);

            Assert.Single(faction.Characters, character);
            Assert.Single(character.Factions, faction);
        }
        
        [Fact]
        public void CharactersCanLeaveFactions()
        {
            var faction = new Faction();
            var character = new Character();

            character.JoinFaction(faction);
            character.LeaveFaction(faction);

            Assert.Empty(faction.Characters);
            Assert.Empty(character.Factions);
        }
    }
}
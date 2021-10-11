using System.Collections.Generic;
using Xunit;

namespace RpgCombat04.Test
{
    public class FactionsTest
    {
        [Fact]
        public void CharactersCanJoinAFaction()
        {
            var character = new Character();

            character.JoinFaction("Red Bulls");

            Assert.Contains("Red Bulls", character.Factions);
        }

        [Fact]
        public void CharactersCanBelongToMultipleFactions()
        {
            var character = new Character
            {
                Factions = new List<string> { "Red Bulls" },
            };

            character.JoinFaction("Black Widows");

            Assert.Contains("Red Bulls", character.Factions);
            Assert.Contains("Black Widows", character.Factions);
        }

        [Fact]
        public void CharactersCannotBelongToFactionTwice()
        {
            var character = new Character
            {
                Factions = new List<string> { "Red Bulls" },
            };

            character.JoinFaction("Red Bulls");

            Assert.Equal(new List<string> { "Red Bulls" }, character.Factions);
        }
        
        [Fact]
        public void CharactersCanLeaveFactions()
        {
            var character = new Character
            {
                Factions = new List<string> { "Red Bulls" },
            };

            character.LeaveFaction("Red Bulls");

            Assert.Empty(character.Factions);
        }
    }
}
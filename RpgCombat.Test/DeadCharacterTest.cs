using System;
using Xunit;

namespace RpgCombat.Test
{
    public class DeadCharacterTest
    {
        [Fact]
        public void DeadCharactersCannotHealThemselves()
        {
            var character = new Character(health: 0);

            Assert.Throws<InvalidOperationException>(() => character.Heal(character, 100));
        }
        
        [Fact]
        public void DeadCharactersCannotHealAllies()
        {
            var faction = new Faction();
            var character = new Character(health: 0, factions: faction);
            var ally = new Character(health: 500, factions: faction);

            Assert.Throws<InvalidOperationException>(() => character.Heal(ally, 100));
        }
        
        [Fact]
        public void DeadCharactersCannotDamageCharacters()
        {
            var character = new Character(health: 0);
            var target = new Character();

            Assert.Throws<InvalidOperationException>(() => character.Damage(target, 100));
        }
        
        [Fact]
        public void DeadCharactersCannotDamageProps()
        {
            var character = new Character(health: 0);
            var target = new Prop(1000);

            Assert.Throws<InvalidOperationException>(() => character.Damage(target, 100));
        }
        
        [Fact]
        public void DeadCharactersCanJoinFactions()
        {
            var faction = new Faction();
            var character = new Character(health: 0);

            character.JoinFaction(faction);

            Assert.Contains(character, faction.Characters);
            Assert.Contains(faction, character.Factions);
        }
        
        [Fact]
        public void DeadCharactersCanLeaveFactions()
        {
            var faction = new Faction();
            var character = new Character(health: 0, factions: faction);

            character.LeaveFaction(faction);
            
            Assert.Empty(faction.Characters);
            Assert.Empty(character.Factions);
        }
    }
}
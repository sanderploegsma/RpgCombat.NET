using System;
using Xunit;

namespace RpgCombat.Test
{
    public class DeadCharacterTest
    {
        [Fact]
        public void DeadCharactersCannotHealThemselves()
        {
            var character = new Character { Health = 0 };

            Assert.Throws<InvalidOperationException>(() => character.Heal(character, 100));
        }

        [Fact]
        public void DeadCharactersCannotHealAllies()
        {
            var faction = new Faction();
            var character = new Character { Health = 0 };
            var ally = new Character { Health = 500 };

            character.JoinFaction(faction);
            ally.JoinFaction(faction);

            Assert.Throws<InvalidOperationException>(() => character.Heal(ally, 100));
        }

        [Fact]
        public void DeadCharactersCannotDamageCharacters()
        {
            var character = new Character { Health = 0 };
            var target = new Character();

            Assert.Throws<InvalidOperationException>(() => character.Damage(target, 100));
        }

        [Fact]
        public void DeadCharactersCannotDamageProps()
        {
            var character = new Character { Health = 0 };
            var target = new Prop(1000);

            Assert.Throws<InvalidOperationException>(() => character.Damage(target, 100));
        }

        [Fact]
        public void DeadCharactersCanJoinAndLeaveFactions()
        {
            var faction = new Faction();
            var character = new Character { Health = 0 };

            character.JoinFaction(faction);

            Assert.Contains(character, faction.Characters);
            Assert.Contains(faction, character.Factions);

            character.LeaveFaction(faction);

            Assert.Empty(faction.Characters);
            Assert.Empty(character.Factions);
        }
    }
}
using System;
using NUnit.Framework;

namespace RpgCombat.Test.Unit
{
    [TestFixtureSource(typeof(CharacterClassProvider))]
    public class DeadCharacterTests
    {
        private readonly CharacterClass _characterClass;

        private Character _subject;
        private Character _ally;
        private Character _other;
        private Faction _faction;

        public DeadCharacterTests(CharacterClass characterClass)
        {
            _characterClass = characterClass;
        }

        [SetUp]
        public void SetUp()
        {
            _subject = new Character("Subject", _characterClass) { Health = 0 };
            _ally = new Character("Ally");
            _other = new Character("Other");
            _faction = new Faction("Faction");

            _subject.JoinFaction(_faction);
            _ally.JoinFaction(_faction);
        }

        [Test]
        public void CannotDamageTargets([Random(1, 100, 5)] double damage)
        {
            foreach (var target in new ITarget[] { new Character("Character"), new Prop("Tree", 1000) })
            {
                var targetHealth = target.Health;
                Assert.Throws<InvalidOperationException>(() => _subject.Damage(target, damage));
                Assert.That(target.Health, Is.EqualTo(targetHealth));
            }
        }

        [Test]
        public void CannotHealCharacters([Random(0, Character.MaximumHealth, 5)] double amount)
        {
            Assert.Throws<InvalidOperationException>(() => _subject.Heal(_subject, amount));
            Assert.Throws<InvalidOperationException>(() => _subject.Heal(_ally, amount));
            Assert.Throws<InvalidOperationException>(() => _subject.Heal(_other, amount));
        }

        [Test]
        public void CannotReceiveHealing([Random(0, Character.MaximumHealth, 5)] double amount)
        {
            Assert.Throws<InvalidOperationException>(() => _subject.ReceiveHealing(amount));
        }

        [Test]
        public void CannotReceiveDamage([Random(0, Character.MaximumHealth, 5)] double damage)
        {
            Assert.Throws<InvalidOperationException>(() =>
                ((ITarget)_subject).ReceiveDamage(new Damage(damage, TestContext.CurrentContext.Random.Next())));
        }

        [Test]
        public void CanJoinFactions()
        {
            var faction = new Faction(TestContext.CurrentContext.Random.GetString());
            _subject.JoinFaction(faction);

            Assert.That(faction.Characters, Contains.Item(_subject));
            Assert.That(_subject.Factions, Contains.Item(faction).And.Contains(_faction));
        }

        [Test]
        public void CanLeaveFactions()
        {
            _subject.LeaveFaction(_faction);

            Assert.That(_subject.Factions, Is.Empty);
            Assert.That(_faction.Characters, Does.Not.Contain(_subject));
        }
    }
}
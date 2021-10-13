using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading;
using NUnit.Framework;

namespace RpgCombat.Test.Unit
{
    [TestFixtureSource(typeof(CharacterClassProvider))]
    public class AliveCharacterTests
    {
        private readonly CharacterClass _characterClass;
        
        private Character _subject;
        private Character _ally;
        private Character _other;

        public AliveCharacterTests(CharacterClass characterClass)
        {
            _characterClass = characterClass;
        }
        
        [SetUp]
        public void SetUp()
        {
            _subject = new Character(_characterClass);
            _ally = new Character();
            _other = new Character();
            
            var faction = new Faction();
            _subject.JoinFaction(faction);
            _ally.JoinFaction(faction);
        }

        [Test]
        public void HasMaximumHealthByDefault()
        {
            Assert.That(_subject.Health, Is.EqualTo(Character.MaximumHealth));
        }

        [Test]
        public void CanHealThemselves(
            [Random(1, 100, 5)] double currentHealth, 
            [Random(1, 100, 5)] double healingAmount)
        {
            _subject.Health = currentHealth;
            Assert.DoesNotThrow(() => _subject.Heal(_subject, healingAmount));
            Assert.That(_subject.Health, Is.GreaterThan(currentHealth));
        }
        
        [Test]
        public void CanHealAllies(
            [Random(1, 100, 5)] double currentHealth, 
            [Random(1, 100, 5)] double healingAmount)
        {
            _ally.Health = currentHealth;
            Assert.DoesNotThrow(() => _subject.Heal(_ally, healingAmount));
            Assert.That(_ally.Health, Is.GreaterThan(currentHealth));
        }
        
        [Test]
        public void CannotHealOthers(
            [Random(1, 100, 5)] double currentHealth, 
            [Random(1, 100, 5)] double healingAmount)
        {
            _other.Health = currentHealth;
            Assert.Throws<InvalidOperationException>(() => _subject.Heal(_other, healingAmount));
            Assert.That(_other.Health, Is.EqualTo(currentHealth));
        }

        [Test]
        public void CannotAttackSelf([Random(1, 100, 5)] double damage)
        {
            var health = _subject.Health;
            Assert.Throws<InvalidOperationException>(() => _subject.Damage(_subject, damage));
            Assert.That(_subject.Health, Is.EqualTo(health));
        }

        [Test]
        public void CannotAttackAllies([Random(1, 100, 5)] double damage)
        {
            var health = _ally.Health;
            Assert.Throws<InvalidOperationException>(() => _subject.Damage(_ally, damage));
            Assert.That(_ally.Health, Is.EqualTo(health));
        }
        
        [Test]
        public void CanAttackOtherCharacters([Random(1, 100, 5)] double damage)
        {
            var health = _other.Health;
            Assert.DoesNotThrow(() => _subject.Damage(_other, damage));
            Assert.That(_other.Health, Is.LessThan(health));
        }

        [Test]
        public void CanAttackTargetsWhenInRange([Random(1, 100, 5)] double damage)
        {
            foreach (var target in TargetsInRange())
            {
                var targetHealth = target.Health;
                Assert.DoesNotThrow(() => _subject.Damage(target, damage));
                Assert.That(target.Health, Is.LessThan(targetHealth));
            }
        }
        
        [Test]
        public void CannotAttackTargetsWhenNotInRange([Random(5)] double damage)
        {
            foreach (var target in TargetsNotInRange())
            {
                var targetHealth = target.Health;
                Assert.Throws<InvalidOperationException>(() => _subject.Damage(target, damage));
                Assert.That(target.Health, Is.EqualTo(targetHealth));
            }
        }
        
        [Test]
        public void ReceivedHealingIsAddedToHealth(
            [Random(100, 200, 5)] double currentHealth, 
            [Random(100, 200, 5)] double amountHealed)
        {
            _subject.Health = currentHealth;
            _subject.ReceiveHealing(amountHealed);
            
            Assert.That(_subject.Health, Is.EqualTo(currentHealth + amountHealed));
        }
        
        [Test]
        public void ReceivedHealingNeverIncreasesHealthAboveMaximum(
            [Random(100, 200, 5)] double amountHealed)
        {
            _subject.Health = Character.MaximumHealth;
            _subject.ReceiveHealing(amountHealed);
            
            Assert.That(_subject.Health, Is.EqualTo(Character.MaximumHealth));
        }
        
        [Test]
        public void CannotReceiveNegativeHealing(
            [Random(100, 200, 5)] double currentHealth, 
            [Random(-200, -100, 5)] double amountHealed)
        {
            _subject.Health = currentHealth;
            _subject.ReceiveHealing(amountHealed);
            
            Assert.That(_subject.Health, Is.EqualTo(currentHealth));
        }

        [Test]
        public void CannotReceiveNegativeDamage([Random(-200, -100, 5)] double damage)
        {
            ((ITarget) _subject).ReceiveDamage(new Damage(damage, TestContext.CurrentContext.Random.Next()));
            
            Assert.That(_subject.Health, Is.EqualTo(Character.MaximumHealth));
        }
        
        [Test]
        public void ReceivedDamageIsSubtractedFromHealth([Random(100, 200, 5)] double damage)
        {
            var currentHealth = _subject.Health;
            ((ITarget) _subject).ReceiveDamage(new Damage(damage, _subject.Level));

            var expected = currentHealth - damage;
            Assert.That(_subject.Health, Is.EqualTo(expected));
        }

        [Test]
        public void ReceivedDamageIsIncreasedWhenAttackedByHigherLevel([Random(100, 200, 5)] double damage)
        {
            var currentHealth = _subject.Health;
            ((ITarget) _subject).ReceiveDamage(new Damage(damage, _subject.Level + 5));

            var expected = currentHealth - damage * 1.5;
            Assert.That(_subject.Health, Is.EqualTo(expected));
        }
        
        [Test]
        public void ReceivedDamageIsDecreasedWhenAttackedByHigherLevel([Random(100, 200, 5)] double damage)
        {
            var currentHealth = _subject.Health;
            ((ITarget) _subject).ReceiveDamage(new Damage(damage, _subject.Level - 5));

            var expected = currentHealth - damage * 0.5;
            Assert.That(_subject.Health, Is.EqualTo(expected));
        }

        private IEnumerable<ITarget> TargetsInRange()
        {
            var positionsInRange = GenerateRandomPositions()
                .Where(position => Vector2.Subtract(_subject.Position, position).Length() <= _subject.Range)
                .Take(2)
                .ToArray();

            yield return new Prop(TestContext.CurrentContext.Random.NextDouble(1000, 2000), positionsInRange[0]);
            yield return new Character { Position = positionsInRange[1] };
        }
        
        private IEnumerable<ITarget> TargetsNotInRange()
        {
            var positionsInRange = GenerateRandomPositions()
                .Where(position => Vector2.Subtract(_subject.Position, position).Length() > _subject.Range)
                .Take(2)
                .ToArray();

            yield return new Prop(TestContext.CurrentContext.Random.NextDouble(1000, 2000), positionsInRange[0]);
            yield return new Character { Position = positionsInRange[1] };
        }

        private static IEnumerable<Vector2> GenerateRandomPositions(CancellationToken cancellationToken = default)
        {
            const float maxCoordinate = 50;
            
            while (!cancellationToken.IsCancellationRequested)
            {
                yield return new Vector2(
                    TestContext.CurrentContext.Random.NextFloat(-maxCoordinate, maxCoordinate),
                    TestContext.CurrentContext.Random.NextFloat(-maxCoordinate, maxCoordinate));
            }
        }
    }
}
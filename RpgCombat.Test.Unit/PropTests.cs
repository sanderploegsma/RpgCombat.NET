using System;
using NUnit.Framework;

namespace RpgCombat.Test.Unit
{
    [TestFixture]
    public class PropTests
    {
        [Test]
        public void PropWithPositiveHealthIsNotDestroyed([Random(0, double.MaxValue, 5)] double health)
        {
            var prop = new Prop(health);

            Assert.That(prop.Status, Is.EqualTo(PropStatus.Normal));
            Assert.That(prop.Health, Is.EqualTo(health));
        }

        [Test]
        public void PropWithHealthOfZeroOrLessIsDestroyed([Random(double.MinValue, 0, 5)] double health)
        {
            var prop = new Prop(health);

            Assert.That(prop.Status, Is.EqualTo(PropStatus.Destroyed));
            Assert.That(prop.Health, Is.Zero);
        }

        [Test]
        public void DamageReceivedByPropIsSubtractedFromHealth([Random(10, 100, 5)] double damage)
        {
            const double health = 2000;

            var prop = new Prop(health);
            ((ITarget)prop).ReceiveDamage(new Damage(damage, 1));

            Assert.That(prop.Health, Is.EqualTo(health - damage));
        }

        [Test]
        public void DamageReceivedByPropIsUnaffectedByAttackerLevel([Random(1, 10, 5)] int attackerLevel)
        {
            const double health = 2000;
            const double damage = 100;

            var prop = new Prop(health);
            ((ITarget)prop).ReceiveDamage(new Damage(damage, attackerLevel));

            Assert.That(prop.Health, Is.EqualTo(health - damage));
        }

        [Test]
        public void PropIsDestroyedWhenDamageExceedsHealth(
            [Random(100, 200, 5)] double health,
            [Random(200, 300, 5)] double damage)
        {
            var prop = new Prop(health);
            var attackerLevel = TestContext.CurrentContext.Random.Next();
            ((ITarget)prop).ReceiveDamage(new Damage(damage, attackerLevel));

            Assert.That(prop.Status, Is.EqualTo(PropStatus.Destroyed));
            Assert.That(prop.Health, Is.Zero);
        }

        [Test]
        public void DestroyedPropCannotReceiveDamage([Random(5)] double damage)
        {
            var prop = new Prop(0);
            Assert.Throws<InvalidOperationException>(() =>
                ((ITarget)prop).ReceiveDamage(new Damage(damage, TestContext.CurrentContext.Random.Next())));
        }
    }
}
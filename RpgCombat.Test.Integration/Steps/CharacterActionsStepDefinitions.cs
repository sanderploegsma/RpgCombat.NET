using System;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace RpgCombat.Test.Integration.Steps
{
    [Binding]
    public sealed class CharacterActionsStepDefinitions
    {
        private readonly ScenarioContext _scenarioContext;

        public CharacterActionsStepDefinitions(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [When(@"""(.*)"" attempts to deal (.*) damage to ""(.*)""")]
        public void WhenCharacterAttemptsToDealDamageToTarget(string attackerName, double damage, string targetName)
        {
            var attacker = _scenarioContext.Get<Character>(attackerName);
            var target = _scenarioContext.Get<ITarget>(targetName);

            attacker.Damage(target, damage);
        }

        [When(@"""(.*)"" heals ""(.*)"" by (.*)")]
        public void WhenCharacterHealsCharacterByAmount(string healerName, string targetName, double health)
        {
            var healer = _scenarioContext.Get<Character>(healerName);
            var target = _scenarioContext.Get<Character>(targetName);

            healer.Heal(target, health);
        }

        [When(@"""(.*)"" heals themselves by (.*)")]
        public void WhenCharacterHealsThemselvesByAmount(string name, double health)
        {
            var character = _scenarioContext.Get<Character>(name);
            character.Heal(character, health);
        }

        [Then(@"""(.*)"" should be able to damage ""(.*)""")]
        public void ThenCharacterCanDamageTarget(string attackerName, string targetName)
        {
            var attacker = _scenarioContext.Get<Character>(attackerName);
            var target = _scenarioContext.Get<ITarget>(targetName);

            Assert.DoesNotThrow(() => attacker.Damage(target, 0));
        }

        [Then(@"""(.*)"" should not be able to damage ""(.*)""")]
        public void ThenCharacterCannotDamageTarget(string attackerName, string targetName)
        {
            var attacker = _scenarioContext.Get<Character>(attackerName);
            var target = _scenarioContext.Get<ITarget>(targetName);

            Assert.Throws<InvalidOperationException>(() => attacker.Damage(target, 0));
        }

        [Then(@"""(.*)"" should be able to heal ""(.*)""")]
        public void ThenCharacterCanHealCharacter(string healerName, string targetName)
        {
            var healer = _scenarioContext.Get<Character>(healerName);
            var target = _scenarioContext.Get<Character>(targetName);

            Assert.DoesNotThrow(() => healer.Heal(target, 0));
        }

        [Then(@"""(.*)"" should not be able to heal ""(.*)""")]
        public void ThenCharacterCannotHealCharacter(string healerName, string targetName)
        {
            var healer = _scenarioContext.Get<Character>(healerName);
            var target = _scenarioContext.Get<Character>(targetName);

            Assert.Throws<InvalidOperationException>(() => healer.Heal(target, 0));
        }
    }
}
using System.Collections.Generic;
using System.Numerics;
using NUnit.Framework;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace RpgCombat.Test.Integration.Steps
{
    [Binding]
    public class CharacterStepDefinitions
    {
        private readonly ScenarioContext _scenarioContext;

        public CharacterStepDefinitions(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Given("the following characters:")]
        public void GivenTheFollowingCharacters(IEnumerable<Character> characters)
        {
            foreach (var character in characters)
            {
                _scenarioContext[character.Name] = character;
            }
        }
        
        [When(@"A new character ""(.*)"" is created")]
        public void WhenANewCharacterIsCreated(string name)
        {
            _scenarioContext[name] = new Character(name);
        }

        [Then(@"the health of ""(.*)"" should be (.*)")]
        public void ThenTheHealthOfCharacterShouldBe(string name, double health)
        {
            var target = _scenarioContext.Get<ITarget>(name);
            Assert.That(target.Health, Is.EqualTo(health));
        }

        [Then(@"the level of ""(.*)"" should be (.*)")]
        public void ThenTheLevelOfCharacterShouldBe(string name, int level)
        {
            var character = _scenarioContext.Get<Character>(name);
            Assert.That(character.Level, Is.EqualTo(level));
        }

        [Then(@"""(.*)"" should be (.*)")]
        public void ThenCharacterShouldHaveStatus(string name, CharacterStatus status)
        {
            var character = _scenarioContext.Get<Character>(name);
            Assert.That(character.Status, Is.EqualTo(status));
        }

        [Then(@"the class of ""(.*)"" should be (.*)")]
        public void ThenTheClassOfCharacterShouldBe(string name, CharacterClass @class)
        {
            var character = _scenarioContext.Get<Character>(name);
            Assert.That(character.Class, Is.EqualTo(@class));
        }

        [StepArgumentTransformation]
        public IEnumerable<Character> TransformCharacters(Table data)
        {
            var characters = data.CreateSet<TestCharacter>();

            foreach (var item in characters)
            {
                var character = new Character(item.Name);

                character.Class = item.Class ?? character.Class;
                character.Health = item.Health ?? character.Health;
                character.Level = item.Level ?? character.Level;
                character.Position = item.X.HasValue && item.Y.HasValue
                    ? new Vector2(item.X.Value, item.Y.Value)
                    : character.Position;

                yield return character;
            }
        }
    }
}
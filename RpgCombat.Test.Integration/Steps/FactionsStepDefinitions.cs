using System.Linq;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace RpgCombat.Test.Integration.Steps
{
    [Binding]
    public class FactionsStepDefinitions
    {
        private readonly ScenarioContext _scenarioContext;

        public FactionsStepDefinitions(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [When(@"""(.*)"" and ""(.*)"" are allies")]
        [Given(@"""(.*)"" and ""(.*)"" are allies")]
        public void GivenWhenTwoCharactersAreAllies(string name1, string name2)
        {
            var faction = new Faction("Faction");
            var character1 = _scenarioContext.Get<Character>(name1);
            var character2 = _scenarioContext.Get<Character>(name2);
            
            character1.JoinFaction(faction);
            character2.JoinFaction(faction);
        }

        [When(@"""(.*)"" joins ""(.*)""")]
        public void WhenCharacterJoinsFaction(string characterName, string factionName)
        {
            var faction = GetOrCreateFaction(factionName);
            var character = _scenarioContext.Get<Character>(characterName);
            character.JoinFaction(faction);
        }
        
        [When(@"""(.*)"" leaves ""(.*)""")]
        public void WhenCharacterLeavesFaction(string characterName, string factionName)
        {
            var faction = _scenarioContext.Get<Faction>(factionName);
            var character = _scenarioContext.Get<Character>(characterName);
            character.LeaveFaction(faction);
        }
        
        [Then(@"""(.*)"" should not belong to any factions")]
        public void ThenCharacterShouldNotBelongToAnyFactions(string name)
        {
            var character = _scenarioContext.Get<Character>(name);
            Assert.That(character.Factions, Is.Empty);
        }
        
        [Then(@"""(.*)"" and ""(.*)"" should be allies")]
        public void ThenTwoCharactersAreAllies(string name1, string name2)
        {
            var character1 = _scenarioContext.Get<Character>(name1);
            var character2 = _scenarioContext.Get<Character>(name2);
            var commonFactions = character1.Factions.Intersect(character2.Factions);
            
            Assert.That(commonFactions, Is.Not.Empty);
        }

        [Then(@"""(.*)"" and ""(.*)"" should not be allies")]
        public void ThenTwoCharactersAreNotAllies(string name1, string name2)
        {
            var character1 = _scenarioContext.Get<Character>(name1);
            var character2 = _scenarioContext.Get<Character>(name2);
            var commonFactions = character1.Factions.Intersect(character2.Factions);
            
            Assert.That(commonFactions, Is.Empty);
        }

        private Faction GetOrCreateFaction(string name)
        {
            if (_scenarioContext.TryGetValue(name, out Faction faction))
            {
                return faction;
            }

            faction = new Faction(name);
            _scenarioContext[name] = faction;
            return faction;
        }
    }
}
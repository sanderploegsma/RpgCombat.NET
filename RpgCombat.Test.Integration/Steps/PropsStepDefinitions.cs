using System.Collections.Generic;
using System.Numerics;
using NUnit.Framework;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace RpgCombat.Test.Integration.Steps
{
    [Binding]
    public class PropsStepDefinitions
    {
        private readonly ScenarioContext _scenarioContext;

        public PropsStepDefinitions(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }
        
        [Given("the following props:")]
        public void GivenTheFollowingProps(IEnumerable<Prop> props)
        {
            foreach (var prop in props)
            {
                _scenarioContext[prop.Name] = prop;
            }
        }
        
        [Then(@"the ""(.*)"" should be (.*)")]
        public void ThenThePropShouldHaveStatus(string name, PropStatus status)
        {
            var prop = _scenarioContext.Get<Prop>(name);
            Assert.That(prop.Status, Is.EqualTo(status));
        }
        
        [StepArgumentTransformation]
        public IEnumerable<Prop> TransformProps(Table data)
        {
            var props = data.CreateSet<TestProp>();

            foreach (var item in props)
            {
                var position = item.X.HasValue && item.Y.HasValue ? new Vector2(item.X.Value, item.Y.Value) : default;
                yield return new Prop(item.Name, item.Health, position);
            }
        }
    }
}
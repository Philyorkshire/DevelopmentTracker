using System;
using TechTalk.SpecFlow;

namespace KanbanTracker.Test
{
    [Binding]
    public class LoginSteps
    {
        [Given(@"I have entered my email address")]
        public void GivenIHaveEnteredMyEmailAddress()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Given(@"I have entered my password")]
        public void GivenIHaveEnteredMyPassword()
        {
            ScenarioContext.Current.Pending();
        }
        
        [When(@"I press login")]
        public void WhenIPressLogin()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"I am taken to the projects page")]
        public void ThenIAmTakenToTheProjectsPage()
        {
            ScenarioContext.Current.Pending();
        }
    }
}

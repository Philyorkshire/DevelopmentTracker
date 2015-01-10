Feature: Login
	In order to access content
	As a user
	I want to be able to login

Scenario: User logins in
	Given I have entered my email address
	And I have entered my password
	When I press login
	Then I am taken to the projects page

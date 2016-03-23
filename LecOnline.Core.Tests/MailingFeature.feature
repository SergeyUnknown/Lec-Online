Feature: Mailing
	In order to inform interested sides about the events
	As a request manager
	I want to send email messages about changes in request processing stages

Scenario: Notify about accepting of the request
	Given Request submitted to committee
	  And manager related to request
	  And manager could send emails
	When request is accepted
	Then messages count equals 1
	 And acceptance message should be sent to manager

Scenario: Notify about rejection of the request
	Given Request submitted to committee
	  And manager related to request
	  And manager could send emails
	When request is rejected with comment 'some reason'
	Then messages count equals 1
	 And rejection message should be sent to manager

Feature: UserManagement
	In order to prevent authorized access
	As a server
	I want to be provide information about which operation is allowed

Scenario Outline: Create user
	Given User with role <rolename>
	When Get actions for user
	Then Common.Create action <contains> in the actions list
	
  Examples:
    | rolename               | contains |
    | Administrator          | True     |
    | Manager                | True     |
    | MedicalCenter          | False    |
    | EthicalCommitteeMember | False    |

Scenario Outline: Editing user
	Given User with role <rolename>
	  And Belongs to client 1
	When Get actions for user from client 1
	Then Common.Edit action <contains> in the actions list
	
  Examples:
    | rolename               | contains |
    | Administrator          | True     |
    | Manager                | True     |
    | MedicalCenter          | False    |
    | EthicalCommitteeMember | False    |

Scenario Outline: Deleting user
	Given User with role <rolename>
	  And Belongs to client 1
	When Get actions for user from client 1
	Then Common.Delete action <contains> in the actions list
	
  Examples:
    | rolename               | contains |
    | Administrator          | True     |
    | Manager                | True     |
    | MedicalCenter          | False    |
    | EthicalCommitteeMember | False    |

Scenario Outline: Editing user from another client
	Given User with role <rolename>
	  And Belongs to client 1
	When Get actions for user from client 2
	Then Common.Edit action <contains> in the actions list
	
  Examples:
    | rolename               | contains |
    | Administrator          | True     |
    | Manager                | False    |
    | MedicalCenter          | False    |
    | EthicalCommitteeMember | False    |

Scenario Outline: Deleting user from another client
	Given User with role <rolename>
	  And Belongs to client 1
	When Get actions for user from client 2
	Then Common.Delete action <contains> in the actions list
	
  Examples:
    | rolename               | contains |
    | Administrator          | True     |
    | Manager                | False    |
    | MedicalCenter          | False    |
    | EthicalCommitteeMember | False    |
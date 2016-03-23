Feature: ManageClients
	In order to prevent authorized access
	As a server
	I want to be provide information about which operation is allowed

Scenario Outline: Create client
	Given User with role <rolename>
	When Get actions for client
	Then Common.Create action <contains> in the actions list
	
  Examples:
    | rolename               | contains |
    | Administrator          | True     |
    | Manager                | False    |
    | MedicalCenter          | False    |
    | EthicalCommitteeMember | False    |

Scenario Outline: Editing client
	Given User with role <rolename>
	When Get actions for client
	Then Common.Edit action <contains> in the actions list
	
  Examples:
    | rolename               | contains |
    | Administrator          | True     |
    | Manager                | False    |
    | MedicalCenter          | False    |
    | EthicalCommitteeMember | False    |

Scenario Outline: Deleting client
	Given User with role <rolename>
	When Get actions for client
	Then Common.Delete action <contains> in the actions list
	
  Examples:
    | rolename               | contains |
    | Administrator          | True     |
    | Manager                | False    |
    | MedicalCenter          | False    |
    | EthicalCommitteeMember | False    |
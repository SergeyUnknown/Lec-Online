Feature: ManageCommitees
	In order to prevent authorized access
	As a server
	I want to be provide information about which operation is allowed

Scenario Outline: Create committee
	Given User with role <rolename>
	When Get actions for committee
	Then Common.Create action <contains> in the actions list
	
  Examples:
    | rolename               | contains |
    | Administrator          | True     |
    | Manager                | False    |
    | MedicalCenter          | False    |
    | EthicalCommitteeMember | False    |

Scenario Outline: Editing committee
	Given User with role <rolename>
	When Get actions for committee
	Then Common.Edit action <contains> in the actions list
	
  Examples:
    | rolename               | contains |
    | Administrator          | True     |
    | Manager                | False    |
    | MedicalCenter          | False    |
    | EthicalCommitteeMember | False    |

Scenario Outline: Deleting committee
	Given User with role <rolename>
	When Get actions for committee
	Then Common.Delete action <contains> in the actions list
	
  Examples:
    | rolename               | contains |
    | Administrator          | True     |
    | Manager                | False    |
    | MedicalCenter          | False    |
    | EthicalCommitteeMember | False    |


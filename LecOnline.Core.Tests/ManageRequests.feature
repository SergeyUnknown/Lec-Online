Feature: ManageRequests
	In order to prevent authorized access
	As a server
	I want to be provide information about which operation is allowed

Scenario Outline: Create request
	Given User with role <rolename>
	  And Request from client 0
	When Get actions for request
	Then Common.Create action <contains> in the actions list
	
  Examples:
    | rolename               | contains |
    | Administrator          | False    |
    | Manager                | True     |
    | MedicalCenter          | True     |
    | EthicalCommitteeMember | False    |

Scenario Outline: Editing created request for same client
	Given User with role <rolename>
	  And Belongs to client 1
	  And Request from client 1
	  And Request status is 'Created'
	When Get actions for request
	Then Common.Edit action <contains> in the actions list
	
  Examples:
    | rolename               | contains |
    | Administrator          | True     |
    | Manager                | True     |
    | MedicalCenter          | True     |
    | EthicalCommitteeMember | False    |

Scenario Outline: Editing request for different client
	Given User with role <rolename>
	  And Belongs to client 1
	  And Request from client 2
	When Get actions for request
	Then Common.Edit action <contains> in the actions list
	
  Examples:
    | rolename               | contains |
    | Administrator          | True     |
    | Manager                | False    |
    | MedicalCenter          | False    |
    | EthicalCommitteeMember | False    |

Scenario Outline: Deleting request for same client
	Given User with role <rolename>
	  And Belongs to client 1
	  And Request from client 1
	When Get actions for request
	Then Common.Delete action <contains> in the actions list
	
  Examples:
    | rolename               | contains |
    | Administrator          | True     |
    | Manager                | True     |
    | MedicalCenter          | False    |
    | EthicalCommitteeMember | False    |

Scenario Outline: Deleting request for different client
	Given User with role <rolename>
	  And Belongs to client 1
	  And Request from client 2
	When Get actions for request
	Then Common.Delete action <contains> in the actions list
	
  Examples:
    | rolename               | contains |
    | Administrator          | True     |
    | Manager                | False    |
    | MedicalCenter          | False    |
    | EthicalCommitteeMember | False    |

Scenario Outline: Editing submitted request for same client
	Given User with role <rolename>
	  And Belongs to client 1
	  And Request from client 1
	  And Request status is 'Submitted'
	When Get actions for request
	Then Common.Edit action <contains> in the actions list
	
  Examples:
    | rolename               | contains |
    | Administrator          | True     |
    | Manager                | True     |
    | MedicalCenter          | True     |
    | EthicalCommitteeMember | False    |

Scenario Outline: Editing accepted request for same client
	Given User with role <rolename>
	  And Belongs to client 1
	  And Request from client 1
	  And Request status is 'Accepted'
	When Get actions for request
	Then Common.Edit action <contains> in the actions list
	
  Examples:
    | rolename               | contains |
    | Administrator          | True     |
    | Manager                | False    |
    | MedicalCenter          | False    |
    | EthicalCommitteeMember | False    |

Scenario Outline: Editing request for which set meeting for same client
	Given User with role <rolename>
	  And Belongs to client 1
	  And Request from client 1
	  And Request status is 'MeetingSet'
	When Get actions for request
	Then Common.Edit action <contains> in the actions list
	
  Examples:
    | rolename               | contains |
    | Administrator          | True     |
    | Manager                | False    |
    | MedicalCenter          | False    |
    | EthicalCommitteeMember | False    |

Scenario Outline: Editing processing request for same client
	Given User with role <rolename>
	  And Belongs to client 1
	  And Request from client 1
	  And Request status is 'Processing'
	When Get actions for request
	Then Common.Edit action <contains> in the actions list
	
  Examples:
    | rolename               | contains |
    | Administrator          | True     |
    | Manager                | False    |
    | MedicalCenter          | False    |
    | EthicalCommitteeMember | False    |

Scenario Outline: Editing invalid request for same client
	Given User with role <rolename>
	  And Belongs to client 1
	  And Request from client 1
	  And Request status is 'InvalidSubmission'
	When Get actions for request
	Then Common.Edit action <contains> in the actions list
	
  Examples:
    | rolename               | contains |
    | Administrator          | True     |
    | Manager                | True     |
    | MedicalCenter          | True     |
    | EthicalCommitteeMember | False    |

Scenario Outline: Editing request which need more information for same client
	Given User with role <rolename>
	  And Belongs to client 1
	  And Request from client 1
	  And Request status is 'NeedMoreInformation'
	When Get actions for request
	Then Common.Edit action <contains> in the actions list
	
  Examples:
    | rolename               | contains |
    | Administrator          | True     |
    | Manager                | True     |
    | MedicalCenter          | True     |
    | EthicalCommitteeMember | False    |

Scenario Outline: Editing resolved request for same client
	Given User with role <rolename>
	  And Belongs to client 1
	  And Request from client 1
	  And Request status is 'Resolved'
	When Get actions for request
	Then Common.Edit action <contains> in the actions list
	
  Examples:
    | rolename               | contains |
    | Administrator          | True     |
    | Manager                | False    |
    | MedicalCenter          | False    |
    | EthicalCommitteeMember | False    |

Scenario Outline: Submitting created request for same client
	Given User with role <rolename>
	  And Belongs to client 1
	  And Request from client 1
	  And Request status is 'Created'
	When Get actions for request
	Then Request.Submit action <contains> in the actions list
	
  Examples:
    | rolename               | contains |
    | Administrator          | False    |
    | Manager                | True     |
    | MedicalCenter          | False    |
    | EthicalCommitteeMember | False    |

Scenario Outline: Submitting submitted request for same client
	Given User with role <rolename>
	  And Belongs to client 1
	  And Request from client 1
	  And Request status is 'Submitted'
	When Get actions for request
	Then Request.Submit action <contains> in the actions list
	
  Examples:
    | rolename               | contains |
    | Administrator          | False    |
    | Manager                | False    |
    | MedicalCenter          | False    |
    | EthicalCommitteeMember | False    |

Scenario Outline: Revoke created request for same client
	Given User with role <rolename>
	  And Belongs to client 1
	  And Request from client 1
	  And Request status is 'Created'
	When Get actions for request
	Then Request.Revoke action <contains> in the actions list
	
  Examples:
    | rolename               | contains |
    | Administrator          | False    |
    | Manager                | False    |
    | MedicalCenter          | False    |
    | EthicalCommitteeMember | False    |

Scenario Outline: Revoke submitted request for same client
	Given User with role <rolename>
	  And Belongs to client 1
	  And Request from client 1
	  And Request status is 'Submitted'
	When Get actions for request
	Then Request.Revoke action <contains> in the actions list
	
  Examples:
    | rolename               | contains |
    | Administrator          | False    |
    | Manager                | True     |
    | MedicalCenter          | False    |
    | EthicalCommitteeMember | False    |

Scenario: Set meeting for accepted request for same client
	Given User with role EthicalCommitteeMember
	  And Belongs to client 1
	  And User is secretary for committee 1
	  And Request from client 1
	  And Request for committee 1
	  And Request status is 'Accepted'
	When Get actions for request
	Then Request.SetMeeting action True in the actions list

Scenario Outline: Set meeting for accepted request for same client when no secretary
	Given User with role <rolename>
	  And Belongs to client 1
	  And Belongs to committee 1
	  And Request from client 1
	  And Request for committee 1
	  And Request status is 'Accepted'
	When Get actions for request
	Then Request.SetMeeting action <contains> in the actions list
	
  Examples:
    | rolename               | contains |
    | Administrator          | False    |
    | Manager                | False    |
    | MedicalCenter          | False    |
    | EthicalCommitteeMember | False    |

Scenario Outline: Could not start meeting when not secretary or chairman
	Given User with role <rolename>
	  And Belongs to client 1
	  And Belongs to committee 1
	  And Request from client 1
	  And Request for committee 1
	  And Request status is 'MeetingSet'
	When Get actions for request
	Then Request.StartMeeting action <contains> in the actions list
	
  Examples:
    | rolename               | contains |
    | Administrator          | False    |
    | Manager                | False    |
    | MedicalCenter          | False    |
    | EthicalCommitteeMember | False    |

Scenario: Could start meeting when secretary
	Given User with role EthicalCommitteeMember
	  And Belongs to client 1
	  And Belongs to committee 1
	  And User is secretary for committee 1
	  And Request from client 1
	  And Request for committee 1
	  And Request status is 'MeetingSet'
	When Get actions for request
	Then Request.StartMeeting action True in the actions list

Scenario: Could start meeting when chairman
	Given User with role EthicalCommitteeMember
	  And Belongs to client 1
	  And Belongs to committee 1
	  And User is chairman for committee 1
	  And Request from client 1
	  And Request for committee 1
	  And Request status is 'MeetingSet'
	When Get actions for request
	Then Request.StartMeeting action True in the actions list

Scenario: Could not stop meeting when secretary
	Given User with role EthicalCommitteeMember
	  And Belongs to client 1
	  And Belongs to committee 1
	  And User is secretary for committee 1
	  And Request from client 1
	  And Request for committee 1
	  And Request status is 'Processing'
	When Get actions for request
	Then Request.StopMeeting action False in the actions list

Scenario: Could stop meeting when chairman
	Given User with role EthicalCommitteeMember
	  And Belongs to client 1
	  And Belongs to committee 1
	  And User is chairman for committee 1
	  And Request from client 1
	  And Request for committee 1
	  And Request status is 'Processing'
	When Get actions for request
	Then Request.StopMeeting action True in the actions list

Scenario: Meeting should start when start time passed
Given Request from client 1
  And has meeting started 2 minutes ago
 When server start pending meeting
 Then Request status now is 'Processing'

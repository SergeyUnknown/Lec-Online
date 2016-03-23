/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

:r ".\ReferenceData\AspNetRoles.sql"
:r ".\ReferenceData\StudyTypes.sql"
:r ".\ReferenceData\RequestStatuses.sql"
:r ".\ReferenceData\StudyTypes.sql"
:r ".\ReferenceData\AttendanceStatuses.sql"
:r ".\ReferenceData\ObjectTypes.sql"

/* Default admin user */
:r ".\ReferenceData\AdminUser.sql"

if '$(TestData)' = '1'
begin
	/* Create clients and committees first, since users relate to these data*/
	:r ".\ReferenceData\Clients.sql"
	:r ".\ReferenceData\Commitees.sql"
	
	/* Users data */
	:r ".\ReferenceData\AspNetUsers.sql"
	:r ".\ReferenceData\AspNetUserRoles.sql"
	
	/* Domain data */
	:r ".\ReferenceData\Requests.sql"
	:r ".\ReferenceData\Meetings.sql"
	:r ".\ReferenceData\MeetingAttendees.sql"

	/* Committee membership */
	:r ".\ReferenceData\Committees-Membership.sql"
end

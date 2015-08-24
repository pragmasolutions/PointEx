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

:r .\Roles.Data.sql	
:r .\Categories.Data.sql
:r .\Section.Data.sql
:r .\BenefitTypes.Data.sql
:r .\BenefitStatus.Data.sql
:r .\Towns.Data.sql
:r .\EducationalInstitution.Data.sql
:r .\Users.Data.sql
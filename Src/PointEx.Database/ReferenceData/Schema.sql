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


IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Student_AspNetUsers]') AND parent_object_id = OBJECT_ID(N'[dbo].[Student]'))
ALTER TABLE [dbo].[Student] DROP CONSTRAINT [FK_Student_AspNetUsers]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Student_EducationalInstitution]') AND parent_object_id = OBJECT_ID(N'[dbo].[Student]'))
ALTER TABLE [dbo].[Student] DROP CONSTRAINT [FK_Student_EducationalInstitution]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Student_Town]') AND parent_object_id = OBJECT_ID(N'[dbo].[Student]'))
ALTER TABLE [dbo].[Student] DROP CONSTRAINT [FK_Student_Town]
GO


/****** Object:  Table [dbo].[Student]    Script Date: 05/19/2015 18:42:13 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Student]') AND type in (N'U'))
DROP TABLE [dbo].[Student]
GO


IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Reward_RewardCatalog]') AND parent_object_id = OBJECT_ID(N'[dbo].[Reward]'))
ALTER TABLE [dbo].[Reward] DROP CONSTRAINT [FK_Reward_RewardCatalog]
GO

/****** Object:  Table [dbo].[Reward]    Script Date: 05/20/2015 20:30:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Reward]') AND type in (N'U'))
DROP TABLE [dbo].[Reward]
GO

/****** Object:  Table [dbo].[RewardCatalog]    Script Date: 05/20/2015 20:30:17 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RewardCatalog]') AND type in (N'U'))
DROP TABLE [dbo].[RewardCatalog]
GO


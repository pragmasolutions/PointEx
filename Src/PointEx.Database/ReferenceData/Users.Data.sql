INSERT INTO [dbo].[AspNetUsers] ([Id], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName], [IsDeleted])
SELECT  N'95f8f8b8-2e50-467a-aea7-3375a558437c', N'admin@gmail.com', 1, N'AKe9MORT0Y+QH3z/uVLbM4t84FeEimGFz8W0Eq7jRISuZAf+KFoEB3BZJHsW348HwA==', N'f5881713-de90-4e3e-9a10-18850f6f5b68', NULL, 0, 0, NULL, 1, 0, N'admin@gmail.com', 0
WHERE NOT EXISTS (SELECT 1 FROM [dbo].[AspNetUsers] WHERE Id = '95f8f8b8-2e50-467a-aea7-3375a558437c')

INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) 
SELECT N'95f8f8b8-2e50-467a-aea7-3375a558437c', N'DD5A4B97-547B-493F-BD35-CFC1F54EC698'
WHERE NOT EXISTS (SELECT 1 FROM [dbo].[AspNetUserRoles] WHERE [UserId] = N'95f8f8b8-2e50-467a-aea7-3375a558437c' AND [RoleId] = N'DD5A4B97-547B-493F-BD35-CFC1F54EC698')


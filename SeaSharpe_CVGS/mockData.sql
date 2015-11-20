SET IDENTITY_INSERT [dbo].[Platforms] ON
INSERT INTO [dbo].[Platforms] ([Id], [Name]) VALUES (1, N'XBOX')
INSERT INTO [dbo].[Platforms] ([Id], [Name]) VALUES (2, N'PC')
INSERT INTO [dbo].[Platforms] ([Id], [Name]) VALUES (3, N'PS4')
INSERT INTO [dbo].[Platforms] ([Id], [Name]) VALUES (4, N'Wii')
INSERT INTO [dbo].[Platforms] ([Id], [Name]) VALUES (5, N'Mobile')
SET IDENTITY_INSERT [dbo].[Platforms] OFF

SET IDENTITY_INSERT [dbo].[Categories] ON
INSERT INTO [dbo].[Categories] ([Id], [Name]) VALUES (1, N'Action')
INSERT INTO [dbo].[Categories] ([Id], [Name]) VALUES (2, N'Adventure')
INSERT INTO [dbo].[Categories] ([Id], [Name]) VALUES (3, N'RPG')
INSERT INTO [dbo].[Categories] ([Id], [Name]) VALUES (4, N'FPS')
INSERT INTO [dbo].[Categories] ([Id], [Name]) VALUES (5, N'RTS')
INSERT INTO [dbo].[Categories] ([Id], [Name]) VALUES (6, N'Puzzle')
SET IDENTITY_INSERT [dbo].[Categories] OFF

SET IDENTITY_INSERT [dbo].[Games] ON
INSERT INTO [dbo].[Games] ([Id], [Name], [ReleaseDate], [SuggestedRetailPrice], [Platform_Id], [ImagePath], [Publisher], [ESRB]) VALUES ('1', 'Peter', '11/11/2015 12:00:00 AM', '19.99', '3', 'http://files.sabotagetimes.com/image/upload/MTI5NDc3Mjk5NTgxMDY1Njk0.jpg', 'test', 'E')
INSERT INTO [dbo].[Games] ([Id], [Name], [ReleaseDate], [SuggestedRetailPrice], [Platform_Id], [ImagePath], [Publisher], [ESRB]) VALUES ('2', 'Test', '11/09/2015 12:00:00 AM', '2.00', '1', 'http://files.sabotagetimes.com/image/upload/MTI5NDc3Mjk5NTgxMDY1Njk0.jpg', 'test', 'test')
INSERT INTO [dbo].[Games] ([Id], [Name], [ReleaseDate], [SuggestedRetailPrice], [Platform_Id], [ImagePath], [Publisher], [ESRB]) VALUES ('4', 'Skyrim', '10/11/2015 12:00:00 AM', '20.00', '2', 'http://files.sabotagetimes.com/image/upload/MTI5NDc3Mjk5NTgxMDY1Njk0.jpg', 'test', 'test')
INSERT INTO [dbo].[Games] ([Id], [Name], [ReleaseDate], [SuggestedRetailPrice], [Platform_Id], [ImagePath], [Publisher], [ESRB]) VALUES ('5', 'rwar', '04/11/2015 12:00:00 AM', '1.00', '1', 'http://files.sabotagetimes.com/image/upload/MTI5NDc3Mjk5NTgxMDY1Njk0.jpg', 'rwar', 'rwar')
INSERT INTO [dbo].[Games] ([Id], [Name], [ReleaseDate], [SuggestedRetailPrice], [Platform_Id], [ImagePath], [Publisher], [ESRB]) VALUES ('7', 'rwar', '10/11/2015 12:00:00 AM', '1.00', '1', 'http://files.sabotagetimes.com/image/upload/MTI5NDc3Mjk5NTgxMDY1Njk0.jpg', 'rwar', 'rwar')
INSERT INTO [dbo].[Games] ([Id], [Name], [ReleaseDate], [SuggestedRetailPrice], [Platform_Id], [ImagePath], [Publisher], [ESRB]) VALUES ('8', 'test', '03/11/2015 12:00:00 AM', '2.00', '1', 'http://files.sabotagetimes.com/image/upload/MTI5NDc3Mjk5NTgxMDY1Njk0.jpg', 'test', 'E')
INSERT INTO [dbo].[Games] ([Id], [Name], [ReleaseDate], [SuggestedRetailPrice], [Platform_Id], [ImagePath], [Publisher], [ESRB]) VALUES ('9', 'test', '03/11/2015 12:00:00 AM', '2.00', '1', 'http://files.sabotagetimes.com/image/upload/MTI5NDc3Mjk5NTgxMDY1Njk0.jpg', 'test', 'AO')
INSERT INTO [dbo].[Games] ([Id], [Name], [ReleaseDate], [SuggestedRetailPrice], [Platform_Id], [ImagePath], [Publisher], [ESRB]) VALUES ('10', 'test', '26/11/2015 12:00:00 AM', '20.00', '2', 'http://files.sabotagetimes.com/image/upload/MTI5NDc3Mjk5NTgxMDY1Njk0.jpg', 'NULL', 'EC')
INSERT INTO [dbo].[Games] ([Id], [Name], [ReleaseDate], [SuggestedRetailPrice], [Platform_Id], [ImagePath], [Publisher], [ESRB]) VALUES ('11', 'test', '16/12/2015 12:00:00 AM', '20.00', '2', 'http://files.sabotagetimes.com/image/upload/MTI5NDc3Mjk5NTgxMDY1Njk0.jpg', 'NULL', 'EC')
INSERT INTO [dbo].[Games] ([Id], [Name], [ReleaseDate], [SuggestedRetailPrice], [Platform_Id], [ImagePath], [Publisher], [ESRB]) VALUES ('12', 'test', '02/11/2015 12:00:00 AM', '20.00', '2', 'http://files.sabotagetimes.com/image/upload/MTI5NDc3Mjk5NTgxMDY1Njk0.jpg', 'NULL', 'EC')
SET IDENTITY_INSERT [dbo].[Games] OFF

INSERT INTO [dbo].[GameCategories] ([Game_Id], [Category_Id]) VALUES (1, 1)
INSERT INTO [dbo].[GameCategories] ([Game_Id], [Category_Id]) VALUES (4, 1)
INSERT INTO [dbo].[GameCategories] ([Game_Id], [Category_Id]) VALUES (1, 2)
INSERT INTO [dbo].[GameCategories] ([Game_Id], [Category_Id]) VALUES (4, 2)
INSERT INTO [dbo].[GameCategories] ([Game_Id], [Category_Id]) VALUES (8, 2)
INSERT INTO [dbo].[GameCategories] ([Game_Id], [Category_Id]) VALUES (1, 3)
INSERT INTO [dbo].[GameCategories] ([Game_Id], [Category_Id]) VALUES (5, 3)
INSERT INTO [dbo].[GameCategories] ([Game_Id], [Category_Id]) VALUES (10, 3)
INSERT INTO [dbo].[GameCategories] ([Game_Id], [Category_Id]) VALUES (11, 3)
INSERT INTO [dbo].[GameCategories] ([Game_Id], [Category_Id]) VALUES (12, 3)
INSERT INTO [dbo].[GameCategories] ([Game_Id], [Category_Id]) VALUES (10, 4)
INSERT INTO [dbo].[GameCategories] ([Game_Id], [Category_Id]) VALUES (11, 4)
INSERT INTO [dbo].[GameCategories] ([Game_Id], [Category_Id]) VALUES (12, 4)
INSERT INTO [dbo].[GameCategories] ([Game_Id], [Category_Id]) VALUES (4, 6)
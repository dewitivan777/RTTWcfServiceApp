USE [RTTTest]
GO
/****** Object:  Table [dbo].[UserAddressTable]    Script Date: 7/2/2020 8:17:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserAddressTable](
	[Id] [nvarchar](100) NULL,
	[UserId] [nvarchar](100) NULL,
	[Address] [nvarchar](100) NULL,
	[AddressType] [nvarchar](100) NULL,
	[City] [nvarchar](100) NULL,
	[Province] [nvarchar](100) NULL,
	[ZipCode_PostalCode] [int] NULL,
	[DateCreated] [datetime2](7) NULL,
	[DateUpdated] [datetime2](7) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserTable]    Script Date: 7/2/2020 8:17:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserTable](
	[UserId] [nvarchar](100) NOT NULL,
	[FirstName] [varchar](100) NULL,
	[Surname] [nvarchar](100) NULL,
	[DOB] [datetime2](7) NULL,
	[Gender] [nvarchar](100) NULL,
	[Mobile] [nvarchar](100) NULL,
	[Email] [nvarchar](100) NULL,
	[WorkMobile] [nvarchar](100) NULL,
	[DateCreated] [datetime2](7) NULL,
	[DateUpdated] [datetime2](7) NULL
) ON [PRIMARY]
GO
INSERT [dbo].[UserAddressTable] ([Id], [UserId], [Address], [AddressType], [City], [Province], [ZipCode_PostalCode], [DateCreated], [DateUpdated]) VALUES (N'3c75291396944f13a6e249120023ea37', N'0e74aee389ed4dd99573e15404b96c0d', N'40 Terry Street', N'Residential', N'Pretoria', N'Gauteng', 181, CAST(N'2020-07-02T13:00:18.2733333' AS DateTime2), CAST(N'2020-07-02T19:05:38.5300000' AS DateTime2))
INSERT [dbo].[UserAddressTable] ([Id], [UserId], [Address], [AddressType], [City], [Province], [ZipCode_PostalCode], [DateCreated], [DateUpdated]) VALUES (N'bf509c6ef51f4e32874eaffacad79355', N'0e74aee389ed4dd99573e15404b96c0d', N'50 Ally Road, Ally Office Park, Building 4', N'Business', N'Johannesburg', N'Gauteng', 2000, CAST(N'2020-07-02T18:30:58.2433333' AS DateTime2), CAST(N'2020-07-02T19:07:21.3166667' AS DateTime2))
INSERT [dbo].[UserTable] ([UserId], [FirstName], [Surname], [DOB], [Gender], [Mobile], [Email], [WorkMobile], [DateCreated], [DateUpdated]) VALUES (N'0e74aee389ed4dd99573e15404b96c0d', N'Ivan', N'de Wit', CAST(N'1753-01-01T00:00:00.0000000' AS DateTime2), N'Male', N'0846889966', N'dewit.ivan777@gmail.com', N'0846889966', CAST(N'2020-07-01T15:43:23.8200000' AS DateTime2), CAST(N'2020-07-02T19:50:24.4966667' AS DateTime2))
INSERT [dbo].[UserTable] ([UserId], [FirstName], [Surname], [DOB], [Gender], [Mobile], [Email], [WorkMobile], [DateCreated], [DateUpdated]) VALUES (N'98f687a9073c422581dacd62bf8bcbac', N'Rodney', N'Yello', CAST(N'1999-02-01T00:00:00.0000000' AS DateTime2), N'Male', N'083453421', N'dsad@gmail.com', N'083453421', CAST(N'2020-07-01T16:37:55.9700000' AS DateTime2), CAST(N'2020-07-01T16:37:55.9700000' AS DateTime2))
INSERT [dbo].[UserTable] ([UserId], [FirstName], [Surname], [DOB], [Gender], [Mobile], [Email], [WorkMobile], [DateCreated], [DateUpdated]) VALUES (N'080c1921d8904de3b7da0b9a890d5e9e', N'Harry', N'Keen', CAST(N'1753-01-01T00:00:00.0000000' AS DateTime2), N'Male', N'0907567544', N'harry@gmail.com', N'0907567544', CAST(N'2020-07-02T10:25:07.5066667' AS DateTime2), CAST(N'2020-07-02T10:25:07.7133333' AS DateTime2))
INSERT [dbo].[UserTable] ([UserId], [FirstName], [Surname], [DOB], [Gender], [Mobile], [Email], [WorkMobile], [DateCreated], [DateUpdated]) VALUES (N'676d34aa8e1641debbaf14028099090b', N'Susan', N'Fourie', CAST(N'1985-04-05T00:00:00.0000000' AS DateTime2), N'Female', N'0845665656', N'susi@gmail.com', N'0845665656', CAST(N'2020-07-02T10:32:22.3800000' AS DateTime2), CAST(N'2020-07-02T10:32:22.3800000' AS DateTime2))

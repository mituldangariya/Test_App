USE [AuthEmployeeDB]
GO
/****** Object:  Table [dbo].[Employees]    Script Date: 1/1/2026 11:11:44 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Employees](
	[EmployeeId] [int] IDENTITY(1,1) NOT NULL,
	[FullName] [nvarchar](100) NOT NULL,
	[Email] [nvarchar](100) NOT NULL,
	[Phone] [nvarchar](15) NULL,
	[Salary] [decimal](18, 2) NULL,
	[Photo] [nvarchar](200) NULL,
	[UserId] [int] NOT NULL,
	[IsActive] [bit] NULL,
	[CreatedDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[EmployeeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 1/1/2026 11:11:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[FullName] [nvarchar](100) NOT NULL,
	[Email] [nvarchar](100) NOT NULL,
	[PasswordHash] [nvarchar](200) NOT NULL,
	[Role] [nvarchar](20) NOT NULL,
	[IsActive] [bit] NULL,
	[CreatedDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Employees] ON 

INSERT [dbo].[Employees] ([EmployeeId], [FullName], [Email], [Phone], [Salary], [Photo], [UserId], [IsActive], [CreatedDate]) VALUES (1, N'duck', N'duck@gmail.com', N'987453210', CAST(100.25 AS Decimal(18, 2)), N'a4ba9af4-dc2f-4279-b948-1a3a37c17923.jpg', 1, 0, CAST(N'2026-01-01T08:22:48.707' AS DateTime))
INSERT [dbo].[Employees] ([EmployeeId], [FullName], [Email], [Phone], [Salary], [Photo], [UserId], [IsActive], [CreatedDate]) VALUES (2, N'Test', N'test@gmail.com', N'7410258930', CAST(789541230.00 AS Decimal(18, 2)), N'585758f5-4a3d-48c4-b071-b60fca457d0a.jpg', 2, 0, CAST(N'2026-01-01T08:55:20.857' AS DateTime))
SET IDENTITY_INSERT [dbo].[Employees] OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([UserId], [FullName], [Email], [PasswordHash], [Role], [IsActive], [CreatedDate]) VALUES (1, N'duck', N'duck@gmail.com', N'JHquN1mUxQJqaZW8tg+eaq+ra6B/0uObTgdRemp6224=', N'User', 1, CAST(N'2025-12-31T23:07:47.307' AS DateTime))
INSERT [dbo].[Users] ([UserId], [FullName], [Email], [PasswordHash], [Role], [IsActive], [CreatedDate]) VALUES (2, N'admin', N'admin@gmail.com', N'6G94qKPK8LYNjnTllCqm2G3BUM08AzOK7yW30tfjrMc=', N'Admin', 1, CAST(N'2026-01-01T08:31:35.923' AS DateTime))
INSERT [dbo].[Users] ([UserId], [FullName], [Email], [PasswordHash], [Role], [IsActive], [CreatedDate]) VALUES (3, N'Jack John', N'jack@gmail.com', N'x72584asTM17Y+bvC+jWl591WFImrQ2mgz8nHiANxbw=', N'User', 1, CAST(N'2026-01-01T08:33:33.063' AS DateTime))
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Users__A9D105344F6F978F]    Script Date: 1/1/2026 11:11:45 AM ******/
ALTER TABLE [dbo].[Users] ADD UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Employees] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Employees] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
/****** Object:  StoredProcedure [dbo].[sp_Employee_Delete]    Script Date: 1/1/2026 11:11:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[sp_Employee_Delete]
(
    @EmployeeId INT
)
AS
BEGIN
    UPDATE Employees SET IsActive=0 WHERE EmployeeId=@EmployeeId
END

GO
/****** Object:  StoredProcedure [dbo].[sp_Employee_GetAll]    Script Date: 1/1/2026 11:11:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[sp_Employee_GetAll]
AS
BEGIN
    SELECT * 
    FROM Employees
    WHERE IsActive = 1
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Employee_GetById]    Script Date: 1/1/2026 11:11:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[sp_Employee_GetById]
(
    @EmployeeId INT
)
AS
BEGIN
    SELECT * FROM Employees WHERE EmployeeId=@EmployeeId
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Employee_GetByUser]    Script Date: 1/1/2026 11:11:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[sp_Employee_GetByUser]
(
    @UserId INT
)
AS
BEGIN
    SELECT * FROM Employees
    WHERE UserId=@UserId AND IsActive=1
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Employee_Insert]    Script Date: 1/1/2026 11:11:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[sp_Employee_Insert]
(
    @FullName NVARCHAR(100),
    @Email NVARCHAR(100),
    @Phone NVARCHAR(15),
    @Salary DECIMAL(18,2),
    @Photo NVARCHAR(200),
    @UserId INT
)
AS
BEGIN
    INSERT INTO Employees
    VALUES (@FullName,@Email,@Phone,@Salary,@Photo,@UserId,1,GETDATE())
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Employee_Update]    Script Date: 1/1/2026 11:11:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[sp_Employee_Update]
(
    @EmployeeId INT,
    @FullName NVARCHAR(100),
    @Phone NVARCHAR(15),
    @Salary DECIMAL(18,2),
    @Photo NVARCHAR(200)
)
AS
BEGIN
    UPDATE Employees
    SET FullName=@FullName,
        Phone=@Phone,
        Salary=@Salary,
        Photo=@Photo
    WHERE EmployeeId=@EmployeeId
END
GO
/****** Object:  StoredProcedure [dbo].[sp_User_Login]    Script Date: 1/1/2026 11:11:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_User_Login]
(
    @Email NVARCHAR(100)
)
AS
BEGIN
    SELECT * FROM Users
    WHERE Email=@Email AND IsActive=1
END
GO
/****** Object:  StoredProcedure [dbo].[sp_User_Register]    Script Date: 1/1/2026 11:11:45 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_User_Register]
(
    @FullName NVARCHAR(100),
    @Email NVARCHAR(100),
    @PasswordHash NVARCHAR(200),
    @Role NVARCHAR(20)
)
AS
BEGIN
    IF EXISTS (SELECT 1 FROM Users WHERE Email=@Email)
    BEGIN
        RAISERROR('Email already exists',16,1)
        RETURN
    END

    INSERT INTO Users
    VALUES (@FullName,@Email,@PasswordHash,@Role,1,GETDATE())
END
GO

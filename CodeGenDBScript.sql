USE [CodeGenToolDb]
GO
ALTER TABLE [dbo].[Projects] DROP CONSTRAINT [FK_Projects_Users]
GO
ALTER TABLE [dbo].[Deployments] DROP CONSTRAINT [FK_Deployments_Projects]
GO
ALTER TABLE [dbo].[Projects] DROP CONSTRAINT [DF__Projects__Status__398D8EEE]
GO
ALTER TABLE [dbo].[Projects] DROP CONSTRAINT [DF__Projects__Update__38996AB5]
GO
ALTER TABLE [dbo].[Projects] DROP CONSTRAINT [DF__Projects__Create__37A5467C]
GO
ALTER TABLE [dbo].[Deployments] DROP CONSTRAINT [DF__Deploymen__Creat__3E52440B]
GO
ALTER TABLE [dbo].[Deployments] DROP CONSTRAINT [DF__Deploymen__Statu__3D5E1FD2]
GO
ALTER TABLE [dbo].[ApplicationUsers] DROP CONSTRAINT [DF__Applicati__Creat__34C8D9D1]
GO
/****** Object:  Table [dbo].[Projects]    Script Date: 13-07-2025 00:46:59 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Projects]') AND type in (N'U'))
DROP TABLE [dbo].[Projects]
GO
/****** Object:  Table [dbo].[Deployments]    Script Date: 13-07-2025 00:46:59 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Deployments]') AND type in (N'U'))
DROP TABLE [dbo].[Deployments]
GO
/****** Object:  Table [dbo].[ApplicationUsers]    Script Date: 13-07-2025 00:46:59 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ApplicationUsers]') AND type in (N'U'))
DROP TABLE [dbo].[ApplicationUsers]
GO
/****** Object:  Table [dbo].[ApplicationUsers]    Script Date: 13-07-2025 00:46:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ApplicationUsers](
	[Id] [nvarchar](450) NOT NULL,
	[FirstName] [nvarchar](100) NOT NULL,
	[LastName] [nvarchar](100) NOT NULL,
	[Email] [nvarchar](256) NOT NULL,
	[PasswordHash] [nvarchar](512) NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Deployments]    Script Date: 13-07-2025 00:46:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Deployments](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ProjectId] [int] NOT NULL,
	[Environment] [nvarchar](100) NOT NULL,
	[DeploymentUrl] [nvarchar](500) NULL,
	[Status] [int] NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[Logs] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Projects]    Script Date: 13-07-2025 00:46:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Projects](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Requirements] [nvarchar](max) NULL,
	[MermaidDiagram] [nvarchar](max) NULL,
	[GeneratedCode] [nvarchar](max) NULL,
	[UserId] [nvarchar](450) NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[UpdatedAt] [datetime2](7) NOT NULL,
	[Status] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[ApplicationUsers] ADD  DEFAULT (getutcdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Deployments] ADD  DEFAULT ((0)) FOR [Status]
GO
ALTER TABLE [dbo].[Deployments] ADD  DEFAULT (getutcdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Projects] ADD  DEFAULT (getutcdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Projects] ADD  DEFAULT (getutcdate()) FOR [UpdatedAt]
GO
ALTER TABLE [dbo].[Projects] ADD  DEFAULT ((0)) FOR [Status]
GO
ALTER TABLE [dbo].[Deployments]  WITH CHECK ADD  CONSTRAINT [FK_Deployments_Projects] FOREIGN KEY([ProjectId])
REFERENCES [dbo].[Projects] ([Id])
GO
ALTER TABLE [dbo].[Deployments] CHECK CONSTRAINT [FK_Deployments_Projects]
GO
ALTER TABLE [dbo].[Projects]  WITH CHECK ADD  CONSTRAINT [FK_Projects_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[ApplicationUsers] ([Id])
GO
ALTER TABLE [dbo].[Projects] CHECK CONSTRAINT [FK_Projects_Users]
GO

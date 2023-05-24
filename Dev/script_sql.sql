create database Poc
go
use Poc
go

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[NoteLists](
	[Id] [varchar](50) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[State] [int] NOT NULL,
	[CreationDate] [datetime] NULL,
	[LastUpdateDate] [datetime] NULL,
	[CreatorUser] [nvarchar](50) NULL,
	[UpdaterUser] [nvarchar](50) NULL
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Notes](
	[Id] [varchar](50) NOT NULL,
	[Title] [varchar](50) NOT NULL,
	[State] [int] NOT NULL,
	[CreationDate] [datetime] NOT NULL,
	[LastUpdateDate] [datetime] NULL,
	[CreatorUser] [varchar](50) NULL,
	[UpdaterUser] [varchar](50) NULL,
	[ListId] [varchar](50) NULL
) ON [PRIMARY]
GO






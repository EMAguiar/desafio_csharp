USE [DESAFIO]
GO

/****** Object:  Table [dbo].[Pessoa]    Script Date: 18/01/2021 04:29:53 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Pessoa](
	[id] [int] NOT NULL,
	[nome] [char](50) NOT NULL,
	[username] [char](30) NOT NULL,
	[street] [char](150) NOT NULL,
	[suite] [char](20) NOT NULL,
	[city] [char](30) NOT NULL,
	[zipcode] [char](15) NOT NULL,
	[lat] [float] NOT NULL,
	[lng] [float] NOT NULL,
	[telefone] [char](30) NOT NULL,
	[website] [char](50) NOT NULL,
	[cnome] [char](100) NOT NULL,
	[catchphrase] [char](100) NOT NULL,
	[bs] [char](100) NOT NULL
) ON [PRIMARY]
GO



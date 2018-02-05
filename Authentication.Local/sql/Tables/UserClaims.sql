if(exists (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'dbo' 
                 AND  TABLE_NAME = 'UserClaims'))
begin
	drop table [dbo].[UserClaims]
end

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[UserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[Type] [varchar](200) NOT NULL,
	[Value] [varchar](max) NOT NULL,
	[ValueType] varchar(255) NULL default('http://www.w3.org/2001/XMLSchema#string'),
	[Issuer] varchar(255) NULL default('Custom Authority')
	PRIMARY KEY(Id),
	CONSTRAINT FK_UserClaims FOREIGN KEY(UserId)
	REFERENCES [User]([Id]) ON DELETE CASCADE
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO



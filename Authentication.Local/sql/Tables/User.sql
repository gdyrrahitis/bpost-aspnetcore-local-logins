if(OBJECT_ID('FK_UserClaims', 'F') IS NOT NULL)
begin
	alter table [dbo].[UserClaims] drop constraint FK_UserClaims;
end

if(exists (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'dbo' 
                 AND  TABLE_NAME = 'User'))
begin
	drop table [dbo].[User]
end

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[User](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Username] varchar(255) UNIQUE NOT NULL,
	[Password] varchar(max) NOT NULL,
	[FirstName] [varchar](200) NOT NULL,
	[Surname] [varchar](max) NOT NULL,
	[Email] varchar(255) NOT NULL,
	[DateOfBirth] datetime NOT NULL
	PRIMARY KEY(Id)
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO



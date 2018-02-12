if(exists (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'dbo' 
                 AND  TABLE_NAME = 'Log'))
begin
	drop table [dbo].[Log]
end

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Log](
	[Id] uniqueidentifier default(newsequentialid()) not null,
	[Date] datetime not null,
	[EventId] varchar(255) not null,
	[Level] varchar(50) not null,
	[Logger] varchar(255) not null,
	[Message] varchar(max) not null,
	[Exception] varchar(max) null
	PRIMARY KEY NONCLUSTERED (Id asc)
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO



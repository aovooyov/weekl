CREATE TABLE [LOG].[Entry]
(
	[Id] INT NOT NULL  IDENTITY(1,1),
	[MachineName] [nvarchar](200) NULL,
	[SiteName] [nvarchar](200) NOT NULL,
	[Logged] [datetime] NOT NULL,
	[Level] [varchar](5) NOT NULL,
	[UserName] [nvarchar](200) NULL,
	[Message] [nvarchar](max) NOT NULL,
	[Logger] [nvarchar](300) NULL,
	[Properties] [nvarchar](max) NULL,
	[ServerName] [nvarchar](200) NULL,
	[Port] [nvarchar](100) NULL,
	[Url] [nvarchar](2000) NULL,
	[Https] [bit] NULL,
	[ServerAddress] [nvarchar](100) NULL,
	[RemoteAddress] [nvarchar](100) NULL,
	[Callsite] [nvarchar](300) NULL,
	[Exception] [nvarchar](max) NULL,
	CONSTRAINT [PK#LOG@Entry@Id] PRIMARY KEY CLUSTERED ([Id] ASC)
	WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY];

GO
create nonclustered index [IX#LOG@Entry@Logged]
    on [LOG].[Entry] ([Logged] desc) include ([Level], [Message], [Exception]);


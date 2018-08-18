CREATE TABLE [FEED].[Channel]
(
	[Id] INT NOT NULL IDENTITY(1000, 1),
	[SourceId] INT NOT NULL,
	[Name] NVARCHAR(150) NOT NULL,
	[Link] NVARCHAR(250) NOT NULL, 
	[Selector] NVARCHAR(250) NULL, 
	[Encoding] NVARCHAR(20) NOT NULL DEFAULT N'utf-8', 
	[Type] INT DEFAULT 1 NOT NULL,
	[DateCreated] DATETIME2 (7) CONSTRAINT [DF#FEED@Channel@DateCreated] DEFAULT (sysdatetime()) NOT NULL,
	[DateUpdated] DATETIME2 (7) CONSTRAINT [DF#FEED@Channel@DateUpdated] DEFAULT (sysdatetime()) NOT NULL,
    CONSTRAINT [PK#FEED@Channel@ID] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK#FEED@SourceId#Source@Id] FOREIGN KEY ([SourceId]) REFERENCES [FEED].[Source] ([Id]) ON DELETE CASCADE
)

--GO
--CREATE NONCLUSTERED INDEX [IX#FEED@Channel@Id@SourceId]
--    ON [FEED].[Channel] ([Id], [SourceId]) INCLUDE ([Id], [SourceId], [Link], [Name]);
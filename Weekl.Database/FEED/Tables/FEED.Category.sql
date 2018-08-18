CREATE TABLE [FEED].[Category]
(
	[Id] INT NOT NULL IDENTITY(1000, 1),
	[Name] nvarchar(150) NOT NULL, 
    [Unique] NVARCHAR(150) NOT NULL,
	[DateCreated] DATETIME2 (7) CONSTRAINT [DF#FEED@Category@DateCreated] DEFAULT (sysdatetime()) NOT NULL,
	CONSTRAINT [PK#FEED@Category@ID] PRIMARY KEY CLUSTERED ([Id] ASC)
)

GO
CREATE NONCLUSTERED INDEX [IX#FEED@Category@Unique]
    ON [FEED].[Category] ([Unique]) INCLUDE ([Id], [Name]);

CREATE TABLE [FEED].[Source]
(
	[Id] INT NOT NULL IDENTITY(1000, 1),
	[Name] NVARCHAR(150) NOT NULL,
	[Link] NVARCHAR(250) NOT NULL, 
    [Description] NVARCHAR(MAX) NULL, 
	[ImageUrl] NVARCHAR(250) NULL,
	[DateCreated] DATETIME2 (7) CONSTRAINT [DF#FEED@Source@DateCreated] DEFAULT (sysdatetime()) NOT NULL,
    [Unique] NVARCHAR(150) NULL, 
    CONSTRAINT [PK#FEED@Source@ID] PRIMARY KEY CLUSTERED ([Id] ASC)
)

--GO
--CREATE NONCLUSTERED INDEX [IX#FEED@Source@Id]
--    ON [FEED].[Source] ([Id]) INCLUDE ([Id], [Name], [Link], [ImageUrl]);
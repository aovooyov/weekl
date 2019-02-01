CREATE TABLE [FEED].[Article]
(
	[Id] INT NOT NULL IDENTITY(1000, 1),
	[ChannelId] INT NOT NULL,
	[Link] NVARCHAR(300) NOT NULL, 
	[Title] NVARCHAR(300) NOT NULL, 
	[SubTitle] NVARCHAR(300) NULL,
	[Description] NVARCHAR(MAX) NULL, 
	[Text] NVARCHAR(MAX) NULL, 
	[ImageUrl] nvarchar(450) NULL, 
	[Date] DATETIME2(7) NOT NULL,
	[DateCreated] DATETIME2 (7) CONSTRAINT [DF#FEED@Article@DateCreated] DEFAULT (sysdatetime()) NOT NULL,
    [Unique] NVARCHAR(350) NOT NULL, 
    CONSTRAINT [PK#FEED@Article@ID] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK#FEED@ChannelId#Channel@Id] FOREIGN KEY ([ChannelId]) REFERENCES [FEED].[Channel] ([Id]) ON DELETE CASCADE
)

GO
create nonclustered index [IX#FEED@Article@Date]
    on [FEED].[Article] ([Date] desc) include ([Id], [Link], [Title], [Description]);

GO
create unique nonclustered index [IX#FEED@Article@Unique]
    on [FEED].[Article] ([Unique]);
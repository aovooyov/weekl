create view [FEED].[ArticleView]
with schemabinding
as
	select 
		a.[Id],
		a.[ChannelId],
		a.[Link],
		a.[Title],
		a.[Description],
		a.[Date],
		a.[ImageUrl],
		a.[Unique],
		c.[SourceId],
		s.[Name] as [SourceName],
		s.[Link] as [SourceLink],
		s.[ImageUrl] as [SourceImageUrl],
		s.[Unique] as [SourceUnique]
	from [FEED].[Article] a
		inner join [FEED].[Channel] c on c.[Id] = a.[ChannelId]
		inner join [FEED].[Source] s on s.[Id] = c.[SourceId]
	--where dateadd(day, -7, getdate()) < a.[Date];

go
create unique clustered index [IX#FEED@ArticleView@Unique]
    on [FEED].[ArticleView] ([Unique]);

go
create nonclustered index [IX#FEED@ArticleView@Date@SourceId]
    on [FEED].[ArticleView] ([Date] desc, [SourceId]);
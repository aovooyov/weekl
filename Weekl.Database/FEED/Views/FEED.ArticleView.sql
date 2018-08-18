create view [FEED].[ArticleView]
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
		s.[Id] as SourceId,
		s.[Name] as SourceName,
		s.[Link] as SourceLink,
		s.[ImageUrl] as SourceImageUrl,
		s.[Unique] as SourceUnique
	from [FEED].[Article] a
		inner join [FEED].[ChannelView] c on c.[Id] = a.[ChannelId]
		inner join [FEED].[SourceView] s on s.[Id] = c.[SourceId]
	WHERE DATEADD(day, -7, GETDATE()) < a.[Date]
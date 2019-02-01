create view [FEED].[ChannelView]
with schemabinding
as
	select 
		c.[Id],
		c.[Name],
		c.[Link],
		c.[Encoding],
		c.[Selector],
		c.[DateUpdated],
		c.[SourceId],
		s.[Name] as SourceName,
		s.[Link] as SourceLink,
		s.[ImageUrl] as SourceImageUrl,
		s.[Unique] as SourceUnique
	from [FEED].[Channel] c
		inner join [FEED].[SourceView] s on s.[Id] = c.[SourceId]


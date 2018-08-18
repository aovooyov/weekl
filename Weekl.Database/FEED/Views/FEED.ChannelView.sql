create view [FEED].[ChannelView]
as
	select 
		[Id],
		[SourceId],
		[Name],
		[Link]
	from [FEED].[Channel]

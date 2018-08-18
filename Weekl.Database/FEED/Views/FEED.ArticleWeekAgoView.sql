create view [FEED].[ArticleWeekAgoView]
as
	select 
		[Id],
		[ChannelId],
		[Link],
		[Title],
		[Description],
		[Date]
	from [FEED].[Article]
	WHERE DATEADD(day, -7, GETDATE()) > [Date]

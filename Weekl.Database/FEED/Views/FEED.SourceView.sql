create view [FEED].[SourceView]
as
	select 
		[Id],
		[Name],
		[Link],
		[ImageUrl],
		[Unique]
	from [FEED].[Source]

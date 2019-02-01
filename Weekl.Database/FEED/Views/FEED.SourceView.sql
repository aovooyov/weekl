create view [FEED].[SourceView]
with schemabinding
as
	select 
		[Id],
		[Name],
		[Link],
		[ImageUrl],
		[Unique]
	from [FEED].[Source]

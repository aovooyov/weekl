CREATE PROCEDURE [FEED].[ArticleClean]
as
    set nocount on
    set xact_abort on
    set ansi_nulls on
    set ansi_warnings on
    set ansi_padding on
    set ansi_null_dflt_on on
    set arithabort on
    set quoted_identifier on
    set concat_null_yields_null on
    set implicit_transactions off
    set cursor_close_on_commit off
begin	

	delete from [FEED].[Article]
	where [Id] in (select top 1000 [Id] from [FEED].[ArticleWeekAgoView] order by [Date] asc)

	;with #dublicates as 
	(
		select row_number() over (partition by [Unique] order by [Unique] desc) as [RowNumber], [Id], [Unique], [Date] from [FEED].[Article]
	)
	delete
	from [FEED].[Article]
	where [Id] in (select [Id] from #dublicates where [RowNumber] > 1)

	;with #ignore as 
	(
		select [Cause] from [FEED].[Ignore] where [Type] = 1
	)
	update [FEED].[Article]
	set 
		[ImageUrl] = null
	where [ImageUrl] in (select [Cause] from #ignore)

	return 0;
end
go
grant execute
    on object::[FEED].[ArticleClean] TO [AppRole]
    AS [dbo];
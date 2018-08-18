CREATE PROCEDURE [FEED].[ArticleGet]
	@articleId int null,
	@source nvarchar(150) null,
	@unique nvarchar(350) null
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

	if @articleId is not null
	begin
		select * 
		from [FEED].[Article]
		where [Id] = @articleId
		order by [Date] desc
	end

	if @source is not null and @unique is not null
	begin		
		declare @articleUnique nvarchar(350);
		set @articleUnique = concat(@source, N'/', @unique, N'/')
		
		select top 1 * 
		from [FEED].[Article]
		where [Unique] = @articleUnique
		order by [Date] desc
	end

    return 0;

end
go
grant execute
    on object::[FEED].[ArticleGet] TO [AppRole]
    AS [dbo];
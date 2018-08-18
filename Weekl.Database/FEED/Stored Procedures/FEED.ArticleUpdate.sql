CREATE PROCEDURE [FEED].[ArticleUpdate]
	@articleId int,
	@link nvarchar(300),
	@title nvarchar(300),
	@subtitle nvarchar(300),
	@description nvarchar(max),
	@text nvarchar(max),
	@date datetime2(7)
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

	declare @error nvarchar(200);
	set @error = 
	case
	    when @articleId is null then N'ArticleId'
		when @title is null then N'Title'
		when @link is null then N'Link'
		when @date is null then N'Date'
        else null
    end;

    if @error is not null
    begin
        raiserror(N'Поле [%s] не может быть NULL', 16, 1, @error);
        return 1;
    end

	declare @sourceUnique nvarchar(150);
	set @sourceUnique = (
		select top 1 s.[Unique] from [FEED].[Article] a
			inner join [FEED].[Channel] c on c.[Id] = a.[ChannelId]
			inner join [FEED].[Source] s on s.[Id] = c.[SourceId]
		where a.[Id] = @articleId)

	update [FEED].[Article]
	set 
		[Title] = @title,
		[SubTitle] = @subtitle,
		[Description] = @description,
		[Text] = @text,
		[Link] = @link,
		[Date] = @date,
		[Unique] = concat(@sourceUnique, N'/', [dbo].[ToUnique](@title), N'/')
	where [Id] = @articleId
	
	select * 
	from [FEED].[Article]
	where [Id] = @articleId

	return 0;
end
go
grant execute
    on object::[FEED].[ArticleUpdate] TO [AppRole]
    AS [dbo];
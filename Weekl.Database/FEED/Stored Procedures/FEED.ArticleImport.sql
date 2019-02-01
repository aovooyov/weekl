CREATE PROCEDURE [FEED].[ArticleImport]
	@channelId int,
	@link nvarchar(300),
	@title nvarchar(300),
	@subtitle nvarchar(300),
	@description nvarchar(max),
	@text nvarchar(max),
	@date datetime2(7),
	@images xml
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
	    when @channelId is null then N'ChannelId'
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
		select c.[SourceUnique] from [FEED].[ChannelView] c
		where c.[Id] = @channelId);

	declare @articleUnique nvarchar(350);
	set @articleUnique = concat(@sourceUnique, N'/', [dbo].[ToUnique](@title), N'/');

	declare @articleId int;
	set @articleId = (
		select [Id] 
		from [FEED].[Article] 
		where [Unique] = @articleUnique);

	declare @xmlPointer int;
	exec sys.sp_xml_preparedocument @xmlPointer output, @images
    create table #image ([Link] nvarchar(450));

    insert into #image ([Link])
    select [Link]
    from openxml (@xmlPointer, '/Images/Image')
    with ([Link] nvarchar(450) 'text()')
	where 1 = 1;

	exec sys.sp_xml_removedocument @xmlPointer;

	declare @imageUrl nvarchar(450);
	set @imageUrl = (
		select top 1 [Link] from #image
		where [Link] COLLATE database_default not in (select [Cause] from [FEED].[Ignore] where [Type] = 1)
		);

	drop table #image

	if @articleId is null
	begin
		insert [FEED].[Article]([ChannelId], [Title], [SubTitle], [Description], [Text], [Link], [Date], [ImageUrl], [Unique])
		select @channelId, @title, @subtitle, @description, @text, @link, @date, @imageUrl, @articleUnique
			
		set @articleId = scope_identity();
	end
	else 
	begin
		update [FEED].[Article]
		set 
			[Title] = @title,
			[SubTitle] = @subtitle,
			[Description] = @description,
			[Text] = @text,
			[Date] = @date,
			[ImageUrl] = @imageUrl,
			[Link] = @link
		where [Id] = @articleId
	end

	update [FEED].[Channel]
	set [DateUpdated] = SYSDATETIME()
	where [Id] = @channelId;

	--declare image_cursor cursor for 
	--select top 1 [Link] from #image
	
	--open image_cursor  
	--fetch next from image_cursor   
	--into @imageLink

	--while @@FETCH_STATUS = 0  
	--begin 

	--	exec [FEED].[ArticleImageAdd] 
	--		@articleId,
	--		@imageLink;

	--	fetch next from image_cursor
	--	into @imageLink;
	--end
	--close image_cursor
	--deallocate image_cursor;

	return 0;
end
go
grant execute
    on object::[FEED].[ArticleImport] TO [AppRole]
    AS [dbo];
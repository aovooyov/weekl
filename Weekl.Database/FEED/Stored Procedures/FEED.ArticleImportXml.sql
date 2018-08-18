CREATE PROCEDURE [FEED].[ArticleImportXml]
	@articles xml
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
	    when @articles is null then N'Articles'
        else null
    end;

    if @error is not null
    begin
        raiserror(N'Поле [%s] не может быть NULL', 16, 1, @error);
        return 1;
    end

	declare @xmlPointer int;

	exec sys.sp_xml_preparedocument @xmlPointer output, @articles;
    create table #article (
		[ChannelId] int, 
		[Link] nvarchar(300),
		[Date] datetime2(7), 
		[Title] nvarchar(300), 
		[SubTitle] nvarchar(300),
		[Description] nvarchar(MAX),
		[Text] nvarchar(MAX),
		[Images] xml
	);

    insert into #article ([ChannelId], [Link], [Date], [Title], [SubTitle], [Description], [Text], [Images])
    select 
		[ChannelId],
		[Link],
		[Date],
		[Title], 
		[SubTitle],
		[Description],
		[Text],
		[Images]
    from openxml (@xmlPointer, '/ArticlesXml/Articles/Article')
    with (
		[ChannelId] int '@ChannelId', 
		[Link] nvarchar(300) '@Link', 
		[Date] datetime2(7) '@Date',
		[Title] nvarchar(300) 'Title',
		[SubTitle] nvarchar(300) 'SubTitle',
		[Description] nvarchar(MAX) 'Description',
		[Text] nvarchar(MAX) 'Text',
		[Images] xml 'Images')

	where 1 = 1;

	exec sys.sp_xml_removedocument @xmlPointer;

	declare 
		@channelId int,
		@link nvarchar(300),
		@date datetime2(7), 
		@title nvarchar(300), 
		@subTitle nvarchar(300),
		@description nvarchar(MAX),
		@text nvarchar(MAX),
		@images xml;

	declare article_cursor cursor for 
	select * from #article

	open article_cursor  
	fetch next from article_cursor   
	into @channelId, @link, @date, @title, @subTitle, @description, @text, @images;

	while @@FETCH_STATUS = 0  
	begin 
		print CAST(@images as nvarchar(MAX));
		
		exec [FEED].[ArticleImport] 
			@channelId, @link, @title, @subTitle, @description, @text, @date, @images;

		fetch next from article_cursor
	into @channelId, @link, @date, @title, @subTitle, @description, @text, @images;
	end
	close article_cursor
	deallocate article_cursor;

	return 0;
end
go
grant execute
    on object::[FEED].[ArticleImportXml] TO [AppRole]
    AS [dbo];
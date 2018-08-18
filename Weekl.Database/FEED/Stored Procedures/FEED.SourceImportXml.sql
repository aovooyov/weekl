CREATE PROCEDURE [FEED].[SourceImportXml]
	@sources xml
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
	    when @sources is null then N'Sources'
        else null
    end;

    if @error is not null
    begin
        raiserror(N'Поле [%s] не может быть NULL', 16, 1, @error);
        return 1;
    end

	declare @xmlPointer int;

	exec sys.sp_xml_preparedocument @xmlPointer output, @sources;
    create table #source (
		[Name] nvarchar(150), 
		[Source] nvarchar(250),
		[Channel] nvarchar(250), 
		[Selector] nvarchar(250), 
		[Encoding] nvarchar(20));

    insert into #source ([Name], [Source], [Channel], [Selector], [Encoding])
    select 
        [Name],
        [Source],
		[Channel],
		[Selector],
		[Encoding]
    from openxml (@xmlPointer, '/Source/Channel')
    with (
		[Name] nvarchar(150) '@Name', 
		[Source] nvarchar(250) '@Source', 
		[Channel] nvarchar(250) '@Channel',
		[Selector] nvarchar(250) '@Selector',
		[Encoding] nvarchar(20) '@Encoding')
	where 1 = 1;

	exec sys.sp_xml_removedocument @xmlPointer;

	declare @trancount int = @@trancount;
    if(@trancount = 0)
        begin tran;


	declare 
		@name nvarchar(150),
		@source nvarchar(250),
		@channel nvarchar(250),
		@selector nvarchar(250),
		@encoding nvarchar(20);

	declare source_cursor cursor for 
	select * from #source

	open source_cursor  
	fetch next from source_cursor   
	into @name, @source, @channel, @selector, @encoding;

	while @@FETCH_STATUS = 0  
	begin 
		exec [FEED].[SourceImport] 
			@name,
			@source,
			@channel,
			@selector,
			@encoding;

		fetch next from source_cursor
		into @name, @source, @channel, @selector, @encoding;
	end
	close source_cursor
	deallocate source_cursor;

	if(@trancount = 0)
        commit tran;

	return 0; 
end
go
grant execute
    on object::[FEED].[SourceImportXml] TO [AppRole]
    AS [dbo];
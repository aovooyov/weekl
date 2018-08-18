CREATE PROCEDURE [FEED].[SourceImport]
	@name nvarchar(150),
	@source nvarchar(250),
	@channel nvarchar(250),
	@selector nvarchar(250),
	@encoding nvarchar(20)
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
		
	declare @sourceId int;
	declare @sourceRow table([Id] int);

	set @sourceId = (select top 1 [Id] from [FEED].[Source] where [Link] = @source)

	if @sourceId is null 
	begin 
		insert [FEED].[Source]([Name], [Link], [Unique])
		select @name, @source, [dbo].[ToUnique](@name)
		
		set @sourceId = scope_identity();

		select @sourceId = [Id] from @sourceRow;
	end

	exec [FEED].[ChannelAdd] 
		@sourceId = @sourceId,
		@name = @name,
		@link = @channel,
		@selector = @selector,
		@encoding = @encoding,
		@type = 1

	return 0; 
end
go
grant execute
    on object::[FEED].[SourceImport] TO [AppRole]
    AS [dbo];
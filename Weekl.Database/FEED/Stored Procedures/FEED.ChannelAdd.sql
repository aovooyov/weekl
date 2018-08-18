CREATE PROCEDURE [FEED].[ChannelAdd]
	@sourceId int,
	@name nvarchar(150),
	@link nvarchar(250),
	@selector nvarchar(250),
	@encoding nvarchar(20),
	@type int
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
	    when @sourceId is null then N'SourceId'
	    when @name is null then N'Name'
		when @link is null then N'Link'
        else null
    end;

    if @error is not null
    begin
        raiserror(N'Поле [%s] не может быть NULL', 16, 1, @error);
        return 1;
    end

	declare @channelId int;
	set @channelId = (select top 1 [Id] from [FEED].[Channel] where [Link] = @link)

	if @channelId is null 
	begin 
		insert [FEED].[Channel]([SourceId], [Name], [Link], [Selector], [Encoding], [Type])
		select @sourceId, @name, @link, @selector, @encoding, @type
			
		set @channelId = scope_identity();
	end
	else
	begin 
		update [FEED].[Channel]
		set
			[Name] = @name,
			[Selector] = @selector,
			[Encoding] = @encoding,
			[Type] = @type
		where [Id] = @channelId
	end

	select * 
	from [FEED].[Channel]
	where [Id] = @channelId

	return 0;
end
go
grant execute
    on object::[FEED].[ChannelAdd] TO [AppRole]
    AS [dbo];
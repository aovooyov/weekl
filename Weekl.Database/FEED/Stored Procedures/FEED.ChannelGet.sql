CREATE PROCEDURE [FEED].[ChannelGet]
	@channelId int
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
	select * 
	from [FEED].[Channel]
	where [Id] = @channelId

	return 0;
end
go
grant execute
    on object::[FEED].[ChannelGet] TO [AppRole]
    AS [dbo];
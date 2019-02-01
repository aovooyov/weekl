CREATE PROCEDURE [LOG].[EntryList]
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
		
	with #entries ([Logged], [Level], [Message], [Exception])
	as
	(
		select
			[Logged],
			[Level],
			[Message],
			[Exception]
		from [LOG].[Entry]
	)
	select
		[Logged],
		[Level],
		[Message],
		[Exception]
	from #entries
	--where [Logged] < dateadd(day,-7, getdate())
	order by [Logged] desc

	return 0;
end
go
grant execute
    on object::[LOG].[EntryList] TO [AppRole]
    AS [dbo];
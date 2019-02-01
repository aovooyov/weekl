CREATE PROCEDURE [LOG].[EntryAdd]
	@machineName nvarchar(200),
	@siteName nvarchar(200),
	@logged datetime,
	@level varchar(5),
	@userName nvarchar(200),
	@message nvarchar(max),
	@logger nvarchar(300),
	@properties nvarchar(max),
	@serverName nvarchar(200),
	@port nvarchar(100),
	@url nvarchar(2000),
	@https bit,
	@serverAddress nvarchar(100),
	@remoteAddress nvarchar(100),
	@callSite nvarchar(300),
	@exception nvarchar(max)
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
	insert into [LOG].[Entry] (
		[MachineName],
		[SiteName],
		[Logged],
		[Level],
		[UserName],
		[Message],
		[Logger],
		[Properties],
		[ServerName],
		[Port],
		[Url],
		[Https],
		[ServerAddress],
		[RemoteAddress],
		[CallSite],
		[Exception]
		) VALUES (
		@machineName,
		@siteName,
		@logged,
		@level,
		@userName,
		@message,
		@logger,
		@properties,
		@serverName,
		@port,
		@url,
		@https,
		@serverAddress,
		@remoteAddress,
		@callSite,
		@exception
	);

end
go
grant execute
    on object::[LOG].[EntryAdd] TO [AppRole]
    AS [dbo];
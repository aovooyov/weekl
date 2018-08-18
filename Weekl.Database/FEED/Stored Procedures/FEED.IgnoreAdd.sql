CREATE PROCEDURE [FEED].[IgnoreAdd]
	@cause nvarchar(500),
	@type int = 1
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
	    when @cause is null then N'Cause'
		when @type is null then N'Type'
        else null
    end;

    if @error is not null
    begin
        raiserror(N'Поле [%s] не может быть NULL', 16, 1, @error);
        return 1;
    end

	declare @ignoreId int;
	set @ignoreId = (select top 1 [Id] from [FEED].[Ignore] where [Cause] = @cause);

	if @ignoreId is null
	begin
		insert [FEED].[Ignore]([Cause], [Type])
		select @cause, @type
			
		set @ignoreId = scope_identity();
	end

	return 0;
end
go
grant execute
    on object::[FEED].[IgnoreAdd] TO [AppRole]
    AS [dbo];
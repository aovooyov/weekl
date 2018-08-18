CREATE PROCEDURE [FEED].[SourceAdd]
	@name nvarchar(150),
	@link nvarchar(250),
	@description nvarchar(max),
	@imageUrl nvarchar(250)
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
	    when @name is null then N'Name'
		when @link is null then N'Link'
        else null
    end;

    if @error is not null
    begin
        raiserror(N'Поле [%s] не может быть NULL', 16, 1, @error);
        return 1;
    end

	declare @sourceId int;
	set @sourceId = (select top 1 [Id] from [FEED].[Source] where [Link] = @link)

	if @sourceId is null 
	begin 
		insert [FEED].[Source]([Name], [Link], [Description], [ImageUrl], [Unique])
		select @name, @link, @description, @imageUrl, [dbo].[ToUnique](@name)
			
		set @sourceId = scope_identity();
	end
	else
	begin 
		update [FEED].[Source]
		set
			[Name] = @name,
			[Link] = @link,
			[Description] = @description,
			[ImageUrl] = @imageUrl,
			[Unique] = [dbo].[ToUnique](@name)
		where [Id] = @sourceId
	end

	select * 
	from [FEED].[Source]
	where [Id] = @sourceId

	return 0;
end
go
grant execute
    on object::[FEED].[SourceAdd] TO [AppRole]
    AS [dbo];
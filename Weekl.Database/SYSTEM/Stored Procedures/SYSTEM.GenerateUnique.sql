CREATE PROCEDURE [SYSTEM].[GenerateUnique]
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
	declare @sourceName NVARCHAR(150);
	declare @sourceUnique NVARCHAR(150);

	declare @articleId int;
	declare @articleTitle NVARCHAR(300);
	declare @articleUnique NVARCHAR(350);

	--declare source_cursor cursor for 
	--	select s.[Id], s.[Name] from [FEED].[Source] s

	--open source_cursor  
	--fetch next from source_cursor   
	--into @sourceId, @sourceName

	--while @@FETCH_STATUS = 0  
	--begin 
	
	--	set @sourceUnique = [dbo].[ToUnique](@sourceName);

	--	declare @i int = 0;
	--	while (1=1)
	--	begin
	--		declare @unique nvarchar(150);
	--		set @unique = @sourceUnique;

	--		if (@i = 1) 
	--			set @unique += concat(N'-', cast(@sourceId as nvarchar(10)));

	--		if(@i >= 2)
	--			set @unique += concat(N'-', cast(@i as nvarchar(10)));

	--		if (not(exists(select s.[Id] from [FEED].[Source] s where s.[Id] != @sourceId and s.[Unique] = @unique))) 
	--		begin
	--			set @sourceName = @unique;
	--			break;
	--		end

	--		set @i += 1;
	--	end

	--	print @sourceUnique;

	--	update [FEED].[Source]
	--	set
	--		[Unique] = @sourceUnique
	--	where [Id] = @sourceId

	--	fetch next from source_cursor
	--	into @sourceId, @sourceName
	--end
	--close source_cursor
	--deallocate source_cursor;


	declare article_cursor cursor for 
	select a.[Id], a.[Title], s.[Unique] as [SourceUnique]
	from [FEED].[Article] a
		inner join [FEED].[Channel] c on c.[Id] = a.[ChannelId]
		inner join [FEED].[Source] s on s.[Id] = c.[SourceId]
	where DATEADD(day, -7, GETDATE()) < a.[Date]
	order by a.[Date] desc

	open article_cursor  
	fetch next from article_cursor   
	into @articleId, @articleTitle, @sourceUnique

	while @@FETCH_STATUS = 0  
	begin 
		set @articleUnique = concat(@sourceUnique, N'/', [dbo].[ToUnique](@articleTitle), N'/');

		print @articleUnique;

		--if (exists(select s.[Id] from [FEED].[Article] s where s.[Id] != @sourceId and s.[Unique] = @articleUnique)) 
		--begin
			
		--end

		update [FEED].[Article]
		set 
			[Unique] = @articleUnique
		where [Id] = @articleId

		fetch next from article_cursor
		into @articleId, @articleTitle, @sourceUnique
	end
	close article_cursor;
	deallocate article_cursor;

end
go
grant execute
    on object::[SYSTEM].[GenerateUnique] TO [AppRole]
    AS [dbo];
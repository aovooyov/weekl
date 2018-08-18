CREATE FUNCTION [dbo].[ToUnique] (@name NVARCHAR(MAX))  
RETURNS NVARCHAR(MAX)  
BEGIN  
 DECLARE @TransTable	TABLE(
		 Rus	nvarchar	PRIMARY KEY
		,Lat	nvarchar(2)
	) INSERT	@TransTable	
		SELECT N'А','A'
		UNION ALL	SELECT N'Б','B'
		UNION ALL	SELECT N'В','V'
		UNION ALL	SELECT N'Г','G'
		UNION ALL	SELECT N'Д','D'
		UNION ALL	SELECT N'Е','E'
		UNION ALL	SELECT N'Ё','Y'
		UNION ALL	SELECT N'Ж','Zh'
		UNION ALL	SELECT N'З','Z'
		UNION ALL	SELECT N'И','I'
		UNION ALL	SELECT N'Й','Y'
		UNION ALL	SELECT N'К','K'
		UNION ALL	SELECT N'Л','L'
		UNION ALL	SELECT N'М','M'
		UNION ALL	SELECT N'Н','N'
		UNION ALL	SELECT N'О','O'
		UNION ALL	SELECT N'П','P'
		UNION ALL	SELECT N'Р','R'
		UNION ALL	SELECT N'С','S'
		UNION ALL	SELECT N'Т','T'
		UNION ALL	SELECT N'У','U'
		UNION ALL	SELECT N'Ф','F'
		UNION ALL	SELECT N'Х','H'
		UNION ALL	SELECT N'Ц','C'
		UNION ALL	SELECT N'Ч','Ch'
		UNION ALL	SELECT N'Ш','Sh'
		UNION ALL	SELECT N'Щ','Sh'
		UNION ALL	SELECT N'Ъ',''
		UNION ALL	SELECT N'Ы','Y'
		UNION ALL	SELECT N'Ь',''
		UNION ALL	SELECT N'Э','E'
		UNION ALL	SELECT N'Ю','U'
		UNION ALL	SELECT N'Я','Ya'
		
		UNION ALL	SELECT N'"',''
		UNION ALL	SELECT N'''',''
		UNION ALL	SELECT N'/',''
		UNION ALL	SELECT N'.',''
		UNION ALL	SELECT N',',''
		UNION ALL	SELECT N'-',''
		UNION ALL	SELECT N';',''
		UNION ALL	SELECT N':',''
		UNION ALL	SELECT N'(',''
		UNION ALL	SELECT N')',''
		UNION ALL	SELECT N'«',''
		UNION ALL	SELECT N'»',''
		--UNION ALL	SELECT N'&',''
		UNION ALL	SELECT N'[',''
		UNION ALL	SELECT N']',''
		UNION ALL	SELECT N'!',''
		UNION ALL	SELECT N'%',''
		UNION ALL	select N'²',''

	DECLARE	@Result	nvarchar(max)
	SET	@Result	= @name
	SELECT	@Result	= Replace(@Result,Upper(Rus) COLLATE Cyrillic_General_CS_AS,Lat) FROM @TransTable WHERE	@name LIKE N'%' + Rus + N'%'
	SELECT	@Result	= Replace(@Result,Lower(Rus) COLLATE Cyrillic_General_CI_AS,LOWER(Lat)) FROM @TransTable WHERE	@name LIKE N'%' + Rus + N'%'

	set @Result = REPLACE(@Result, SUBSTRING(@Result, PATINDEX('%[.,-,–,?,\,!,~,@,#,$,%,*,^,%,*,(,),:,#,/]%', @Result), 1), N'')
	set @Result = REPLACE(@Result, N'&mdash', N'-')
	set @Result = REPLACE(@Result, N'&amp', N'and')
	set @Result = REPLACE(@Result, N'&', N'and')
	set @Result = REPLACE(@Result, N'+', N'plus')

	set @Result = LTRIM(RTRIM(@Result))
	set @Result = REPLACE(@Result, N'  ', N' ')
	set @Result = REPLACE(@Result, N' ', N'-')
	set @Result = REPLACE(@Result, N'--', N'-')
	set @Result = REPLACE(@Result, N' ', N'')

	return LOWER(@Result)
	RETURN	@Result
   
END  
--CREATE APPLICATION ROLE [AppRole]
--	WITH PASSWORD = '4jqmo8zelhjLuodNnmavbtubmsFT7_&#$!~<{dosujVacxaf'
CREATE ROLE [AppRole]
AUTHORIZATION [dbo];

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Application role', @level0type = N'USER', @level0name = N'AppRole';

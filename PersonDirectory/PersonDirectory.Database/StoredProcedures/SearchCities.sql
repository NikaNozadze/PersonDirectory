USE PersonDirectoryDB;
GO

CREATE PROCEDURE [dbo].[SearchCities]
    @SearchTerm NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 100 Id, Name
    FROM Cities
    WHERE Name LIKE '%' + @SearchTerm + '%';
END;
GO

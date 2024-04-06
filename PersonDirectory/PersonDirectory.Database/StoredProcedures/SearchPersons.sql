USE PersonDirectoryDB;
GO

CREATE PROCEDURE [dbo].[SearchPersons]
    @SearchTerm NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 100 Id, FirstName, LastName, Gender, PersonalNumber, DateOfBirth, CityId, ImagePath
    FROM People
    WHERE FirstName LIKE '%' + @SearchTerm + '%'
        OR LastName LIKE '%' + @SearchTerm + '%'
        OR PersonalNumber LIKE '%' + @SearchTerm + '%';
END;
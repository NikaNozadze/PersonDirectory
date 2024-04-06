USE PersonDirectoryDB;
GO

CREATE PROCEDURE [dbo].[DetailedSearchPersons]
    @FirstName NVARCHAR(100) = NULL,
    @LastName NVARCHAR(100) = NULL,
    @Gender INT = NULL,
    @PersonalNumber NVARCHAR(100) = NULL,
    @DateOfBirth DATETIME = NULL,
    @CityId INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 100 *
    FROM People
    WHERE (@FirstName IS NULL OR FirstName = @FirstName)
        AND (@LastName IS NULL OR LastName = @LastName)
        AND (@Gender IS NULL OR Gender = @Gender)
        AND (@PersonalNumber IS NULL OR PersonalNumber = @PersonalNumber)
        AND (@DateOfBirth IS NULL OR DateOfBirth = @DateOfBirth)
        AND (@CityId IS NULL OR CityId = @CityId);
END;

USE PersonDirectoryDB;
GO

CREATE PROCEDURE dbo.GetAllPersonPhoneNumbers
    @PersonId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 100 *
    FROM PhoneNumbers
    WHERE PersonId = @PersonId;
END;
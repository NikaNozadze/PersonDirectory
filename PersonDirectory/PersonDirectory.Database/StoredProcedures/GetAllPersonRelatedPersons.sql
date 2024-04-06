USE PersonDirectoryDB;
GO

CREATE PROCEDURE [dbo].[GetAllPersonRelatedPersons]
    @PersonId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 100 *
    FROM RelatedPeople
    WHERE PersonId = @PersonId or RelatedPersonId = @PersonId;
END;
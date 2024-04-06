USE PersonDirectoryDB;
GO

CREATE PROCEDURE [dbo].[SearchPhoneNumbers]
    @SearchTerm NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 100 * 
    FROM PhoneNumbers
    WHERE Number LIKE '%' + @SearchTerm + '%';
END;
GO

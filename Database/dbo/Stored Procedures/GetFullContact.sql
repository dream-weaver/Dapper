CREATE procedure [dbo].[GetFullContact]
	@Id int
AS
BEGIN
	SELECT [Id]
		  ,[FirstName]
		  ,[LastName]
		  ,[Company]
		  ,[Title]
		  ,[Email]
	  FROM [dbo].[Contacts]
	SELECT 
		Id,
		ContactId,
		AddressType,
		StreetAddress,
		City,
		StateId,
		PostalCode
	FROM [dbo].[Addresses] 
END

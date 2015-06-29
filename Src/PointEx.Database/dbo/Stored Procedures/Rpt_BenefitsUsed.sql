CREATE PROCEDURE [dbo].[Rpt_BenefitsUsed]
	@From datetime2(7),
	@To datetime2(7),
	@ShopId int
AS
BEGIN
	SELECT Beneficiary = B.Name
	  ,[Date] = P.PurchaseDate
	  ,P.Amount
	  ,Benefit = B.Name
	FROM
	  Purchase P
	  INNER JOIN Benefit B
		ON B.Id = P.BenefitId
	  
	WHERE 
	        (@From IS NULL OR P.PurchaseDate >= @From)
		AND (@To IS NULL OR P.PurchaseDate <= @To)
		AND (@ShopId IS NULL OR P.ShopId = @ShopId)
END
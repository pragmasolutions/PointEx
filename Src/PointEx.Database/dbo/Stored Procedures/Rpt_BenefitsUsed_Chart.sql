CREATE PROCEDURE [dbo].[Rpt_BenefitsUsed_Chart]
	@From datetime2(7),
	@To datetime2(7),
	@ShopId int
AS
BEGIN
	SELECT Benefit = B.Name,
			[Count] = Count(*)
	FROM
	  Purchase P
	  INNER JOIN Benefit B
		ON B.Id = P.BenefitId
	  
	WHERE (@From IS NULL OR P.PurchaseDate >= @From)
		AND (@To IS NULL OR P.PurchaseDate <= @To)
		AND (@ShopId IS NULL OR P.ShopId = @ShopId)
	GROUP BY B.Name
END
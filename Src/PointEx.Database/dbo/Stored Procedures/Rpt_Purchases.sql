CREATE PROCEDURE [dbo].[Rpt_Purchases]
	@From datetime2(7),
	@To datetime2(7),
	@ShopId int,
	@EducationalInstitutionId int
AS
BEGIN
	SELECT
	   ShopName = S.Name
	  ,EducationalInstitutionName = E.Name 
	  ,Amount = SUM(P.Amount) 
	FROM
	  Purchase P
	  INNER JOIN Shop S
		ON S.Id = P.ShopId
	  INNER JOIN Card C
		ON C.Id = P.CardId
	  INNER JOIN Beneficiary B
		ON B.Id = C.BeneficiaryId
	  INNER JOIN EducationalInstitution E
		ON E.Id = B.EducationalInstitutionId
	  
	WHERE 
	        (@From IS NULL OR P.PurchaseDate >= @From)
		AND (@To IS NULL OR P.PurchaseDate <= @To)
		AND (@ShopId IS NULL OR P.ShopId = @ShopId)
		AND (@EducationalInstitutionId IS NULL OR E.Id = @EducationalInstitutionId)
	GROUP BY S.Name,E.Name
END
CREATE PROCEDURE [dbo].[Rpt_MostUsedBenefits]
	@From datetime2(7),
	@To datetime2(7),
	@ShopId int,
	@EducationalInstitutionId int
AS
BEGIN
	SELECT
	   BenefitName = B.Name
	  ,ShopName = S.Name
	  ,EducationalInstitutionName = E.Name 
	  ,UsesCount = COUNT(P.Id)
	FROM
	  Benefit B
	  INNER JOIN Purchase P
		ON P.BenefitId = B.Id
	  INNER JOIN Shop S
		ON S.Id = P.ShopId
	  INNER JOIN Card C
		ON C.Id = P.CardId
	  INNER JOIN Beneficiary BF
		ON B.Id = C.BeneficiaryId
	  INNER JOIN EducationalInstitution E
		ON E.Id = BF.EducationalInstitutionId
	  
	WHERE 
	        (@From IS NULL OR P.PurchaseDate >= @From)
		AND (@To IS NULL OR P.PurchaseDate <= @To)
		AND (@ShopId IS NULL OR P.ShopId = @ShopId)
		AND (@EducationalInstitutionId IS NULL OR E.Id = @EducationalInstitutionId)
	GROUP BY B.Name, S.Name, E.Name
	ORDER BY UsesCount
END
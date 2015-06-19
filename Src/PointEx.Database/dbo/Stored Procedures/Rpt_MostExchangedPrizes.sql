CREATE PROCEDURE [dbo].[Rpt_MostExchangedPrizes]
	@From datetime2(7),
	@To datetime2(7),
	@EducationalInstitutionId int
AS
BEGIN
	SELECT
	    PrizeName = P.Name
	   ,EducationalInstitutionName = E.Name 
	   ,ExchangeCount = COUNT(PE.Id) 
	FROM
	  Prize P
	  INNER JOIN PointsExchange PE
		ON PE.PrizeId = P.Id
	  INNER JOIN Beneficiary B
		ON B.Id = PE.BeneficiaryId
	  INNER JOIN EducationalInstitution E
		ON E.Id = B.EducationalInstitutionId
	  
	WHERE 
	        (@From IS NULL OR PE.ExchangeDate >= @From)
		AND (@To IS NULL OR PE.ExchangeDate <= @To)
		AND (@EducationalInstitutionId IS NULL OR E.Id = @EducationalInstitutionId)
	GROUP BY P.Name, E.Name 
	ORDER BY ExchangeCount
END
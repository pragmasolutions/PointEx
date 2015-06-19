CREATE PROCEDURE [dbo].[Rpt_GeneratedPoints]
	@From datetime2(7),
	@To datetime2(7),
	@ShopId int,
	@BeneficiaryId int,
	@EducationalInstitutionId int
AS
BEGIN
	SELECT
	   ShopName = S.Name
	  ,Beneficiary = B.Name
	  ,EducationalInstitutionName = E.Name 
	  ,[Date] = P.PurchaseDate
	  ,Amount = SUM(P.Amount) 
	  ,Points = CASE FLOOR(P.Amount / 100)
				WHEN 0 THEN 1
				ELSE FLOOR(P.Amount / 100)
			END	
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
		AND (@BeneficiaryId IS NULL OR B.Id = @BeneficiaryId)
		AND (@EducationalInstitutionId IS NULL OR E.Id = @EducationalInstitutionId)
	GROUP BY S.Name,B.Name,E.Name,P.PurchaseDate, P.Amount
END
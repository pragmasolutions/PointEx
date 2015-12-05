CREATE PROCEDURE [dbo].[Rpt_Beneficiaries]
	@From datetime2(7),
	@To datetime2(7),
	@TownId int,
	@Sex int,
	@EducationalInstitutionId int
AS
BEGIN
	SELECT
	   Name = B.Name
	  ,Age = A.Age
	  ,Sex = CASE WHEN B.Sex = 1 THEN 'MASCULINO'
				  WHEN B.Sex = 2 THEN 'FEMENINO' 
				  ELSE 'OTROS' END
	  ,EducationalInstitutionName = E.Name 
	  ,Town = T.Name

	FROM

	  Beneficiary B
	  LEFT JOIN EducationalInstitution E
		ON E.Id = B.EducationalInstitutionId
	  INNER JOIN Town T
		ON B.TownId = T.Id
	  CROSS APPLY [dbo].[CalculateAge](B.BirthDate,GETDATE()) A
	  
	WHERE 
	        (@From IS NULL OR B.CreatedDate >= @From)
		AND (@To IS NULL OR B.CreatedDate <= @To)
		AND (@TownId IS NULL OR B.TownId = @TownId)
		AND (@Sex IS NULL OR B.Sex = @Sex)
		AND (@EducationalInstitutionId IS NULL OR E.Id = @EducationalInstitutionId)
END
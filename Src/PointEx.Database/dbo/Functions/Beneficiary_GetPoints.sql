﻿CREATE FUNCTION Beneficiary_GetPoints
(
	@BeneficiaryId INT
)
RETURNS INT
AS
BEGIN
	DECLARE @ResultVar INT
	DECLARE @Purchases INT
	DECLARE @Exchanges INT
	
	SELECT @Purchases =	ISNULL(FLOOR(P.Amount), 0)
	FROM Purchase P
		LEFT JOIN [Card] C
			ON P.CardId = C.Id
		LEFT JOIN Shop S
			ON P.ShopId = S.Id
	WHERE C.BeneficiaryId = @BeneficiaryId
		
	SELECT @Exchanges = ISNULL(SUM(PE.PointsUsed), 0)
	FROM PointsExchange PE
	WHERE PE.BeneficiaryId = @BeneficiaryId
	
	SET @ResultVar = @Purchases - @Exchanges
	
	RETURN @ResultVar

END
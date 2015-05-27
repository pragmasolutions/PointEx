CREATE PROCEDURE [dbo].[Beneficiary_PurchasesAndExchanges] 
	@BeneficiaryId int
AS
BEGIN
	DECLARE @Table TABLE
	(
		Id int,
		TransactionDate datetime,
		[Description] varchar(500),
		Debit int,
		Credit int		 
	)
	
	INSERT INTO @Table
	SELECT P.Id,
			P.PurchaseDate,
			'Acumulación por compra en ' + UPPER(S.Name),
			0,
			CASE FLOOR(P.Amount / 100)
				WHEN 0 THEN 1
				ELSE P.Amount
			END	
	FROM Purchase P
		LEFT JOIN [Card] C
			ON P.CardId = C.Id
		LEFT JOIN Shop S
			ON P.ShopId = S.Id
	WHERE C.BeneficiaryId = @BeneficiaryId
	
	INSERT INTO @Table
	SELECT PE.Id,
			PE.ExchangeDate,
			'Canje por ' + UPPER(P.Name),
			PE.PointsUsed,
			0
	FROM PointsExchange PE
		INNER JOIN Prize P
			ON PE.PrizeId = P.Id			
	WHERE PE.BeneficiaryId = @BeneficiaryId
	
	SELECT *
	FROM @Table
END
CREATE FUNCTION [dbo].[CalculateAge]
(
	@DOB date,
	@Date date 
)
RETURNS TABLE
AS
RETURN
 (
	SELECT
	CASE
	WHEN (@DOB IS NULL) OR (@Date IS NULL) THEN -1
	ELSE DATEDIFF(hour,@DOB, @Date)/8766
	END AS Age
)
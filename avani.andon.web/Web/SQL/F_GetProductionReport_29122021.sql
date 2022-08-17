
CREATE PROC SP_GetProductionReport
@StartYear int, @StartMonth int, @StartDay int,
@EndYear int, @EndMonth int, @EndDay int, @ProductCode nvarchar
AS 
BEGIN
	DECLARE @FromDate as date = DATEFROMPARTS(@StartYear, @StartMonth, @StartDay)
	DECLARE @ToDate as date = DATEFROMPARTS(@EndYear, @EndMonth, @EndDay)
	
	CREATE TABLE #RESULT(
		[DATE] date,
		Quantity int,
		TaktTime float,
		UPH int,
		UPPH int
	)

	CREATE TABLE #DATA(
		[DATE] date,
		Quantity int,
		TaktTime float,
		UPH int,
		UPPH int
	)

	IF (@ProductCode = '')
	BEGIN
		INSERT INTO #DATA
		SELECT [Started], ActualQuantity, ActualTaktTime, UPH, UPPH
		FROM [tblWorkOrderPlan]
		WHERE CAST([Started] AS date) BETWEEN @FromDate AND @ToDate
	END
	ELSE
	BEGIN
		INSERT INTO #DATA
		SELECT [Started], ActualQuantity, ActualTaktTime, UPH, UPPH
		FROM [tblWorkOrderPlan]
		WHERE ProductCode = @ProductCode AND (CAST([Started] AS date) BETWEEN @FromDate AND @ToDate)
	END

	WHILE(@ToDate >= @FromDate)
	BEGIN
		INSERT INTO #RESULT
		SELECT @FromDate,
			(SELECT SUM(Quantity) FROM #DATA WHERE #DATA.[DATE] = @FromDate),
			(SELECT SUM(TaktTime) FROM #DATA WHERE #DATA.[DATE] = @FromDate),
			(SELECT SUM(UPH) FROM #DATA WHERE #DATA.[DATE] = @FromDate),
			(SELECT SUM(UPPH) FROM #DATA WHERE #DATA.[DATE] = @FromDate)
		SET @FromDate = DATEADD(day,1,@FromDate)
	END

	SELECT *
	FROM #RESULT
END
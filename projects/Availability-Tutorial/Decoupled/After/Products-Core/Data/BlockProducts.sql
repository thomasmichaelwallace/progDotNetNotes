USE PRODUCTS
GO

BEGIN TRANSACTION 

SELECT * FROM Products WITH (TABLOCKX, HOLDLOCK) WHERE 0 = 1 

WAITFOR DELAY '00:05:00' 

ROLLBACK TRANSACTION
use master;


IF (NOT EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = 'PaperTrading'))
BEGIN 

	CREATE DATABASE PaperTrading;
	GO
	USE PaperTrading;
	GO

	CREATE TABLE Securities( 
		isin VARCHAR(12) NOT NULL, 
		price DECIMAL(15,2) NOT NULL,
		CONSTRAINT PK_SECURITIES PRIMARY KEY CLUSTERED (isin),
		CONSTRAINT CHK_VALIDATE CHECK (len(isin)=12)
	);
	GO

	--drop database PaperTrading;
	--drop table Securities
	--SELECT * FROM Securities
	--INSERT INTO dbo.Securities VALUES('abcdefghijkl', 15687.78);

END
use master;

--would be great create a another user and grant permissions
--CREATE LOGIN developer WITH PASSWORD = '1StrongPassword';
--exec master..sp_addsrvrolemember @loginame = 'developer', @rolename = 'sysadmin'

create database PaperTrading;
use PaperTrading;

CREATE TABLE Securities( 
    isin VARCHAR(12) NOT NULL, 
    price DECIMAL(15,2) NOT NULL,
    CONSTRAINT PK_SECURITIES PRIMARY KEY CLUSTERED (isin),
    CONSTRAINT CHK_VALIDATE CHECK (len(isin)=12)
);


--drop database PaperTrading;
--drop table Securities
--SELECT * FROM Securities
--INSERT INTO dbo.Securities VALUES('abcdefghijkl', 15687.78);





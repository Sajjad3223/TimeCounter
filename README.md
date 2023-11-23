# ساعت شمار
یه نرم افزار خیلی ساده و جمع و جور برای ذخیره و محاسبه تایم کاری، درسی و غیره.

برای استفاده باید یک دیتابیس SQL با نام TimerDB ایجاد و اسکریپت زیر را روی آن اجرا کنید:
```sql
USE TimerDB
GO

IF DB_NAME() <> N'TimerDB' SET NOEXEC ON
GO

--
-- Create table [dbo].[Work]
--
PRINT (N'Create table [dbo].[Work]')
GO
CREATE TABLE dbo.Work (
  Id int IDENTITY,
  WorkName nvarchar(200) NOT NULL,
  CONSTRAINT PK_Work_Id PRIMARY KEY CLUSTERED (Id)
)
ON [PRIMARY]
GO

--
-- Create table [dbo].[TimerData]
--
PRINT (N'Create table [dbo].[TimerData]')
GO
CREATE TABLE dbo.TimerData (
  Id int IDENTITY,
  Hours int NOT NULL DEFAULT (0),
  Minutes int NOT NULL DEFAULT (0),
  Seconds int NOT NULL DEFAULT (0),
  IsClosed bit NULL,
  WorkId int NULL,
  CONSTRAINT PK_TimerData_Id PRIMARY KEY CLUSTERED (Id)
)
ON [PRIMARY]
GO

--
-- Create foreign key [FK_TimerWork] on table [dbo].[TimerData]
--
PRINT (N'Create foreign key [FK_TimerWork] on table [dbo].[TimerData]')
GO
ALTER TABLE dbo.TimerData
  ADD CONSTRAINT FK_TimerWork FOREIGN KEY (WorkId) REFERENCES dbo.Work (Id) ON DELETE CASCADE ON UPDATE CASCADE
GO
SET NOEXEC OFF
GO
```

## ساخته شده توسط سجاد میرشبی

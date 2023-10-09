Create database AptekDb

Use AptekDb

Create Table Pharmacies(
PharmacyId int Primary Key Identity,
StockCount int,
PharmacyName varchar(max),
PharmacyAddress varchar(max),
PharmacyPhone varchar(max),
)

Create Table Stocks(
StockId int Primary Key Identity,
MedicineCount int,
PharmacyId int References Pharmacies(PharmacyId)
)


Create Table MedicineDetails(
MedicineId int primary key identity,
MedicineName varchar(max),
MedicinePrice float,
MedicineType int,
MedicinePurpose int,
MedicineProdDate DateTime,
MedicineExpDate DateTime
)


Create Table Medicines(
MedicineId int References MedicineDetails(MedicineId),
StockId int References Stocks(StockId)
)

select p.PharmacyName,p.PharmacyId,s.StockId,s.MedicineCount from Stocks as s
join Pharmacies as p
on p.PharmacyId = s.PharmacyId


Insert into Pharmacies 
Values
(100,'NaturalAptek','Baku','000222999')

Insert Into Stocks
Values
(200,1)

Insert Into MedicineDetails
Values('parol',4.20,1,2,04-09-2023,25-09-2023)
SELECT SCOPE_IDENTITY()

insert into Medicines
values 
(3,4)

INSERT INTO Medicines VALUES(3,4);

SELECT SCOPE_IDENTITY()


select p.PharmacyName,p.PharmacyId,s.StockId,s.MedicineCount , p.StockCount from Stocks as s join Pharmacies as p on p.PharmacyId = s.PharmacyId

select md.MedicineName,md.MedicinePrice,md.MedicineProdDate,md.MedicineExpDate from Medicines as m
join MedicineDetails as md
on md.MedicineId=m.MedicineId
where md.MedicinePrice=4.2


select s.MedicineCount from Medicines as m
join MedicineDetails as  md
on m.MedicineId = md.MedicineId
join Stocks as s
on s.StockId = m.StockId
where md.MedicineName='parol'

create procedure UptadeCountt @count int , @name nvarchar(max), @pId int
As
Begin
update Stocks
set MedicineCount = @count
WHERE StockId = (
select Medicines.StockId FROM MedicineDetails
JOIN Medicines ON Medicines.MedicineId = MedicineDetails.MedicineId
join Pharmacies on Pharmacies.PharmacyId = Stocks.PharmacyId
WHERE MedicineDetails.MedicineName = @name and Pharmacies.PharmacyId = @pId
)
End



Insert Into MedicineDetails Values ('ll',25,0,1,12-09-2023,11-20-2024) SELECT SCOPE_IDENTITY()


select s.MedicineCount from Stocks as s join Medicines as m on m.StockId = s.StockId join MedicineDetails as md
on md.MedicineId = m.MedicineId where md.MedicineName = 'paroL'

select p.PharmacyId from Pharmacies as p where p.PharmacyName = 'oo'




select md.MedicineId, md.MedicineName,md.MedicinePrice from MedicineDetails as md 


select md.MedicineName,md.MedicinePrice,md.MedicineProdDate,md.MedicineExpDate from MedicineDetails as md 



select md.MedicineName,md.MedicinePrice,md.MedicineProdDate,md.MedicineExpDate from  MedicineDetails as md 


select md.MedicineName,md.MedicinePrice from MedicineDetails as md


select md.MedicineName,md.MedicinePrice,md.MedicineProdDate,md.MedicineExpDate from MedicineDetails as md 
join Medicines as m on m.MedicineId = md.MedicineId
join Stocks as s on s.StockId = m.StockId
join Pharmacies as p on p.PharmacyId = s.PharmacyId
where md.MedicineName=@name and s.StockId = @stockId and p.PharmacyId = @pId

select Count(*) from MedicineDetails as md 
join Medicines as m on m.MedicineId = md.MedicineId 
join Stocks as s on s.StockId = m.StockId
join Pharmacies as p on p.PharmacyId = s.PharmacyId
where p.PharmacyId = @pId and md.MedicineId = @id


select count(*) from MedicineDetails as md 
join Medicines as m on m.MedicineId = md.MedicineId 
join Stocks as s on s.StockId = m.StockId


select count(*) from Stocks as s



Update Stocks Set MedicineCount = 250 
where StockId = (
select Medicines.StockId FROM MedicineDetails
JOIN Medicines ON Medicines.MedicineId = MedicineDetails.MedicineId
join Pharmacies on Pharmacies.PharmacyId = Stocks.PharmacyId
WHERE Stocks.StockId=4 and Pharmacies.PharmacyId = 1
)


select * from Stocks as s where s.StockId = 4 and s.PharmacyId = 2
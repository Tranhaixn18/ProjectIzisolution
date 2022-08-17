use [Avani.Andon]
select * from tblEmployee
go
alter table tblEmployee
add RFId int
go
alter table tblEmployee
add RFCode nvarchar(50)
go
alter table tblEmployee
add Code nvarchar(50)
go
alter table tblEmployee
add Active bit not null


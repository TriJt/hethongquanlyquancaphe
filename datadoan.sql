create database quanlyquanan
go 

use quanlyquanan
go 


create table tablefood
(
	id int identity primary key, 
	name nvarchar(100) not null  , 
	status nvarchar(100) not null default N'Trống'  
)
go 


create table account 
(
	userName nvarchar(100) primary key,
	displayName nvarchar(100) not null ,  
	password nvarchar(1000) not null default  N'1' , 
	type int not null 
)
go



create table foodcategory
(
	id int primary key, 
	name nvarchar(100) not null default 'Chưa đặt tên' ,
)
 
go

create table food
(
	id int identity primary key, 
	name nvarchar(100) not null default 'Chưa đặt tên',
	idCategory int not null, 
	price float not null,
	foreign key (idCategory) references dbo.foodcategory(id)
 )
go 

create table bill
(
	id int identity primary key, 
	idTable int not null, 
	dateCheckin date not null, 
	dateCheckout date, 
	status int not null default 0,
	discount int , 
	totalPrice float,	
	foreign key (idTable) references dbo.tablefood(id)
)
go  
 
 

create table billInfo 
(
	id int identity primary key, 
	idBill int not null, 
	idFood int not null, 
	count int not null default 0, 
	foreign key (idBill) references dbo.bill(id), 
	foreign key (idFood) references dbo.food(id)
)

go 
-- add into account 
insert into dbo.account(username,displayname,password, type) values (N'thanhtri0122',N'Lê Thành Trí',N'0991edad',1); 
go 
insert into dbo.account(username,displayname,password, type) values (N'thanhtri0123',N'Lê Thành Tha',N'0991eqqqww',0); 
go
insert  dbo.account(username,displayname,password, type) values (N'staff',N'Nhan vien',N'1',0);
go
insert into dbo.account(username,displayname,password, type) values (N'K1',N'Lê ',N'1',1); 
go



-- lấy danh sách tài khoản qua account 
create proc USP_GetListAccountByUsername 
@username nvarchar(100) 
as 
begin 
	select * from dbo.account where userName = @username
end 
go 


-- đăng nhập vào hệ thống 
create proc USP_Login
@username nvarchar(100), @password  nvarchar(100)
as
begin 
	select * from dbo.account where @username = userName and @password = password  
end 
go


-- insert into tablefood bên server 
declare @i  int = 1 while @i <=10 
begin
	insert dbo.tablefood(name,status) values (N'Bàn ' + CAST(@i as nvarchar(100)),N'Trống')
	set @i = @i +1
end 
go

-- thêm category 
insert	dbo.foodcategory(id,name) values  (001,N'Thức ăn')
insert	dbo.foodcategory(id, name) values  (002,N'Nước uống')
insert	dbo.foodcategory(id,name) values  (003,N'Đồ ăn vặt')
go
-- thêm food 
insert dbo.food(name,idCategory,price) values (N'Cà phê',002,15000)
insert dbo.food(name,idCategory,price) values (N'Cơm',001,25000)
insert dbo.food(name,idCategory,price) values (N'Bánh tráng',003,10000)
insert dbo.food(name,idCategory,price) values (N'Nước ngọt',002,15000)
insert dbo.food(name,idCategory,price) values (N'Bún cá',001,35000)
insert dbo.food(name,idCategory,price) values (N'Snack',003,5000)
go
-- thêm bill 


-- lấy danh sách table ra 
create proc USP_GetTableList as select * from dbo.tablefood 
go 
 


-- thêm bill vào bàn 
create proc USP_InsertBill 
@idTable int 
as 
begin 
	insert into dbo.bill (dateCheckin,dateCheckout,idTable,status,discount)
	values (GETDATE(), null, @idTable, 0, 0) 
	
end 
go 

-- thêm billinfo vào bàn 
create proc USP_InsertBillInfo
@idBill int , @idFood int , @count int
as begin 
	declare @isExitsBillInfo int 
	declare @foodCount int =1
	
		select @isExitsBillInfo = id, @foodCount = b.count 
		from dbo.billInfo as b 
		where idBill = @idBill and idFood = @idFood
	if(@isExitsBillInfo > 0)
	begin 
		declare @newCount int = @foodCount + @count 
		if(@newCount > 0)
			update dbo.billInfo set count = @foodCount+ @count where idFood = @idFood
		else
			delete dbo.billInfo where idBill = @idBill and idFood = @idFood
	end 
	else
	begin 
		insert dbo.billInfo(idBill,idFood,count)
		values (@idBill , @idFood , @count )
	end 		
end 
go 


-- update billinfo 
CREATE trigger UTG_UpdateBillInfo 
on dbo.billInfo for Insert, Update 
as 
begin 
	declare @idBill Int
	
	select @idBill = idBill from Inserted 
	
	declare @idTable int 
	
	select @idTable = idTable from dbo.Bill where id =@idBill and status = 0 
	
	update dbo.tablefood SET status  = N'Có người' where id = @idTable  
end 
go

-- thêm  bill vào bàn đã có người 

create trigger UTG_UpdateBill 
on dbo.bill  for update 
as 
begin 
	declare @idBill int 
	select @idBill = id from Inserted 
	
	declare @idTable int 
	select @idTable = idTable from dbo.bill where id = @idBill 
	declare @count int = 0 
	select @count = COUNT(*) from dbo.bill where idTable = @idTable AND status = 0 
	
	if(@count = 0)
		update dbo.tablefood set status = N'Trống' where id = @idTable
end 
go 

-- lấy danh sách bill qua ngày tháng được  chọn 
create proc USP_GetListBillByDate
@checkIn date , @checkOut date 
as 
begin 
	select t.name AS [Tên bàn], b.totalPrice as [Tổng tiền], dateCheckin as [Ngày vào], dateCheckout as [Ngày ra], discount as [Giảm giá]
	from dbo.bill as b, dbo.tablefood as t 
	where dateCheckin >= @checkIn and dateCheckout <= @checkOut and b.status = 1
	and t.id = b.idTable
end 
go 

-- update phần thông tin cá nhân 

create proc USP_UpdateAccount 
@userName nvarchar(100), @displayName nvarchar(100) , @password nvarchar(1000), @newPassword nvarchar(100)
as 
begin 
	declare @isRightPass int 
	select @isRightPass = COUNT(*) from dbo.account where userName = @userName and password = @password 
	
	if(@isRightPass = 1)
		begin 
			if(@newPassword = null or @newPassword ='')
				begin 
				update dbo.account set displayName = @displayName where userName = @userName 
				end 
			else 
				begin 
				update dbo.account set displayName = @displayName, password = @newPassword where userName = @userName 
				end 
		end 
end 
go 
 
 -- TRIGGER  xóa billinfo khi food bị xóa 
 
 create trigger UTG_DeleteBillInfoByFoodID 
 on dbo.billInfo for delete 
 as 
 begin 
	declare @idbillInfo int 
	declare @idbill int
	select @idbillInfo = id , @idbill = Deleted.idbill from Deleted
	
	declare @idTable int 
	select @idTable = idTable from  dbo.bill where id = @idbill
	
	declare @count int = 0 
	select @count = COUNT(*) from dbo.billInfo as bi, dbo.bill as b  where b.id = bi.idBill and b.id = @idbill and b.status = 0
	if( @count = 0 )
		update dbo.tablefood SET status = N'Trống' where id = @idTable 
		
 end 
 go 
 
 -- fuction loại bỏ dấu tiếng việt 
 CREATE FUNCTION [dbo].[fuConvertToUnsign1]
(
 @strInput NVARCHAR(4000)
)
RETURNS NVARCHAR(4000)
AS
BEGIN 
 IF @strInput IS NULL RETURN @strInput
 IF @strInput = '' RETURN @strInput
 DECLARE @RT NVARCHAR(4000)
 DECLARE @SIGN_CHARS NCHAR(136)
 DECLARE @UNSIGN_CHARS NCHAR (136)
 SET @SIGN_CHARS = N'ăâđêôơưàảãạáằẳẵặắầẩẫậấèẻẽẹéềểễệế
 ìỉĩịíòỏõọóồổỗộốờởỡợớùủũụúừửữựứỳỷỹỵý
 ĂÂĐÊÔƠƯÀẢÃẠÁẰẲẴẶẮẦẨẪẬẤÈẺẼẸÉỀỂỄỆẾÌỈĨỊÍ
 ÒỎÕỌÓỒỔỖỘỐỜỞỠỢỚÙỦŨỤÚỪỬỮỰỨỲỶỸỴÝ'
 +NCHAR(272)+ NCHAR(208)
 SET @UNSIGN_CHARS = N'aadeoouaaaaaaaaaaaaaaaeeeeeeeeee
 iiiiiooooooooooooooouuuuuuuuuuyyyyy
 AADEOOUAAAAAAAAAAAAAAAEEEEEEEEEEIIIII
 OOOOOOOOOOOOOOOUUUUUUUUUUYYYYYDD'
 DECLARE @COUNTER int
 DECLARE @COUNTER1 int
 SET @COUNTER = 1
 WHILE (@COUNTER <=LEN(@strInput))
 BEGIN 
 SET @COUNTER1 = 1
 WHILE (@COUNTER1 <=LEN(@SIGN_CHARS)+1)
 BEGIN
 IF UNICODE(SUBSTRING(@SIGN_CHARS, @COUNTER1,1))
 = UNICODE(SUBSTRING(@strInput,@COUNTER ,1) )
 BEGIN 
 IF @COUNTER=1
 SET @strInput = SUBSTRING(@UNSIGN_CHARS, @COUNTER1,1)
 + SUBSTRING(@strInput, @COUNTER+1,LEN(@strInput)-1) 
 ELSE
 SET @strInput = SUBSTRING(@strInput, 1, @COUNTER-1)
 +SUBSTRING(@UNSIGN_CHARS, @COUNTER1,1)
 + SUBSTRING(@strInput, @COUNTER+1,LEN(@strInput)- @COUNTER)
 BREAK
 END
 SET @COUNTER1 = @COUNTER1 +1
 END
 SET @COUNTER = @COUNTER +1
 END
 SET @strInput = replace(@strInput,' ','-')
 RETURN @strInput
END
go 



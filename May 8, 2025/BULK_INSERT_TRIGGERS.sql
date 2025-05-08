  select * from products where 
  try_cast(json_value(details,'$.spec.cpu') as nvarchar(20)) ='i7'

  create proc proc_FilterProducts(@pcpu varchar(20), @pcount int out)
  as
  begin
      set @pcount = (select count(*) from products where 
	  try_cast(json_value(details,'$.spec.cpu') as nvarchar(20)) =@pcpu)
  end
  go
 begin
  declare @cnt int
 exec proc_FilterProducts 'i7', @cnt out
  print concat('The number of computers is ',@cnt)
  end


sp_help authors

create table people
(id int primary key,
name nvarchar(20),
age int)

create or alter proc proc_BulkInsert(@filepath nvarchar(500))
as
begin
   declare @insertQuery nvarchar(max)

   set @insertQuery = 'BULK INSERT people from '''+ @filepath +'''
   with(
   FIRSTROW =2,
   FIELDTERMINATOR='','',
   ROWTERMINATOR = ''\n'')'
   exec sp_executesql @insertQuery
end

proc_BulkInsert 'D:\Corp\GenSpark\Presidio\2025\Participants\Day3\Data.csv'
drop proc proc_BulkInsert
select * from people

create table BulkInsertLog
(LogId int identity(1,1) primary key,
FilePath nvarchar(1000),
status nvarchar(50) constraint chk_status Check(status in('Success','Failed')),
Message nvarchar(1000),
InsertedOn DateTime default GetDate())


create or alter proc proc_BulkInsert(@filepath nvarchar(500))
as
begin
  Begin try
	   declare @insertQuery nvarchar(max)

	   set @insertQuery = 'BULK INSERT people from '''+ @filepath +'''
	   with(
	   FIRSTROW =2,
	   FIELDTERMINATOR='','',
	   ROWTERMINATOR = ''\n'')'

	   exec sp_executesql @insertQuery

	   insert into BulkInsertLog(filepath,status,message)
	   values(@filepath,'Success','Bulk insert completed')
  end try
  begin catch
		 insert into BulkInsertLog(filepath,status,message)
		 values(@filepath,'Failed',Error_Message())
  END Catch
end

proc_BulkInsert 'D:\Corp\GenSpark\Presidio\2025\Participants\Day3\Data.csv'

select * from BulkInsertLog

truncate table people

with cteAuthors
as
(select au_id, concat(au_fname,' ',au_lname) author_name from authors)

select * from cteAuthors

declare @page int =2, @pageSize int=10;
with PaginatedBooks as
( select  title_id,title, price, ROW_Number() over (order by price desc) as RowNum
  from titles
)
select * from PaginatedBooks where rowNUm between((@page-1)*@pageSize) and (@page*@pageSize)

--create a sp that will take the page number and size as param and print the books
create or alter proc proc_PaginateTitles( @page int =1, @pageSize int=10)
as
begin
with PaginatedBooks as
( select  title_id,title, price, ROW_Number() over (order by price desc) as RowNum
  from titles
)
select * from PaginatedBooks where rowNUm between((@page-1)*(@pageSize+1)) and (@page*@pageSize)
end

exec proc_PaginateTitles 2,5

 select  title_id,title, price
  from titles
  order by price desc
  offset 10 rows fetch next 10 rows only

  create function  fn_CalculateTax(@baseprice float, @tax float)
  returns float
  as
  begin
     return (@baseprice +(@baseprice*@tax/100))
  end

  select dbo.fn_CalculateTax(1000,10)

  select title,dbo.fn_CalculateTax(price,12) from titles

  create function fn_tableSample(@minprice float)
  returns table
  as

    return select title,price from titles where price>= @minprice


drop function fn_tableSample

	select * from dbo.fn_tableSample(10)

--older and slower but supports more logic
create function fn_tableSampleOld(@minprice float)
  returns @Result table(Book_Name nvarchar(100), price float)
  as
  begin
    insert into @Result select title,price from titles where price>= @minprice
    return 
end
select * from dbo.fn_tableSampleOld(10)




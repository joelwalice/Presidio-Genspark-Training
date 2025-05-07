use pubs;

SELECT * from publishers;
SELECT * FROM titles;
SELECT * FROM sales;
SELECT * FROM stores;

SELECT * FROM titleauthor;

Select au_id, title from titleauthor JOIN titles ON titleauthor.title_id = titles.title_id;

--Print the publisher's name, book name and the order date of the  books
select pub_name as Publisher_Name, title, ord_date from publishers JOIN titles on publishers.pub_id = titles.pub_id JOIN sales ON titles.title_id = sales.title_id;

-- Print the publisher name and the first book sale date for all the publishers
SELECT p.pub_name as Publisher_Name, MIN(s.ord_date) as Ord_Date from publishers p LEFT JOIN titles t ON p.pub_id = t.pub_id LEFT JOIN sales s ON t.title_id = s.title_id GROUP BY p.pub_name ORDER BY 2 DESC;

-- Print the bookname and the store address of the sale
SELECT title, stor_address from titles JOIN sales ON sales.title_id = titles.title_id JOIN stores ON stores.stor_id = sales.stor_id;

-- ADHOC Query -> Improves performance -> Compiles first, Execution Plan

-- Stored Procedure -> It Pre Compiled, For security reasons -> Pass the parameters as Objects, Encryption, Encapsulate, Not neede to understand the underlying table
-- --In Parameter, Out Parameter

create procedure proc_FirstProcedure
as
begin
	print 'Hello World'
end
Go
exec proc_FirstProcedure

create table Products
(id int identity(1,1) constraint pk_productId primary key,
name nvarchar(100) not null,
details nvarchar(max))
Go
create proc proc_InsertProduct(@pname nvarchar(100),@pdetails nvarchar(max))
as
begin
    insert into Products(name,details) values(@pname,@pdetails)
end
go
proc_InsertProduct 'Laptop','{"brand":"Dell","spec":{"ram":"16GB","cpu":"i5"}}'
go
select * from Products

-- Get the JSON Data as Output
select JSON_QUERY(details, '$.spec') Product_Specification from products;

-- Update value using Procedure
create proc proc_UpdateProductSpec(@pid int,@newvalue varchar(20))
as
begin
   update products set details = JSON_MODIFY(details, '$.spec.ram',@newvalue) where id = @pid
end

proc_UpdateProductSpec 1, '24GB';

create table Posts
(id int primary key,
title nvarchar(100),
user_id int,
body nvarchar(max))
Go

select * from Posts

create proc proc_BulInsertPosts(@jsondata nvarchar(max))
as
begin
	insert into Posts(user_id,id,title,body)
	select userId,id,title,body from openjson(@jsondata)
	with (userId int,id int, title varchar(100), body varchar(max))
end
  

proc_BulInsertPosts '
[
  {
    "userId": 1,
    "id": 1,
    "title": "sunt aut facere repellat provident occaecati excepturi optio reprehenderit",
    "body": "quia et suscipit\nsuscipit recusandae consequuntur expedita et cum\nreprehenderit molestiae ut ut quas totam\nnostrum rerum est autem sunt rem eveniet architecto"
  },
  {
    "userId": 1,
    "id": 2,
    "title": "qui est esse",
    "body": "est rerum tempore vitae\nsequi sint nihil reprehenderit dolor beatae ea dolores neque\nfugiat blanditiis voluptate porro vel nihil molestiae ut reiciendis\nqui aperiam non debitis possimus qui neque nisi nulla"
  }]'

 create procedure proc_BringPosts(@pid int)
 as
 begin
	SELECT * FROM Posts Where id = @pid;
 end

 proc_BringPosts 1
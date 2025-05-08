use pubs;

select * from Orders;
select * from Products;
select * from Customers;
select * from Employees;

-- 1). List all orders with the customer name and the employee who handled the order.
   -- (Join Orders, Customers, and Employees)

select o.OrderId as Order_ID, o.OrderDate as Order_Date, o.EmployeeID as Employee_ID, o.ShipName as Shipment_Name, c.CompanyName as Customer_Name from Orders o JOIN Employees e ON o.EmployeeID = e.EmployeeID JOIN Customers c ON c.CustomerID = o.CustomerID;

-- 2). Get a list of products along with their category and supplier name.
   -- (Join Products, Categories, and Suppliers)

select DISTINCT p.ProductName as Product_Name, c.CategoryName as Categories, s.CompanyName as Suppliers from Products p JOIN Categories c on p.CategoryID = c.CategoryId JOIN Suppliers s ON s.SupplierID = p.SupplierID;

-- 3). Show all orders and the products included in each order with quantity and unit price.
   --(Join Orders, Order Details, Products)

select o.OrderID, od.UnitPrice, od.Quantity, p.ProductName from Orders o JOIN OrderDetails od ON o.OrderID = od.OrderID JOIN Products p ON p.ProductID = od.ProductID;

-- 4). List employees who report to other employees (manager-subordinate relationship).
   -- (Self join on Employees)

SELECT CONCAT(e.FirstName, ' ', e.LastName) as Employees, CONCAT(m.FirstName, ' ', m.LastName) as ReportedTo FROM Employees e JOIN Employees m ON m.ReportsTo = e.EmployeeID;

-- 5). Display each customer and their total order count.
   -- (Join Customers and Orders, then GROUP BY)

SELECT c.CompanyName as Customer_Name, Count(o.OrderID) as No_Of_Orders from Customers c JOIN Orders o ON o.CustomerID = c.CustomerID GROUP BY c.CompanyName;

-- 6). Find the average unit price of products per category.
   --Use AVG() with GROUP BY

SELECT c.CategoryName, AVG(p.UnitPrice) as Average_Unit_Price from Products p JOIN Categories c ON c.CategoryID = p.CategoryID GROUP BY CategoryName;

-- 7). List customers where the contact title starts with 'Owner'.
   -- Use LIKE or LEFT(ContactTitle, 5)

SELECT CustomerID, CompanyName as Customer_Name FROM Customers WHERE ContactTitle LIKE 'Owner';

-- 8). Show the top 5 most expensive products.
   -- Use ORDER BY UnitPrice DESC and TOP 5

SELECT TOP 5 * FROM Products Order By UnitPrice DESC;

-- 9). Return the total sales amount (quantity × unit price) per order.
   -- Use SUM(OrderDetails.Quantity * OrderDetails.UnitPrice) and GROUP BY

SELECT SUM(Quantity * UnitPrice) AS Total_Sales_Amount FROM OrderDetails;

-- 10). Create a stored procedure that returns all orders for a given customer ID.
   --Input: @CustomerID

create proc proc_ReturnAll(@CustomerID nvarchar(20))
as
begin
	SELECT * FROM Orders Where CustomerId = @CustomerID;
end

proc_ReturnAll 'FOLKO'

-- 11). Write a stored procedure that inserts a new product.
   --Inputs: ProductName, SupplierID, CategoryID, UnitPrice, etc.

create or alter proc proc_InsertData
as
begin
	INSERT INTO Products VALUES('Picorino Romano', 17, 4, '5 kg pkg.', 18.00, 30, 100, 100, 0);
end

proc_InsertData

-- 12). Create a stored procedure that returns total sales per employee.
   -- Join Orders, Order Details, and Employees 

create or alter proc proc_totalSales
as 
begin 
	SELECT e.EmployeeID, CONCAT(e.FirstName, ' ', e.LastName) as Employee, SUM(od.Quantity * od.UnitPrice) AS Total_Sales FROM Employees e JOIN Orders o
	ON o.EmployeeID = e.EmployeeID JOIN OrderDetails od ON od.OrderID = o.OrderID GROUP BY e.EmployeeID, e.FirstName, e.LastName;
end

proc_totalSales

-- 13). Use a CTE to rank products by unit price within each category.
   -- Use ROW_NUMBER() or RANK() with PARTITION BY CategoryID

WITH cte_RankProducts 
AS (
SELECT ProductID, ProductName, CategoryID, UnitPrice, RANK() OVER (PARTITION BY CategoryID ORDER BY UnitPrice DESC) AS PriceRank
FROM Products
)

SELECT * FROM cte_RankProducts;

-- 14). Create a CTE to calculate total revenue per product and filter products with revenue > 10,000.

WITH cte_TotalRevenue
AS
(
	SELECT p.ProductID, SUM(od.UnitPrice * od.Quantity) as Total_Revenue FROM Products p JOIN OrderDetails od ON od.ProductID = p.ProductID GROUP BY p.ProductId
)

SELECT * FROM cte_TotalRevenue WHERE Total_Revenue > 10000;

-- 15). Use a CTE with recursion to display employee hierarchy.
   -- Start from top-level employee (ReportsTo IS NULL) and drill down

With cte_Employee
AS
(
	Select EmployeeId, CONCAT(FirstName, ' ', LastName) as Employee_Name, ReportsTo FROM Employees WHERE ReportsTo is NULL
	UNION ALL
	Select e1.EmployeeId, CONCAT(e1.FirstName, ' ', e1.LastName) as Employee_Name, e2.ReportsTo as Reported_Employee_Name FROM Employees e1 JOIN Employees e2 ON e2.ReportsTo = e1.EmployeeId
)
Select * FROM cte_Employee;
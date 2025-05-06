# The DDL, DML Codes for the DB Schema given below

## Table Schema:	 

### Created Tables with Integrity Constrains for the below DB Schema: 

- EMP (empno - primary key, empname, salary, deptname - references entries in a deptname of department table with null constraint, bossno - references entries in an empno of emp table with null constraint) 
- DEPARTMENT (deptname - primary key, floor, phone, empno - references entries in an empno of emp table not null) 
- SALES (salesno - primary key, saleqty, itemname -references entries in a itemname of item table with not null constraint, deptname - references entries in a deptname of department table with not null constraint) 
- ITEM (itemname - primary key, itemtype, itemcolor) 

### The SQL Snippet is added to the repository as .sql file
CREATE DATABASE deptEmp;
USE deptEmp;

CREATE TABLE Department (
    deptname VARCHAR(50) PRIMARY KEY,
    deptfloor INT,
    deptphone INT,
    empno INT NOT NULL
);

CREATE TABLE Emp (
    empno INT PRIMARY KEY,
    empname VARCHAR(50),
    salary INT,
    deptname VARCHAR(50) NOT NULL,
    bossno INT
);

CREATE TABLE Item (
    itemname VARCHAR(100) PRIMARY KEY,
    itemtype CHAR(1),
    itemcolor VARCHAR(50)
);

CREATE TABLE Sales (
    salesno INT PRIMARY KEY,
    saleqty INT,
    itemname VARCHAR(100) NOT NULL,
    deptname VARCHAR(50) NOT NULL,
    FOREIGN KEY (itemname) REFERENCES Item(itemname),
    FOREIGN KEY (deptname) REFERENCES Department(deptname)
);

INSERT INTO Emp VALUES (1, 'Alice', 75000, 'Management', NULL);
INSERT INTO Emp VALUES (2, 'Ned', 45000, 'Marketing', 1);
INSERT INTO Emp VALUES (3, 'Andrew', 25000, 'Marketing', 2);
INSERT INTO Emp VALUES (4, 'Clare', 22000, 'Marketing', 2);
INSERT INTO Emp VALUES (5, 'Todd', 38000, 'Accounting', 1);
INSERT INTO Emp VALUES (6, 'Nancy', 22000, 'Accounting', 5);
INSERT INTO Emp VALUES (7, 'Brier', 43000, 'Purchasing', 1);
INSERT INTO Emp VALUES (8, 'Sarah', 56000, 'Purchasing', 7);
INSERT INTO Emp VALUES (9, 'Sophile', 35000, 'Personnel', 1);
INSERT INTO Emp VALUES (10, 'Sanjay', 15000, 'Navigation', 3);
INSERT INTO Emp VALUES (11, 'Rita', 15000, 'Books', 4);
INSERT INTO Emp VALUES (12, 'Gigi', 16000, 'Clothes', 4);
INSERT INTO Emp VALUES (13, 'Maggie', 11000, 'Clothes', 4);
INSERT INTO Emp VALUES (14, 'Paul', 15000, 'Equipment', 3);
INSERT INTO Emp VALUES (15, 'James', 15000, 'Equipment', 3);
INSERT INTO Emp VALUES (16, 'Pat', 15000, 'Furniture', 3);
INSERT INTO Emp VALUES (17, 'Mark', 15000, 'Recreation', 3);

INSERT INTO Department VALUES ('Management', 5, 34, 1);
INSERT INTO Department VALUES ('Books', 1, 81, 4);
INSERT INTO Department VALUES ('Clothes', 2, 24, 4);
INSERT INTO Department VALUES ('Equipment', 3, 57, 3);
INSERT INTO Department VALUES ('Furniture', 4, 14, 3);
INSERT INTO Department VALUES ('Navigation', 1, 41, 3);
INSERT INTO Department VALUES ('Recreation', 2, 29, 4);
INSERT INTO Department VALUES ('Accounting', 5, 35, 5);
INSERT INTO Department VALUES ('Purchasing', 5, 36, 7);
INSERT INTO Department VALUES ('Personnel', 5, 37, 9);
INSERT INTO Department VALUES ('Marketing', 5, 38, 2);

ALTER TABLE Emp ADD CONSTRAINT fk_emp_deptname FOREIGN KEY (deptname) REFERENCES Department(deptname);
ALTER TABLE Emp ADD CONSTRAINT fk_emp_bossno FOREIGN KEY (bossno) REFERENCES Emp(empno);
ALTER TABLE Department ADD CONSTRAINT fk_dept_empno FOREIGN KEY (empno) REFERENCES Emp(empno);

INSERT INTO Item VALUES ('Pocket Knife-Nile', 'E', 'Brown');
INSERT INTO Item VALUES ('Pocket Knife-Avon', 'E', 'Brown');
INSERT INTO Item VALUES ('Compass', 'N', NULL);
INSERT INTO Item VALUES ('Geo positioning system', 'N', NULL);
INSERT INTO Item VALUES ('Elephant Polo stick', 'R', 'Bamboo');
INSERT INTO Item VALUES ('Camel Saddle', 'R', 'Brown');
INSERT INTO Item VALUES ('Sextant', 'N', NULL);
INSERT INTO Item VALUES ('Map Measure', 'N', NULL);
INSERT INTO Item VALUES ('Boots-snake proof', 'C', 'Green');
INSERT INTO Item VALUES ('Pith Helmet', 'C', 'Khaki');
INSERT INTO Item VALUES ('Hat-polar Explorer', 'C', 'White');
INSERT INTO Item VALUES ('Exploring in 10 Easy Lessons', 'B', NULL);
INSERT INTO Item VALUES ('Hammock', 'F', 'Khaki');
INSERT INTO Item VALUES ('How to win Foreign Friends', 'B', NULL);
INSERT INTO Item VALUES ('Map case', 'E', 'Brown');
INSERT INTO Item VALUES ('Safari Chair', 'F', 'Khaki');
INSERT INTO Item VALUES ('Safari cooking kit', 'F', 'Khaki');
INSERT INTO Item VALUES ('Stetson', 'C', 'Black');
INSERT INTO Item VALUES ('Tent - 2 person', 'F', 'Khaki');
INSERT INTO Item VALUES ('Tent -8 person', 'F', 'Khaki');

INSERT INTO Sales VALUES (101, 2, 'Boots-snake proof', 'Clothes');
INSERT INTO Sales VALUES (102, 1, 'Pith Helmet', 'Clothes');
INSERT INTO Sales VALUES (103, 1, 'Sextant', 'Navigation');
INSERT INTO Sales VALUES (104, 3, 'Hat-polar Explorer', 'Clothes');
INSERT INTO Sales VALUES (105, 5, 'Pith Helmet', 'Equipment');
INSERT INTO Sales VALUES (106, 2, 'Pocket Knife-Nile', 'Clothes');
INSERT INTO Sales VALUES (107, 3, 'Pocket Knife-Nile', 'Recreation');
INSERT INTO Sales VALUES (108, 1, 'Compass', 'Navigation');
INSERT INTO Sales VALUES (109, 2, 'Geo positioning system', 'Navigation');
INSERT INTO Sales VALUES (110, 5, 'Map Measure', 'Navigation');
INSERT INTO Sales VALUES (111, 1, 'Geo positioning system', 'Books');
INSERT INTO Sales VALUES (112, 1, 'Sextant', 'Books');
INSERT INTO Sales VALUES (113, 3, 'Pocket Knife-Nile', 'Books');
INSERT INTO Sales VALUES (114, 1, 'Pocket Knife-Nile', 'Navigation');
INSERT INTO Sales VALUES (115, 1, 'Pocket Knife-Nile', 'Equipment');
INSERT INTO Sales VALUES (116, 1, 'Sextant', 'Clothes');
INSERT INTO Sales VALUES (117, 1, 'Sextant', 'Equipment');
INSERT INTO Sales VALUES (118, 1, 'Sextant', 'Recreation');
INSERT INTO Sales VALUES (119, 1, 'Sextant', 'Furniture');
INSERT INTO Sales VALUES (120, 1, 'Pocket Knife-Nile', 'Furniture');
INSERT INTO Sales VALUES (121, 1, 'Exploring in 10 Easy Lessons', 'Books');
INSERT INTO Sales VALUES (122, 1, 'How to win Foreign Friends', 'Books');
INSERT INTO Sales VALUES (123, 1, 'Compass', 'Books');
INSERT INTO Sales VALUES (124, 1, 'Pith Helmet', 'Books');
INSERT INTO Sales VALUES (125, 1, 'Elephant Polo stick', 'Recreation');
INSERT INTO Sales VALUES (126, 1, 'Camel Saddle', 'Recreation');

SELECT * FROM Department;
SELECT * FROM Sales;
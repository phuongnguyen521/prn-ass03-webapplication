CREATE DATABASE SaleManagement

USE SaleManagement

USE master
/*DROP DATABASE SaleManagement
DROP TABLE OrderDetail
DROP TABLE Orders
DROP TABLE Member
DROP TABLE Product
*/

CREATE TABLE Member(
	MemberId		INT				PRIMARY KEY,
	Email			VARCHAR(100)	NOT NULL,
	CompanyName		VARCHAR(40)		NOT NULL,
	City			VARCHAR(15)		NOT NULL,
	Country			VARCHAR(15)		NOT NULL,
	Password		VARCHAR(30)		NOT NULL
)

CREATE TABLE Product(
	ProductId		INT				PRIMARY KEY,
	CategoryId		INT				NOT NULL,
	ProductName		VARCHAR(40)		NOT NULL,
	Weight			VARCHAR(20)		NOT NULL,
	UnitPrice		money			NOT NULL,
	UnitsInStock		INT				NOT NULL
)

CREATE TABLE Orders(
	OrderId			INT				PRIMARY KEY,
	MemberId		INT				NOT NULL,
	OrderDate		datetime		NOT NULL,
	RequiredDate	datetime		NULL,
	ShippedDate		datetime		NULL,
	Freight			money			NULL,
)

CREATE TABLE OrderDetail(
	OrderId			INT				FOREIGN KEY REFERENCES Orders(OrderId),
	ProductId		INT				FOREIGN KEY REFERENCES Product(ProductId),
	UnitPrice		money			NOT NULL,
	Quantity		INT				NOT NULL,
	Discount		FLOAT			NOT NULL,
	PRIMARY KEY (OrderId, ProductId)
)

INSERT INTO dbo.Member (MemberId, Email, CompanyName, City, Country, Password)
VALUES
(2,'phuong@gmail.com','KMS','Da Nang','Viet Nam','123'),
(3,'hanh@gmail.com','Dien Luc Tan Thuan','Da Nang','USA','123'),
(4,'binh@gmail.com','Thanh Da High School','Ha Noi','UK','123'),
(5,'huy@gmail.com','FPTU','Ho Chi Minh','Viet Nam','456'),
(6,'vinhnd@fpt.edu.vn','KMS','Da Nang','Viet Nam','456'),
(7,'huong123@gmail.com','Viet Uc','Sydney','Australia','456'),
(8,'hieu@gmail.com','Gia Dinh High School','Tay Nguyen','Viet Nam','123'),
(9,'kim123@gmail.com','FPTU','Ho Chi Minh','Viet Nam','789'),
(10,'hung@gmail.com','KMS','Da Nang','Viet Nam','789'),
(11,'linhnv@fpt.edu.vn','HighLand Inc','Da Nang','USA','112'),
(12,'mytdv@gmail.com','Hoang Hoa THam High School','Ha Noi','UK','112'),
(13,'ky@fpt.edu.vn','FPTU','Ho Chi Minh','Japan','132'),
(14,'henry@gmail.com','Gia Dinh High School','Ha Noi','Japan','132'),
(15,'ducdh@netcompany.com','NetCompany','Ho Chi Minh','Viet Nam','132')
-- USE SaleManagement
--DELETE FROM dbo.Member
--SELECT * FROM dbo.Member

INSERT INTO dbo.Product(ProductId, CategoryId, ProductName, Weight, UnitPrice, UnitsInStock)
VALUES
(1,1,'Pork','12',125000,5),
(2,1,'Beef','1',90000,6),
(3,1,'Chicken Meat','2',80000,7),
(4,2,'Fish','5',100000,5),
(5,3,'Goat Meat','7',160000,2),
(6,4,'Egg','0.5',125000,5),
(7,5,'Tiramisu','0.25',30000,6),
(8,5,'Cheese Cake','0.25',15000,7),
(9,5,'Strawberry Cake','0.3',20000,10),
(10,2,'Tuna','1',100000,11),
(11,2,'Silver Fish','1',125000,6),
(12,4,'Milk','1.5',90000,6),
(13,2,'Yogurt','2',80000,7),
(14,1,'Frog Meat','5',100000,5),
(15,3,'Sushi','7',160000,15)
-- USE SaleManagement
--DELETE FROM dbo.Product
--SELECT * FROM dbo.Product

--SELECT * FROM Member
--SELECT * FROM Product

INSERT INTO dbo.Orders(OrderId,MemberId,OrderDate, RequiredDate, ShippedDate, Freight)
VALUES
(1,2,'2021-10-20','2021-10-22','2021-10-21',10000),
(2,2,'2021-09-20','2021-09-22','2021-09-21',20000)

-- SELECT * FROM dbo.Orders

INSERT INTO dbo.OrderDetail(OrderId, ProductId, Quantity,UnitPrice, Discount)
VALUES
(1,1,2,125000,5),
(1,2,3,90000,10),
(2,3,1,80000,10),
(2,4,2,100000,5)

-- SELECT * FROM dbo.OrderDetail
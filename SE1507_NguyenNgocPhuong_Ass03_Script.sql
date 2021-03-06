USE [SaleManagement]
GO
ALTER TABLE [dbo].[OrderDetail] DROP CONSTRAINT [FK__OrderDeta__Produ__55F4C372]
GO
ALTER TABLE [dbo].[OrderDetail] DROP CONSTRAINT [FK__OrderDeta__Order__55009F39]
GO
/****** Object:  Table [dbo].[Product]    Script Date: 11/6/2021 2:30:29 PM ******/
DROP TABLE [dbo].[Product]
GO
/****** Object:  Table [dbo].[Orders]    Script Date: 11/6/2021 2:30:29 PM ******/
DROP TABLE [dbo].[Orders]
GO
/****** Object:  Table [dbo].[OrderDetail]    Script Date: 11/6/2021 2:30:29 PM ******/
DROP TABLE [dbo].[OrderDetail]
GO
/****** Object:  Table [dbo].[Member]    Script Date: 11/6/2021 2:30:29 PM ******/
DROP TABLE [dbo].[Member]
GO
USE [master]
GO
/****** Object:  Database [SaleManagement]    Script Date: 11/6/2021 2:30:29 PM ******/
DROP DATABASE [SaleManagement]
GO
/****** Object:  Database [SaleManagement]    Script Date: 11/6/2021 2:30:29 PM ******/
CREATE DATABASE [SaleManagement]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'SaleManagement', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\SaleManagement.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'SaleManagement_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\SaleManagement_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [SaleManagement] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [SaleManagement].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [SaleManagement] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [SaleManagement] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [SaleManagement] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [SaleManagement] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [SaleManagement] SET ARITHABORT OFF 
GO
ALTER DATABASE [SaleManagement] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [SaleManagement] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [SaleManagement] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [SaleManagement] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [SaleManagement] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [SaleManagement] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [SaleManagement] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [SaleManagement] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [SaleManagement] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [SaleManagement] SET  ENABLE_BROKER 
GO
ALTER DATABASE [SaleManagement] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [SaleManagement] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [SaleManagement] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [SaleManagement] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [SaleManagement] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [SaleManagement] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [SaleManagement] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [SaleManagement] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [SaleManagement] SET  MULTI_USER 
GO
ALTER DATABASE [SaleManagement] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [SaleManagement] SET DB_CHAINING OFF 
GO
ALTER DATABASE [SaleManagement] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [SaleManagement] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [SaleManagement] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [SaleManagement] SET QUERY_STORE = OFF
GO
USE [SaleManagement]
GO
/****** Object:  Table [dbo].[Member]    Script Date: 11/6/2021 2:30:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Member](
	[MemberId] [int] NOT NULL,
	[Email] [varchar](100) NOT NULL,
	[CompanyName] [varchar](40) NOT NULL,
	[City] [varchar](15) NOT NULL,
	[Country] [varchar](15) NOT NULL,
	[Password] [varchar](30) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[MemberId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderDetail]    Script Date: 11/6/2021 2:30:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderDetail](
	[OrderId] [int] NOT NULL,
	[ProductId] [int] NOT NULL,
	[UnitPrice] [money] NOT NULL,
	[Quantity] [int] NOT NULL,
	[Discount] [float] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[OrderId] ASC,
	[ProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Orders]    Script Date: 11/6/2021 2:30:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Orders](
	[OrderId] [int] NOT NULL,
	[MemberId] [int] NOT NULL,
	[OrderDate] [datetime] NOT NULL,
	[RequiredDate] [datetime] NULL,
	[ShippedDate] [datetime] NULL,
	[Freight] [money] NULL,
PRIMARY KEY CLUSTERED 
(
	[OrderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Product]    Script Date: 11/6/2021 2:30:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Product](
	[ProductId] [int] NOT NULL,
	[CategoryId] [int] NOT NULL,
	[ProductName] [varchar](40) NOT NULL,
	[Weight] [varchar](20) NOT NULL,
	[UnitPrice] [money] NOT NULL,
	[UnitsInStock] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[Member] ([MemberId], [Email], [CompanyName], [City], [Country], [Password]) VALUES (2, N'phuong@gmail.com', N'KMS', N'Da Nang', N'Viet Nam', N'123')
INSERT [dbo].[Member] ([MemberId], [Email], [CompanyName], [City], [Country], [Password]) VALUES (3, N'hanh@gmail.com', N'Dien Luc Tan Thuan', N'Da Nang', N'USA', N'123')
INSERT [dbo].[Member] ([MemberId], [Email], [CompanyName], [City], [Country], [Password]) VALUES (4, N'binh@gmail.com', N'Thanh Da High School', N'Ha Noi', N'UK', N'123')
INSERT [dbo].[Member] ([MemberId], [Email], [CompanyName], [City], [Country], [Password]) VALUES (5, N'huy@gmail.com', N'FPTU', N'Ho Chi Minh', N'Viet Nam', N'456')
INSERT [dbo].[Member] ([MemberId], [Email], [CompanyName], [City], [Country], [Password]) VALUES (6, N'vinhnd@fpt.edu.vn', N'KMS', N'Ho Chi Minh', N'Viet Nam', N'456')
INSERT [dbo].[Member] ([MemberId], [Email], [CompanyName], [City], [Country], [Password]) VALUES (7, N'huong123@gmail.com', N'Viet Uc', N'Sydney', N'Australia', N'456')
INSERT [dbo].[Member] ([MemberId], [Email], [CompanyName], [City], [Country], [Password]) VALUES (8, N'hieu@gmail.com', N'Gia Dinh High School', N'Tay Nguyen', N'Viet Nam', N'123')
INSERT [dbo].[Member] ([MemberId], [Email], [CompanyName], [City], [Country], [Password]) VALUES (9, N'kim123@gmail.com', N'FPTU', N'Ho Chi Minh', N'Viet Nam', N'789')
INSERT [dbo].[Member] ([MemberId], [Email], [CompanyName], [City], [Country], [Password]) VALUES (10, N'hung@gmail.com', N'KMS', N'Da Nang', N'Viet Nam', N'789')
INSERT [dbo].[Member] ([MemberId], [Email], [CompanyName], [City], [Country], [Password]) VALUES (11, N'linhnv@fpt.edu.vn', N'HighLand Inc', N'Da Nang', N'USA', N'112')
INSERT [dbo].[Member] ([MemberId], [Email], [CompanyName], [City], [Country], [Password]) VALUES (12, N'mytdv@gmail.com', N'Hoang Hoa THam High School', N'Ha Noi', N'UK', N'112')
INSERT [dbo].[Member] ([MemberId], [Email], [CompanyName], [City], [Country], [Password]) VALUES (13, N'ky@fpt.edu.vn', N'FPTU', N'Ho Chi Minh', N'Japan', N'132')
INSERT [dbo].[Member] ([MemberId], [Email], [CompanyName], [City], [Country], [Password]) VALUES (14, N'henry@gmail.com', N'Gia Dinh High School', N'Ha Noi', N'Japan', N'132')
INSERT [dbo].[Member] ([MemberId], [Email], [CompanyName], [City], [Country], [Password]) VALUES (15, N'ducdh@netcompany.com', N'NetCompany', N'Ho Chi Minh', N'Viet Nam', N'132')
INSERT [dbo].[OrderDetail] ([OrderId], [ProductId], [UnitPrice], [Quantity], [Discount]) VALUES (1, 1, 125000.0000, 2, 5)
INSERT [dbo].[OrderDetail] ([OrderId], [ProductId], [UnitPrice], [Quantity], [Discount]) VALUES (1, 2, 90000.0000, 3, 10)
INSERT [dbo].[OrderDetail] ([OrderId], [ProductId], [UnitPrice], [Quantity], [Discount]) VALUES (2, 3, 80000.0000, 1, 10)
INSERT [dbo].[OrderDetail] ([OrderId], [ProductId], [UnitPrice], [Quantity], [Discount]) VALUES (2, 4, 100000.0000, 2, 5)
INSERT [dbo].[OrderDetail] ([OrderId], [ProductId], [UnitPrice], [Quantity], [Discount]) VALUES (3, 1, 125000.0000, 2, 0)
INSERT [dbo].[OrderDetail] ([OrderId], [ProductId], [UnitPrice], [Quantity], [Discount]) VALUES (3, 5, 160000.0000, 2, 0)
INSERT [dbo].[Orders] ([OrderId], [MemberId], [OrderDate], [RequiredDate], [ShippedDate], [Freight]) VALUES (1, 2, CAST(N'2021-10-20T00:00:00.000' AS DateTime), CAST(N'2021-10-22T00:00:00.000' AS DateTime), CAST(N'2021-10-21T00:00:00.000' AS DateTime), 10000.0000)
INSERT [dbo].[Orders] ([OrderId], [MemberId], [OrderDate], [RequiredDate], [ShippedDate], [Freight]) VALUES (2, 2, CAST(N'2021-09-20T00:00:00.000' AS DateTime), CAST(N'2021-09-22T00:00:00.000' AS DateTime), CAST(N'2021-09-21T00:00:00.000' AS DateTime), 20000.0000)
INSERT [dbo].[Orders] ([OrderId], [MemberId], [OrderDate], [RequiredDate], [ShippedDate], [Freight]) VALUES (3, 3, CAST(N'2021-11-01T16:37:00.000' AS DateTime), CAST(N'2021-11-08T16:37:00.000' AS DateTime), CAST(N'2021-11-07T16:37:00.000' AS DateTime), 15000.0000)
INSERT [dbo].[Product] ([ProductId], [CategoryId], [ProductName], [Weight], [UnitPrice], [UnitsInStock]) VALUES (1, 1, N'Pork', N'12', 125000.0000, 3)
INSERT [dbo].[Product] ([ProductId], [CategoryId], [ProductName], [Weight], [UnitPrice], [UnitsInStock]) VALUES (2, 1, N'Beef', N'1', 90000.0000, 6)
INSERT [dbo].[Product] ([ProductId], [CategoryId], [ProductName], [Weight], [UnitPrice], [UnitsInStock]) VALUES (3, 1, N'Chicken Meat', N'2', 80000.0000, 6)
INSERT [dbo].[Product] ([ProductId], [CategoryId], [ProductName], [Weight], [UnitPrice], [UnitsInStock]) VALUES (4, 2, N'Fish', N'5', 100000.0000, 5)
INSERT [dbo].[Product] ([ProductId], [CategoryId], [ProductName], [Weight], [UnitPrice], [UnitsInStock]) VALUES (5, 3, N'Goat Meat', N'7', 160000.0000, 0)
INSERT [dbo].[Product] ([ProductId], [CategoryId], [ProductName], [Weight], [UnitPrice], [UnitsInStock]) VALUES (6, 4, N'Egg', N'0.5', 125000.0000, 5)
INSERT [dbo].[Product] ([ProductId], [CategoryId], [ProductName], [Weight], [UnitPrice], [UnitsInStock]) VALUES (7, 5, N'Tiramisu', N'0.25', 30000.0000, 6)
INSERT [dbo].[Product] ([ProductId], [CategoryId], [ProductName], [Weight], [UnitPrice], [UnitsInStock]) VALUES (8, 5, N'Cheese Cake', N'0.25', 15000.0000, 7)
INSERT [dbo].[Product] ([ProductId], [CategoryId], [ProductName], [Weight], [UnitPrice], [UnitsInStock]) VALUES (9, 5, N'Strawberry Cake', N'0.3', 20000.0000, 10)
INSERT [dbo].[Product] ([ProductId], [CategoryId], [ProductName], [Weight], [UnitPrice], [UnitsInStock]) VALUES (10, 2, N'Tuna', N'1', 100000.0000, 11)
INSERT [dbo].[Product] ([ProductId], [CategoryId], [ProductName], [Weight], [UnitPrice], [UnitsInStock]) VALUES (11, 2, N'Silver Fish', N'1', 125000.0000, 6)
INSERT [dbo].[Product] ([ProductId], [CategoryId], [ProductName], [Weight], [UnitPrice], [UnitsInStock]) VALUES (12, 4, N'Milk', N'1.5', 90000.0000, 6)
INSERT [dbo].[Product] ([ProductId], [CategoryId], [ProductName], [Weight], [UnitPrice], [UnitsInStock]) VALUES (13, 2, N'Yogurt', N'2', 80000.0000, 7)
INSERT [dbo].[Product] ([ProductId], [CategoryId], [ProductName], [Weight], [UnitPrice], [UnitsInStock]) VALUES (14, 1, N'Frog Meat', N'5', 100000.0000, 5)
INSERT [dbo].[Product] ([ProductId], [CategoryId], [ProductName], [Weight], [UnitPrice], [UnitsInStock]) VALUES (15, 3, N'Sushi', N'7', 160000.0000, 15)
ALTER TABLE [dbo].[OrderDetail]  WITH CHECK ADD FOREIGN KEY([OrderId])
REFERENCES [dbo].[Orders] ([OrderId])
GO
ALTER TABLE [dbo].[OrderDetail]  WITH CHECK ADD FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([ProductId])
GO
USE [master]
GO
ALTER DATABASE [SaleManagement] SET  READ_WRITE 
GO

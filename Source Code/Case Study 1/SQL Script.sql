USE [master]
GO
/****** Object:  Database [MarketDB]    Script Date: 1/28/2018 3:18:46 PM ******/
CREATE DATABASE [MarketDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'MarketDB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\MarketDB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'MarketDB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\MarketDB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [MarketDB] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [MarketDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [MarketDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [MarketDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [MarketDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [MarketDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [MarketDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [MarketDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [MarketDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [MarketDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [MarketDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [MarketDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [MarketDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [MarketDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [MarketDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [MarketDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [MarketDB] SET  DISABLE_BROKER 
GO
ALTER DATABASE [MarketDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [MarketDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [MarketDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [MarketDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [MarketDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [MarketDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [MarketDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [MarketDB] SET RECOVERY FULL 
GO
ALTER DATABASE [MarketDB] SET  MULTI_USER 
GO
ALTER DATABASE [MarketDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [MarketDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [MarketDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [MarketDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [MarketDB] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'MarketDB', N'ON'
GO
ALTER DATABASE [MarketDB] SET QUERY_STORE = OFF
GO
USE [MarketDB]
GO
ALTER DATABASE SCOPED CONFIGURATION SET IDENTITY_CACHE = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET LEGACY_CARDINALITY_ESTIMATION = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET MAXDOP = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET PARAMETER_SNIFFING = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET QUERY_OPTIMIZER_HOTFIXES = PRIMARY;
GO
USE [MarketDB]
GO
/****** Object:  Table [dbo].[Orders]    Script Date: 1/28/2018 3:18:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Orders](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[userId] [int] NOT NULL,
	[payMethodId] [int] NOT NULL,
	[sumMoney] [money] NOT NULL,
	[note] [nvarchar](500) NULL,
	[cardHolderName] [nvarchar](100) NOT NULL,
	[cardNumber] [int] NOT NULL,
	[expirationDate] [nvarchar](5) NOT NULL,
	[name] [nvarchar](100) NOT NULL,
	[adressLine1] [nvarchar](100) NOT NULL,
	[adressLine2] [nvarchar](100) NULL,
	[zipCode] [int] NOT NULL,
	[phoneNumber] [int] NOT NULL,
 CONSTRAINT [PK_Orders] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Orders_Has_Products]    Script Date: 1/28/2018 3:18:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Orders_Has_Products](
	[orderId] [int] NOT NULL,
	[productId] [int] NOT NULL,
	[quantity] [int] NOT NULL,
	[pricePerUnit] [money] NOT NULL,
 CONSTRAINT [PK_Orders_Has_Products] PRIMARY KEY CLUSTERED 
(
	[orderId] ASC,
	[productId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PayMethods]    Script Date: 1/28/2018 3:18:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PayMethods](
	[id] [int] NOT NULL,
	[name] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_PayMethods] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Products]    Script Date: 1/28/2018 3:18:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Products](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](100) NOT NULL,
	[description] [nvarchar](max) NOT NULL,
	[shortDescription] [nvarchar](100) NULL,
	[price] [money] NOT NULL,
	[isActive] [bit] NOT NULL,
	[imageURL] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Products] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 1/28/2018 3:18:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[id] [int] NOT NULL,
	[name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 1/28/2018 3:18:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[roleId] [int] NOT NULL,
	[username] [nvarchar](50) NOT NULL,
	[password] [nchar](16) NOT NULL,
	[isActive] [bit] NOT NULL,
 CONSTRAINT [PK_Table_1] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[PayMethods] ([id], [name]) VALUES (1, N'Credit Card')
INSERT [dbo].[PayMethods] ([id], [name]) VALUES (2, N'PayPal')
SET IDENTITY_INSERT [dbo].[Products] ON 

INSERT [dbo].[Products] ([id], [name], [description], [shortDescription], [price], [isActive], [imageURL]) VALUES (1, N'Laptop MSI GT72VR 6RD 231VN Dominator Tobii', N'<p>sdfsdfsdf</p>', N'This is a laptop', 1.2500, 1, N'https://www.anphatpc.com.vn/media/product/20849_laptop_msi_gt72vr_6rd.png')
SET IDENTITY_INSERT [dbo].[Products] OFF
INSERT [dbo].[Roles] ([id], [name]) VALUES (1, N'User')
INSERT [dbo].[Roles] ([id], [name]) VALUES (2, N'Admin')
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([id], [roleId], [username], [password], [isActive]) VALUES (7, 1, N'sdfsdf', N'sdfsdf          ', 1)
INSERT [dbo].[Users] ([id], [roleId], [username], [password], [isActive]) VALUES (10, 1, N'asdasd', N'123             ', 1)
INSERT [dbo].[Users] ([id], [roleId], [username], [password], [isActive]) VALUES (11, 1, N'vu', N'123             ', 1)
INSERT [dbo].[Users] ([id], [roleId], [username], [password], [isActive]) VALUES (1002, 2, N'batman', N'123             ', 1)
SET IDENTITY_INSERT [dbo].[Users] OFF
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_Orders_PayMethods] FOREIGN KEY([payMethodId])
REFERENCES [dbo].[PayMethods] ([id])
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_PayMethods]
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_Orders_Users] FOREIGN KEY([userId])
REFERENCES [dbo].[Users] ([id])
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_Users]
GO
ALTER TABLE [dbo].[Orders_Has_Products]  WITH CHECK ADD  CONSTRAINT [FK_Orders_Has_Products_Orders1] FOREIGN KEY([orderId])
REFERENCES [dbo].[Orders] ([id])
GO
ALTER TABLE [dbo].[Orders_Has_Products] CHECK CONSTRAINT [FK_Orders_Has_Products_Orders1]
GO
ALTER TABLE [dbo].[Orders_Has_Products]  WITH CHECK ADD  CONSTRAINT [FK_Orders_Has_Products_Products1] FOREIGN KEY([productId])
REFERENCES [dbo].[Products] ([id])
GO
ALTER TABLE [dbo].[Orders_Has_Products] CHECK CONSTRAINT [FK_Orders_Has_Products_Products1]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Roles] FOREIGN KEY([roleId])
REFERENCES [dbo].[Roles] ([id])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Roles]
GO
USE [master]
GO
ALTER DATABASE [MarketDB] SET  READ_WRITE 
GO

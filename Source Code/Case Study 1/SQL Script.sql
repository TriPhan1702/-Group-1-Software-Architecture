USE [master]
GO
/****** Object:  Database [MarketDB]    Script Date: 1/30/2018 7:33:02 PM ******/
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
/****** Object:  Table [dbo].[Orders]    Script Date: 1/30/2018 7:33:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Orders](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[userId] [int] NOT NULL,
	[paymentTypeId] [int] NOT NULL,
	[sumMoney] [money] NOT NULL,
 CONSTRAINT [PK_Orders] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Orders_Has_Products]    Script Date: 1/30/2018 7:33:02 PM ******/
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
/****** Object:  Table [dbo].[PaymentTypes]    Script Date: 1/30/2018 7:33:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PaymentTypes](
	[id] [int] NOT NULL,
	[name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_PaymentTypes] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Products]    Script Date: 1/30/2018 7:33:02 PM ******/
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
/****** Object:  Table [dbo].[Roles]    Script Date: 1/30/2018 7:33:02 PM ******/
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
/****** Object:  Table [dbo].[Users]    Script Date: 1/30/2018 7:33:02 PM ******/
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
SET IDENTITY_INSERT [dbo].[Orders] ON 

INSERT [dbo].[Orders] ([id], [userId], [paymentTypeId], [sumMoney]) VALUES (11, 11, 1, 1.2500)
INSERT [dbo].[Orders] ([id], [userId], [paymentTypeId], [sumMoney]) VALUES (12, 11, 2, 2.5000)
INSERT [dbo].[Orders] ([id], [userId], [paymentTypeId], [sumMoney]) VALUES (13, 11, 2, 1.2500)
INSERT [dbo].[Orders] ([id], [userId], [paymentTypeId], [sumMoney]) VALUES (14, 11, 2, 1.2500)
INSERT [dbo].[Orders] ([id], [userId], [paymentTypeId], [sumMoney]) VALUES (15, 11, 2, 1001.2500)
INSERT [dbo].[Orders] ([id], [userId], [paymentTypeId], [sumMoney]) VALUES (16, 11, 2, 1201.2500)
INSERT [dbo].[Orders] ([id], [userId], [paymentTypeId], [sumMoney]) VALUES (17, 11, 2, 1.2500)
INSERT [dbo].[Orders] ([id], [userId], [paymentTypeId], [sumMoney]) VALUES (18, 11, 2, 1.2500)
SET IDENTITY_INSERT [dbo].[Orders] OFF
INSERT [dbo].[Orders_Has_Products] ([orderId], [productId], [quantity], [pricePerUnit]) VALUES (11, 1, 1, 1.2500)
INSERT [dbo].[Orders_Has_Products] ([orderId], [productId], [quantity], [pricePerUnit]) VALUES (12, 1, 2, 1.2500)
INSERT [dbo].[Orders_Has_Products] ([orderId], [productId], [quantity], [pricePerUnit]) VALUES (13, 1, 1, 1.2500)
INSERT [dbo].[Orders_Has_Products] ([orderId], [productId], [quantity], [pricePerUnit]) VALUES (14, 1, 1, 1.2500)
INSERT [dbo].[Orders_Has_Products] ([orderId], [productId], [quantity], [pricePerUnit]) VALUES (15, 1, 1, 1.2500)
INSERT [dbo].[Orders_Has_Products] ([orderId], [productId], [quantity], [pricePerUnit]) VALUES (15, 2, 1, 1000.0000)
INSERT [dbo].[Orders_Has_Products] ([orderId], [productId], [quantity], [pricePerUnit]) VALUES (16, 1, 1, 1.2500)
INSERT [dbo].[Orders_Has_Products] ([orderId], [productId], [quantity], [pricePerUnit]) VALUES (16, 2, 1, 1000.0000)
INSERT [dbo].[Orders_Has_Products] ([orderId], [productId], [quantity], [pricePerUnit]) VALUES (16, 1002, 1, 200.0000)
INSERT [dbo].[Orders_Has_Products] ([orderId], [productId], [quantity], [pricePerUnit]) VALUES (17, 1, 1, 1.2500)
INSERT [dbo].[Orders_Has_Products] ([orderId], [productId], [quantity], [pricePerUnit]) VALUES (18, 1, 1, 1.2500)
INSERT [dbo].[PaymentTypes] ([id], [name]) VALUES (1, N'Paypal')
INSERT [dbo].[PaymentTypes] ([id], [name]) VALUES (2, N'Credit Card')
SET IDENTITY_INSERT [dbo].[Products] ON 

INSERT [dbo].[Products] ([id], [name], [description], [shortDescription], [price], [isActive], [imageURL]) VALUES (1, N'Laptop MSI GT72VR 6RD 231VN Dominator Tobii', N'<p>sdfsdfsdf</p>', N'This is a laptop', 1.2500, 1, N'https://www.anphatpc.com.vn/media/product/20849_laptop_msi_gt72vr_6rd.png')
INSERT [dbo].[Products] ([id], [name], [description], [shortDescription], [price], [isActive], [imageURL]) VALUES (2, N'Card Màn Hình MSI GEFORCE® GTX 1080 TI GAMING X 11G', N'<p style="text-align: center;"><img src="https://xgear.vn/wp-content/uploads/2017/06/1080Ti_lookandfeel01.jpg" width="600" height="338" /><br />GTX 1080 Ti với tấm nền kim loại nguy&ecirc;n khối, cho vẻ ho&agrave;n thiện tuyệt vời v&agrave; si&ecirc;u vững chắc.</p>
<p>NỀN TẢNG TỐT NHẤT CHO NHỮNG BỘ M&Aacute;Y GAMING<br />H&atilde;y lu&ocirc;n sẵn s&agrave;ng cho mọi game với GeForce&reg; GTX.</p>
<p>GeForce GTX 1080 Ti l&agrave; chiếc Card m&agrave;n h&igrave;nh Gaming tốt nhất từng được sản xuất. Kh&aacute;m ph&aacute; những hiệu năng đ&aacute;ng kinh ngạc, ti&ecirc;u thụ điện năng hợp l&yacute;, v&agrave; trải nghiệm ho&agrave;n hảo những game đời mới nhất.</p>
<p>Kh&aacute;m ph&aacute; c&ocirc;ng nghệ thực tế ảo mới nhất, giảm thiểu tối đa c&aacute;c ứng dụng chạy ngầm, v&agrave; khả năng kết nối &ndash; chơi ngay bằng c&aacute;c bộ k&iacute;nh thực tế ảo h&agrave;ng đầu với c&ocirc;ng nghệ NVIDIA VRWorks&trade;. C&ocirc;ng nghệ thực tế ảo &acirc;m thanh, h&igrave;nh ảnh, v&agrave; x&uacute;c gi&aacute;c gi&uacute;p bạn nghe v&agrave; cảm nhận từng khoảnh khắc tuyệt vời nhất.</p>
<p>GTX 1080 Ti được thiết kế để đ&aacute;p ứng những nhu cầu hiện đại, bao gồm thực tế ảo, độ ph&acirc;n giải si&ecirc;u cao, v&agrave; đa m&agrave;n h&igrave;nh. Chiếc card m&agrave;n h&igrave;nh được trang bị c&ocirc;ng nghệ VIDIA GameWorks&trade; mang lại trải nghiệm chơi game đẹp v&agrave; mượt m&agrave; nhất. Hơn nữa, 1080 Ti cũng được trang bị c&ocirc;ng nghệ cho cuộc c&aacute;ch mạng mới &ndash; h&igrave;nh ảnh 360 độ.</p>
<p>Card m&agrave;n h&igrave;nh c&ocirc;ng nghệ Pascal mang đến cho bạn hiệu năng đ&aacute;ng kinh ngạc v&agrave; ti&ecirc;u thụ điện năng hợp l&yacute;, sử dụng c&ocirc;ng nghệ b&aacute;n dẫn 3D &ndash; FinFET si&ecirc;u nhanh v&agrave; hỗ trợ nền tảng DirectX&trade; 12 để mang đến trải nghiệm gaming nhanh, mượt, v&agrave; hiệu quả điện năng.</p>
<p>&nbsp;</p>
<p>L&agrave;m việc nh&oacute;m mang lại sức mạnh</p>
<p>&nbsp;</p>
<p>Cũng giống như trong game, c&ocirc;ng nghệ quạt tản nhiệt TORX 2.0 độc quyền của MSI tận dụng sức mạnh từ việc kết hợp c&aacute;c thiết bị nhỏ nhất, l&agrave;m cho TWIN FROZR VI đạt được cột mốc mới về hiệu năng tản nhiệt.</p>
<p>TORX 2.0 tạo được th&ecirc;m 22% &aacute;p lực kh&ocirc;ng kh&iacute;, gi&uacute;p mang lại sự y&ecirc;n lặng trong l&uacute;c l&agrave;m dịu sức n&oacute;ng của trận đấu.</p>
<p><br />Lưỡi quạt ph&acirc;n t&aacute;n<br />c&oacute; một r&atilde;nh s&acirc;u tr&ecirc;n c&aacute;nh quạt , c&oacute; thể gia tốc d&ograve;ng kh&ocirc;ng kh&iacute;, l&agrave;m tăng hiệu quả tản nhiệt</p>
<p>Lưỡi quạt truyền thống<br />Đẩy luồng kh&ocirc;ng kh&iacute; đi xuống li&ecirc;n tục, nơi c&oacute; bộ tản nhiệt lớn ph&iacute;a dưới.</p>
<p>&nbsp;</p>
<p>V&ograve;ng bi hai d&atilde;y gi&uacute;p quạt tản nhiệt MSI TORX 2.0 c&oacute; một bộ l&otilde;i mạnh mẽ v&agrave; bền bỉ qua năm th&aacute;ng, mang lại trải nghiệm mượt m&agrave; cho game thủ. Ch&uacute;ng cũng gi&uacute;p quạt giữ được sự y&ecirc;n lặng kể cả khi phải l&agrave;m việc qu&aacute; sức, duy tr&igrave; nhiệt độ kh&ocirc;ng qu&aacute; cao cho GTX 1080 Ti, trong những trận đấu kh&oacute; khăn v&agrave; dai dẳng.</p>
<p>Được giới thiệu lần đầu ti&ecirc;n v&agrave;o năm 2008 bởi MSI, c&ocirc;ng nghệ ZeroFrozr đ&atilde; g&acirc;y ấn tượng mạnh mẽ, v&agrave; hiện giờ đ&atilde; trở th&agrave;nh ti&ecirc;u chuẩn cho ng&agrave;nh c&ocirc;ng nghiệp card m&agrave;n h&igrave;nh. N&oacute; loại bỏ ho&agrave;n to&agrave;n tiếng ồn của quạt, bằng c&aacute;ch tắt quạt trong những điều kiện kh&ocirc;ng cần thiết. C&oacute; nghĩa l&agrave; bạn c&oacute; thể tập trung ho&agrave;n to&agrave;n v&agrave;o trận đấu, khi kh&ocirc;ng bị ph&acirc;n t&acirc;m bởi tiếng ồn từ quạt.</p>
<p><br />QUẠT QUAY</p>
<p>Trong những game c&oacute; cường độ lớn, hoặc khi đến chu kỳ</p>
<p>QUẠT DỪNG</p>
<p>Tuyệt đối y&ecirc;n lặng, l&uacute;c nghe nhạc, xem phim hay chơi game nhẹ</p>
<p>&nbsp;</p>
<p>Nếu kh&ocirc;ng nghe tiếng quạt chạy, nghĩa l&agrave; đến l&uacute;c bạn cần phải tăng nhịp độ trận đấu l&ecirc;n</p>
<p>Mỗi khu đ&egrave;n Led c&oacute; thể được điều chỉnh độc lập bằng c&aacute;ch chọn bất kỳ một hiệu ứng &aacute;nh s&aacute;ng c&oacute; sẵn n&agrave;o trong MSI Gaming App, từ chế độ s&aacute;ng theo &acirc;m thanh trong game, hay b&agrave;i nhạc đến chế độ s&aacute;ng li&ecirc;n tục, nhấp nh&aacute;y hay từng đợt. Tất nhi&ecirc;n, bạn cũng c&oacute; thể tắt đ&egrave;n đi.</p>
<p>&nbsp;</p>
<p>&nbsp;</p>
<p>Ẩn dưới bộ c&aacute;nh Gaming của GTX 1080 Ti l&agrave; một kiệt t&aacute;c về kỹ thuật được thiết kế để giữ cho cho chiếc Card 1080 Ti lu&ocirc;n được m&aacute;t mẻ. Mỗi chi tiết nhỏ nhất của miếng tản nhiệt phức tạp đều đ&oacute;ng một vai tr&ograve; cần thiết để mang đến một cảm gi&aacute;c Gaming m&aacute;t mẻ v&agrave; y&ecirc;n lặng.</p>
<p>&nbsp;</p>
<p>C&Ocirc;NG NGHỆ KIỂM SO&Aacute;T D&Ograve;NG KH&Iacute;</p>
<p>Thiết kế kh&iacute; động học ti&ecirc;n tiến, c&ocirc;ng nghệ kiểm so&aacute;t d&ograve;ng kh&iacute; mang nhiều kh&ocirc;ng kh&iacute; trực tiếp đến c&aacute;c ống nhiệt của TWIN FROZR VI. C&aacute;c r&atilde;nh tr&ecirc;n bề mặt gi&uacute;p tăng diện t&iacute;ch tiếp x&uacute;c, nghĩa l&agrave; y&ecirc;n lặng hơn v&agrave; chất lượng game tốt hơn.</p>
<p>&nbsp;</p>
<p>&nbsp;</p>
<p>Một chiếc card m&agrave;n h&igrave;nh mạnh mẽ, cần một bộ c&aacute;nh ki&ecirc;u h&atilde;nh đi c&ugrave;ng. GeForce GTX 1080 Ti GAMING X được bao quanh bởi một tấm nền kim loại đen cao cấp, mang đến một cảm gi&aacute;c mạnh mẽ. Lớp sơn phủ đen mờ ph&iacute;a b&ecirc;n ho&agrave;n l&agrave;m ho&agrave;n thiện thiết kế của TWIN FROZR VI.</p>
<p>&nbsp;</p>
<p>GTX 1080 Ti mang đến trải nghiệm game mượt m&agrave;, nhanh, v&agrave; th&uacute; vị hơn bao giờ hết, bằng c&ocirc;ng nghệ khử r&aacute;ch h&igrave;nh, khử chập chờn, khử lag.</p>
<p>&nbsp;</p>
<p>Hiển thị ph&acirc;n giả cao 4K, mang đến cho bạn chất lượng h&igrave;nh ảnh sắc n&eacute;t, mạnh mẽ v&agrave; ch&iacute;nh x&aacute;c đến từng chi tiết nhỏ.</p>
<p>Để trải nghiệm c&ocirc;ng nghệ thực tế ảo, bạn cần chiếc m&aacute;y t&iacute;nh của m&igrave;nh trong trạng th&aacute;i tốt nhất. Phần mềm MSI Gaming App sẽ l&agrave;m chiếc m&aacute;y t&iacute;nh của bạn sẵn s&agrave;ng cho game thực tế ảo chỉ bằng một c&uacute; nhấn chuột, giải ph&oacute;ng tất cả tiềm năng phần cứng của m&aacute;y t&iacute;nh bạn, v&agrave; kh&oacute;a c&aacute;c ứng dụng kh&aacute;c, đảm bảo ch&uacute;ng kh&ocirc;ng ảnh hưởng đến trải nghiệm của bạn. V&agrave; chiếc card GTX 1080 Ti được thiết kế cho trải nghiệm thực tế ảo tốt nhất.</p>
<p>&nbsp;</p>
<p>V&agrave; c&ograve;n hỗ trợ rất nhiều chức năng tối t&acirc;n:</p>
<p>Hỗ trợ chuyển game 2D th&agrave;nh 3D bằng TriDef VR<br />Tối ưu phần cứng v&agrave; phần mềm cho Gaming qua MSI Gaming App<br />Hiển thị t&igrave;nh trạng v&agrave; nhiệt độ GPU, RAM, CPU tr&ecirc;n m&agrave;n h&igrave;nh<br />Chế độ đa m&agrave;n h&igrave;nh, hay vừa chơi game vừa xem video tr&ecirc;n c&ugrave;ng một m&agrave;n h&igrave;nh bằng MSI Dragon Eye.</p>', N'Card Màn Hình MSI GEFORCE® GTX 1080 TI GAMING X 11G', 1000.0000, 1, N'https://xgear.vn/wp-content/uploads/2017/06/GTX-1080-Ti-sp-428x428.jpg')
INSERT [dbo].[Products] ([id], [name], [description], [shortDescription], [price], [isActive], [imageURL]) VALUES (1002, N'Tai Nghe MSI Gaming Headset DS502', N'<p>TAI NGHE CHẤT LƯỢNG CAO<br />&ndash; Tai Nghe MSI Gaming Headset DS502 được thiết kế với 2 củ tai nghe với Driver 40mm chất lượng cao.<br />&ndash; Củ loa chất lượng cao đem đến &acirc;m trường ở c&aacute;c tần số cao, trung v&agrave; thấp với chất lượng rất tốt.</p>
<p>THIẾT KẾ TH&Ocirc;NG MINH<br />Trọng lượng nhẹ, headband t&ugrave;y chỉnh dễ d&agrave;ng<br />Tai Nghe MSI Gaming Headset DS502 thiết kế th&acirc;n thiện, gi&uacute;p bạn thoải m&aacute;i suốt thời gian d&agrave;i sử dụng. Headband của tai nghe điều chỉnh &ocirc;m theo v&ograve;ng đầu của bạn v&agrave; ear-cup chuẩn &ocirc;m k&iacute;n tai đảm bảo &acirc;m thanh kh&ocirc;ng hề bị mất đi.</p>
<p><br /> <br />HỆ THỐNG RUNG ĐỘC Đ&Aacute;O<br />&ndash; Hệ thống rung độc đ&aacute;o của Tai Nghe MSI Gaming Headset DS502 n&acirc;ng cao trải nghiệm &acirc;m thanh của bạn.</p>
<p><br /> <br />MICRO ĐIỀU HƯỚNG <br />&ndash; Micro của Tai Nghe MSI Gaming Headset DS502 điều chỉnh hướng dễ d&agrave;ng.<br />&ndash; Micro cao cấp của Tai Nghe MSI Gaming Headset DS502 cho &acirc;m thanh trong trẻo, khả năng lọc tiếng ồn si&ecirc;u việt.</p>
<p><br />ĐIỀU KHIỂN TR&Ecirc;N D&Acirc;Y<br />&ndash; Điều khiển tr&ecirc;n d&acirc;y gi&uacute;p việc tăng giảm &acirc;m lượng, ngắt mic, chỉnh rung trở n&ecirc;n dễ d&agrave;ng hơn bao giờ hết</p>
<p><br /> <br />&Acirc;M THANH V&Ograve;M 7.1<br />&ndash; T&iacute;ch hợp c&ocirc;ng nghệ Xear&trade; Living, Tai Nghe MSI Gaming Headset DS502 mang đến cho bạn một trải nghiệm ho&agrave;n to&agrave;n mới với game cũng như phim ảnh.</p>
<p>C&ocirc;ng nghệ Xear&trade; Living cho ph&eacute;p tai nghe của bạn thể hiện ở một tầm cao mới.</p>', N'Tai Nghe MSI Gaming Headset DS502', 200.0000, 1, N'https://xgear.vn/wp-content/uploads/2017/07/Tai-Nghe-MSI-Gaming-Headset-DS502-4-428x428.jpg')
INSERT [dbo].[Products] ([id], [name], [description], [shortDescription], [price], [isActive], [imageURL]) VALUES (1003, N'asdasdasda', N'<p>sdfsdfsdf</p>', N'asdasdasda', 234324.0000, 0, N'dsfdsf')
INSERT [dbo].[Products] ([id], [name], [description], [shortDescription], [price], [isActive], [imageURL]) VALUES (1004, N'sdfsfsdf', N'<p>asdasd</p>', N'sdfsfsdf', 2.0000, 0, N'asdasd')
SET IDENTITY_INSERT [dbo].[Products] OFF
INSERT [dbo].[Roles] ([id], [name]) VALUES (1, N'User')
INSERT [dbo].[Roles] ([id], [name]) VALUES (2, N'Admin')
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([id], [roleId], [username], [password], [isActive]) VALUES (7, 1, N'sdfsdf', N'sdfsdf          ', 1)
INSERT [dbo].[Users] ([id], [roleId], [username], [password], [isActive]) VALUES (10, 1, N'asdasd', N'123             ', 1)
INSERT [dbo].[Users] ([id], [roleId], [username], [password], [isActive]) VALUES (11, 1, N'vu', N'123             ', 1)
INSERT [dbo].[Users] ([id], [roleId], [username], [password], [isActive]) VALUES (1002, 2, N'batman', N'123             ', 1)
INSERT [dbo].[Users] ([id], [roleId], [username], [password], [isActive]) VALUES (1003, 1, N'batman1', N'067854774       ', 1)
INSERT [dbo].[Users] ([id], [roleId], [username], [password], [isActive]) VALUES (2003, 1, N'canhvedepzai', N'12345678        ', 1)
SET IDENTITY_INSERT [dbo].[Users] OFF
SET ANSI_PADDING ON
GO
/****** Object:  Index [Users_Username_Unique]    Script Date: 1/30/2018 7:33:02 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [Users_Username_Unique] ON [dbo].[Users]
(
	[username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_Orders_PaymentTypes] FOREIGN KEY([paymentTypeId])
REFERENCES [dbo].[PaymentTypes] ([id])
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_PaymentTypes]
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

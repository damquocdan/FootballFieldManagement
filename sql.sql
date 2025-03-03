USE [FootballFieldManagement]
GO
/****** Object:  Table [dbo].[Admin]    Script Date: 3/3/2025 9:24:24 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Admin](
	[admin_id] [int] IDENTITY(1,1) NOT NULL,
	[username] [nvarchar](255) NOT NULL,
	[email] [nvarchar](255) NOT NULL,
	[password] [nvarchar](255) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[admin_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Booking_Services]    Script Date: 3/3/2025 9:24:24 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Booking_Services](
	[booking_service_id] [int] IDENTITY(1,1) NOT NULL,
	[booking_id] [int] NULL,
	[service_id] [int] NULL,
	[quantity] [int] NOT NULL,
	[total_price] [decimal](10, 2) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[booking_service_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Bookings]    Script Date: 3/3/2025 9:24:24 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Bookings](
	[booking_id] [int] IDENTITY(1,1) NOT NULL,
	[customer_id] [int] NULL,
	[field_id] [int] NULL,
	[booking_time] [datetime] NOT NULL,
	[duration] [int] NULL,
	[total_price] [decimal](10, 2) NULL,
	[total_service_price] [decimal](10, 2) NULL,
	[total_amount] [decimal](10, 2) NULL,
PRIMARY KEY CLUSTERED 
(
	[booking_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Customer]    Script Date: 3/3/2025 9:24:24 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customer](
	[customer_id] [int] IDENTITY(1,1) NOT NULL,
	[username] [nvarchar](255) NOT NULL,
	[email] [nvarchar](255) NOT NULL,
	[password] [nvarchar](255) NOT NULL,
	[phone] [nvarchar](15) NOT NULL,
	[address] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[customer_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Fields]    Script Date: 3/3/2025 9:24:24 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Fields](
	[field_id] [int] IDENTITY(1,1) NOT NULL,
	[field_name] [nvarchar](255) NOT NULL,
	[capacity] [int] NOT NULL,
	[price] [decimal](10, 2) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[field_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Payments]    Script Date: 3/3/2025 9:24:24 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Payments](
	[payment_id] [int] IDENTITY(1,1) NOT NULL,
	[booking_id] [int] NOT NULL,
	[payment_method] [nvarchar](255) NOT NULL,
	[amount] [decimal](10, 2) NOT NULL,
	[payment_time] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[payment_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Services]    Script Date: 3/3/2025 9:24:24 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Services](
	[service_id] [int] IDENTITY(1,1) NOT NULL,
	[service_name] [nvarchar](255) NOT NULL,
	[description] [nvarchar](max) NULL,
	[price] [decimal](10, 2) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[service_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Booking_Services] ADD  DEFAULT ((1)) FOR [quantity]
GO
ALTER TABLE [dbo].[Booking_Services]  WITH CHECK ADD FOREIGN KEY([booking_id])
REFERENCES [dbo].[Bookings] ([booking_id])
GO
ALTER TABLE [dbo].[Booking_Services]  WITH CHECK ADD FOREIGN KEY([service_id])
REFERENCES [dbo].[Services] ([service_id])
GO
ALTER TABLE [dbo].[Bookings]  WITH CHECK ADD FOREIGN KEY([customer_id])
REFERENCES [dbo].[Customer] ([customer_id])
GO
ALTER TABLE [dbo].[Bookings]  WITH CHECK ADD FOREIGN KEY([field_id])
REFERENCES [dbo].[Fields] ([field_id])
GO
ALTER TABLE [dbo].[Payments]  WITH CHECK ADD FOREIGN KEY([booking_id])
REFERENCES [dbo].[Bookings] ([booking_id])
GO

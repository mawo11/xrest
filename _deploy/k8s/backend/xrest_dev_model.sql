USE [master]
GO
CREATE LOGIN xrest_dev WITH PASSWORD = 'xrest_dev!@#$12314';
GO

CREATE DATABASE [xrest_dev]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'xrest_main_data', FILENAME = N'/var/opt/mssql/data/xrest_dev.mdf' , SIZE = 96640KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'xrest_main_log', FILENAME = N'/var/opt/mssql/data/xrest_dev.ldf' , SIZE = 102144KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [xrest_dev] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [xrest_dev].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [xrest_dev] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [xrest_dev] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [xrest_dev] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [xrest_dev] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [xrest_dev] SET ARITHABORT OFF 
GO
ALTER DATABASE [xrest_dev] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [xrest_dev] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [xrest_dev] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [xrest_dev] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [xrest_dev] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [xrest_dev] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [xrest_dev] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [xrest_dev] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [xrest_dev] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [xrest_dev] SET  DISABLE_BROKER 
GO
ALTER DATABASE [xrest_dev] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [xrest_dev] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [xrest_dev] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [xrest_dev] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [xrest_dev] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [xrest_dev] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [xrest_dev] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [xrest_dev] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [xrest_dev] SET  MULTI_USER 
GO
ALTER DATABASE [xrest_dev] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [xrest_dev] SET DB_CHAINING OFF 
GO
ALTER DATABASE [xrest_dev] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [xrest_dev] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [xrest_dev] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [xrest_dev] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'xrest_dev', N'ON'
GO
ALTER DATABASE [xrest_dev] SET QUERY_STORE = OFF
GO
USE [xrest_dev]
GO

CREATE USER [xrest_dev] FOR LOGIN [xrest_dev] WITH DEFAULT_SCHEMA=[dbo]
GO

ALTER ROLE [db_owner] ADD MEMBER [xrest_dev]
GO

CREATE SCHEMA [ident]
GO

CREATE SCHEMA [loc]
GO

CREATE SCHEMA [ord]
GO

CREATE SCHEMA [rest]
GO

CREATE SCHEMA [shared]
GO

CREATE TYPE [dbo].[inttable] AS TABLE(
	[id] [int] NOT NULL
)
GO

CREATE TYPE [ord].[NewOnlineOrderHeader] AS TABLE(
	[customer_id] [int] NULL,
	[restaurant_id] [int] NOT NULL,
	[create_date] [datetime] NOT NULL,
	[address_city] [nvarchar](100) NOT NULL,
	[address_street] [nvarchar](100) NULL,
	[address_street_number] [nvarchar](100) NULL,
	[address_house_number] [nvarchar](100) NULL,
	[worker_id] [int] NULL,
	[amount] [decimal](19, 2) NOT NULL,
	[transport_amount] [decimal](19, 2) NOT NULL,
	[email] [nvarchar](256) NULL,
	[phone] [nvarchar](20) NULL,
	[note] [nvarchar](1024) NULL,
	[status_id] [tinyint] NOT NULL,
	[payment_type_id] [tinyint] NOT NULL,
	[term_type_id] [tinyint] NOT NULL,
	[term] [time](7) NOT NULL,
	[type_of_delivery_id] [tinyint] NULL,
	[modify_date] [datetime] NOT NULL,
	[restaurant_transport_id] [int] NULL,
	[terms_of_use] [bit] NOT NULL,
	[realization] [bit] NOT NULL,
	[marketing] [bit] NOT NULL,
	[audit_date] [datetime] NOT NULL,
	[order_day] [int] NOT NULL,
	[firstname] [nvarchar](500) NULL,
	[discount] [decimal](9, 2) NULL,
	[source] [tinyint] NOT NULL
)
GO
CREATE TYPE [ord].[NewOnlineOrderRow] AS TABLE(
	[product_id] [int] NOT NULL,
	[price] [decimal](19, 2) NOT NULL,
	[points] [smallint] NOT NULL,
	[type_id] [tinyint] NOT NULL,
	[order_product_id] [int] NULL,
	[display_name] [nvarchar](500) NOT NULL,
	[from_source_id] [tinyint] NULL,
	[index] [smallint] NOT NULL,
	[note] [nvarchar](max) NULL,
	[sub_index] [smallint] NULL,
	[base_price] [decimal](19, 2) NOT NULL,
	[vat] [int] NULL
)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[GetValueByLang] 
(
	@doc xml,
	@tag nvarchar(max)
)
RETURNS nvarchar(max)
AS
BEGIN
	DECLARE @result nvarchar(max)
	SELECT @result = @doc.value('(/langs/*[local-name()=sql:variable("@tag")])[1]', 'varchar(max)' )  
    RETURN @result
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[xrest_account](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[parent_account_id] [int] NULL,
	[email] [nvarchar](128) NOT NULL,
	[password] [nvarchar](256) NOT NULL,
	[created_date] [datetime2](7) NULL,
	[deleted] [bit] NOT NULL,
	[locked] [bit] NOT NULL,
	[locked_reason] [nvarchar](1024) NULL,
	[ip_address] [nvarchar](128) NULL,
	[phone] [nvarchar](100) NULL,
	[display_name] [nvarchar](500) NULL,
	[data] [nvarchar](1024) NULL,
	[account_type] [tinyint] NOT NULL,
	[points] [int] NOT NULL,
	[audit_date] [datetime2](7) NULL,
	[audit_user] [int] NULL,
	[must_change_password] [bit] NOT NULL,
	[code] [nvarchar](200) NULL,
	[max_orders] [tinyint] NOT NULL,
	[firstname] [nvarchar](500) NULL,
	[lastname] [nvarchar](500) NULL,
	[status] [tinyint] NOT NULL,
	[terms_accepted] [bit] NOT NULL,
	[marketing] [bit] NOT NULL,
	[birth_date] [datetime2](7) NULL,
	[card_code] [varchar](200) NULL,
 CONSTRAINT [PK__xrest_acc__3213E83F926D02FB] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[xrest_account_acceptance](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[account_id] [int] NOT NULL,
	[marketing_id] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[xrest_account_external](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[account_id] [int] NOT NULL,
	[provider] [tinyint] NOT NULL,
	[external_id] [nvarchar](500) NOT NULL,
	[created] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[xrest_account_history](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[account_id] [int] NOT NULL,
	[operation] [char](5) NOT NULL,
	[activity] [datetime] NOT NULL,
	[message] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[xrest_account_operations](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[token] [nvarchar](100) NOT NULL,
	[created] [datetime] NOT NULL,
	[account_id] [int] NOT NULL,
	[operation_type] [tinyint] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[xrest_dict_account_type](
	[id] [tinyint] NOT NULL,
	[name] [nvarchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[xrest_dict_address](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[voivodeship_id] [tinyint] NOT NULL,
	[county] [nvarchar](100) NULL,
	[commune] [nvarchar](100) NULL,
	[city] [nvarchar](100) NULL,
	[street] [nvarchar](1024) NULL,
	[postcode] [nvarchar](16) NULL,
	[sector] [nvarchar](500) NULL,
	[attribute] [nvarchar](50) NULL,
	[audit_date] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[xrest_dict_delivery_types](
	[id] [tinyint] NOT NULL,
	[name] [nvarchar](300) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[xrest_dict_mail_status](
	[id] [tinyint] NOT NULL,
	[name] [nvarchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[xrest_dict_marketing](
	[id] [int] NOT NULL,
	[content] [text] NOT NULL,
	[content_trans] [xml] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[xrest_dict_order_from_source](
	[id] [tinyint] NOT NULL,
	[name] [nvarchar](300) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[xrest_dict_order_status](
	[id] [tinyint] NOT NULL,
	[name] [nvarchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[xrest_dict_payment_types](
	[id] [tinyint] NOT NULL,
	[name] [nvarchar](300) NOT NULL,
	[order] [bit] NOT NULL,
	[callcenter] [bit] NOT NULL,
	[name_trans] [xml] NULL,
	[fiscal] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[xrest_dict_product_set_types](
	[id] [tinyint] NOT NULL,
	[name] [nvarchar](300) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[xrest_dict_product_types](
	[id] [tinyint] NOT NULL,
	[name] [nvarchar](300) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[xrest_dict_term_types](
	[id] [tinyint] NOT NULL,
	[name] [nvarchar](300) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[xrest_dict_voivodeship](
	[id] [tinyint] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](300) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[xrest_fav_addr](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[account_id] [int] NOT NULL,
	[address_city] [nvarchar](100) NOT NULL,
	[address_street] [nvarchar](100) NULL,
	[address_street_number] [nvarchar](100) NULL,
	[address_house_number] [nvarchar](100) NULL,
	[default] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[xrest_images](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[item_id] [int] NULL,
	[item_group] [tinyint] NOT NULL,
	[mime] [nvarchar](30) NULL,
	[audit_date] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[xrest_mail_hour](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[create_date] [datetime] NULL,
	[mail_id] [int] NULL
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[xrest_mail_templates](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](100) NOT NULL,
	[path] [nvarchar](500) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[xrest_mail_token](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[token] [nvarchar](500) NOT NULL,
	[expire] [datetime] NOT NULL,
	[operation] [tinyint] NOT NULL,
	[item_id] [int] NULL,
	[mail_id] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[xrest_mails](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[status_id] [tinyint] NOT NULL,
	[template_id] [int] NOT NULL,
	[tries] [tinyint] NOT NULL,
	[min_send_hour] [datetime] NULL,
	[max_send_hour] [datetime] NULL,
	[address] [nvarchar](100) NOT NULL,
	[person] [nvarchar](100) NOT NULL,
	[subject] [nvarchar](100) NOT NULL,
	[smtp_server] [nvarchar](200) NULL,
	[replacement] [nvarchar](max) NULL,
	[erros] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[xrest_order_acceptance](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[order_id] [int] NOT NULL,
	[marketing_id] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[xrest_order_history](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[order_id] [int] NOT NULL,
	[created] [datetime] NOT NULL,
	[status] [tinyint] NOT NULL,
	[info] [nvarchar](1024) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[xrest_order_products](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[order_id] [int] NOT NULL,
	[product_id] [int] NOT NULL,
	[price] [decimal](19, 2) NOT NULL,
	[points] [smallint] NOT NULL,
	[type_id] [tinyint] NOT NULL,
	[order_product_id] [int] NULL,
	[display_name] [nvarchar](500) NOT NULL,
	[from_source_id] [tinyint] NULL,
	[index] [smallint] NOT NULL,
	[note] [nvarchar](max) NULL,
	[sub_index] [smallint] NULL,
	[base_price] [decimal](19, 2) NOT NULL,
	[vat] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[xrest_orders](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[customer_id] [int] NULL,
	[restaurant_id] [int] NOT NULL,
	[create_date] [datetime] NOT NULL,
	[address_city] [nvarchar](100) NOT NULL,
	[address_country] [nvarchar](100) NULL,
	[address_street] [nvarchar](100) NULL,
	[address_street_number] [nvarchar](100) NULL,
	[address_house_number] [nvarchar](100) NULL,
	[address_postcode] [nvarchar](100) NULL,
	[worker_id] [int] NULL,
	[amount] [decimal](19, 2) NOT NULL,
	[transport_amount] [decimal](19, 2) NOT NULL,
	[email] [nvarchar](256) NULL,
	[phone] [nvarchar](20) NULL,
	[note] [nvarchar](1024) NULL,
	[ip_address] [nvarchar](128) NULL,
	[status_id] [tinyint] NOT NULL,
	[payment_type_id] [tinyint] NOT NULL,
	[term_type_id] [tinyint] NOT NULL,
	[term] [time](7) NOT NULL,
	[status_info] [nvarchar](1024) NULL,
	[type_of_delivery_id] [tinyint] NULL,
	[modify_date] [datetime] NOT NULL,
	[payment_information] [nvarchar](4000) NULL,
	[restaurant_transport_id] [int] NULL,
	[address_sort] [nvarchar](2000) NULL,
	[terms_of_use] [bit] NOT NULL,
	[realization] [bit] NOT NULL,
	[marketing] [bit] NOT NULL,
	[driver_id] [int] NULL,
	[audit_date] [datetime] NOT NULL,
	[audit_user] [int] NULL,
	[order_day] [int] NOT NULL,
	[payment_status] [tinyint] NOT NULL,
	[firstname] [nvarchar](500) NULL,
	[lastname] [nvarchar](500) NULL,
	[discount] [decimal](9, 2) NULL,
	[source] [tinyint] NOT NULL,
	[external_id] [nvarchar](100) NULL,
	[settled] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[xrest_orders_invoices](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[order_id] [int] NULL,
	[nip] [nvarchar](15) NOT NULL,
	[address] [nvarchar](max) NOT NULL,
	[name] [nvarchar](max) NOT NULL,
	[number] [nvarchar](500) NULL,
	[create_date] [smalldatetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[xrest_points](
	[account_id] [int] NULL,
	[points_amount] [int] NULL,
	[order_id] [int] NULL,
	[comment] [varchar](50) NULL,
	[create_date] [smalldatetime] NULL,
	[id] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_xrest_points] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[xrest_product_groups](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](200) NOT NULL,
	[audit_date] [datetime] NOT NULL,
	[audit_user] [int] NULL,
	[system] [bit] NOT NULL,
	[package] [bit] NOT NULL,
	[background_color] [nvarchar](20) NULL,
	[text_color] [nvarchar](20) NULL,
	[type] [tinyint] NOT NULL,
	[name_trans] [xml] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[xrest_product_groups_restaurant](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[product_group_id] [int] NOT NULL,
	[restaurant_id] [int] NOT NULL,
	[index] [tinyint] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[xrest_product_sets](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[type_id] [tinyint] NOT NULL,
	[display_name] [nvarchar](500) NOT NULL,
	[name] [nvarchar](500) NOT NULL,
	[audit_date] [datetime] NOT NULL,
	[audit_user] [int] NULL,
	[display_name_trans] [xml] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[xrest_product_sets_item](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[product_sets_id] [int] NOT NULL,
	[product_id] [int] NOT NULL,
	[amount] [tinyint] NOT NULL,
	[order] [tinyint] NOT NULL,
	[selected] [bit] NOT NULL,
	[audit_date] [datetime] NOT NULL,
	[audit_user] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[xrest_product_sets_product](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[product_id] [int] NOT NULL,
	[product_set_id] [int] NOT NULL,
	[order] [tinyint] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[xrest_products](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[display_name] [nvarchar](500) NOT NULL,
	[name] [nvarchar](500) NOT NULL,
	[plu] [nvarchar](50) NOT NULL,
	[price] [decimal](19, 2) NOT NULL,
	[visible] [bit] NOT NULL,
	[vat_id] [int] NOT NULL,
	[product_group_id] [int] NOT NULL,
	[desription] [nvarchar](4000) NULL,
	[archive] [bit] NOT NULL,
	[points] [smallint] NOT NULL,
	[type_id] [tinyint] NOT NULL,
	[audit_date] [datetime] NOT NULL,
	[audit_user] [int] NULL,
	[fiscal_print] [bit] NOT NULL,
	[package_id] [int] NULL,
	[package] [bit] NOT NULL,
	[take_points] [smallint] NOT NULL,
	[destination] [tinyint] NOT NULL,
	[external_id] [nvarchar](100) NULL,
	[background_color] [nvarchar](20) NULL,
	[text_color] [nvarchar](20) NULL,
	[printer] [tinyint] NOT NULL,
	[zestaw_id] [int] NULL,
	[recipe] [bit] NULL,
	[restaurant_id] [int] NULL,
	[display_name_trans] [xml] NULL,
	[desription_trans] [xml] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[xrest_products_bundle](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[main_product_id] [int] NOT NULL,
	[product_id] [int] NOT NULL,
	[price] [decimal](19, 2) NULL,
	[can_delete] [bit] NOT NULL,
	[audit_date] [datetime] NOT NULL,
	[audit_user] [int] NULL,
	[default] [bit] NOT NULL,
	[label] [nvarchar](500) NULL,
	[canChangePrice] [bit] NULL,
	[label_trans] [xml] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[xrest_products_bundle_label](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](50) NULL,
	[label_trans] [xml] NULL,
 CONSTRAINT [PK_xrest_products_bundle_label] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[xrest_refresh_token](
	[id] [uniqueidentifier] NOT NULL,
	[client_id] [nvarchar](50) NOT NULL,
	[subject] [nvarchar](50) NOT NULL,
	[protected_ticket] [nvarchar](max) NOT NULL,
	[issued_utc] [datetime2](7) NOT NULL,
	[expires_utc] [datetime2](7) NOT NULL,
 CONSTRAINT [PK__xrest_ref__3213E83F86DC41E2] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[xrest_restaurant](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](100) NOT NULL,
	[description] [nvarchar](max) NULL,
	[address] [nvarchar](300) NULL,
	[phone] [nvarchar](50) NULL,
	[post_code] [nvarchar](10) NULL,
	[city] [nvarchar](100) NULL,
	[nip] [nvarchar](50) NULL,
	[active] [bit] NOT NULL,
	[alias] [nvarchar](100) NULL,
	[email] [nvarchar](256) NULL,
	[audit_date] [datetime] NOT NULL,
	[audit_user] [int] NULL,
	[deleted] [bit] NOT NULL,
	[defaultRealizationTime] [int] NOT NULL,
	[realizationTime] [int] NOT NULL,
	[invoice_address] [nvarchar](max) NULL,
	[day_number] [int] NOT NULL,
	[min_order] [decimal](18, 0) NOT NULL,
	[terms] [nvarchar](max) NULL,
	[payment_id] [nvarchar](500) NULL,
	[payment_secret] [nvarchar](500) NULL,
	[jpk_street] [nvarchar](100) NULL,
	[jpk_home_number] [nvarchar](3) NULL,
	[jpk_house_number] [varchar](10) NULL,
	[is_pos_checkout] [bit] NOT NULL,
 CONSTRAINT [PK__xrest_res__3213E83F189CEB53] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[xrest_restaurant_exclude_days](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[restaurant_id] [int] NULL,
	[from] [datetime] NOT NULL,
	[to] [datetime] NOT NULL,
	[audit_date] [datetime] NOT NULL,
	[audit_user] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[xrest_restaurant_invoice_number](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[restaurant_id] [int] NOT NULL,
	[year] [int] NOT NULL,
	[month] [int] NOT NULL,
	[number] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[xrest_restaurant_payment_type](
	[restaurant_id] [int] NOT NULL,
	[payment_id] [int] NOT NULL,
	[id] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_xrest_restaurant_payment_type] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[xrest_restaurant_products](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[restaurant_id] [int] NOT NULL,
	[product_id] [int] NOT NULL,
	[fiscal_name] [nvarchar](300) NULL,
	[hidden] [bit] NOT NULL,
	[can_change_for_points] [bit] NOT NULL,
	[audit_date] [datetime] NOT NULL,
	[audit_user] [int] NULL,
	[day_of_week] [tinyint] NOT NULL,
	[price] [decimal](19, 2) NULL,
	[quick_access] [bit] NULL,
	[pos_price] [decimal](19, 2) NULL,
	[order_name] [varchar](300) NULL,
	[price_change] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[xrest_restaurant_transport](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[restaurant_id] [int] NOT NULL,
	[name] [nvarchar](256) NOT NULL,
	[plu] [nvarchar](50) NOT NULL,
	[audit_date] [datetime] NOT NULL,
	[audit_user] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[xrest_restaurant_transport_price](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[xrest_restaurant_transport_id] [int] NOT NULL,
	[delivery_price] [decimal](19, 2) NOT NULL,
	[from_price] [decimal](19, 2) NOT NULL,
	[audit_date] [datetime] NOT NULL,
	[audit_user] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[xrest_restaurant_transport_zones](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[restaurant_transport_id] [int] NOT NULL,
	[address_id] [int] NOT NULL,
	[even_from] [nvarchar](16) NULL,
	[even_to] [nvarchar](16) NULL,
	[odd_from] [nvarchar](16) NULL,
	[odd_to] [nvarchar](16) NULL,
	[number_from] [nvarchar](16) NULL,
	[number_to] [nvarchar](16) NULL,
	[audit_date] [datetime] NOT NULL,
	[audit_user] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[xrest_restaurant_working_hours](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[restaurant_id] [int] NULL,
	[day] [tinyint] NOT NULL,
	[working_from] [time](7) NOT NULL,
	[working_to] [time](7) NOT NULL,
	[online_from] [time](7) NOT NULL,
	[online_to] [time](7) NOT NULL,
	[working] [bit] NOT NULL,
	[audit_date] [datetime] NOT NULL,
	[audit_user] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[xrest_session](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[token] [char](40) NOT NULL,
	[logon] [datetime] NOT NULL,
	[last_activity] [datetime] NOT NULL,
	[logoff] [datetime] NULL,
	[account_id] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[xrest_undelivered_reason](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[reason] [nvarchar](max) NOT NULL,
	[source_type] [tinyint] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[xrest_vat](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](100) NULL,
	[group] [nvarchar](5) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[xrest_vat_value](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[vat_id] [int] NOT NULL,
	[value] [decimal](19, 2) NOT NULL,
	[valid_from] [datetime] NOT NULL,
	[valid_to] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [execution_plan_order_product_id_vat] ON [dbo].[xrest_order_products]
(
	[product_id] ASC,
	[vat] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [execution_plan_vat_product_id_ordes] ON [dbo].[xrest_order_products]
(
	[vat] ASC
)
INCLUDE([product_id]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [index_orderId_FromSourceId_subIndex] ON [dbo].[xrest_order_products]
(
	[order_id] ASC,
	[from_source_id] ASC,
	[sub_index] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [index_pos_products] ON [dbo].[xrest_order_products]
(
	[product_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[xrest_account] ADD  CONSTRAINT [DF_xrest_account_parent_account_id]  DEFAULT ((1)) FOR [parent_account_id]
GO
ALTER TABLE [dbo].[xrest_account] ADD  DEFAULT ((0)) FOR [deleted]
GO
ALTER TABLE [dbo].[xrest_account] ADD  DEFAULT ((0)) FOR [locked]
GO
ALTER TABLE [dbo].[xrest_account] ADD  DEFAULT ((6)) FOR [account_type]
GO
ALTER TABLE [dbo].[xrest_account] ADD  DEFAULT ((0)) FOR [points]
GO
ALTER TABLE [dbo].[xrest_account] ADD  CONSTRAINT [DF_xrest_account_audit_date]  DEFAULT (getdate()) FOR [audit_date]
GO
ALTER TABLE [dbo].[xrest_account] ADD  DEFAULT ((0)) FOR [must_change_password]
GO
ALTER TABLE [dbo].[xrest_account] ADD  DEFAULT ((0)) FOR [max_orders]
GO
ALTER TABLE [dbo].[xrest_account] ADD  DEFAULT ((1)) FOR [status]
GO
ALTER TABLE [dbo].[xrest_account] ADD  DEFAULT ((0)) FOR [terms_accepted]
GO
ALTER TABLE [dbo].[xrest_account] ADD  DEFAULT ((0)) FOR [marketing]
GO
ALTER TABLE [dbo].[xrest_dict_address] ADD  DEFAULT (getdate()) FOR [audit_date]
GO
ALTER TABLE [dbo].[xrest_dict_payment_types] ADD  DEFAULT ((0)) FOR [order]
GO
ALTER TABLE [dbo].[xrest_dict_payment_types] ADD  DEFAULT ((0)) FOR [callcenter]
GO
ALTER TABLE [dbo].[xrest_dict_payment_types] ADD  DEFAULT ((1)) FOR [fiscal]
GO
ALTER TABLE [dbo].[xrest_fav_addr] ADD  DEFAULT ((0)) FOR [default]
GO
ALTER TABLE [dbo].[xrest_images] ADD  DEFAULT (getdate()) FOR [audit_date]
GO
ALTER TABLE [dbo].[xrest_mail_token] ADD  DEFAULT ((0)) FOR [operation]
GO
ALTER TABLE [dbo].[xrest_mails] ADD  DEFAULT ((0)) FOR [tries]
GO
ALTER TABLE [dbo].[xrest_order_products] ADD  DEFAULT ((1)) FOR [from_source_id]
GO
ALTER TABLE [dbo].[xrest_order_products] ADD  DEFAULT ((0)) FOR [index]
GO
ALTER TABLE [dbo].[xrest_order_products] ADD  DEFAULT ((0)) FOR [base_price]
GO
ALTER TABLE [dbo].[xrest_orders] ADD  DEFAULT (getdate()) FOR [audit_date]
GO
ALTER TABLE [dbo].[xrest_orders] ADD  DEFAULT ((-1)) FOR [order_day]
GO
ALTER TABLE [dbo].[xrest_orders] ADD  DEFAULT ((0)) FOR [payment_status]
GO
ALTER TABLE [dbo].[xrest_orders] ADD  DEFAULT ((0)) FOR [source]
GO
ALTER TABLE [dbo].[xrest_orders] ADD  DEFAULT ((0)) FOR [settled]
GO
ALTER TABLE [dbo].[xrest_orders_invoices] ADD  DEFAULT (getdate()) FOR [create_date]
GO
ALTER TABLE [dbo].[xrest_product_groups] ADD  DEFAULT (getdate()) FOR [audit_date]
GO
ALTER TABLE [dbo].[xrest_product_groups] ADD  DEFAULT ((0)) FOR [system]
GO
ALTER TABLE [dbo].[xrest_product_groups] ADD  DEFAULT ((0)) FOR [package]
GO
ALTER TABLE [dbo].[xrest_product_groups] ADD  DEFAULT ((0)) FOR [type]
GO
ALTER TABLE [dbo].[xrest_product_sets] ADD  DEFAULT (getdate()) FOR [audit_date]
GO
ALTER TABLE [dbo].[xrest_product_sets_item] ADD  DEFAULT ((0)) FOR [selected]
GO
ALTER TABLE [dbo].[xrest_product_sets_item] ADD  DEFAULT (getdate()) FOR [audit_date]
GO
ALTER TABLE [dbo].[xrest_products] ADD  CONSTRAINT [DF_xrest_products_plu]  DEFAULT ((0)) FOR [plu]
GO
ALTER TABLE [dbo].[xrest_products] ADD  DEFAULT ((0)) FOR [archive]
GO
ALTER TABLE [dbo].[xrest_products] ADD  DEFAULT ((0)) FOR [points]
GO
ALTER TABLE [dbo].[xrest_products] ADD  DEFAULT ((1)) FOR [type_id]
GO
ALTER TABLE [dbo].[xrest_products] ADD  DEFAULT (getdate()) FOR [audit_date]
GO
ALTER TABLE [dbo].[xrest_products] ADD  DEFAULT ((0)) FOR [fiscal_print]
GO
ALTER TABLE [dbo].[xrest_products] ADD  DEFAULT ((0)) FOR [package]
GO
ALTER TABLE [dbo].[xrest_products] ADD  DEFAULT ((0)) FOR [take_points]
GO
ALTER TABLE [dbo].[xrest_products] ADD  DEFAULT ((1)) FOR [destination]
GO
ALTER TABLE [dbo].[xrest_products] ADD  DEFAULT ((1)) FOR [printer]
GO
ALTER TABLE [dbo].[xrest_products] ADD  CONSTRAINT [DF__xrest_prod__resta__3CA9F2BB]  DEFAULT (NULL) FOR [restaurant_id]
GO
ALTER TABLE [dbo].[xrest_products_bundle] ADD  DEFAULT ((0)) FOR [can_delete]
GO
ALTER TABLE [dbo].[xrest_products_bundle] ADD  DEFAULT (getdate()) FOR [audit_date]
GO
ALTER TABLE [dbo].[xrest_products_bundle] ADD  DEFAULT ((0)) FOR [default]
GO
ALTER TABLE [dbo].[xrest_restaurant] ADD  CONSTRAINT [DF__xrest_rest__activ__03F0984C]  DEFAULT ((0)) FOR [active]
GO
ALTER TABLE [dbo].[xrest_restaurant] ADD  CONSTRAINT [DF__xrest_rest__audit__04E4BC85]  DEFAULT (getdate()) FOR [audit_date]
GO
ALTER TABLE [dbo].[xrest_restaurant] ADD  CONSTRAINT [DF__xrest_rest__delet__05D8E0BE]  DEFAULT ((0)) FOR [deleted]
GO
ALTER TABLE [dbo].[xrest_restaurant] ADD  CONSTRAINT [DF__xrest_rest__defau__06CD04F7]  DEFAULT ((0)) FOR [defaultRealizationTime]
GO
ALTER TABLE [dbo].[xrest_restaurant] ADD  CONSTRAINT [DF__xrest_rest__reali__07C12930]  DEFAULT ((0)) FOR [realizationTime]
GO
ALTER TABLE [dbo].[xrest_restaurant] ADD  CONSTRAINT [DF__xrest_rest__min_o__08B54D69]  DEFAULT ((0)) FOR [min_order]
GO
ALTER TABLE [dbo].[xrest_restaurant] ADD  CONSTRAINT [DF_xrest_restaurant_is_pos_checkout]  DEFAULT ((0)) FOR [is_pos_checkout]
GO
ALTER TABLE [dbo].[xrest_restaurant_exclude_days] ADD  DEFAULT (getdate()) FOR [audit_date]
GO
ALTER TABLE [dbo].[xrest_restaurant_invoice_number] ADD  DEFAULT ((1)) FOR [number]
GO
ALTER TABLE [dbo].[xrest_restaurant_products] ADD  DEFAULT ((0)) FOR [hidden]
GO
ALTER TABLE [dbo].[xrest_restaurant_products] ADD  DEFAULT ((0)) FOR [can_change_for_points]
GO
ALTER TABLE [dbo].[xrest_restaurant_products] ADD  DEFAULT (getdate()) FOR [audit_date]
GO
ALTER TABLE [dbo].[xrest_restaurant_products] ADD  CONSTRAINT [DF_xrest_restaurant_products_day_of_weeks]  DEFAULT ((0)) FOR [day_of_week]
GO
ALTER TABLE [dbo].[xrest_restaurant_transport] ADD  DEFAULT (getdate()) FOR [audit_date]
GO
ALTER TABLE [dbo].[xrest_restaurant_transport_price] ADD  DEFAULT (getdate()) FOR [audit_date]
GO
ALTER TABLE [dbo].[xrest_restaurant_transport_zones] ADD  DEFAULT (getdate()) FOR [audit_date]
GO
ALTER TABLE [dbo].[xrest_restaurant_working_hours] ADD  DEFAULT (getdate()) FOR [audit_date]
GO
ALTER TABLE [dbo].[xrest_undelivered_reason] ADD  CONSTRAINT [DF_xrest_undelivered_reason_source_type]  DEFAULT ((1)) FOR [source_type]
GO
ALTER TABLE [dbo].[xrest_account]  WITH CHECK ADD  CONSTRAINT [FK_xrest_account_dict_account_parent] FOREIGN KEY([parent_account_id])
REFERENCES [dbo].[xrest_account] ([id])
GO
ALTER TABLE [dbo].[xrest_account] CHECK CONSTRAINT [FK_xrest_account_dict_account_parent]
GO
ALTER TABLE [dbo].[xrest_account]  WITH CHECK ADD  CONSTRAINT [FK_xrest_account_dict_account_type] FOREIGN KEY([account_type])
REFERENCES [dbo].[xrest_dict_account_type] ([id])
GO
ALTER TABLE [dbo].[xrest_account] CHECK CONSTRAINT [FK_xrest_account_dict_account_type]
GO
ALTER TABLE [dbo].[xrest_account_history]  WITH CHECK ADD  CONSTRAINT [FK_xrest_account_history_account] FOREIGN KEY([account_id])
REFERENCES [dbo].[xrest_account] ([id])
GO
ALTER TABLE [dbo].[xrest_account_history] CHECK CONSTRAINT [FK_xrest_account_history_account]
GO
ALTER TABLE [dbo].[xrest_dict_address]  WITH CHECK ADD  CONSTRAINT [FK_xrest_dict_address_xrest_dict_voivodeship] FOREIGN KEY([voivodeship_id])
REFERENCES [dbo].[xrest_dict_voivodeship] ([id])
GO
ALTER TABLE [dbo].[xrest_dict_address] CHECK CONSTRAINT [FK_xrest_dict_address_xrest_dict_voivodeship]
GO
ALTER TABLE [dbo].[xrest_fav_addr]  WITH CHECK ADD  CONSTRAINT [FK_xrest_fav_addraccount] FOREIGN KEY([account_id])
REFERENCES [dbo].[xrest_account] ([id])
GO
ALTER TABLE [dbo].[xrest_fav_addr] CHECK CONSTRAINT [FK_xrest_fav_addraccount]
GO
ALTER TABLE [dbo].[xrest_mails]  WITH CHECK ADD  CONSTRAINT [FK_xrest_mails_status] FOREIGN KEY([status_id])
REFERENCES [dbo].[xrest_dict_mail_status] ([id])
GO
ALTER TABLE [dbo].[xrest_mails] CHECK CONSTRAINT [FK_xrest_mails_status]
GO
ALTER TABLE [dbo].[xrest_mails]  WITH CHECK ADD  CONSTRAINT [FK_xrest_mails_template] FOREIGN KEY([template_id])
REFERENCES [dbo].[xrest_mail_templates] ([id])
GO
ALTER TABLE [dbo].[xrest_mails] CHECK CONSTRAINT [FK_xrest_mails_template]
GO
ALTER TABLE [dbo].[xrest_order_history]  WITH CHECK ADD  CONSTRAINT [FK_xrest_order_history_order] FOREIGN KEY([order_id])
REFERENCES [dbo].[xrest_orders] ([id])
GO
ALTER TABLE [dbo].[xrest_order_history] CHECK CONSTRAINT [FK_xrest_order_history_order]
GO
ALTER TABLE [dbo].[xrest_order_products]  WITH CHECK ADD  CONSTRAINT [FK_xrest_order_products_order] FOREIGN KEY([order_id])
REFERENCES [dbo].[xrest_orders] ([id])
GO
ALTER TABLE [dbo].[xrest_order_products] CHECK CONSTRAINT [FK_xrest_order_products_order]
GO
ALTER TABLE [dbo].[xrest_order_products]  WITH CHECK ADD  CONSTRAINT [FK_xrest_order_products_product] FOREIGN KEY([product_id])
REFERENCES [dbo].[xrest_products] ([id])
GO
ALTER TABLE [dbo].[xrest_order_products] CHECK CONSTRAINT [FK_xrest_order_products_product]
GO
ALTER TABLE [dbo].[xrest_order_products]  WITH CHECK ADD  CONSTRAINT [FK_xrest_order_products_source] FOREIGN KEY([from_source_id])
REFERENCES [dbo].[xrest_dict_order_from_source] ([id])
GO
ALTER TABLE [dbo].[xrest_order_products] CHECK CONSTRAINT [FK_xrest_order_products_source]
GO
ALTER TABLE [dbo].[xrest_order_products]  WITH CHECK ADD  CONSTRAINT [FK_xrest_order_products_type] FOREIGN KEY([type_id])
REFERENCES [dbo].[xrest_dict_product_types] ([id])
GO
ALTER TABLE [dbo].[xrest_order_products] CHECK CONSTRAINT [FK_xrest_order_products_type]
GO
ALTER TABLE [dbo].[xrest_orders]  WITH CHECK ADD  CONSTRAINT [FK_orders_customer] FOREIGN KEY([customer_id])
REFERENCES [dbo].[xrest_account] ([id])
GO
ALTER TABLE [dbo].[xrest_orders] CHECK CONSTRAINT [FK_orders_customer]
GO
ALTER TABLE [dbo].[xrest_orders]  WITH CHECK ADD  CONSTRAINT [FK_orders_deliver] FOREIGN KEY([type_of_delivery_id])
REFERENCES [dbo].[xrest_dict_delivery_types] ([id])
GO
ALTER TABLE [dbo].[xrest_orders] CHECK CONSTRAINT [FK_orders_deliver]
GO
ALTER TABLE [dbo].[xrest_orders]  WITH CHECK ADD  CONSTRAINT [FK_orders_driver] FOREIGN KEY([driver_id])
REFERENCES [dbo].[xrest_account] ([id])
GO
ALTER TABLE [dbo].[xrest_orders] CHECK CONSTRAINT [FK_orders_driver]
GO
ALTER TABLE [dbo].[xrest_orders]  WITH CHECK ADD  CONSTRAINT [FK_orders_paymenttype] FOREIGN KEY([payment_type_id])
REFERENCES [dbo].[xrest_dict_payment_types] ([id])
GO
ALTER TABLE [dbo].[xrest_orders] CHECK CONSTRAINT [FK_orders_paymenttype]
GO
ALTER TABLE [dbo].[xrest_orders]  WITH CHECK ADD  CONSTRAINT [FK_orders_restaurant] FOREIGN KEY([restaurant_id])
REFERENCES [dbo].[xrest_restaurant] ([id])
GO
ALTER TABLE [dbo].[xrest_orders] CHECK CONSTRAINT [FK_orders_restaurant]
GO
ALTER TABLE [dbo].[xrest_orders]  WITH CHECK ADD  CONSTRAINT [FK_orders_restaurant_transport] FOREIGN KEY([restaurant_transport_id])
REFERENCES [dbo].[xrest_restaurant_transport] ([id])
GO
ALTER TABLE [dbo].[xrest_orders] CHECK CONSTRAINT [FK_orders_restaurant_transport]
GO
ALTER TABLE [dbo].[xrest_orders]  WITH CHECK ADD  CONSTRAINT [FK_orders_status] FOREIGN KEY([status_id])
REFERENCES [dbo].[xrest_dict_order_status] ([id])
GO
ALTER TABLE [dbo].[xrest_orders] CHECK CONSTRAINT [FK_orders_status]
GO
ALTER TABLE [dbo].[xrest_orders]  WITH CHECK ADD  CONSTRAINT [FK_orders_termtype] FOREIGN KEY([term_type_id])
REFERENCES [dbo].[xrest_dict_term_types] ([id])
GO
ALTER TABLE [dbo].[xrest_orders] CHECK CONSTRAINT [FK_orders_termtype]
GO
ALTER TABLE [dbo].[xrest_orders]  WITH CHECK ADD  CONSTRAINT [FK_orders_worker] FOREIGN KEY([worker_id])
REFERENCES [dbo].[xrest_account] ([id])
GO
ALTER TABLE [dbo].[xrest_orders] CHECK CONSTRAINT [FK_orders_worker]
GO
ALTER TABLE [dbo].[xrest_orders_invoices]  WITH CHECK ADD  CONSTRAINT [FK_xrest_orders_invoices_orderid] FOREIGN KEY([order_id])
REFERENCES [dbo].[xrest_orders] ([id])
GO
ALTER TABLE [dbo].[xrest_orders_invoices] CHECK CONSTRAINT [FK_xrest_orders_invoices_orderid]
GO
ALTER TABLE [dbo].[xrest_product_sets]  WITH CHECK ADD  CONSTRAINT [FK_xrest_product_sets_type] FOREIGN KEY([type_id])
REFERENCES [dbo].[xrest_dict_product_set_types] ([id])
GO
ALTER TABLE [dbo].[xrest_product_sets] CHECK CONSTRAINT [FK_xrest_product_sets_type]
GO
ALTER TABLE [dbo].[xrest_product_sets_item]  WITH CHECK ADD  CONSTRAINT [FK_product_sets_item_product] FOREIGN KEY([product_id])
REFERENCES [dbo].[xrest_products] ([id])
GO
ALTER TABLE [dbo].[xrest_product_sets_item] CHECK CONSTRAINT [FK_product_sets_item_product]
GO
ALTER TABLE [dbo].[xrest_product_sets_item]  WITH CHECK ADD  CONSTRAINT [FK_product_sets_item_product_sets] FOREIGN KEY([product_sets_id])
REFERENCES [dbo].[xrest_product_sets] ([id])
GO
ALTER TABLE [dbo].[xrest_product_sets_item] CHECK CONSTRAINT [FK_product_sets_item_product_sets]
GO
ALTER TABLE [dbo].[xrest_product_sets_product]  WITH CHECK ADD  CONSTRAINT [FK_xrest_product_sets_product_roduct] FOREIGN KEY([product_id])
REFERENCES [dbo].[xrest_products] ([id])
GO
ALTER TABLE [dbo].[xrest_product_sets_product] CHECK CONSTRAINT [FK_xrest_product_sets_product_roduct]
GO
ALTER TABLE [dbo].[xrest_product_sets_product]  WITH CHECK ADD  CONSTRAINT [FK_xrest_product_sets_product_roductsets] FOREIGN KEY([product_set_id])
REFERENCES [dbo].[xrest_product_sets] ([id])
GO
ALTER TABLE [dbo].[xrest_product_sets_product] CHECK CONSTRAINT [FK_xrest_product_sets_product_roductsets]
GO
ALTER TABLE [dbo].[xrest_products]  WITH CHECK ADD  CONSTRAINT [FK_xrest_products_group] FOREIGN KEY([product_group_id])
REFERENCES [dbo].[xrest_product_groups] ([id])
GO
ALTER TABLE [dbo].[xrest_products] CHECK CONSTRAINT [FK_xrest_products_group]
GO
ALTER TABLE [dbo].[xrest_products]  WITH CHECK ADD  CONSTRAINT [FK_xrest_products_type] FOREIGN KEY([type_id])
REFERENCES [dbo].[xrest_dict_product_types] ([id])
GO
ALTER TABLE [dbo].[xrest_products] CHECK CONSTRAINT [FK_xrest_products_type]
GO
ALTER TABLE [dbo].[xrest_products]  WITH CHECK ADD  CONSTRAINT [FK_xrest_products_vat] FOREIGN KEY([vat_id])
REFERENCES [dbo].[xrest_vat] ([id])
GO
ALTER TABLE [dbo].[xrest_products] CHECK CONSTRAINT [FK_xrest_products_vat]
GO
ALTER TABLE [dbo].[xrest_products_bundle]  WITH CHECK ADD  CONSTRAINT [FK_xrest_products_bundle_mainproduct] FOREIGN KEY([main_product_id])
REFERENCES [dbo].[xrest_products] ([id])
GO
ALTER TABLE [dbo].[xrest_products_bundle] CHECK CONSTRAINT [FK_xrest_products_bundle_mainproduct]
GO
ALTER TABLE [dbo].[xrest_products_bundle]  WITH CHECK ADD  CONSTRAINT [FK_xrest_products_bundle_product] FOREIGN KEY([product_id])
REFERENCES [dbo].[xrest_products] ([id])
GO
ALTER TABLE [dbo].[xrest_products_bundle] CHECK CONSTRAINT [FK_xrest_products_bundle_product]
GO
ALTER TABLE [dbo].[xrest_restaurant_exclude_days]  WITH CHECK ADD  CONSTRAINT [FK_xrest_restaurant_exclude_days_restaurant] FOREIGN KEY([restaurant_id])
REFERENCES [dbo].[xrest_restaurant] ([id])
GO
ALTER TABLE [dbo].[xrest_restaurant_exclude_days] CHECK CONSTRAINT [FK_xrest_restaurant_exclude_days_restaurant]
GO
ALTER TABLE [dbo].[xrest_restaurant_invoice_number]  WITH CHECK ADD  CONSTRAINT [FK_xrest_restaurant_invoice_number_rest] FOREIGN KEY([restaurant_id])
REFERENCES [dbo].[xrest_restaurant] ([id])
GO
ALTER TABLE [dbo].[xrest_restaurant_invoice_number] CHECK CONSTRAINT [FK_xrest_restaurant_invoice_number_rest]
GO
ALTER TABLE [dbo].[xrest_restaurant_products]  WITH CHECK ADD  CONSTRAINT [FK_xrest_restaurant_products_product] FOREIGN KEY([product_id])
REFERENCES [dbo].[xrest_products] ([id])
GO
ALTER TABLE [dbo].[xrest_restaurant_products] CHECK CONSTRAINT [FK_xrest_restaurant_products_product]
GO
ALTER TABLE [dbo].[xrest_restaurant_products]  WITH CHECK ADD  CONSTRAINT [FK_xrest_restaurant_products_restaurant] FOREIGN KEY([restaurant_id])
REFERENCES [dbo].[xrest_restaurant] ([id])
GO
ALTER TABLE [dbo].[xrest_restaurant_products] CHECK CONSTRAINT [FK_xrest_restaurant_products_restaurant]
GO
ALTER TABLE [dbo].[xrest_restaurant_transport]  WITH CHECK ADD  CONSTRAINT [FK_xrest_restaurant_transport_restaurant] FOREIGN KEY([restaurant_id])
REFERENCES [dbo].[xrest_restaurant] ([id])
GO
ALTER TABLE [dbo].[xrest_restaurant_transport] CHECK CONSTRAINT [FK_xrest_restaurant_transport_restaurant]
GO
ALTER TABLE [dbo].[xrest_restaurant_transport_price]  WITH CHECK ADD  CONSTRAINT [FK_xrest_restaurant_transport_price_restaurant] FOREIGN KEY([xrest_restaurant_transport_id])
REFERENCES [dbo].[xrest_restaurant_transport] ([id])
GO
ALTER TABLE [dbo].[xrest_restaurant_transport_price] CHECK CONSTRAINT [FK_xrest_restaurant_transport_price_restaurant]
GO
ALTER TABLE [dbo].[xrest_restaurant_transport_zones]  WITH CHECK ADD  CONSTRAINT [FK_xrest_restaurant_transport_zones_address] FOREIGN KEY([address_id])
REFERENCES [dbo].[xrest_dict_address] ([id])
GO
ALTER TABLE [dbo].[xrest_restaurant_transport_zones] CHECK CONSTRAINT [FK_xrest_restaurant_transport_zones_address]
GO
ALTER TABLE [dbo].[xrest_restaurant_transport_zones]  WITH CHECK ADD  CONSTRAINT [FK_xrest_restaurant_transport_zones_transport] FOREIGN KEY([restaurant_transport_id])
REFERENCES [dbo].[xrest_restaurant_transport] ([id])
GO
ALTER TABLE [dbo].[xrest_restaurant_transport_zones] CHECK CONSTRAINT [FK_xrest_restaurant_transport_zones_transport]
GO
ALTER TABLE [dbo].[xrest_restaurant_working_hours]  WITH CHECK ADD  CONSTRAINT [FK_xrest_restaurant_working_hours_restaurant] FOREIGN KEY([restaurant_id])
REFERENCES [dbo].[xrest_restaurant] ([id])
GO
ALTER TABLE [dbo].[xrest_restaurant_working_hours] CHECK CONSTRAINT [FK_xrest_restaurant_working_hours_restaurant]
GO
ALTER TABLE [dbo].[xrest_session]  WITH CHECK ADD  CONSTRAINT [FK_xrest_session_account] FOREIGN KEY([account_id])
REFERENCES [dbo].[xrest_account] ([id])
GO
ALTER TABLE [dbo].[xrest_session] CHECK CONSTRAINT [FK_xrest_session_account]
GO
ALTER TABLE [dbo].[xrest_vat_value]  WITH CHECK ADD  CONSTRAINT [FK_xrest_vat_value_vat] FOREIGN KEY([vat_id])
REFERENCES [dbo].[xrest_vat] ([id])
GO
ALTER TABLE [dbo].[xrest_vat_value] CHECK CONSTRAINT [FK_xrest_vat_value_vat]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [ident].[AddAccountOperation]
    @token nvarchar(300),
    @created datetime,
    @accountId int,
	@operationType tinyint
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [dbo].[xrest_account_operations]
           ([token],
            [created],
            [account_id],
			[operation_type])
     VALUES
           (@token,
            @created,
            @accountId,
			@operationType)
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [ident].[AddCustomerMarketingAgreement]
    @accountId int,
    @marketingId int
AS
BEGIN
	SET NOCOUNT ON;
    
    INSERT INTO [dbo].[xrest_account_acceptance]
                           ([account_id]
                           ,[marketing_id])
                     VALUES
                           (@accountId
                           ,@marketingId)
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [ident].[AddPoints]
    @accountId int, 
	@points int, 
	@comment NVARCHAR(50), 
	@creation datetime, 
	@orderId int = null
AS
BEGIN
	SET NOCOUNT ON;

	BEGIN TRAN

    UPDATE [dbo].[xrest_account] SET  [points] = [points]  + @points  WHERE [id] = @accountId
	
	INSERT INTO [dbo].[xrest_points]
           ([account_id]
           ,[points_amount]
           ,[order_id]
           ,[comment]
           ,[create_date])
     VALUES
           (@accountId
           ,@points
           ,@orderId
           ,@comment
           ,@creation)

        
		 SELECT SCOPE_IDENTITY()
	COMMIT 
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [ident].[CustomerAddNewLoginMethodToAccount]
    @accountId int, 
    @externalId nvarchar(500),
    @provider tinyint
AS
BEGIN
	SET NOCOUNT ON;

    IF EXISTS(SELECT [id] FROM [dbo].[xrest_account_external] WHERE [external_id] = @externalId and [provider] = @provider)
	BEGIN
		SELECT -1
		RETURN
	END

    INSERT INTO [dbo].[xrest_account_external] ([account_id],[provider],[external_id],[created]) 
    VALUES(@accountId, @provider, @externalId, getdate()) 
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [ident].[CustomerGetPointsHistory]
    @accountId int
AS
BEGIN
	SET NOCOUNT ON;

        SELECT [points_amount] [Points]
              ,[comment] [Comment]
              ,[create_date] [Created]
          FROM [dbo].[xrest_points]
         WHERE account_id= @accountId
      ORDER BY create_date 

END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [ident].[CustomerNewAccount]
    @firstname NVARCHAR(500), 
	@lastname NVARCHAR(500), 
	@email NVARCHAR(128), 
	@password NVARCHAR(256), 
	@marketing BIT,
	@terms BIT,
    @externalId nvarchar(500),
    @provider tinyint
AS
BEGIN
	SET NOCOUNT ON;

    IF EXISTS(SELECT [id] FROM [dbo].[xrest_account_external] WHERE [external_id] = @externalId and [provider] = @provider)
	BEGIN
		SELECT -1
		RETURN
	END

	BEGIN TRAN

    declare @accountId int 
	
	INSERT INTO [dbo].[xrest_account]
           ([email]
           ,[password]
           ,[created_date]
           ,[deleted]
           ,[locked]
           ,[locked_reason]
           ,[account_type]
           ,[audit_date]
           ,[must_change_password]
           ,[firstname]
           ,[lastname]
           ,[status]
           ,[terms_accepted]
           ,[marketing])
     VALUES(
          @email,
		  @password,
		  GETDATE(),
		  0,
		  0,
		  NULL,
          6,
     
		  GETDATE(),
          0,
          @firstname,
          @lastname,
          2,
           @terms,
          @marketing)

        
		 SELECT @accountId =  SCOPE_IDENTITY()
         IF @externalId IS NOT NULL 
             INSERT INTO [dbo].[xrest_account_external] ([account_id],[provider],[external_id],[created]) 
                VALUES(@accountId, @provider, @externalId, getdate())

         SELECT @accountId
	COMMIT 
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [ident].[CustomerNewExternalAccount]
    @firstname NVARCHAR(500), 
	@lastname NVARCHAR(500), 
	@email NVARCHAR(128), 
	@password NVARCHAR(256), 
	@marketing BIT,
	@terms BIT,
    @externalId nvarchar(500),
    @provider tinyint
AS
BEGIN
	SET NOCOUNT ON;

    IF EXISTS(SELECT [id] FROM [dbo].[xrest_account_external] WHERE [external_id] = @externalId and [provider] = @provider)
	BEGIN
		SELECT -1
		RETURN
	END

	BEGIN TRAN

    declare @accountId int 
	
	INSERT INTO [dbo].[xrest_account]
           ([email]
           ,[password]
           ,[created_date]
           ,[deleted]
           ,[locked]
           ,[locked_reason]
           ,[account_type]
           ,[audit_date]
           ,[must_change_password]
           ,[firstname]
           ,[lastname]
           ,[status]
           ,[terms_accepted]
           ,[marketing])
     VALUES(
          @email,
		  @password,
		  GETDATE(),
		  0,
		  0,
		  NULL,
          6,
     
		  GETDATE(),
          0,
          @firstname,
          @lastname,
          2,
           @terms,
          @marketing)

        
		 SELECT @accountId =  SCOPE_IDENTITY()
         INSERT INTO [dbo].[xrest_account_external] ([account_id],[provider],[external_id],[created]) 
            VALUES(@accountId, @provider, @externalId, getdate())

         SELECT @accountId
	COMMIT 
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [ident].[CustomerNewLoginMethodToAccount]
    @accountId int, 
    @externalId nvarchar(500),
    @provider tinyint
AS
BEGIN
	SET NOCOUNT ON;

    IF EXISTS(SELECT [id] FROM [dbo].[xrest_account_external] WHERE [external_id] = @externalId and [provider] = @provider)
	BEGIN
		SELECT -1
		RETURN
	END

    INSERT INTO [dbo].[xrest_account_external] ([account_id],[provider],[external_id],[created]) 
    VALUES(@accountId, @provider, @externalId, getdate()) 
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [ident].[DisableAgreementCounter]
    @accountId int
AS
BEGIN
	SET NOCOUNT ON;
    
   update [dbo].[xrest_account] set [max_orders] = 5 where [id] = @accountId
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [ident].[GetAccountOperationByToken]
    @token nvarchar(200)
AS
BEGIN
	SET NOCOUNT ON;
    SELECT  [token] [Token]
      ,[created] [Created]
	  ,[account_id] [AccountId]
	  ,[operation_type] [OperationType]
  FROM [dbo].[xrest_account_operations] 
  WHERE [token] = @token
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [ident].[GetCustomerMarketingAgreement]
    @accountId int
AS
BEGIN
	SET NOCOUNT ON;
    
    SELECT a.[id] [Id]
        ,a.[content_trans] [Content]
		,CASE WHEN b.id IS NOT NULL THEN 1 ELSE 0 END [Checked]
    FROM [dbo].[xrest_dict_marketing] a
	LEFT JOIN [dbo].[xrest_account_acceptance] b on b.[marketing_id] = a.[id] AND b.[account_id]  = @accountId 

END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [ident].[GetSessionByAccountIdAsync]
    @accountId int
AS
BEGIN
	SET NOCOUNT ON;

   SELECT [id] [Id]
          ,[token] [Token]
          ,[logon] [Logon]
          ,[last_activity] [LastActivity]
          ,[logoff] [LogOff]
          ,[account_id] [AccountId]
      FROM [dbo].[xrest_session]
      where [account_id] = @accountId
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [ident].[GetTerms]
AS
    SELECT [id] [Id]
          ,[content_trans] [Content]
      FROM [dbo].[xrest_dict_marketing]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [ident].[GetWorkerInfoForReport]
    @workerId int,
    @year int = null,
    @month int = null,
    @day int = null
AS
BEGIN

    declare @name nvarchar(500)
    declare @workFrom datetime
    declare @workTo datetime

    select @name  = isnull(firstname,'') +' ' + isnull(lastname,'') from [dbo].[xrest_account] where id  = @workerId

    SELECT @workFrom= min(logon)
      FROM [dbo].[xrest_session_history]
      where   year([logon]) = @year
    and month([logon]) =@month
    and day([logon])= @day

    SELECT @workTo= max(logoff)
      FROM [dbo].[xrest_session_history]
      where   year([logon]) = @year
    and month([logon]) =@month
    and day([logon])= @day

    select @name [Driver], @workFrom [WorkFrom], @workTo [WorkTo]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [ident].[IncreaseCustomerMarketingAgreementTries]
    @accountId int
AS
BEGIN
	SET NOCOUNT ON;

    update [dbo].[xrest_account] set [max_orders] = [max_orders] +1 where [id] = @accountId
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [ident].[MoveSessionToHistory]
    @accountId int
AS
BEGIN
	SET NOCOUNT ON;
    INSERT INTO [dbo].[xrest_session_history] (
      [token],
      [logon],
      [last_activity],
      [logoff],
      [account_id])
    SELECT [token],
           [logon],
           getdate(),
           getdate(),
           [account_id]
      FROM [dbo].[xrest_session]
      WHERE [account_id] = @accountId
    
      DELETE FROM [dbo].[xrest_session] WHERE [account_id] = @accountId
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [ident].[RemoveAccountOperationByToken]
    @token nvarchar(200)
AS
BEGIN
	SET NOCOUNT ON;
    DELETE FROM [dbo].[xrest_account_operations]  WHERE [token] = @token
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [ident].[RemoveAllCustomerMarketingAgreement]
    @accountId int
AS
BEGIN
	SET NOCOUNT ON;
    
   delete from  [dbo].[xrest_account_acceptance] where [account_id] = @accountId
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [ident].[SetAccountStatus]
    @accountId int, 
	@status tinyint
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[xrest_account] SET [status] = @status WHERE [id] = @accountId
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [ident].[TokenAddNew]
    @token nvarchar(300),
    @created datetime,
    @expired datetime
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [dbo].[xrest_refresh_tokens]
           ([token]
           ,[created]
           ,[expired])
     VALUES
           (@token
           ,@created
           ,@created)
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [ident].[TokenGetByToken]
    @token nvarchar(300)
AS
BEGIN
	SET NOCOUNT ON;
    SELECT  [id] [Id]
      ,[token] [Token]
      ,[created] [Created]
      ,[expired] [Expired]
  FROM [dbo].[xrest_refresh_tokens] 
  WHERE [token] = @token
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [ident].[TokenRemoveById]
    @id int
AS
BEGIN
	SET NOCOUNT ON;
    DELETE FROM [dbo].[xrest_refresh_tokens]  WHERE [id] = @id
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [ident].[UpdateAccountPassword]
    @accountId int, 
	@password nvarchar(256)
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[xrest_account] SET [password] = @password WHERE [id] = @accountId
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [ident].[WorkerSaveRcp]
        @accountId INT,
        @rcpId TINYINT,
        @operationDate DATETIME,
        @orderDay INT
AS
BEGIN
		INSERT INTO [dbo].[xrest_rcp]
           ([account_id]
           ,[rcp_id]
           ,[operation_date]
           ,[order_day])
     VALUES
           (@accountId,
            @rcpId,
            @operationDate,
            @orderDay)

		SELECT @@IDENTITY
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [loc].[CheckForChangesInAddresses]
    @audit DATETIME2
AS
   DECLARE @newData BIT = 0

 IF EXISTS( SELECT *
      FROM [dbo].[xrest_dict_address]
      WHERE audit_date> @audit)
   SET @newData = 1

SELECT @newData NewData
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [ord].[BurnCode]
    @codeId int,
    @used DateTime,
    @orderId int,
    @customerId int
AS
    update [dbo].[xrest_discount_code]
     SET [burned] = 1,
         [used] = @used,
         [customer_id] = @customerId,
         [order_id] = @orderId
    WHERE [id] = @codeId
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [ord].[CheckForChangesInProducts]
    @audit DATETIME2
AS
   DECLARE @newData BIT = 0

 IF EXISTS( SELECT audit_date 
      FROM [dbo].[xrest_products]
      WHERE audit_date> @audit)
   SET @newData = 1

 IF @newData = 0 AND EXISTS( SELECT audit_date 
      FROM [dbo].[xrest_restaurant_product_extra]
      WHERE audit_date> @audit)
   SET @newData = 1
   
 IF @newData = 0 AND EXISTS( SELECT audit_date
      FROM  [dbo].[xrest_restaurant_product_exclude]
      WHERE audit_date> @audit)
   SET @newData = 1

 IF @newData = 0 AND EXISTS( SELECT audit_date
      FROM  [dbo].[xrest_products_bundle]
      WHERE audit_date> @audit)
   SET @newData = 1

 IF @newData = 0 AND EXISTS( SELECT audit_date
      FROM  [dbo].[xrest_restaurant_products]
      WHERE audit_date> @audit)
   SET @newData = 1

 IF @newData = 0 AND EXISTS( SELECT audit_date
      FROM  [dbo].[xrest_product_sets_item]
      WHERE audit_date> @audit)
   SET @newData = 1

 IF @newData = 0 AND EXISTS( SELECT audit_date
      FROM  [dbo].[xrest_product_sets]
      WHERE audit_date> @audit)
   SET @newData = 1

 IF @newData = 0 AND EXISTS( SELECT audit_date
      FROM  [dbo].[xrest_product_groups]
      WHERE audit_date> @audit)
   SET @newData = 1

SELECT @newData NewData
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [ord].[CreateInvoice]
 @orderId int,
 @nip nvarchar(15),
 @address nvarchar(max),
 @name nvarchar(max),
 @number nvarchar(max),
 @createDate smalldatetime
AS
    INSERT INTO [dbo].[xrest_orders_invoices]
               ([order_id]
               ,[nip]
               ,[address]
               ,[name]
               ,[number]
               ,[create_date])
         VALUES
               (@orderId
               ,@nip
               ,@address
               ,@name
               ,@number
               ,@createDate)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [ord].[CreateOnlineOrder]
  @header ord.NewOnlineOrderHeader readonly,
  @rows ord.NewOnlineOrderRow  readonly,
  @marketingIds dbo.inttable readonly
AS
   declare @orderId int 

	BEGIN TRY  
		BEGIN TRANSACTION

	INSERT INTO [dbo].[xrest_orders]
           ([customer_id]
           ,[restaurant_id]
           ,[create_date]
           ,[address_city]
           ,[address_street]
           ,[address_street_number]
           ,[address_house_number]
           ,[worker_id]
           ,[amount]
           ,[transport_amount]
           ,[email]
           ,[phone]
           ,[note]
           ,[status_id]
           ,[payment_type_id]
           ,[term_type_id]
           ,[term]
           ,[type_of_delivery_id]
           ,[modify_date]
           ,[restaurant_transport_id]
           ,[terms_of_use]
           ,[realization]
           ,[marketing]
           ,[audit_date]
           ,[order_day]
           ,[firstname]
           ,[discount]
           ,[source])
       SELECT TOP 1 [customer_id],
				[restaurant_id],
				[create_date],
				[address_city],
				[address_street] ,
				[address_street_number],
				[address_house_number],
				[worker_id],
				[amount],
				[transport_amount],
				[email],
				[phone],
				[note],
				[status_id],
				[payment_type_id],
				[term_type_id],
				[term],
				[type_of_delivery_id],
				[modify_date],
				[restaurant_transport_id],
				[terms_of_use],
				[realization],
				[marketing],
				[audit_date],
				[order_day],
				[firstname],
				[discount],
				[source]
		FROM  @header 

		SELECT @ORDERID = SCOPE_IDENTITY()

		INSERT INTO [dbo].[xrest_order_products]
           ([order_id]
           ,[product_id]
           ,[price]
           ,[points]
           ,[type_id]
           ,[order_product_id]
           ,[display_name]
           ,[from_source_id]
           ,[index]
           ,[note]
           ,[sub_index]
           ,[base_price]
		   ,[vat])
     SELECT @ORDERID,
            [product_id],
			[price],
			[points],
			[type_id],
			[order_product_id],
			[display_name],
			[from_source_id],
			[index],
			[note],
			[sub_index],
			[base_price],
			[vat]
	   FROM @rows

    INSERT INTO [dbo].[xrest_order_acceptance] ([order_id],[marketing_id])
        SELECT @ORDERID, id from @marketingIds
    
     COMMIT TRANSACTION
  
	 SELECT @ORDERID [OrderId], 1 [Success], NULL 'Message'
	
	END TRY  
	BEGIN CATCH  
		ROLLBACK TRANSACTION
		 SELECT NULL [OrderId], 0 [Success], ERROR_MESSAGE() 'Message'
	END CATCH; 
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [ord].[CustomerMyOrders]
	@account_id int
AS
    SELECT [orders].[id] [Id]
          ,[orders].[restaurant_id] [RestaurantId]
          ,[orders].[create_date] [CreateDate]
          ,[orders].[amount] [Amount]
          ,[orders].[status_id] [Status]
          ,[restaurant].[name] [RestaurantName]
      FROM [dbo].[xrest_orders] [orders]
      LEFT JOIN [dbo].[xrest_restaurant] [restaurant] ON [restaurant].[id] = [orders].[restaurant_id]
      where customer_id = @account_id
  
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [ord].[DriverGetOrderIds]
    @restaurnatId int,
    @driverId int,
    @orderType tinyint
AS
BEGIN
    
    DECLARE @orderDay int 
    DECLARE @status table (status_id tinyint)

    IF @orderType = 0 
	    INSERT INTO @status (status_id) VALUES(0)

    IF @orderType = 4
	    INSERT INTO @status (status_id) VALUES(7)
   
    IF @orderType = 1
	    INSERT INTO @status (status_id) VALUES(1)
  
    IF @orderType = 2
	    INSERT INTO @status (status_id) VALUES(4)
                
    IF @orderType = 3
    BEGIN
	    INSERT INTO @status (status_id) VALUES(5) 
	    INSERT INTO @status (status_id) VALUES(7) 
	    INSERT INTO @status (status_id) VALUES(3) 
    END

    SELECT @orderDay = [day_number] FROM [dbo].[xrest_restaurant] WHERE id = @restaurnatId 

    SELECT [ord].[id]
      FROM [dbo].[xrest_orders] [ord]
      JOIN @status [st] on  [st].[status_id] = [ord].[status_id]
      WHERE  [ord].[restaurant_id] = @restaurnatId
      AND [ord].[type_of_delivery_id] = 0
      AND [ord].[order_day] = @orderDay
      AND (@driverId IS NULL OR (@driverId IS NOT NULL AND [ord].driver_id = @driverId))
      ORDER BY [ord].[create_date]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [ord].[FindPosOrder]
    @billNumber int,
	@restaurantId int,
	@orderDay int,
	@cashDeskId int = null

AS
     SELECT [id] [Id]
       FROM [dbo].[xrest_pos_orders] 
     
      WHERE bill_number = @billNumber 
	    AND restaurant_id = @restaurantId
		AND order_day = @orderDay
		AND (@cashDeskId IS NULL OR cashdesk_id = @cashDeskId)

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [ord].[GetActiveRestaurant]
AS
    SELECT  [id]
    FROM [dbo].[xrest_restaurant] where active = 1
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [ord].[GetAllGroups]
AS
	 SELECT [gr].[id] [Id]
      ,[gr].[name] [Name]
      ,[gr].[audit_date] [AuditDate]
      ,[gr].[system] [System]
      ,[gr].[package] [Package]
      ,[gr].[background_color] [BackgroundColor]
      ,[gr].[text_color] [TextColor]
      ,[gr].[type] [type]
      ,[gr].[name_trans] [NameTranslations]
	  ,[img].[audit_date] [ImageAudit]
  FROM [dbo].[xrest_product_groups] [gr]
   LEFT JOIN [dbo].[xrest_images] [img] ON [img].[item_id] = [gr].[id] AND  [item_group] = 3
  
  
  SELECT [id] [Id]
      ,[product_group_id] [ProductGroupId]
      ,[restaurant_id] [RestaurantId]
      ,[index] [Index]
  FROM [dbo].[xrest_product_groups_restaurant]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [ord].[GetAllProducts]
AS

        SELECT [pr].[id] [Id],
              [pr].[display_name] [DisplayName],
              [pr].[display_name_trans] [DisplayNameTranslations],
              [pr].[name] [Name],
              [pr].[plu] [Plu],
              [pr].[price] [Price],
              [pr].[visible] [Visible],
              [pr].[vat_id] [VatId],
              [pr].[product_group_id] [ProductGroupId],
              [pr].[desription] [Description],
              [pr].[desription_trans] [DescriptionTranslations],
              [pr].[archive] [Archive],
              [pr].[points] [Points],
              [pr].[type_id] [Type],
              [pr].[audit_date] [AuditDate],
              [pr].[fiscal_print] [FiscalPrint],
              [pr].[package_id] [PackageId],
              [pr].[package] [IsPackage],
              [pr].[take_points] [TakePoints],
              [pr].[destination] [Destination],
              [pr].[background_color] [BackgroundColor],
              [pr].[text_color] [TextColor],
              [pr].[printer] [Printer],
              [vat].[name] [VatName],
              [vat].[group] [VatGroup],
              [vv].[value] [VatValue],
			  [img].[audit_date] [ImageAudit]
          FROM [dbo].[xrest_products] [pr]
     LEFT JOIN [dbo].[xrest_vat] [vat] ON [vat].[id] = [pr].[vat_id]
     LEFT JOIN [dbo].[xrest_vat_value] [vv] ON [vv].[vat_id] = [pr].[vat_id] AND [vv].[valid_to] IS NULL
	 LEFT JOIN [dbo].[xrest_images] [img] ON [img].[item_id] = [pr].[id] AND  [item_group] = 2 

        SELECT [bundle].[id] [Id],
              [bundle].[main_product_id] [MainProductId],
              [bundle].[product_id] [ProductId],
              [bundle].[price] [price],
              [bundle].[can_delete] [CanDelete],
              [bundle].[audit_date] [AuditDate],
              [bundle].[default] [Default],
              [bundle].[label] [Label],
              [bundle_label].[label_trans] [LabelTranslations],
              [bundle].[canChangePrice] [CanChagnePrice]
          FROM [dbo].[xrest_products_bundle] [bundle]
          LEFT JOIN [dbo].[xrest_products_bundle_label] [bundle_label] ON [bundle_label].[name] = [bundle].[label]

        SELECT [id] [Id],
              [product_id] [ProductId],
              [product_set_id] [ProductSetId],
              [order] [Order]
          FROM [dbo].[xrest_product_sets_product]

        SELECT [id] [Id],
              [restaurant_id] [RestaurantId],
              [product_id] [ProductId],
              [fiscal_name] [FiscalName],
              [hidden] [Hidden],
              [can_change_for_points] [CanChangeForPoints],
              [audit_date] [AuditDate],
              [day_of_week] [DayOfWeek],
              [price] [Price],
              [quick_access] [QuickAccess],
              [pos_price] [PosPrice],
              [order_name] [OrderName],
              [price_change] [PriceChange]
          FROM [dbo].[xrest_restaurant_products]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [ord].[GetAllProductSets]
AS
    SELECT [id] [Id]
          ,[type_id] [Type]
          ,[display_name] [DisplayName]
          ,[display_name_trans] [DisplayNameTranslations]
          ,[name] [Name]
          ,[audit_date] [AuditDate] 
      FROM [dbo].[xrest_product_sets]

    SELECT [id] [Id]
          ,[product_sets_id] [ProductSetsId]
          ,[product_id] [ProductId]
          ,[amount] [Amount]
          ,[order] [Order]
          ,[selected] [Selected]
          ,[audit_date] [AuditDate]
      FROM [dbo].[xrest_product_sets_item]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [ord].[GetAllVat]
AS
	 SELECT vat.[id] [Id]
      ,vat.[name] [Name]
      ,vat.[group] [Group]
	  ,vatval.value
  FROM [dbo].[xrest_vat] vat
  LEFT JOIN [dbo].[xrest_vat_value] vatval on vatval.vat_id = vat.id AND vatval.valid_to IS NULL
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [ord].[GetCurrentInvoiceNumber]
    @restaurantId int,
    @year int,
    @month int
AS
    SELECT [number] [Number] 
    from  [dbo].[xrest_restaurant_invoice_number]
    where restaurant_id = @restaurantId 
    and [year] = @year
    and [month] = @month
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [ord].[GetFavAddressForAccount]
    @accountId int
AS
    SELECT 
       [id] [Id]
      ,[account_id] [AccountId]
      ,[address_city] [AddressCity]
      ,[address_street] [AddressStreet]
      ,[address_street_number] [AddressStreetNumber]
      ,[address_house_number] [AddressHouseNumber]
      ,[default] [Default]
    FROM [dbo].[xrest_fav_addr] WHERE [account_id] = @accountId
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [ord].[GetNextInvoiceNumber]
    @restaurantId int,
    @year int,
    @month int
AS
    BEGIN TRY  
		BEGIN TRANSACTION

        MERGE  [dbo].[xrest_restaurant_invoice_number] as T
        USING  (SELECT @restaurantId [restaurant_id], @year [year], @month  [month] ) as S 
        ON T.[year] = S.[year] AND T.[restaurant_id] = S.[restaurant_id] AND T.[month] = S.[month]
        WHEN MATCHED THEN UPDATE 
            SET T.[number] = T.[number] + 1
        WHEN NOT MATCHED BY TARGET THEN 
            INSERT([restaurant_id], [year], [month]) 
            VALUES(@restaurantId, @year, @month);
       
        DECLARE @number int 
        SELECT @number = [number] FROM [dbo].[xrest_restaurant_invoice_number] 
         WHERE [restaurant_id] = @restaurantId 
         AND [month] = @month
         AND [year] =@year

	    COMMIT TRANSACTION

	    SELECT @number Number, 1 [Success], NULL 'Message'
    END TRY  
	BEGIN CATCH  
		ROLLBACK TRANSACTION
		SELECT NULL Number, 0 [Success], ERROR_MESSAGE() 'Message'
	END CATCH; 
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [ord].[GetOnlineOrderHistory]
    @orderId int
AS
BEGIN
    SELECT [id] [Id]
          ,[order_id] [OrderId]
          ,[created] [Created]
          ,[status] [Status]
          ,[info] [Info]
      FROM [dbo].[xrest_order_history] where [order_id] = @orderId
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [ord].[GetOrderPaymentInfo]
    @orderId int
AS
    SELECT 
        [payment_information] [PaymentInfo], 
        [status_id] [Status]    
    FROM [dbo].[xrest_orders] WHERE [id] = @orderId
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [ord].[GetUndeliveredReasonById]
    @id int
AS
BEGIN
	SELECT [reason] FROM [dbo].[xrest_undelivered_reason]  WHERE [id] = @id
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [ord].[GetUndeliveredReasonsBySource]
    @sourceType tinyint
AS
BEGIN
	SELECT [id] [Id]
            ,[reason] [Reason]
            ,[source_type] [SourceType]
        FROM [dbo].[xrest_undelivered_reason] WHERE @sourceType is NULL OR [source_type] = @sourceType
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [ord].[OnlineOrderAddStatusToHistory]
    @orderId int,
    @orderStatus tinyint,
    @info nvarchar(1024),
    @modifyDate datetime
AS
    INSERT INTO [dbo].[xrest_order_history]
           ([order_id]
           ,[created]
           ,[status]
           ,[info])
     VALUES
           (@orderId
           ,@modifyDate
           ,@orderStatus
           ,@info)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [ord].[OnlineOrderGetHeader]
    @orderId int
AS
    SELECT 
           [status_id] [Status],
           [restaurant_id] [RestaurantId],
           [amount] [Amount],
           [status_info] [StatusInfo],
           [modify_date] [ModifyDate]
    FROM [dbo].[xrest_orders]
     WHERE [id] = @orderId
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [ord].[OnlineOrderUpdatePaymentStatus]
    @orderId int,
    @orderStatus tinyint,
    @payed tinyint,
    @paymentPayload nvarchar(4000),
    @modifyDate datetime
AS
    UPDATE [dbo].[xrest_orders]
       SET [status_id] = @orderStatus
          ,[payment_information] = @paymentPayload
          ,[payment_status] = @payed

     WHERE [id] = @orderId
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [ord].[RemoveFavAddressForAccount]
    @accountId int,
    @id int 
AS
    DELETE FROM [dbo].[xrest_fav_addr] WHERE [account_id] = @accountId AND  [id]  = @id
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [ord].[SaveCurrentInvoiceNumber]
    @restaurantId int,
    @year int,
    @month int,
    @number int 
AS
    MERGE  [dbo].[xrest_restaurant_invoice_number] as T
    USING  [dbo].[xrest_restaurant_invoice_number] as S 
    ON S.[year] = T.[year] AND S.[restaurant_id] = t.[restaurant_id] AND S.[month] = T.[month]
    WHEN MATCHED THEN UPDATE 
        SET T.[number] = @number
    WHEN NOT MATCHED THEN 
        INSERT([restaurant_id], [year], [month], [number]) 
            VALUES(@restaurantId, @year, @month, @number);
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [ord].[SaveFavAddressForAccount]
    @id int,
	@account_id int,
	@address_city nvarchar(100),
	@address_street nvarchar(100),
	@address_street_number nvarchar(100),
	@address_house_number nvarchar(100),
	@default bit
AS

    IF @default = 1
       UPDATE [dbo].[xrest_fav_addr]  SET [default] = 0 WHERE [account_id] = @account_id AND  [id]  != @id

    MERGE [dbo].[xrest_fav_addr] as T
        USING  (SELECT @id [id] ) as S 
        ON T.[id] = S.[id] 

     WHEN MATCHED THEN UPDATE 
            SET 
				t.[account_id] = @account_id,
				t.[address_city] = @address_city,
				t.[address_street] = @address_street,
				t.[address_street_number] = @address_street_number,
				t.[address_house_number] = @address_house_number,
				t.[default] = @default
        WHEN NOT MATCHED BY TARGET THEN 
            INSERT( [account_id],
					[address_city],
					[address_street],
					[address_street_number],
					[address_house_number],
					[default]) 
            VALUES(@account_id,
					@address_city,
					@address_street,
					@address_street_number,
					@address_house_number,
                    @default);
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [ord].[SavePosPaymentHistory]
   @date datetime,
   @fromPaymentId int,
   @toPaymentId int,
   @workerId int ,
   @billNumber int
AS
BEGIN
    INSERT INTO [dbo].[xrest_pos_order_payment_history]
                       ([date]
                       ,[from_payment_id]
                       ,[to_payment_id]
                       ,[worker_id]
                       ,[billnumber])
                 VALUES
                       (@date
                       ,@fromPaymentId
                       ,@toPaymentId
                       ,@workerId
                       ,@billNumber)
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [ord].[SetDefaultFavAddressForAccount]
    @accountId int,
    @id int 
AS
    UPDATE [dbo].[xrest_fav_addr]  SET [default] = 0 WHERE [account_id] = @accountId
    UPDATE [dbo].[xrest_fav_addr]  SET [default] = 1 WHERE [account_id] = @accountId AND  [id]  = @id
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [ord].[UpdatePosOrderPayment]
    @id int,
	@paymentId int

AS
     UPDATE [dbo].[xrest_pos_orders] SET payment_type_id = @paymentId WHERE [id] = @id   
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [rest].[CheckForChangesInRestaurants]
    @audit DATETIME2
AS
   DECLARE @newData BIT = 0

 IF EXISTS( SELECT audit_date 
      FROM [dbo].[xrest_restaurant_working_hours]
      WHERE audit_date> @audit)
   SET @newData = 1

 IF @newData = 0 AND EXISTS( SELECT audit_date 
      FROM [dbo].[xrest_restaurant]
      WHERE audit_date> @audit)
   SET @newData = 1
   
 IF @newData = 0 AND EXISTS( SELECT audit_date
      FROM  [dbo].[xrest_restaurant_exclude_days]
      WHERE audit_date> @audit)
   SET @newData = 1

SELECT @newData NewData
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [rest].[CheckForChangesInTransportZones]
    @audit DATETIME2
AS
   DECLARE @newData BIT = 0

 IF EXISTS( SELECT audit_date 
      FROM [dbo].[xrest_restaurant_transport]
      WHERE audit_date> @audit)
   SET @newData = 1

 IF @newData = 0 AND EXISTS( SELECT audit_date 
      FROM [dbo].[xrest_restaurant_transport_price]
      WHERE audit_date> @audit)
   SET @newData = 1
   
 IF @newData = 0 AND EXISTS( SELECT audit_date
      FROM  [dbo].[xrest_restaurant_transport_zones]
      WHERE audit_date> @audit)
   SET @newData = 1

SELECT @newData NewData
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [rest].[GetAllPayments]
AS

    SELECT [id] [Id],
           [name] [Name],
           [order] [Ordering],
           [callcenter] [CallCenter],
           [name_trans] [NameTranslations],
           [fiscal] [Fiscal]
     FROM  [dbo].[xrest_dict_payment_types]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [rest].[GetImage]
    @itemId int,
    @itemGroup tinyint
AS
SELECT [mime] [Mime]
  FROM [dbo].[xrest_images] 
  WHERE [item_id] = @itemId 
  and  (@itemGroup is null or [item_group] = @itemGroup)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [rest].[GetRestaurantCacheData]
AS
    SELECT 
        rest.id RestaurantId,
	    rest.min_order MinOrder,
        rest.day_number [OrderDay]
      FROM [dbo].[xrest_restaurant] rest

    SELECT 
         rest.id RestaurantId,
	     resttran.id TransportZoneId,
	     resttran.name [Name],
	     resttran.plu [Plu],
	     resttranpric.delivery_price [DeliveryPrice],
	     resttranpric.from_price [FromPrice]
      FROM [dbo].[xrest_restaurant] rest
      left join [dbo].[xrest_restaurant_transport] resttran on resttran.restaurant_id = rest.id
      left join [dbo].[xrest_restaurant_transport_price] resttranpric on resttranpric.xrest_restaurant_transport_id = resttran.id
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [rest].[GetRestaurantOrderInformationById]
    @restaurantId int
AS
    SELECT [day_number] [OrderDay]
          ,[min_order] [MinOrder]
      FROM [dbo].[xrest_restaurant]
     WHERE [id] = @restaurantId
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [rest].[LoadAllTransportZones]
AS
    SELECT  [id] [Id]
          ,[restaurant_id] [RestaurantId]
          ,[name] [Name]
          ,[plu] [Plu]
          ,[audit_date] [AuditDate]
      FROM [dbo].[xrest_restaurant_transport]

       SELECT  [id] [Id]
          ,[xrest_restaurant_transport_id] [RestaurantTransportId]
          ,[delivery_price] [DeliveryPrice]
          ,[from_price] [FromPrice]
          ,[audit_date] [AuditDate]
      FROM [dbo].[xrest_restaurant_transport_price]

    SELECT DISTINCT [restaurant_transport_id] [RestaurantTransportId]
          ,[address_id] [AddressId]
          ,[even_from] [EvenFrom]
          ,[even_to] [EvenTo]
          ,[odd_from] [OddFrom]
          ,[odd_to] [OddTo]
          ,[number_from] [NumberFrom]
          ,[number_to] [NumberTo]
          ,[audit_date] [AuditDate]
      FROM [dbo].[xrest_restaurant_transport_zones]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [rest].[RestaurantCloseDay]
        @restaurantId INT
AS
BEGIN
    update [dbo].[xrest_restaurant] set [day_number] = [day_number] + 1 where [id] = @restaurantId    
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [shared].[SaveMail]
    @id int,
    @status_id tinyint,
    @template_id int,
    @tries tinyint,
    @address nvarchar(100),
    @subject nvarchar(100),
    @replacement nvarchar(max),
    @errors nvarchar(max)
AS
        MERGE  [dbo].[xrest_mails] as T
        USING  (SELECT @id [id] ) as S 
        ON T.[id] = S.[id] 
        WHEN MATCHED THEN UPDATE 
            SET  t.[status_id] = @status_id,
            t.[template_id] = @template_id,
            t.[tries] = @tries,
            t.[address] = @address,
            t.[person] = @address,
            t.[subject] = @subject,
            t.[replacement] = @replacement,
            t.[erros] = @errors
        WHEN NOT MATCHED BY TARGET THEN 
            INSERT([status_id]
           ,[template_id]
           ,[tries]
           ,[address]
           ,[person]
           ,[subject]
           ,[replacement]
           ,[erros]) 
            VALUES(@status_id, @template_id, @tries ,@address, @address, @subject, @replacement, @errors);

GO
USE [master]
GO
ALTER DATABASE [xrest_dev] SET  READ_WRITE 
GO

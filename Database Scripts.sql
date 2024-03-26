--Table creation scripts

--USE [Db21032024]
--GO

/****** Object:  Table [dbo].[Users]    Script Date: 26/03/2024 11:04:58 PM ******/
--SET ANSI_NULLS ON
--GO

--SET QUOTED_IDENTIFIER ON
--GO

CREATE TABLE [dbo].[Users](
	[ID] [int] NOT NULL,
	[Username] [varchar](60) NULL,
	[Password] [nvarchar](256) NULL,
	[IsVerified] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

 insert into Users values(1,'ashina','Hello_123',1)
-------------------------------------------------------------------------------------------------------------------------------------
CREATE TABLE [dbo].[TopUpBeneficiary](
	[TopUpBeneficiaryId] [int] IDENTITY(1,1) NOT NULL,
	[Nickname] [varchar](20) NULL,
	[UserId] [int] NULL,
	[BeneficiaryName] [varchar](255) NULL,
	[BeneficiaryPhoneNumber] [varchar](15) NULL,
PRIMARY KEY CLUSTERED 
(
	[TopUpBeneficiaryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[TopUpBeneficiary]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([ID])
GO
---------------------------------------------------------------------------------------------------------------------------------------

CREATE TABLE [dbo].[TopUpOption](
	[TopUpOptionId] [int] NOT NULL,
	[Amount] [decimal](18, 0) NULL,
PRIMARY KEY CLUSTERED 
(
	[TopUpOptionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

 insert Into TopUpOption values (1,5)
  insert Into TopUpOption values (2,10)
   insert Into TopUpOption values (3,20)
    insert Into TopUpOption values (4,30)
    insert Into TopUpOption values (5,50)
    insert Into TopUpOption values (6,75)
    insert Into TopUpOption values (7,100)



--------------------------------------------------------------------------------------------------------------------------------------

CREATE TABLE [dbo].[TopUpTransaction](
	[TopUpTransactionId] [int] IDENTITY(1,1) NOT NULL,
	[TubID] [int] NULL,
	[UserId] [int] NULL,
	[Amount] [decimal](18, 0) NULL,
	[Transactiondate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[TopUpTransactionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[TopUpTransaction]  WITH CHECK ADD FOREIGN KEY([TubID])
REFERENCES [dbo].[TopUpBeneficiary] ([TopUpBeneficiaryId])
GO

ALTER TABLE [dbo].[TopUpTransaction]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([ID])
GO

--------------------------------------------------------------------------------------------------------------------------------------

CREATE TABLE [dbo].[TopUpTransactionResult](
	[TopUpTransactionResultId] [int] IDENTITY(1,1) NOT NULL,
	[success] [bit] NULL,
	[message] [varchar](max) NULL,
	[NewBalance] [decimal](18, 0) NULL,
PRIMARY KEY CLUSTERED 
(
	[TopUpTransactionResultId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

--------------------------------------------------------------------------------------------------------------------------------------
CREATE TABLE [dbo].[BalanceInformation](
	[BalanceInfoId] [int] IDENTITY(1,1) NOT NULL,
	[Balance] [decimal](18, 0) NULL,
	[UserId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[BalanceInfoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


 insert into BalanceInformation values (6000,1)
  insert into BalanceInformation values (200,2)
   insert into BalanceInformation values (150,3)







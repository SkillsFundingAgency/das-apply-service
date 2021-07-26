CREATE TABLE [dbo].[FinancialData]
(
	[Id] BIGINT NOT NULL IDENTITY(1,1),
	[ApplicationId] UNIQUEIDENTIFIER NOT NULL,
	TurnOver BIGINT NULL,
	Depreciation BIGINT NULL,
	ProfitLoss BIGINT NULL,
	Dividends BIGINT NULL,
	IntangibleAssets BIGINT NULL,
	Assets BIGINT NULL,
	Liabilities BIGINT NULL,
	ShareholderFunds BIGINT NULL,
	Borrowings BIGINT NULL,
	AccountingReferenceDate DATE NULL,
	AccountingPeriod TINYINT NULL,
	AverageNumberofFTEEmployees BIGINT NULL, 
    CONSTRAINT PK_FinancialData PRIMARY KEY CLUSTERED ([Id] ASC)
)
GO
CREATE INDEX [IX_FinancialData_ApplicationId] ON [FinancialData] ([ApplicationId])
GO
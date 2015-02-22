CREATE TABLE [dbo].[Order] (
    [Id]     INT           IDENTITY (1, 1) NOT NULL,
    [Date]   DATETIME      NOT NULL,
    [Number] INT           NOT NULL,
    [Text]   NVARCHAR (50) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
CREATE TABLE [Tenders] (
    [Id] uniqueidentifier NOT NULL,
    [Title] nvarchar(max) NOT NULL,
    [Description] nvarchar(max) NOT NULL,
    [Deadline] datetime2 NOT NULL,
    [BudgetAmount] decimal(18,2) NOT NULL,
    [BudgetCurrency] nvarchar(max) NOT NULL,
    [State] int NOT NULL,
    [DocumentPath] nvarchar(max) NULL,
    CONSTRAINT [PK_Tenders] PRIMARY KEY ([Id])
);

CREATE TABLE [Users] (
    [Id] uniqueidentifier NOT NULL,
    [FullName] nvarchar(max) NOT NULL,
    [Email] nvarchar(max) NOT NULL,
    [PasswordHash] nvarchar(max) NOT NULL,
    [Role] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY ([Id])
);

CREATE TABLE [Bids] (
    [Id] uniqueidentifier NOT NULL,
    [TenderId] uniqueidentifier NOT NULL,
    [BidderId] uniqueidentifier NOT NULL,
    [SubmittedAt] datetime2 NOT NULL,
    [Amount] decimal(18,2) NOT NULL,
    [SupportingDocuments] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Bids] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Bids_Tenders_TenderId] FOREIGN KEY ([TenderId]) REFERENCES [Tenders] ([Id]) ON DELETE CASCADE
);

CREATE INDEX [IX_Bids_TenderId] ON [Bids] ([TenderId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250412013108_InitialCreate', N'9.0.4');

COMMIT;
GO


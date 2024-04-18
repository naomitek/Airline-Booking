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
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240416224835_Initial'
)
BEGIN
    IF SCHEMA_ID(N'Identity') IS NULL EXEC(N'CREATE SCHEMA [Identity];');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240416224835_Initial'
)
BEGIN
    CREATE TABLE [Identity].[BookingFlights] (
        [BookingFlightId] int NOT NULL IDENTITY,
        [FlightId] int NOT NULL,
        [PassengerId] int NOT NULL,
        [UserId] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_BookingFlights] PRIMARY KEY ([BookingFlightId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240416224835_Initial'
)
BEGIN
    CREATE TABLE [Identity].[CarBookings] (
        [CarBookingId] int NOT NULL IDENTITY,
        [CarId] int NOT NULL,
        [NameOfHolder] nvarchar(max) NOT NULL,
        [PhoneOfHolder] nvarchar(max) NOT NULL,
        [UserId] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_CarBookings] PRIMARY KEY ([CarBookingId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240416224835_Initial'
)
BEGIN
    CREATE TABLE [Identity].[Cars] (
        [CarId] int NOT NULL IDENTITY,
        [CarCompany] nvarchar(max) NOT NULL,
        [Model] nvarchar(max) NOT NULL,
        [OrigenLocation] nvarchar(max) NOT NULL,
        [DestinationLocation] nvarchar(max) NOT NULL,
        [Pricing] decimal(18,2) NOT NULL,
        [DepartureTime] datetime2 NOT NULL,
        [ArrivalTime] datetime2 NOT NULL,
        [LimitPassengers] int NOT NULL,
        [Available] bit NOT NULL,
        [Url] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Cars] PRIMARY KEY ([CarId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240416224835_Initial'
)
BEGIN
    CREATE TABLE [Identity].[Carts] (
        [ProductId] int NOT NULL IDENTITY,
        [ProductType] nvarchar(max) NOT NULL,
        [FlightId] int NULL,
        [CarId] int NULL,
        [RoomId] int NULL,
        [Price] decimal(18,2) NOT NULL,
        [UserId] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Carts] PRIMARY KEY ([ProductId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240416224835_Initial'
)
BEGIN
    CREATE TABLE [Identity].[Flights] (
        [FlightId] int NOT NULL IDENTITY,
        [Airlines] nvarchar(max) NOT NULL,
        [Origen] nvarchar(max) NOT NULL,
        [Destination] nvarchar(max) NOT NULL,
        [MaxSeat] int NOT NULL,
        [NumOfPassengers] int NOT NULL,
        [DepartureTime] datetime2 NOT NULL,
        [ArrivalTime] datetime2 NOT NULL,
        [Price] decimal(18,2) NOT NULL,
        CONSTRAINT [PK_Flights] PRIMARY KEY ([FlightId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240416224835_Initial'
)
BEGIN
    CREATE TABLE [Identity].[Passengers] (
        [PassengerId] int NOT NULL IDENTITY,
        [FirstName] nvarchar(max) NOT NULL,
        [LastName] nvarchar(max) NOT NULL,
        [Email] nvarchar(max) NOT NULL,
        [Phone] nvarchar(max) NOT NULL,
        [Address] nvarchar(max) NOT NULL,
        [Birth] datetime2 NOT NULL,
        [Citizen] nvarchar(max) NOT NULL,
        [Passport] nvarchar(max) NOT NULL,
        [Gender] nvarchar(max) NOT NULL,
        [EmergencyContactName] nvarchar(max) NULL,
        [EmergencyContactPhone] nvarchar(max) NULL,
        CONSTRAINT [PK_Passengers] PRIMARY KEY ([PassengerId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240416224835_Initial'
)
BEGIN
    CREATE TABLE [Identity].[Role] (
        [Id] nvarchar(450) NOT NULL,
        [Name] nvarchar(256) NULL,
        [NormalizedName] nvarchar(256) NULL,
        [ConcurrencyStamp] nvarchar(max) NULL,
        CONSTRAINT [PK_Role] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240416224835_Initial'
)
BEGIN
    CREATE TABLE [Identity].[RoomBookings] (
        [RoomBookingId] int NOT NULL IDENTITY,
        [RoomId] int NOT NULL,
        [NameOfHolder] nvarchar(max) NOT NULL,
        [PhoneOfHolder] nvarchar(max) NOT NULL,
        [UserId] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_RoomBookings] PRIMARY KEY ([RoomBookingId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240416224835_Initial'
)
BEGIN
    CREATE TABLE [Identity].[Rooms] (
        [RoomId] int NOT NULL IDENTITY,
        [HotelName] nvarchar(max) NOT NULL,
        [Location] nvarchar(max) NOT NULL,
        [Checking] datetime2 NOT NULL,
        [Checkout] datetime2 NOT NULL,
        [PricePerNight] decimal(18,2) NOT NULL,
        [NumOfGuests] int NOT NULL,
        [MaxGuests] int NOT NULL,
        [NumberOfBeds] int NOT NULL,
        [Rating] int NOT NULL,
        [PetFriendly] bit NOT NULL,
        [AirConditioning] bit NOT NULL,
        [Wifi] bit NOT NULL,
        [Url] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Rooms] PRIMARY KEY ([RoomId])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240416224835_Initial'
)
BEGIN
    CREATE TABLE [Identity].[User] (
        [Id] nvarchar(450) NOT NULL,
        [FirstName] nvarchar(max) NOT NULL,
        [LastName] nvarchar(max) NOT NULL,
        [UsernameChangeLimit] int NOT NULL,
        [ProfilePicture] varbinary(max) NULL,
        [UserName] nvarchar(256) NULL,
        [NormalizedUserName] nvarchar(256) NULL,
        [Email] nvarchar(256) NULL,
        [NormalizedEmail] nvarchar(256) NULL,
        [EmailConfirmed] bit NOT NULL,
        [PasswordHash] nvarchar(max) NULL,
        [SecurityStamp] nvarchar(max) NULL,
        [ConcurrencyStamp] nvarchar(max) NULL,
        [PhoneNumber] nvarchar(max) NULL,
        [PhoneNumberConfirmed] bit NOT NULL,
        [TwoFactorEnabled] bit NOT NULL,
        [LockoutEnd] datetimeoffset NULL,
        [LockoutEnabled] bit NOT NULL,
        [AccessFailedCount] int NOT NULL,
        CONSTRAINT [PK_User] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240416224835_Initial'
)
BEGIN
    CREATE TABLE [Identity].[RoleClaims] (
        [Id] int NOT NULL IDENTITY,
        [RoleId] nvarchar(450) NOT NULL,
        [ClaimType] nvarchar(max) NULL,
        [ClaimValue] nvarchar(max) NULL,
        CONSTRAINT [PK_RoleClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_RoleClaims_Role_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Identity].[Role] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240416224835_Initial'
)
BEGIN
    CREATE TABLE [Identity].[UserClaims] (
        [Id] int NOT NULL IDENTITY,
        [UserId] nvarchar(450) NOT NULL,
        [ClaimType] nvarchar(max) NULL,
        [ClaimValue] nvarchar(max) NULL,
        CONSTRAINT [PK_UserClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_UserClaims_User_UserId] FOREIGN KEY ([UserId]) REFERENCES [Identity].[User] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240416224835_Initial'
)
BEGIN
    CREATE TABLE [Identity].[UserLogins] (
        [LoginProvider] nvarchar(450) NOT NULL,
        [ProviderKey] nvarchar(450) NOT NULL,
        [ProviderDisplayName] nvarchar(max) NULL,
        [UserId] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_UserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
        CONSTRAINT [FK_UserLogins_User_UserId] FOREIGN KEY ([UserId]) REFERENCES [Identity].[User] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240416224835_Initial'
)
BEGIN
    CREATE TABLE [Identity].[UserRoles] (
        [UserId] nvarchar(450) NOT NULL,
        [RoleId] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_UserRoles] PRIMARY KEY ([UserId], [RoleId]),
        CONSTRAINT [FK_UserRoles_Role_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Identity].[Role] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_UserRoles_User_UserId] FOREIGN KEY ([UserId]) REFERENCES [Identity].[User] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240416224835_Initial'
)
BEGIN
    CREATE TABLE [Identity].[UserTokens] (
        [UserId] nvarchar(450) NOT NULL,
        [LoginProvider] nvarchar(450) NOT NULL,
        [Name] nvarchar(450) NOT NULL,
        [Value] nvarchar(max) NULL,
        CONSTRAINT [PK_UserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
        CONSTRAINT [FK_UserTokens_User_UserId] FOREIGN KEY ([UserId]) REFERENCES [Identity].[User] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240416224835_Initial'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [RoleNameIndex] ON [Identity].[Role] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240416224835_Initial'
)
BEGIN
    CREATE INDEX [IX_RoleClaims_RoleId] ON [Identity].[RoleClaims] ([RoleId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240416224835_Initial'
)
BEGIN
    CREATE INDEX [EmailIndex] ON [Identity].[User] ([NormalizedEmail]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240416224835_Initial'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [UserNameIndex] ON [Identity].[User] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL');
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240416224835_Initial'
)
BEGIN
    CREATE INDEX [IX_UserClaims_UserId] ON [Identity].[UserClaims] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240416224835_Initial'
)
BEGIN
    CREATE INDEX [IX_UserLogins_UserId] ON [Identity].[UserLogins] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240416224835_Initial'
)
BEGIN
    CREATE INDEX [IX_UserRoles_RoleId] ON [Identity].[UserRoles] ([RoleId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240416224835_Initial'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240416224835_Initial', N'8.0.4');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240416230250_NombreDeLaMigracion'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240416230250_NombreDeLaMigracion', N'8.0.4');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240416233644_mssql_migration_144'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240416233644_mssql_migration_144', N'8.0.4');
END;
GO

COMMIT;
GO


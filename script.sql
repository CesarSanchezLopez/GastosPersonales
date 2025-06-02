IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;


BEGIN TRANSACTION;


CREATE TABLE [AspNetRoles] (
    [Id] nvarchar(450) NOT NULL,
    [Name] nvarchar(256) NULL,
    [NormalizedName] nvarchar(256) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
);


CREATE TABLE [AspNetUsers] (
    [Id] nvarchar(450) NOT NULL,
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
    CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id])
);


CREATE TABLE [AspNetRoleClaims] (
    [Id] int NOT NULL IDENTITY,
    [RoleId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
);


CREATE TABLE [AspNetUserClaims] (
    [Id] int NOT NULL IDENTITY,
    [UserId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);


CREATE TABLE [AspNetUserLogins] (
    [LoginProvider] nvarchar(128) NOT NULL,
    [ProviderKey] nvarchar(128) NOT NULL,
    [ProviderDisplayName] nvarchar(max) NULL,
    [UserId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
    CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);


CREATE TABLE [AspNetUserRoles] (
    [UserId] nvarchar(450) NOT NULL,
    [RoleId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
    CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);


CREATE TABLE [AspNetUserTokens] (
    [UserId] nvarchar(450) NOT NULL,
    [LoginProvider] nvarchar(128) NOT NULL,
    [Name] nvarchar(128) NOT NULL,
    [Value] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
    CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);


CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);


CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL;


CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);


CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);


CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);


CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);


CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL;


INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'00000000000000_CreateIdentitySchema', N'7.0.20');


COMMIT;


BEGIN TRANSACTION;


INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250531214946_InitialIdentity', N'7.0.20');


COMMIT;


BEGIN TRANSACTION;


CREATE TABLE [FondosMonetarios] (
    [Id] int NOT NULL IDENTITY,
    [Nombre] nvarchar(max) NOT NULL,
    [Descripcion] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_FondosMonetarios] PRIMARY KEY ([Id])
);


CREATE TABLE [TiposGasto] (
    [Id] int NOT NULL IDENTITY,
    [Codigo] nvarchar(max) NOT NULL,
    [Descripcion] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_TiposGasto] PRIMARY KEY ([Id])
);


CREATE TABLE [Depositos] (
    [Id] int NOT NULL IDENTITY,
    [Fecha] datetime2 NOT NULL,
    [FondoMonetarioId] int NOT NULL,
    [Monto] decimal(18,2) NOT NULL,
    [UserId] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Depositos] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Depositos_FondosMonetarios_FondoMonetarioId] FOREIGN KEY ([FondoMonetarioId]) REFERENCES [FondosMonetarios] ([Id]) ON DELETE CASCADE
);


CREATE TABLE [RegistroGastoEncabezados] (
    [Id] int NOT NULL IDENTITY,
    [Fecha] datetime2 NOT NULL,
    [FondoMonetarioId] int NOT NULL,
    [Observaciones] nvarchar(max) NOT NULL,
    [NombreComercio] nvarchar(max) NOT NULL,
    [TipoDocumento] nvarchar(max) NOT NULL,
    [UserId] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_RegistroGastoEncabezados] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_RegistroGastoEncabezados_FondosMonetarios_FondoMonetarioId] FOREIGN KEY ([FondoMonetarioId]) REFERENCES [FondosMonetarios] ([Id]) ON DELETE CASCADE
);


CREATE TABLE [Presupuestos] (
    [Id] int NOT NULL IDENTITY,
    [UserId] nvarchar(max) NOT NULL,
    [TipoGastoId] int NOT NULL,
    [Monto] decimal(18,2) NOT NULL,
    [Mes] int NOT NULL,
    [Año] int NOT NULL,
    CONSTRAINT [PK_Presupuestos] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Presupuestos_TiposGasto_TipoGastoId] FOREIGN KEY ([TipoGastoId]) REFERENCES [TiposGasto] ([Id]) ON DELETE CASCADE
);


CREATE TABLE [RegistroGastoDetalles] (
    [Id] int NOT NULL IDENTITY,
    [RegistroGastoEncabezadoId] int NOT NULL,
    [TipoGastoId] int NOT NULL,
    [Monto] decimal(18,2) NOT NULL,
    CONSTRAINT [PK_RegistroGastoDetalles] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_RegistroGastoDetalles_RegistroGastoEncabezados_RegistroGastoEncabezadoId] FOREIGN KEY ([RegistroGastoEncabezadoId]) REFERENCES [RegistroGastoEncabezados] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_RegistroGastoDetalles_TiposGasto_TipoGastoId] FOREIGN KEY ([TipoGastoId]) REFERENCES [TiposGasto] ([Id]) ON DELETE CASCADE
);


CREATE INDEX [IX_Depositos_FondoMonetarioId] ON [Depositos] ([FondoMonetarioId]);


CREATE INDEX [IX_Presupuestos_TipoGastoId] ON [Presupuestos] ([TipoGastoId]);


CREATE INDEX [IX_RegistroGastoDetalles_RegistroGastoEncabezadoId] ON [RegistroGastoDetalles] ([RegistroGastoEncabezadoId]);


CREATE INDEX [IX_RegistroGastoDetalles_TipoGastoId] ON [RegistroGastoDetalles] ([TipoGastoId]);


CREATE INDEX [IX_RegistroGastoEncabezados_FondoMonetarioId] ON [RegistroGastoEncabezados] ([FondoMonetarioId]);


INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250531222116_InitialModels', N'7.0.20');


COMMIT;


BEGIN TRANSACTION;


INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250531222507_AjusteDecimales', N'7.0.20');


COMMIT;



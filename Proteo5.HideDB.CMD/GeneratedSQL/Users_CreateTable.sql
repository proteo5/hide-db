-- Tabla generada automáticamente para Users
-- Version: 1.0 | Entity Version: 1
-- Generado: 2025-08-10 00:19:19

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Users' AND xtype='U')
BEGIN
    CREATE TABLE [Users] (
[Id] INT IDENTITY(1,1) NOT NULL,
[Username] NVARCHAR(50)  NOT NULL,
[PasswordHash] NVARCHAR(255)  NOT NULL,
[Email] NVARCHAR(100)  NOT NULL,
[FirstName] NVARCHAR(50)  NULL,
[LastName] NVARCHAR(50)  NULL,
[status] NVARCHAR(MAX)  NULL DEFAULT 'active',
[CreatedAt] DATETIME2  NOT NULL DEFAULT GETUTCDATE(),
[UpdatedAt] DATETIME2  NOT NULL DEFAULT GETUTCDATE()
,
        CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([Id])
    );
END

-- Catálogos disponibles:
-- statuses: active, inactive, banned

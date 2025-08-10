-- Tabla generada automáticamente para Roles
-- Version: 1.0 | Entity Version: 1
-- Generado: 2025-08-10 01:46:32

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Roles' AND xtype='U')
BEGIN
    CREATE TABLE [Roles] (
[Id] INT IDENTITY(1,1) NOT NULL,
[Name] NVARCHAR(50)  NOT NULL,
[Description] NVARCHAR(255)  NULL,
[Status] NVARCHAR(20)  NOT NULL DEFAULT 'active',
[CreatedAt] DATETIME2  NOT NULL DEFAULT GETUTCDATE(),
[UpdatedAt] DATETIME2  NOT NULL DEFAULT GETUTCDATE()
,
        CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED ([Id])
    );
END

-- Catálogos disponibles:
-- statuses: active, inactive, pending, archived

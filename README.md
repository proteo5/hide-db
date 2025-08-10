# ?? Proteo5.HideDB - YAML DSL Generator

**Generador de c�digo autom�tico basado en DSL YAML para entidades de base de datos**

[![.NET](https://img.shields.io/badge/.NET-8.0-blue.svg)](https://dotnet.microsoft.com/download/dotnet/8.0)
[![C#](https://img.shields.io/badge/C%23-13.0-green.svg)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)

## ?? Descripci�n

Proteo5.HideDB es una herramienta de generaci�n de c�digo que convierte definiciones YAML de entidades en c�digo C# completamente funcional, incluyendo modelos, repositorios, interfaces y scripts SQL. Utiliza un DSL (Domain Specific Language) YAML intuitivo y potente para definir entidades de base de datos y generar autom�ticamente todo el c�digo necesario para operaciones CRUD.

## ? Caracter�sticas Principales

- ?? **Generaci�n Autom�tica de C�digo**: Modelos, repositorios, interfaces y SQL
- ?? **DSL YAML Intuitivo**: Sintaxis clara y expresiva para definir entidades
- ?? **File Watcher**: Regeneraci�n autom�tica al modificar archivos YAML
- ??? **Type Safety**: Soporte completo para nullable reference types de C# 8+
- ??? **Multi-Database**: Soporte para SQL Server, PostgreSQL, MySQL, SQLite y Oracle
- ?? **M�todos Async/Sync**: Generaci�n de m�todos s�ncronos y as�ncronos
- ?? **Data Annotations**: Integraci�n con Entity Framework DataAnnotations
- ?? **SQL Personalizado**: Soporte para consultas SQL personalizadas
- ?? **Cat�logos/Enums**: Generaci�n autom�tica de enumeraciones
- ? **Alta Performance**: C�digo optimizado y libre de warnings

## ??? Arquitectura

```
Proteo5.HideDB/
??? Proteo5.HideDB.Lib/           # Librer�a principal
?   ??? Configuration/            # Configuraci�n del generador
?   ??? Generators/               # Generadores de c�digo
?   ?   ??? YamlDslGenerator.cs   # Generador principal
?   ?   ??? ModelGenerator.cs     # Generador de modelos
?   ?   ??? RepositoryGenerator.cs # Generador de repositorios
?   ?   ??? SqlGenerator.cs       # Generador de SQL
?   ?   ??? EnumGenerator.cs      # Generador de enums
?   ??? Models/                   # Modelos de definici�n
?   ??? Utils/                    # Utilidades y helpers
??? Proteo5.HideDB.CMD/           # Aplicaci�n de consola
?   ??? Entities/                 # Definiciones YAML
?   ??? GeneratedCode/            # C�digo generado
?   ??? GeneratedSQL/             # Scripts SQL generados
??? README.md                     # Este archivo
```

## ?? Inicio R�pido

### Prerrequisitos

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) o superior
- Base de datos (SQL Server, PostgreSQL, MySQL, SQLite u Oracle)

### Instalaci�n

1. **Clonar el repositorio:**
   ```bash
   git clone https://github.com/tu-usuario/hide-db.git
   cd hide-db
   ```

2. **Restaurar dependencias:**
   ```bash
   dotnet restore
   ```

3. **Configurar base de datos:**
   Edita `Proteo5.HideDB.CMD/appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": {
         "Type": "SqlServer",
         "ConnectionString": "Server=localhost;Database=MiDB;Trusted_Connection=true;"
       }
     }
   }
   ```

### Uso B�sico

1. **Ejecutar el generador:**
   ```bash
   cd Proteo5.HideDB.CMD
   dotnet run
   ```

2. **Comandos disponibles:**
   ```bash
   dotnet run                # Modo file watcher
   dotnet run test          # Generar c�digo y mostrar ejemplos
   dotnet run directtest    # Ejecutar test de la librer�a
   ```

## ?? Sintaxis YAML

### Ejemplo B�sico: Users.yaml

```yaml
# Definici�n de entidad Users
entity: "Users"
entityversion: "1"
version: "1.0"
description: "Entidad para gesti�n de usuarios del sistema"

fields:
  - name: "Id"
    type: "int"
    primaryKey: true
    autoIncrement: true
    required: true
    description: "Identificador �nico del usuario"
    
  - name: "Username"
    type: "string"
    maxLength: 50
    required: true
    description: "Nombre de usuario �nico"
    
  - name: "Email"
    type: "string"
    maxLength: 100
    required: true
    description: "Direcci�n de correo electr�nico"
    
  - name: "FirstName"
    type: "string"
    maxLength: 50
    nullable: true
    description: "Nombre del usuario"
    
  - name: "Status"
    type: "string"
    defaultValue: "active"
    catalog: "statuses"
    description: "Estado del usuario"

catalogs:
  statuses:
    - name: "active"
      description: "Usuario activo"
    - name: "inactive"
      description: "Usuario inactivo"

statements:
  - name: "Insert"
    type: "Insert"
    return: "nothing"
    description: "Crea un nuevo usuario"
    sql: |
      INSERT INTO Users (Username, Email, FirstName, Status)
      VALUES (@Username, @Email, @FirstName, @Status);
      
  - name: "GetById"
    type: "Select"
    return: "one"
    description: "Obtiene un usuario por ID"
    sql: |
      SELECT * FROM Users WHERE Id = @Id;
```

### Tipos de Datos Soportados

| Tipo YAML | Tipo C# | Tipo SQL Server | Descripci�n |
|-----------|---------|-----------------|-------------|
| `int` | `int` | `INT` | Entero 32-bit |
| `long` | `long` | `BIGINT` | Entero 64-bit |
| `string` | `string` | `NVARCHAR` | Cadena de texto |
| `datetime` | `DateTime` | `DATETIME2` | Fecha y hora |
| `bool` | `bool` | `BIT` | Booleano |
| `decimal` | `decimal` | `DECIMAL(18,2)` | Decimal de precisi�n |
| `double` | `double` | `FLOAT` | Punto flotante |
| `guid` | `Guid` | `UNIQUEIDENTIFIER` | Identificador �nico |

### Atributos de Campo

- **`name`**: Nombre del campo (requerido)
- **`type`**: Tipo de dato (requerido)
- **`primaryKey`**: Es clave primaria (boolean)
- **`autoIncrement`**: Auto-incremento (boolean)
- **`required`**: Campo requerido (boolean)
- **`nullable`**: Permite valores nulos (boolean)
- **`maxLength`**: Longitud m�xima para strings
- **`defaultValue`**: Valor por defecto
- **`catalog`**: Referencia a cat�logo/enum
- **`description`**: Descripci�n del campo

## ?? Configuraci�n

### appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": {
      "Type": "SqlServer",
      "ConnectionString": "Server=localhost;Database=MiDB;Trusted_Connection=true;"
    }
  },
  "Generator": {
    "YamlPath": "./Entities",
    "OutputPath": "./GeneratedCode",
    "SqlOutputPath": "./GeneratedSQL",
    "Namespace": "MiProyecto.Generated",
    "Provider": "SqlServer",
    "GenerateInterfaces": true,
    "GenerateAsync": true,
    "GenerateSync": true,
    "AddDataAnnotations": true
  }
}
```

### Opciones de Configuraci�n

- **`YamlPath`**: Directorio de archivos YAML
- **`OutputPath`**: Directorio de c�digo generado
- **`SqlOutputPath`**: Directorio de scripts SQL
- **`Namespace`**: Namespace del c�digo generado
- **`Provider`**: Proveedor de base de datos (SqlServer, PostgreSQL, MySQL, SQLite, Oracle)
- **`GenerateInterfaces`**: Generar interfaces de repositorios
- **`GenerateAsync`**: Generar m�todos as�ncronos
- **`GenerateSync`**: Generar m�todos s�ncronos
- **`AddDataAnnotations`**: A�adir DataAnnotations a modelos

## ?? C�digo Generado

Para cada entidad definida en YAML, se generan los siguientes archivos:

### 1. Modelo (UsersModel.cs)
```csharp
[Table("Users")]
public class UsersModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Username { get; set; } = string.Empty;
    
    [MaxLength(50)]
    public string? FirstName { get; set; }
    
    // ... m�s propiedades
}
```

### 2. Interfaz del Repositorio (IUsersRepository.cs)
```csharp
public interface IUsersRepository
{
    Task<int> InsertAsync(string username, string email, string? firstName);
    Task<UsersModel?> GetByIdAsync(int id);
    Task<List<UsersModel>> GetAllAsync();
    Task<int> UpdateAsync(string username, string email, int id);
    Task<int> DeleteByIdAsync(int id);
    // ... m�s m�todos
}
```

### 3. Implementaci�n del Repositorio (UsersRepository.cs)
```csharp
public class UsersRepository : IUsersRepository
{
    private readonly string _connectionString;
    
    public UsersRepository(string connectionString)
    {
        _connectionString = connectionString;
    }
    
    public async Task<int> InsertAsync(string username, string email, string? firstName)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand(/* SQL generado */, connection);
        // ... implementaci�n
    }
    
    // ... m�s m�todos implementados
}
```

### 4. Enumeraciones (UsersEnums.cs)
```csharp
public enum UsersStatuses
{
    [Description("Usuario activo")]
    Active = 0,
    
    [Description("Usuario inactivo")]
    Inactive = 1
}
```

### 5. Script SQL (Users_CreateTable.sql)
```sql
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Users' AND xtype='U')
BEGIN
    CREATE TABLE [Users] (
        [Id] INT IDENTITY(1,1) NOT NULL,
        [Username] NVARCHAR(50) NOT NULL,
        [Email] NVARCHAR(100) NOT NULL,
        [FirstName] NVARCHAR(50) NULL,
        [Status] NVARCHAR(50) NULL DEFAULT 'active',
        [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        [UpdatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([Id])
    );
END
```

## ?? Testing

### Ejecutar Tests

```bash
# Test interactivo con pausas para verificaci�n manual
dotnet run directtest

# Generar c�digo y ejemplos de test
dotnet run test
```

### Test Features

- ? Creaci�n autom�tica de tablas
- ? Inserci�n de datos de prueba
- ? Consultas y b�squedas
- ? Actualizaciones con timestamps
- ? Eliminaci�n de datos
- ? Limpieza autom�tica
- ? Pausas interactivas para verificaci�n manual
- ? Comandos SQL sugeridos para verificaci�n

## ?? Estad�sticas del Proyecto

- **Lenguaje Principal**: C# 13.0
- **Framework**: .NET 8.0
- **Dependencias Principales**:
  - YamlDotNet 16.3.0
  - Microsoft.Data.SqlClient 6.0.2
  - Microsoft.Extensions.* 9.0.8
- **Arquitectura**: Clean Architecture
- **Patrones**: Repository, Factory, Builder
- **Testing**: Unit Tests + Integration Tests

## ?? Contribuir

1. Fork el proyecto
2. Crear feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit cambios (`git commit -m 'Add AmazingFeature'`)
4. Push al branch (`git push origin feature/AmazingFeature`)
5. Abrir Pull Request

### Convenciones de C�digo

- Usar C# 13.0 features
- Seguir convenciones de nomenclatura .NET
- Documentar m�todos p�blicos con XML comments
- Incluir tests para nuevas funcionalidades
- Mantener cobertura de tests > 80%

## ?? Licencia

Este proyecto est� licenciado bajo la Licencia MIT - ver el archivo [LICENSE](LICENSE) para m�s detalles.

## ?? Agradecimientos

- [YamlDotNet](https://github.com/aaubry/YamlDotNet) - Serializaci�n YAML
- [Microsoft.Extensions](https://github.com/dotnet/extensions) - Dependency Injection y Configuration
- [.NET Community](https://dotnet.microsoft.com/platform/community) - Inspiraci�n y mejores pr�cticas

## ?? Soporte

- ?? **Issues**: [GitHub Issues](https://github.com/tu-usuario/hide-db/issues)
- ?? **Discusiones**: [GitHub Discussions](https://github.com/tu-usuario/hide-db/discussions)
- ?? **Email**: tu-email@ejemplo.com

---

<div align="center">
  <strong>Hecho con ?? para la comunidad .NET</strong>
</div>
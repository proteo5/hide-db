# Proteo5.HideDB - YAML DSL Generator

**Automatic code generator based on YAML DSL for database entities**

[![.NET](https://img.shields.io/badge/.NET-8.0-blue.svg)](https://dotnet.microsoft.com/download/dotnet/8.0)
[![C#](https://img.shields.io/badge/C%23-13.0-green.svg)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)
[![Build Status](https://img.shields.io/badge/build-passing-brightgreen.svg)](https://github.com)
[![Tests](https://img.shields.io/badge/tests-passing-brightgreen.svg)](https://github.com)

## Description

Proteo5.HideDB is a code generation tool that converts YAML entity definitions into fully functional C# code, including models, repositories, interfaces, and SQL scripts. It uses an intuitive and powerful YAML DSL (Domain Specific Language) to define database entities and automatically generate all the necessary code for CRUD operations.

## Key Features

- **Automatic Code Generation**: Models, repositories, interfaces, and SQL
- **Intuitive YAML DSL**: Clear and expressive syntax for defining entities
- **File Watcher**: Automatic regeneration when modifying YAML files
- **Type Safety**: Full support for C# 8+ nullable reference types
- **Multi-Database**: Support for SQL Server, PostgreSQL, MySQL, SQLite, and Oracle
- **Async/Sync Methods**: Generation of both synchronous and asynchronous methods
- **Data Annotations**: Integration with Entity Framework DataAnnotations
- **Custom SQL**: Support for custom SQL queries
- **Catalogs/Enums**: Automatic generation of enumerations
- **High Performance**: Optimized and warning-free code

## Architecture

```
Proteo5.HideDB/
??? Proteo5.HideDB.Lib/           # Core Library
?   ??? Configuration/            # Generator configuration
?   ?   ??? GeneratorConfig.cs    # Main configuration class
?   ??? Generators/               # Code generators
?   ?   ??? YamlDslGenerator.cs   # Main generator (File watcher)
?   ?   ??? ModelGenerator.cs     # Entity models generator
?   ?   ??? RepositoryGenerator.cs # Repository pattern generator
?   ?   ??? SqlGenerator.cs       # SQL DDL scripts generator
?   ?   ??? EnumGenerator.cs      # Enumerations generator
?   ??? Models/                   # Definition models
?   ?   ??? EntityDefinition.cs   # YAML entity structure
?   ?   ??? FieldDefinition.cs    # Field definitions
?   ?   ??? StatementDefinition.cs # SQL statement definitions
?   ??? Utils/                    # Utilities and helpers
?       ??? TypeMapper.cs         # YAML to C#/SQL type mapping
?       ??? SqlParameterExtractor.cs # SQL parameter extraction
??? Proteo5.HideDB.CMD/           # Console Application
?   ??? Entities/                 # YAML definitions
?   ?   ??? Users.yaml            # Example: Users entity
?   ?   ??? Roles.yaml            # Example: Roles entity
?   ??? GeneratedCode/            # Generated C# code
?   ?   ??? Models/               # Generated entity models
?   ?   ??? Repositories/         # Generated repositories
?   ?   ??? Enums/                # Generated enumerations
?   ??? GeneratedSQL/             # Generated SQL scripts
?   ??? DirectTest.cs             # Interactive testing suite
?   ??? Program.cs                # Main application entry point
?   ??? appsettings.json          # Configuration file
??? README.md                     # This file
```

## Quick Start

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or higher
- Database (SQL Server, PostgreSQL, MySQL, SQLite, or Oracle)

### Installation

1. **Clone the repository:**
   ```bash
   git clone https://github.com/your-username/hide-db.git
   cd hide-db
   ```

2. **Restore dependencies:**
   ```bash
   dotnet restore
   ```

3. **Configure database:**
   Edit `Proteo5.HideDB.CMD/appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": {
         "Type": "SqlServer",
         "ConnectionString": "Server=localhost;Database=MyDB;Trusted_Connection=true;"
       }
     }
   }
   ```

### Basic Usage

1. **Run the generator:**
   ```bash
   cd Proteo5.HideDB.CMD
   dotnet run
   ```

2. **Available commands:**
   ```bash
   dotnet run                # File watcher mode
   dotnet run test          # Generate code and show examples
   dotnet run directtest    # Run library test suite
   ```

## YAML Syntax

### Basic Example: Users.yaml

```yaml
# Users entity definition
entity: "Users"
entityversion: "1"
version: "1.0"
description: "Entity for system user management"

fields:
  - name: "Id"
    type: "int"
    primaryKey: true
    autoIncrement: true
    required: true
    description: "Unique user identifier"
    
  - name: "Username"
    type: "string"
    maxLength: 50
    required: true
    description: "Unique username"
    
  - name: "Email"
    type: "string"
    maxLength: 100
    required: true
    description: "Email address"
    
  - name: "FirstName"
    type: "string"
    maxLength: 50
    nullable: true
    description: "User's first name"
    
  - name: "Status"
    type: "string"
    defaultValue: "active"
    catalog: "statuses"
    description: "User status"

catalogs:
  statuses:
    - name: "active"
      description: "Active user"
    - name: "inactive"
      description: "Inactive user"

statements:
  - name: "Insert"
    type: "Insert"
    return: "nothing"
    description: "Create a new user"
    sql: |
      INSERT INTO Users (Username, Email, FirstName, Status)
      VALUES (@Username, @Email, @FirstName, @Status);
      
  - name: "GetById"
    type: "Select"
    return: "one"
    description: "Get user by ID"
    sql: |
      SELECT * FROM Users WHERE Id = @Id;
```

### Supported Data Types

| YAML Type | C# Type | SQL Server Type | Description |
|-----------|---------|-----------------|-------------|
| `int` | `int` | `INT` | 32-bit integer |
| `long` | `long` | `BIGINT` | 64-bit integer |
| `string` | `string` | `NVARCHAR` | Text string |
| `datetime` | `DateTime` | `DATETIME2` | Date and time |
| `bool` | `bool` | `BIT` | Boolean |
| `decimal` | `decimal` | `DECIMAL(18,2)` | Precision decimal |
| `double` | `double` | `FLOAT` | Floating point |
| `guid` | `Guid` | `UNIQUEIDENTIFIER` | Unique identifier |

### Field Attributes

- **`name`**: Field name (required)
- **`type`**: Data type (required)
- **`primaryKey`**: Is primary key (boolean)
- **`autoIncrement`**: Auto-increment (boolean)
- **`required`**: Required field (boolean)
- **`nullable`**: Allows null values (boolean)
- **`maxLength`**: Maximum length for strings
- **`defaultValue`**: Default value
- **`catalog`**: Reference to catalog/enum
- **`description`**: Field description

## Configuration

### appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": {
      "Type": "SqlServer",
      "ConnectionString": "Server=localhost;Database=MyDB;Trusted_Connection=true;"
    }
  },
  "Generator": {
    "YamlPath": "./Entities",
    "OutputPath": "./GeneratedCode",
    "SqlOutputPath": "./GeneratedSQL",
    "Namespace": "MyProject.Generated",
    "Provider": "SqlServer",
    "GenerateInterfaces": true,
    "GenerateAsync": true,
    "GenerateSync": true,
    "AddDataAnnotations": true
  }
}
```

### Configuration Options

- **`YamlPath`**: YAML files directory
- **`OutputPath`**: Generated code directory
- **`SqlOutputPath`**: SQL scripts directory
- **`Namespace`**: Generated code namespace
- **`Provider`**: Database provider (SqlServer, PostgreSQL, MySQL, SQLite, Oracle)
- **`GenerateInterfaces`**: Generate repository interfaces
- **`GenerateAsync`**: Generate asynchronous methods
- **`GenerateSync`**: Generate synchronous methods
- **`AddDataAnnotations`**: Add DataAnnotations to models

## Generated Code

For each entity defined in YAML, the following files are generated:

### 1. Model (UsersModel.cs)
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
    
    // ... more properties
}
```

### 2. Repository Interface (IUsersRepository.cs)
```csharp
public interface IUsersRepository
{
    Task<int> InsertAsync(string username, string email, string? firstName);
    Task<UsersModel?> GetByIdAsync(int id);
    Task<List<UsersModel>> GetAllAsync();
    Task<int> UpdateAsync(string username, string email, int id);
    Task<int> DeleteByIdAsync(int id);
    // ... more methods
}
```

### 3. Repository Implementation (UsersRepository.cs)
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
        using var command = new SqlCommand(/* Generated SQL */, connection);
        // ... implementation
    }
    
    // ... more implemented methods
}
```

### 4. Enumerations (UsersEnums.cs)
```csharp
public enum UsersStatuses
{
    [Description("Active user")]
    Active = 0,
    
    [Description("Inactive user")]
    Inactive = 1
}
```

### 5. SQL Script (Users_CreateTable.sql)
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

## Testing

### Running Tests

```bash
# Interactive test with pauses for manual verification
dotnet run directtest

# Generate code and test examples
dotnet run test
```

### Testing Features

- ? Automatic Table Creation
- ? Test Data Insertion
- ? Query and Search Operations
- ? Timestamp Updates
- ? Data Deletion
- ? Automatic Cleanup
- ? Interactive Pauses
- ? Suggested SQL Commands

### Interactive Testing Process

1. **Database Connection Test**: Verifies connectivity
2. **Table Creation**: Creates test table with proper schema
3. **Data Insertion**: Inserts multiple test records
4. **Data Query**: Retrieves and displays all records
5. **Data Update**: Modifies existing records with timestamp updates
6. **Data Deletion**: Removes all test records
7. **Table Cleanup**: Drops test table

Each step includes:
- **Status Information**: Clear status messages
- **Manual Verification**: Pause with suggested SQL commands
- **Error Handling**: Comprehensive error reporting
- **Database State Validation**: Confirms expected database state

## Project Statistics

- **Primary Language**: C# 13.0
- **Framework**: .NET 8.0
- **Main Dependencies**:
  - YamlDotNet 16.3.0
  - Microsoft.Data.SqlClient 6.0.2
  - Microsoft.Extensions.* 9.0.8
- **Architecture**: Clean Architecture
- **Patterns**: Repository, Factory, Builder
- **Testing**: Unit Tests + Integration Tests

## Contributing

1. Fork the project
2. Create feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit changes (`git commit -m 'Add AmazingFeature'`)
4. Push to branch (`git push origin feature/AmazingFeature`)
5. Open Pull Request

### Code Conventions

- Use C# 13.0 features
- Follow .NET naming conventions
- Document public methods with XML comments
- Include tests for new functionality
- Maintain test coverage > 80%

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Acknowledgments

- [YamlDotNet](https://github.com/aaubry/YamlDotNet) - YAML serialization
- [Microsoft.Extensions](https://github.com/dotnet/extensions) - Dependency Injection and Configuration
- [.NET Community](https://dotnet.microsoft.com/platform/community) - Inspiration and best practices

## Support

- **Issues**: [GitHub Issues](https://github.com/your-username/hide-db/issues)
- **Discussions**: [GitHub Discussions](https://github.com/your-username/hide-db/discussions)
- **Email**: your-email@example.com

---

<div align="center">
  <strong>Made with ?? for the .NET community</strong>
</div>
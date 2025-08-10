# Proteo5.HideDB - Real-Time YAML DSL Code Generator with Roslyn Source Generators

**Modern, compile-time code generation for .NET 8+ using Roslyn Source Generators**

[![.NET](https://img.shields.io/badge/.NET-8.0-blue.svg)](https://dotnet.microsoft.com/download/dotnet/8.0)
[![C#](https://img.shields.io/badge/C%23-13.0-green.svg)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)
[![Build Status](https://img.shields.io/badge/build-passing-brightgreen.svg)](https://github.com)
[![Source Generators](https://img.shields.io/badge/Roslyn-Source%20Generators-purple.svg)](https://docs.microsoft.com/en-us/dotnet/csharp/roslyn-sdk/source-generators-overview)

## ?? Description

Proteo5.HideDB revolutionizes database development by automatically generating type-safe C# code from simple YAML entity definitions using **Roslyn Source Generators**. Experience real-time IntelliSense as you define your database entities!

**?? New in v2.0: Now powered by Roslyn Source Generators for instant, real-time code generation!**

## ? Key Features

### ?? **Real-Time Code Generation**
- **Instant IntelliSense**: Generated code appears immediately in your IDE
- **Compile-Time Generation**: No external tools or build steps required
- **Live Updates**: Edit YAML ? Build ? Instant code with IntelliSense

### ??? **Comprehensive Code Generation**
- **Entity Models** with Data Annotations (`[Key]`, `[Required]`, `[MaxLength]`)
- **Repository Interfaces** for dependency injection and testing
- **Repository Implementations** with async/await support
- **Type-Safe SQL Operations** with automatic parameter mapping
- **Enum Catalogs** for string-based fields with predefined values

### ?? **Modern .NET Integration**
- **.NET 8.0** native support with latest C# features
- **Nullable Reference Types** for compile-time null safety
- **Async/Await** patterns throughout generated code
- **Dependency Injection** ready interfaces

### ?? **Developer Experience**
- **Simple YAML DSL** - No complex SQL schema definitions
- **IntelliSense Support** - Full IDE integration
- **Error Reporting** - Clear diagnostics during compilation
- **Hot Reload** - Changes reflect immediately
- **Entity-First Structure** - Organized by entity with "E" suffix folders

## ?? Proteo5.HideDB vs sqlc - A Better Approach

While [sqlc](https://github.com/sqlc-dev/sqlc) pioneered compile-time safe SQL code generation for Go, Proteo5.HideDB takes the concept further for the .NET ecosystem:

| Feature | **Proteo5.HideDB** (.NET/C#) | **sqlc** (Go) |
|---------|-------------------------------|---------------|
| **Definition Language** | **Declarative YAML DSL** - Business-focused entity definitions | **Raw SQL** - Database-focused schema definitions |
| **Code Generation** | **Complete Entity Ecosystem** - Models, repositories, interfaces | **Query Methods Only** - Limited function generation |
| **IDE Integration** | **Real-Time IntelliSense** - Roslyn Source Generators | **External Compilation** - Manual build steps required |
| **Type Safety** | **C# 13.0 + Nullable Reference Types** - Compile-time null safety | **Go Type System** - Runtime null pointer risks |
| **Database Abstraction** | **High-Level Entity Focus** - Business logic first | **SQL-First Approach** - Database internals exposed |
| **Schema Management** | **Declarative Evolution** - YAML-driven changes | **Manual SQL Migration** - Hand-written DDL scripts |

### ?? **Why Choose Proteo5.HideDB?**

- **?? Faster Development**: Focus on business logic, not SQL boilerplate
- **??? Type Safety**: Compile-time errors prevent runtime database issues  
- **?? Real-Time Feedback**: Instant IntelliSense as you define entities
- **?? Modern Patterns**: Repository pattern, async/await, dependency injection
- **?? Clean Code**: Generated code follows .NET conventions and best practices

## ??? Architecture

```
Proteo5.HideDB/
??? Proteo5.HideDB.SourceGenerator/     # ?? Roslyn Source Generator
?   ??? YamlDslSourceGenerator.cs       # Main source generator
?   ??? Models/                         # YAML entity definitions
?   ?   ??? EntityDefinition.cs         # Strongly-typed YAML schema
?   ??? Generators/                     # Code generation engines
?       ??? ModelGenerator.cs           # Entity model generation
?       ??? RepositoryGenerator.cs      # Repository pattern generation
??? Proteo5.HideDB.Lib/                 # Legacy file-watching generator
??? Proteo5.HideDB.CMD/                 # Command-line tool (legacy support)
?   ??? GeneratedCode/                  # ?? Entity-First Structure
?       ??? UsersE/                     # Users entity folder
?       ?   ??? UsersModel.cs           # Generated model
?       ?   ??? IUsersRepository.cs     # Repository interface
?       ?   ??? UsersRepository.cs      # Repository implementation
?       ?   ??? UsersEnums.cs           # Status enums
?       ??? RolesE/                     # Roles entity folder
?           ??? RolesModel.cs           # Generated model
?           ??? IRolesRepository.cs     # Repository interface
?           ??? RolesRepository.cs      # Repository implementation
?           ??? RolesEnums.cs           # Status enums
??? Proteo5.HideDB.TestApp/             # ?? Source Generator demo
?   ??? Entities/                       # YAML entity definitions
?   ?   ??? Users.yaml                  # Example entity
?   ??? Program.cs                      # Working demo application
??? README.md                           # This file
```

## ?? Quick Start (Roslyn Source Generators)

### 1. **Project Setup**

Create a new .NET 8.0 project and add the source generator:

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <LangVersion>latest</LangVersion>
    <Nullable>enable</Nullable>
  </PropertyGroup>

  <ItemGroup>
    <!-- Add the Source Generator -->
    <ProjectReference Include="path\to\Proteo5.HideDB.SourceGenerator.csproj" 
                      OutputItemType="Analyzer" 
                      ReferenceOutputAssembly="false" />
    
    <!-- Include YAML files for processing -->
    <AdditionalFiles Include="Entities\**\*.yaml" />
  </ItemGroup>

  <ItemGroup>
    <!-- Required packages for generated code -->
    <PackageReference Include="Microsoft.Data.SqlClient" Version="6.0.2" />
  </ItemGroup>
</Project>
```

### 2. **Define Your Entity**

Create `Entities/Users.yaml`:

```yaml
# Simple, declarative entity definition
entity: "Users"
version: "1.0"
description: "User management entity"

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
    maxLength: 20
    defaultValue: "active"
    catalog: "statuses"
    required: true
    description: "User status"

catalogs:
  statuses:
    - name: "active"
      description: "Active user"
    - name: "inactive"
      description: "Inactive user"
    - name: "pending"
      description: "Pending approval"

statements:
  - name: "Insert"
    type: "Insert"
    return: "nothing"
    description: "Create a new user"
    sql: |
      INSERT INTO Users (Username, Email, FirstName, Status)
      VALUES (@Username, @Email, @FirstName, @Status);
  
  - name: "GetAll"
    type: "Select"
    return: "many"
    description: "Get all users"
    sql: |
      SELECT Id, Username, Email, FirstName, Status
      FROM Users ORDER BY Username;
  
  - name: "GetById"
    type: "Select"
    return: "one"
    description: "Get user by ID"
    sql: |
      SELECT Id, Username, Email, FirstName, Status
      FROM Users WHERE Id = @Id;
```

### 3. **Build and Use Generated Code**

```bash
# Build your project
dotnet build
```

**? That's it! The code is generated automatically with full IntelliSense support:**

```csharp
using Proteo5.HideDB.Generated.UsersE;

// Generated model with Data Annotations
var user = new UsersModel
{
    Username = "john_doe",
    Email = "john@example.com",
    FirstName = "John",
    Status = "active"
};

// Generated repository with full IntelliSense
var repository = new UsersRepository(connectionString);

// Type-safe async operations
await repository.InsertAsync(user.Username, user.Email, user.FirstName, user.Status);
var users = await repository.GetAllAsync();
var specificUser = await repository.GetByIdAsync(1);

// Use generated enums for type safety
var activeStatus = UsersStatuses.Active.ToString().ToLower(); // "active"
```

## ?? YAML DSL Reference

### **Entity Definition**

```yaml
entity: "EntityName"           # Required: Entity name
version: "1.0"                # Required: Version for tracking
description: "Description"    # Optional: Entity description
```

### **Field Types**

| YAML Type | C# Type | SQL Server Type | Description |
|-----------|---------|-----------------|-------------|
| `int` | `int` | `INT` | 32-bit integer |
| `long` | `long` | `BIGINT` | 64-bit integer |
| `string` | `string` | `NVARCHAR` | Text string |
| `datetime` | `DateTime` | `DATETIME2` | Date and time |
| `bool` | `bool` | `BIT` | Boolean value |
| `decimal` | `decimal` | `DECIMAL(18,2)` | Precision decimal |
| `guid` | `Guid` | `UNIQUEIDENTIFIER` | Unique identifier |

### **Field Attributes**

```yaml
fields:
  - name: "FieldName"          # Required: Field name
    type: "string"             # Required: Data type
    primaryKey: true           # Optional: Is primary key
    autoIncrement: true        # Optional: Auto-increment
    required: true             # Optional: Required field
    nullable: true             # Optional: Allows null values
    maxLength: 100             # Optional: Maximum length for strings
    defaultValue: "active"     # Optional: Default value
    catalog: "statuses"        # Optional: Reference to catalogs section
    description: "Description" # Optional: Field description
```

### **Catalogs (Enums)**

```yaml
catalogs:
  statuses:                    # Catalog name (referenced by fields)
    - name: "active"           # Enum value
      description: "Active"    # Enum description
    - name: "inactive"
      description: "Inactive"
```

### **Statement Types**

```yaml
statements:
  - name: "MethodName"         # Generated method name
    type: "Select"             # Insert | Update | Delete | Select
    return: "many"             # nothing | one | many | scalar
    description: "Description" # Method description
    sql: |                     # SQL query with @parameters
      SELECT * FROM Users WHERE Id = @Id;
```

## ?? Generated Code Examples

### **Generated Model**
```csharp
[Table("Users")]
public class UsersModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Required]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]  
    public string Email { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? FirstName { get; set; }

    [Required]
    [MaxLength(20)]
    public string Status { get; set; } = string.Empty;
}
```

### **Generated Enums**
```csharp
public enum UsersStatuses
{
    [Description("Active user")]
    Active = 0,
    
    [Description("Inactive user")]
    Inactive = 1,
    
    [Description("Pending approval")]
    Pending = 2
}
```

### **Generated Repository Interface**
```csharp
public interface IUsersRepository
{
    Task<int> InsertAsync(string username, string email, string? firstName, string status);
    Task<List<UsersModel>> GetAllAsync();
    Task<UsersModel?> GetByIdAsync(int id);
}
```

### **Generated Repository Implementation**
```csharp
public class UsersRepository : IUsersRepository
{
    private readonly string _connectionString;

    public UsersRepository(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
    }

    public async Task<int> InsertAsync(string username, string email, string? firstName, string status)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand();
        await connection.OpenAsync();
        command.Connection = connection;
        command.CommandText = @"INSERT INTO Users (Username, Email, FirstName, Status)
                               VALUES (@Username, @Email, @FirstName, @Status);";
        
        command.Parameters.AddWithValue("@Username", username ?? DBNull.Value);
        command.Parameters.AddWithValue("@Email", email ?? DBNull.Value);
        command.Parameters.AddWithValue("@FirstName", firstName ?? DBNull.Value);
        command.Parameters.AddWithValue("@Status", status ?? DBNull.Value);
        
        return await command.ExecuteNonQueryAsync();
    }

    // ... other generated methods
}
```

## ?? Entity-First Folder Structure

**NEW in v2.1: Entity-First Organization with "E" Suffix**

Proteo5.HideDB now organizes generated code by entity, making it easier to navigate and maintain:

### **Traditional Structure (Old)**
```
GeneratedCode/
??? Models/
?   ??? UsersModel.cs
?   ??? RolesModel.cs
??? Repositories/
?   ??? IUsersRepository.cs
?   ??? UsersRepository.cs
?   ??? IRolesRepository.cs
?   ??? RolesRepository.cs
??? Enums/
    ??? UsersEnums.cs
    ??? RolesEnums.cs
```

### **Entity-First Structure (New)**
```
GeneratedCode/
??? UsersE/                    # ?? Users entity folder
?   ??? UsersModel.cs          # Model with namespace: Proteo5.HideDB.Generated.UsersE
?   ??? IUsersRepository.cs    # Interface
?   ??? UsersRepository.cs     # Implementation
?   ??? UsersEnums.cs          # Enums
??? RolesE/                    # ?? Roles entity folder
    ??? RolesModel.cs          # Model with namespace: Proteo5.HideDB.Generated.RolesE
    ??? IRolesRepository.cs    # Interface
    ??? RolesRepository.cs     # Implementation
    ??? RolesEnums.cs          # Enums
```

### **Benefits of Entity-First Structure**

1. **??? Better Organization**: All files for an entity are in one folder
2. **?? Easier Navigation**: Find all Users-related code in `UsersE/` folder
3. **?? Cleaner Namespaces**: Each entity has its own namespace
4. **?? Easier Refactoring**: Move or rename entity folders independently
5. **?? Logical Grouping**: Related models, repositories, and enums together

### **Using Entity-First Structure**

```csharp
// Import specific entity namespace
using Proteo5.HideDB.Generated.UsersE;
using Proteo5.HideDB.Generated.RolesE;

// Use types from specific entities
var usersRepo = new UsersRepository(connectionString);
var rolesRepo = new RolesRepository(connectionString);

// Types are scoped to their entity
var user = new UsersModel();  // From UsersE namespace
var role = new RolesModel();  // From RolesE namespace

// Enums are also entity-scoped
var userStatus = UsersStatuses.Active;
var roleStatus = RolesStatuses.Active;
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
    
  - name: "IsActive"
    type: "bool"
    required: true
    description: "Whether the user is active"

statements:
  - name: "Insert"
    type: "Insert"
    return: "nothing"
    description: "Create a new user"
    sql: |
      INSERT INTO Users (Username, Email, FirstName, IsActive)
      VALUES (@Username, @Email, @FirstName, @IsActive);
  
  - name: "GetAll"
    type: "Select"
    return: "many"
    description: "Get all users"
    sql: |
      SELECT Id, Username, Email, FirstName, IsActive
      FROM Users ORDER BY Username;
  
  - name: "GetById"
    type: "Select"
    return: "one"
    description: "Get user by ID"
    sql: |
      SELECT Id, Username, Email, FirstName, IsActive
      FROM Users WHERE Id = @Id;
```

### 3. **Build and Use Generated Code**

```bash
# Build your project
dotnet build
```

**? That's it! The code is generated automatically with full IntelliSense support:**

```csharp
using Proteo5.HideDB.Generated.Models;
using Proteo5.HideDB.Generated.Repositories;

// Generated model with Data Annotations
var user = new UsersModel
{
    Username = "john_doe",
    Email = "john@example.com",
    FirstName = "John",
    IsActive = true
};

// Generated repository with full IntelliSense
var repository = new UsersRepository(connectionString);

// Type-safe async operations
await repository.InsertAsync(user.Username, user.Email, user.FirstName, user.IsActive);
var users = await repository.GetAllAsync();
var specificUser = await repository.GetByIdAsync(1);
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
    description: "Description" # Optional: Field description
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
    public bool IsActive { get; set; }
}
```

### **Generated Repository Interface**
```csharp
public interface IUsersRepository
{
    Task<int> InsertAsync(string username, string email, string? firstName, bool isActive);
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

    public async Task<int> InsertAsync(string username, string email, string? firstName, bool isActive)
    {
        using var connection = new SqlConnection(_connectionString);
        using var command = new SqlCommand();
        await connection.OpenAsync();
        command.Connection = connection;
        command.CommandText = @"INSERT INTO Users (Username, Email, FirstName, IsActive)
                               VALUES (@Username, @Email, @FirstName, @IsActive);";
        
        command.Parameters.AddWithValue("@Username", username ?? DBNull.Value);
        command.Parameters.AddWithValue("@Email", email ?? DBNull.Value);
        command.Parameters.AddWithValue("@FirstName", firstName ?? DBNull.Value);
        command.Parameters.AddWithValue("@IsActive", isActive);
        
        return await command.ExecuteNonQueryAsync();
    }

    // ... other generated methods
}
```

## ?? Usage Examples

### **Dependency Injection Setup**
```csharp
// Program.cs (.NET 8 minimal APIs)
var builder = WebApplication.CreateBuilder(args);

// Register generated repository
builder.Services.AddScoped<IUsersRepository>(provider => 
    new UsersRepository(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();
```

### **Controller Usage**
```csharp
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUsersRepository _usersRepository;

    public UsersController(IUsersRepository usersRepository)
    {
        _usersRepository = usersRepository;
    }

    [HttpGet]
    public async Task<ActionResult<List<UsersModel>>> GetUsers()
    {
        var users = await _usersRepository.GetAllAsync();
        return Ok(users);
    }

    [HttpPost]
    public async Task<ActionResult> CreateUser(CreateUserRequest request)
    {
        await _usersRepository.InsertAsync(
            request.Username, 
            request.Email, 
            request.FirstName, 
            true);
        return Ok();
    }
}
```

## ?? Development Workflow

### **Real-Time Development Experience**
1. **Edit** YAML entity definitions
2. **Build** project (`Ctrl+Shift+B`)
3. **Code appears** instantly with IntelliSense
4. **Use generated classes** with full type safety

### **IDE Integration**
- ? **Visual Studio**: Full IntelliSense support
- ? **VS Code**: IntelliSense with C# extension
- ? **JetBrains Rider**: Complete code generation support
- ? **Error Reporting**: Clear diagnostics for YAML issues

### **Debugging Source Generators**
```bash
# View generated code in build output
dotnet build --verbosity detailed

# Generated files location:
# obj/Debug/net8.0/Proteo5.HideDB.SourceGenerator/
```

## ?? Try It Now!

### **Clone and Run the Demo**
```bash
git clone https://github.com/your-username/hide-db.git
cd hide-db/Proteo5.HideDB.TestApp
dotnet build
dotnet run
```

### **What You'll See**
- ? Real-time code generation in action
- ? Generated `UsersModel` with Data Annotations  
- ? Generated `IUsersRepository` interface
- ? Generated `UsersRepository` implementation
- ? Full IntelliSense support for all generated code

## ?? Legacy Mode (File Watcher)

For backward compatibility, the legacy file-watching mode is still available:

```bash
cd Proteo5.HideDB.CMD

# Show Source Generator setup instructions
dotnet run

# Legacy file watching mode
dotnet run watch

# Manual generation mode  
dotnet run generate
```

## ?? Project Statistics

- **Primary Language**: C# 13.0 with .NET 8.0
- **Architecture**: Roslyn Source Generators + Repository Pattern
- **Generated Code Features**:
  - ? Data Annotations
  - ? Nullable Reference Types
  - ? Async/Await patterns
  - ? Dependency Injection ready
  - ? Type-safe operations
- **Dependencies**:
  - Microsoft.CodeAnalysis.CSharp 4.5.0
  - YamlDotNet 16.3.0
  - Microsoft.Data.SqlClient 6.0.2

## ?? Key Benefits Over Traditional Approaches

### **vs Entity Framework Code-First**
- ? **Explicit SQL Control** - Write exact SQL you want
- ? **Performance** - No heavy ORM overhead
- ? **Simple Mapping** - Direct database-to-object mapping
- ? **Clear Intent** - SQL queries are visible and maintainable

### **vs Dapper + Manual Code**
- ? **No Boilerplate** - Models and repositories auto-generated
- ? **Type Safety** - Compile-time checking of SQL parameters
- ? **Consistency** - All repositories follow same patterns
- ? **Maintainability** - Single source of truth in YAML

### **vs sqlc (Go)**
- ? **Higher-Level Abstractions** - Entity-focused vs SQL-focused
- ? **Complete Code Generation** - Models + repositories vs just queries
- ? **Real-Time IDE Integration** - Instant IntelliSense vs build-time only
- ? **Modern .NET Features** - Nullable types, async/await, DI ready

## ?? Contributing

We welcome contributions! Please see our [Contributing Guidelines](CONTRIBUTING.md) for details.

### **Development Setup**
```bash
git clone https://github.com/your-username/hide-db.git
cd hide-db
dotnet restore
dotnet build
```

### **Running Tests**
```bash
# Build and test the source generator
cd Proteo5.HideDB.TestApp
dotnet build
dotnet run
```

## ?? License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## ?? Acknowledgments

- **[sqlc](https://github.com/sqlc-dev/sqlc)** - Original inspiration for compile-time safe SQL generation
- **[Roslyn Source Generators](https://docs.microsoft.com/en-us/dotnet/csharp/roslyn-sdk/source-generators-overview)** - Enabling real-time code generation
- **[YamlDotNet](https://github.com/aaubry/YamlDotNet)** - YAML parsing and serialization
- **[.NET Community](https://dotnet.microsoft.com/platform/community)** - For continuous innovation and best practices

## ?? Support & Community

- **Issues**: [GitHub Issues](https://github.com/your-username/hide-db/issues)
- **Discussions**: [GitHub Discussions](https://github.com/your-username/hide-db/discussions)
- **Documentation**: [Wiki](https://github.com/your-username/hide-db/wiki)

---

<div align="center">

**? Experience the future of database development with real-time code generation!**

*Made with ?? for the .NET community*

[![GitHub Stars](https://img.shields.io/github/stars/your-username/hide-db?style=social)](https://github.com/your-username/hide-db/stargazers)
[![GitHub Forks](https://img.shields.io/github/forks/your-username/hide-db?style=social)](https://github.com/your-username/hide-db/network/members)

</div>
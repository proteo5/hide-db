using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Proteo5.HideDB.Lib.Configuration;
using Proteo5.HideDB.Lib.Generators;

namespace Proteo5.HideDB.CMD;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("🚀 Proteo5.HideDB - YAML DSL Generator");
        Console.WriteLine("=====================================\n");

        try
        {
            // Check arguments
            if (args.Length > 0)
            {
                switch (args[0].ToLower())
                {
                    case "test":
                        Console.WriteLine("🧪 Test mode detected. Generating code and preparing test...");
                        await RunTestMode();
                        return;
                    case "directtest":
                        Console.WriteLine("🔬 Running direct library test...");
                        await DirectTest.RunDirectTest();
                        return;
                }
            }

            // Configure host
            var host = CreateHostBuilder(args).Build();
            
            // Get services
            var logger = host.Services.GetRequiredService<ILogger<Program>>();
            var config = host.Services.GetRequiredService<GeneratorConfig>();
            
            logger.LogInformation("Starting YAML DSL Generator...");
            
            // Create and configure generator
            var generator = new YamlDslGenerator(config, host.Services.GetRequiredService<ILogger<YamlDslGenerator>>());
            
            // Create example file if it doesn't exist
            await CreateExampleYamlFile(config.YamlPath);
            
            // Start watching
            generator.StartWatching();
            
            logger.LogInformation("🔍 Generator started. Press 'q' to exit...");
            Console.WriteLine("\nAvailable commands:");
            Console.WriteLine("• dotnet run test      - Generate code and show test example");
            Console.WriteLine("• dotnet run directtest - Run direct database test");
            Console.WriteLine("• q                     - Exit\n");
            
            // Wait for user input
            ConsoleKey key;
            do
            {
                key = Console.ReadKey(true).Key;
            } while (key != ConsoleKey.Q);
            
            // Cleanup
            generator.Stop();
            logger.LogInformation("👋 Generator stopped. See you later!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Fatal error: {ex.Message}");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }

    static async Task RunTestMode()
    {
        try
        {
            // Configure host for test
            var host = CreateHostBuilder(new string[0]).Build();
            var config = host.Services.GetRequiredService<GeneratorConfig>();
            
            // Create and configure generator
            var generator = new YamlDslGenerator(config, host.Services.GetRequiredService<ILogger<YamlDslGenerator>>());
            
            // Create example file if it doesn't exist
            await CreateExampleYamlFile(config.YamlPath);
            
            // Generate code
            Console.WriteLine("Generating code...");
            generator.StartWatching();
            await Task.Delay(3000); // Wait for code generation
            generator.Stop();
            
            // Verify code was generated
            var modelsPath = Path.Combine(config.OutputPath, "Models", "UsersModel.cs");
            var repoPath = Path.Combine(config.OutputPath, "Repositories", "UsersRepository.cs");
            
            if (File.Exists(modelsPath) && File.Exists(repoPath))
            {
                Console.WriteLine("✅ Code generated successfully!");
                Console.WriteLine("\n📄 Generated files:");
                Console.WriteLine($"• {modelsPath}");
                Console.WriteLine($"• {repoPath}");
                Console.WriteLine($"• {Path.Combine(config.OutputPath, "Repositories", "IUsersRepository.cs")}");
                Console.WriteLine($"• {Path.Combine(config.SqlOutputPath, "Users_CreateTable.sql")}");
                
                Console.WriteLine("\n🧪 To test the library:");
                Console.WriteLine("1. Run: dotnet run directtest");
                Console.WriteLine("2. Or copy the content from TestExample.cs for a custom test");
                
                await CreateTestExample();
            }
            else
            {
                Console.WriteLine("❌ Error: Could not generate code");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error in test mode: {ex.Message}");
        }
    }

    static async Task CreateTestExample()
    {
        var testCode = @"
// Complete test example for the generated library
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Proteo5.HideDB.Generated.Repositories;
using Proteo5.HideDB.Generated.Models;
using Microsoft.Data.SqlClient;

class TestRunner
{
    static async Task Main()
    {
        Console.WriteLine(""🧪 GENERATED LIBRARY TEST"");
        Console.WriteLine(""========================\n"");
        
        try
        {
            // 1. Configuration
            var configuration = new ConfigurationBuilder()
                .AddJsonFile(""appsettings.json"")
                .Build();
            var connectionString = configuration.GetConnectionString(""DefaultConnection:ConnectionString"");
            
            if (string.IsNullOrEmpty(connectionString))
            {
                Console.WriteLine(""❌ Connection string not found"");
                return;
            }
            
            Console.WriteLine(""✅ Connection string configured"");
            
            // 2. Create repository
            var usersRepo = new UsersRepository(connectionString);
            Console.WriteLine(""✅ UsersRepository created"");
            
            // 3. Create table (using direct SQL)
            await CreateUsersTable(connectionString);
            
            // 4. Insertion test
            Console.WriteLine(""\n📝 INSERTING USERS..."");
            var testUsers = new[]
            {
                new { Username = ""admin"", Password = ""hash123"", Email = ""admin@test.com"", FirstName = ""Admin"", LastName = ""User"", Status = ""active"" },
                new { Username = ""john"", Password = ""hash456"", Email = ""john@test.com"", FirstName = ""John"", LastName = ""Doe"", Status = ""active"" },
                new { Username = ""jane"", Password = ""hash789"", Email = ""jane@test.com"", FirstName = ""Jane"", LastName = ""Smith"", Status = ""inactive"" }
            };
            
            foreach (var user in testUsers)
            {
                await usersRepo.InsertAsync(user.Username, user.Password, user.Email, 
                                          user.FirstName, user.LastName, user.Status);
                Console.WriteLine($""✅ {user.Username} inserted"");
            }
            
            // 5. Query test
            Console.WriteLine(""\n📋 QUERYING USERS..."");
            var users = await usersRepo.GetAllAsync();
            Console.WriteLine($""📊 Total users: {users.Count}"");
            
            foreach (var user in users)
            {
                Console.WriteLine($""👤 ID: {user.Id}, Username: {user.Username}, Email: {user.Email}"");
            }
            
            // 6. Search by ID test
            Console.WriteLine(""\n🔍 SEARCHING USER BY ID..."");
            var user1 = await usersRepo.GetByIdAsync(1);
            if (user1 != null)
            {
                Console.WriteLine($""✅ User found: {user1.Username} - {user1.Email}"");
            }
            
            // 7. Search by username test
            Console.WriteLine(""\n🔍 SEARCHING USER BY USERNAME..."");
            var userByName = await usersRepo.GetByUserAsync(""john"");
            if (userByName != null)
            {
                Console.WriteLine($""✅ User found: {userByName.Username} - {userByName.Email}"");
            }
            
            // 8. Update test
            if (user1 != null)
            {
                Console.WriteLine(""\n✏️ UPDATING USER..."");
                await usersRepo.UpdateAsync(""admin_updated"", ""newhash"", ""admin.new@test.com"",
                                           ""Admin Updated"", ""User Modified"", ""inactive"", user1.Id);
                
                var updatedUser = await usersRepo.GetByIdAsync(user1.Id);
                if (updatedUser != null)
                {
                    Console.WriteLine($""✅ User updated: {updatedUser.Username} - {updatedUser.Email}"");
                }
            }
            
            // 9. Query by status test
            Console.WriteLine(""\n🔍 QUERYING ACTIVE USERS..."");
            var activeUsers = await usersRepo.GetByStatusAsync(""active"");
            Console.WriteLine($""📊 Active users: {activeUsers.Count}"");
            
            // 10. Count test
            Console.WriteLine(""\n📊 COUNTING ACTIVE USERS..."");
            var activeCount = await usersRepo.GetActiveCountAsync();
            Console.WriteLine($""✅ Active users count: {activeCount}"");
            
            // 11. Cleanup - Delete users
            Console.WriteLine(""\n🗑️ CLEANING UP DATA..."");
            foreach (var user in users)
            {
                await usersRepo.DeleteByIdAsync(user.Id);
            }
            Console.WriteLine(""✅ Users deleted"");
            
            // 12. Drop table
            await DropUsersTable(connectionString);
            
            Console.WriteLine(""\n✅ TEST COMPLETED SUCCESSFULLY!"");
            Console.WriteLine(""🎉 The library works perfectly!"");
        }
        catch (Exception ex)
        {
            Console.WriteLine($""\n❌ ERROR: {ex.Message}"");
        }
    }
    
    static async Task CreateUsersTable(string connectionString)
    {
        var sql = @""
        IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Users' AND xtype='U')
        BEGIN
            CREATE TABLE Users (
                Id INT IDENTITY(1,1) PRIMARY KEY,
                Username NVARCHAR(50) NOT NULL,
                PasswordHash NVARCHAR(255) NOT NULL,
                Email NVARCHAR(100) NOT NULL,
                FirstName NVARCHAR(50) NULL,
                LastName NVARCHAR(50) NULL,
                status NVARCHAR(50) NULL DEFAULT 'active',
                CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
                UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE()
            )
            PRINT 'Users table created'
        END"";
        
        using var connection = new SqlConnection(connectionString);
        using var command = new SqlCommand(sql, connection);
        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
        Console.WriteLine(""✅ Users table prepared"");
    }
    
    static async Task DropUsersTable(string connectionString)
    {
        var sql = ""DROP TABLE IF EXISTS Users"";
        using var connection = new SqlConnection(connectionString);
        using var command = new SqlCommand(sql, connection);
        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
        Console.WriteLine(""✅ Users table dropped"");
    }
}";

        await File.WriteAllTextAsync("TestExample.cs", testCode);
        Console.WriteLine("✅ TestExample.cs file created with complete test");
    }

    static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, config) =>
            {
                config.AddJsonFile("appsettings.json", optional: true);
                config.AddEnvironmentVariables();
            })
            .ConfigureServices((context, services) =>
            {
                // Configure GeneratorConfig from appsettings
                var generatorConfig = new GeneratorConfig();
                context.Configuration.GetSection("Generator").Bind(generatorConfig);
                services.AddSingleton(generatorConfig);
                
                // Configure logging
                services.AddLogging(builder =>
                {
                    builder.AddConsole();
                    builder.SetMinimumLevel(LogLevel.Information);
                });
            });

    static async Task CreateExampleYamlFile(string yamlPath)
    {
        var exampleFile = Path.Combine(yamlPath, "Users.yaml");
        
        if (!File.Exists(exampleFile))
        {
            Directory.CreateDirectory(yamlPath);
            
            var exampleContent = @"# Description: This file defines the Users entity for the Proteo5.HideDB.CMD project.
entity: ""Users""
entityversion: ""1""
version: ""1.0""
description: ""Entity for system user management""

fields:
  - name: ""Id""
    type: ""int""
    primaryKey: true
    autoIncrement: true
    required: true
    description: ""Unique user identifier""
  - name: ""Username""
    type: ""string""
    maxLength: 50
    required: true
    description: ""Unique username""
  - name: ""PasswordHash""
    type: ""string""
    maxLength: 255
    required: true
    description: ""Password hash""
  - name: ""Email""
    type: ""string""
    maxLength: 100
    required: true
    description: ""Email address""
  - name: ""FirstName""
    type: ""string""
    maxLength: 50
    description: ""User's first name""
  - name: ""LastName""
    type: ""string""
    maxLength: 50
    description: ""User's last name""
  - name: ""status""
    type: ""string""
    defaultValue: ""active""
    catalog: ""statuses""
    description: ""User status""
  - name: ""CreatedAt""
    type: ""DateTime""
    defaultValue: ""CURRENT_TIMESTAMP_UTC""
    required: true
    description: ""Creation date""
  - name: ""UpdatedAt""
    type: ""DateTime""
    defaultValue: ""CURRENT_TIMESTAMP_UTC""
    required: true
    description: ""Last update date""

catalogs:
  statuses:
    - name: ""active""
      description: ""Active user""
    - name: ""inactive""
      description: ""Inactive user""
    - name: ""banned""
      description: ""Banned user""

statements:
  - name: ""Insert""
    type: ""Insert""
    return: ""nothing""
    description: ""Create a new user""
    sql: |
      INSERT INTO Users (Username, PasswordHash, Email, FirstName, LastName, status)
      VALUES (@Username, @PasswordHash, @Email, @FirstName, @LastName, @status);
  - name: ""Update""
    type: ""Update""
    return: ""nothing""
    description: ""Update an existing user""
    sql: |
      UPDATE Users 
      SET Username = @Username,
          PasswordHash = @PasswordHash,
          Email = @Email,
          FirstName = @FirstName,
          LastName = @LastName,
          status = @status,
          UpdatedAt = GETUTCDATE()
      WHERE Id = @Id;
  - name: ""DeleteById""
    type: ""Delete""
    return: ""nothing""
    description: ""Delete a user by ID""
    sql: |
      DELETE FROM Users 
      WHERE Id = @Id;
  - name: ""GetAll""
    type: ""Select""
    return: ""many""
    description: ""Get all users ordered by creation date""
    sql: |
      SELECT Id, Username, PasswordHash, Email, FirstName, LastName, status, CreatedAt, UpdatedAt
      FROM Users
      ORDER BY CreatedAt DESC;
  - name: ""GetById""
    type: ""Select""
    return: ""one""
    description: ""Get a user by ID""
    sql: |
      SELECT Id, Username, PasswordHash, Email, FirstName, LastName, status, CreatedAt, UpdatedAt
      FROM Users
      WHERE Id = @Id;
  - name: ""GetByUser""
    type: ""Select""
    return: ""one""
    description: ""Get a user by username""
    sql: |
      SELECT Id, Username, PasswordHash, Email, FirstName, LastName, status, CreatedAt, UpdatedAt
      FROM Users
      WHERE Username = @Username;
  - name: ""GetByStatus""
    type: ""Select""
    return: ""many""
    description: ""Get users filtered by status""
    sql: |
      SELECT Id, Username, PasswordHash, Email, FirstName, LastName, status, CreatedAt, UpdatedAt
      FROM Users
      WHERE status = @status
      ORDER BY CreatedAt DESC;
  - name: ""GetActiveCount""
    type: ""Select""
    return: ""scalar""
    description: ""Get count of active users""
    sql: |
      SELECT COUNT(*)
      FROM Users
      WHERE status = 'active';
  - name: ""GetByEmailAndStatus""
    type: ""Select""
    return: ""many""
    description: ""Advanced user search""
    sql: |
      SELECT Id, Username, PasswordHash, Email, FirstName, LastName, status, CreatedAt, UpdatedAt,
             CONCAT(FirstName, ' ',
             CASE 
               WHEN status = 'active' THEN 'Active'
               WHEN status = 'inactive' THEN 'Inactive'
               WHEN status = 'banned' THEN 'Banned'
               ELSE 'Unknown'
             END as Status
      FROM Users 
      WHERE (@searchTerm IS NULL OR CONCAT(FirstName, ' ', LastName) LIKE CONCAT('%', @searchTerm, '%') 
             OR Email LIKE CONCAT('%', @searchTerm, '%'))
        AND (@statusFilter IS NULL OR status = @statusFilter)
      ORDER BY CreatedAt DESC;";

            await File.WriteAllTextAsync(exampleFile, exampleContent);
            Console.WriteLine($"📄 Example file created: {exampleFile}");
        }
    }
}

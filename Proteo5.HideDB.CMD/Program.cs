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
        Console.WriteLine("[START] Proteo5.HideDB - YAML DSL Generator with Roslyn Source Generators");
        Console.WriteLine("========================================================================\n");

        try
        {
            // Check arguments
            if (args.Length > 0)
            {
                switch (args[0].ToLower())
                {
                    case "generate":
                        Console.WriteLine("[MODE] Manual generation mode...");
                        await RunGenerationMode();
                        return;
                    case "watch":
                        Console.WriteLine("[MODE] File watching mode (legacy)...");
                        await RunWatchMode();
                        return;
                    case "help":
                    case "--help":
                    case "-h":
                        ShowHelp();
                        return;
                }
            }

            // Default behavior: Show information about source generators
            ShowSourceGeneratorInfo();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] Fatal error: {ex.Message}");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }

    static void ShowSourceGeneratorInfo()
    {
        Console.WriteLine("[INFO] This project now uses Roslyn Source Generators for real-time code generation!");
        Console.WriteLine();
        Console.WriteLine("🚀 How it works:");
        Console.WriteLine("  1. Add YAML files to your project as AdditionalFiles");
        Console.WriteLine("  2. Build your project (dotnet build)");
        Console.WriteLine("  3. Generated code appears automatically in your IDE");
        Console.WriteLine("  4. Enjoy real-time IntelliSense for generated classes!");
        Console.WriteLine();
        Console.WriteLine("📁 Project Structure:");
        Console.WriteLine("  YourProject/");
        Console.WriteLine("  ├── Entities/");
        Console.WriteLine("  │   ├── Users.yaml");
        Console.WriteLine("  │   └── Products.yaml");
        Console.WriteLine("  └── YourProject.csproj");
        Console.WriteLine();
        Console.WriteLine("🔧 Project Configuration Required:");
        Console.WriteLine("  <ItemGroup>");
        Console.WriteLine("    <PackageReference Include=\"Proteo5.HideDB.SourceGenerator\" Version=\"1.0.0\">");
        Console.WriteLine("      <PrivateAssets>all</PrivateAssets>");
        Console.WriteLine("      <IncludeAssets>runtime; build; native; contentfiles; analyzers</IncludeAssets>");
        Console.WriteLine("    </PackageReference>");
        Console.WriteLine("  </ItemGroup>");
        Console.WriteLine();
        Console.WriteLine("  <ItemGroup>");
        Console.WriteLine("    <AdditionalFiles Include=\"Entities\\**\\*.yaml\" />");
        Console.WriteLine("  </ItemGroup>");
        Console.WriteLine();
        Console.WriteLine("💡 Available Commands:");
        Console.WriteLine("  dotnet run generate  - Manual generation (legacy mode)");
        Console.WriteLine("  dotnet run watch     - File watching (legacy mode)");
        Console.WriteLine("  dotnet run help      - Show this help");
        Console.WriteLine();
        Console.WriteLine("✨ With Source Generators, you no longer need to run this tool manually!");
        Console.WriteLine("   Just build your project and the code is generated automatically.");
    }

    static void ShowHelp()
    {
        Console.WriteLine("Proteo5.HideDB - YAML DSL Code Generator");
        Console.WriteLine("==========================================");
        Console.WriteLine();
        Console.WriteLine("USAGE:");
        Console.WriteLine("  dotnet run [command]");
        Console.WriteLine();
        Console.WriteLine("COMMANDS:");
        Console.WriteLine("  (no args)            Show Source Generator information");
        Console.WriteLine("  generate             Run manual code generation (legacy)");
        Console.WriteLine("  watch               Start file watching mode (legacy)");
        Console.WriteLine("  help, --help, -h    Show this help");
        Console.WriteLine();
        Console.WriteLine("EXAMPLES:");
        Console.WriteLine("  dotnet run                    # Show info about Source Generators");
        Console.WriteLine("  dotnet run generate           # Manual generation");
        Console.WriteLine("  dotnet run watch             # Legacy file watching");
        Console.WriteLine();
        Console.WriteLine("For more information, visit: https://github.com/your-username/hide-db");
    }

    static async Task RunGenerationMode()
    {
        try
        {
            Console.WriteLine("[INFO] Running manual generation (legacy mode)...");
            
            // Configure host for generation
            var host = CreateHostBuilder(new string[0]).Build();
            var config = host.Services.GetRequiredService<GeneratorConfig>();
            
            // Create and configure generator
            var generator = new YamlDslGenerator(config, host.Services.GetRequiredService<ILogger<YamlDslGenerator>>());
            
            // Create example file if it doesn't exist
            await CreateExampleYamlFile(config.YamlPath);
            
            // Generate code once
            Console.WriteLine("[GENERATE] Processing YAML files...");
            generator.StartWatching();
            await Task.Delay(3000); // Wait for code generation
            generator.Stop();
            
            // Verify code was generated
            var modelsPath = Path.Combine(config.OutputPath, "Models", "UsersModel.cs");
            var repoPath = Path.Combine(config.OutputPath, "Repositories", "UsersRepository.cs");
            
            if (File.Exists(modelsPath) && File.Exists(repoPath))
            {
                Console.WriteLine("[SUCCESS] Code generated successfully!");
                Console.WriteLine("\n[FILES] Generated files:");
                Console.WriteLine($"• {modelsPath}");
                Console.WriteLine($"• {repoPath}");
                Console.WriteLine($"• {Path.Combine(config.OutputPath, "Repositories", "IUsersRepository.cs")}");
                Console.WriteLine($"• {Path.Combine(config.SqlOutputPath, "Users_CreateTable.sql")}");
                
                Console.WriteLine("\n[RECOMMENDATION] Consider using Roslyn Source Generators for real-time generation!");
                Console.WriteLine("                  Run 'dotnet run' (no arguments) for setup instructions.");
            }
            else
            {
                Console.WriteLine("[ERROR] Could not generate code. Check YAML files and configuration.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] Error in generation mode: {ex.Message}");
        }
    }

    static async Task RunWatchMode()
    {
        try
        {
            Console.WriteLine("[INFO] Starting legacy file watching mode...");
            Console.WriteLine("[RECOMMENDATION] Consider using Roslyn Source Generators instead!");
            Console.WriteLine("                  Run 'dotnet run' (no arguments) for setup instructions.");
            Console.WriteLine();
            
            // Configure host
            var host = CreateHostBuilder(new string[0]).Build();
            
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
            
            logger.LogInformation("[READY] Generator started. Press 'q' to exit...");
            Console.WriteLine("\nFile watcher is active. Monitoring YAML files for changes...");
            Console.WriteLine("Press 'q' to quit\n");
            
            // Wait for user input
            ConsoleKey key;
            do
            {
                key = Console.ReadKey(true).Key;
            } while (key != ConsoleKey.Q);
            
            // Cleanup
            generator.Stop();
            logger.LogInformation("[EXIT] Generator stopped.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] Error in watch mode: {ex.Message}");
        }
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
            
            var exampleContent = @"# Description: This file defines the Users entity for the Proteo5.HideDB project.
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
    nullable: true
    description: ""User's first name""
  - name: ""LastName""
    type: ""string""
    maxLength: 50
    nullable: true
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
      WHERE status = 'active';";

            await File.WriteAllTextAsync(exampleFile, exampleContent);
            Console.WriteLine($"[FILE] Example file created: {exampleFile}");
        }
    }
}

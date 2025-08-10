using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Proteo5.HideDB.Generated.Repositories;
using Proteo5.HideDB.Generated.Models;

namespace Proteo5.HideDB.TestApp;

/// <summary>
/// Test application demonstrating Roslyn Source Generator usage
/// Generated code is available with full IntelliSense support!
/// </summary>
class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("?? Proteo5.HideDB Source Generator Test Application");
        Console.WriteLine("==================================================\n");

        try
        {
            // Configuration
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true)
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            
            if (string.IsNullOrEmpty(connectionString))
            {
                Console.WriteLine("? Connection string not found in appsettings.json");
                Console.WriteLine("Please configure your database connection string.");
                return;
            }

            Console.WriteLine("? Configuration loaded successfully");

            // Create repository using generated code
            // Notice how you get full IntelliSense support for these generated classes!
            var usersRepository = new UsersRepository(connectionString);
            Console.WriteLine("? UsersRepository created using generated code");

            // Example of using generated models and repositories
            Console.WriteLine("\n?? Available operations from generated code:");
            Console.WriteLine("- usersRepository.InsertAsync(...)");
            Console.WriteLine("- usersRepository.GetAllAsync()");
            Console.WriteLine("- usersRepository.GetByIdAsync(id)");
            Console.WriteLine("- usersRepository.GetByUserAsync(username)");
            Console.WriteLine("- usersRepository.GetByStatusAsync(status)");
            Console.WriteLine("- usersRepository.UpdateAsync(...)");
            Console.WriteLine("- usersRepository.DeleteByIdAsync(id)");

            Console.WriteLine("\n?? Generated Models:");
            Console.WriteLine("- UsersModel with all properties and data annotations");
            Console.WriteLine("- IUsersRepository interface for dependency injection");

            Console.WriteLine("\n?? Key Benefits:");
            Console.WriteLine("? Real-time IntelliSense as you type");
            Console.WriteLine("? Compile-time code generation");
            Console.WriteLine("? No external tools required");
            Console.WriteLine("? Seamless IDE integration");
            Console.WriteLine("? Type-safe database operations");

            Console.WriteLine("\n?? To add more entities:");
            Console.WriteLine("1. Create new YAML files in the Entities folder");
            Console.WriteLine("2. Build the project (Ctrl+Shift+B)");
            Console.WriteLine("3. Generated code appears automatically!");

            Console.WriteLine("\n? Source generation completed successfully!");
            Console.WriteLine("Check your IDE for generated code with full IntelliSense support.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"? Error: {ex.Message}");
        }

        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }
}
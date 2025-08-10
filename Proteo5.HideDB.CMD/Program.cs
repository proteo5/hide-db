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
            // Verificar argumentos
            if (args.Length > 0)
            {
                switch (args[0].ToLower())
                {
                    case "test":
                        Console.WriteLine("🧪 Modo test detectado. Generando código y preparando test...");
                        await RunTestMode();
                        return;
                    case "directtest":
                        Console.WriteLine("🔬 Ejecutando test directo de la librería...");
                        await DirectTest.RunDirectTest();
                        return;
                }
            }

            // Configurar host
            var host = CreateHostBuilder(args).Build();
            
            // Obtener servicios
            var logger = host.Services.GetRequiredService<ILogger<Program>>();
            var config = host.Services.GetRequiredService<GeneratorConfig>();
            
            logger.LogInformation("Iniciando YAML DSL Generator...");
            
            // Crear y configurar generador
            var generator = new YamlDslGenerator(config, host.Services.GetRequiredService<ILogger<YamlDslGenerator>>());
            
            // Crear archivo de ejemplo si no existe
            await CreateExampleYamlFile(config.YamlPath);
            
            // Iniciar observación
            generator.StartWatching();
            
            logger.LogInformation("🔍 Generator iniciado. Presiona 'q' para salir...");
            Console.WriteLine("\nComandos disponibles:");
            Console.WriteLine("• dotnet run test      - Generar código y mostrar ejemplo de test");
            Console.WriteLine("• dotnet run directtest - Ejecutar test directo de base de datos");
            Console.WriteLine("• q                     - Salir\n");
            
            // Esperar input del usuario
            ConsoleKey key;
            do
            {
                key = Console.ReadKey(true).Key;
            } while (key != ConsoleKey.Q);
            
            // Cleanup
            generator.Stop();
            logger.LogInformation("👋 Generator detenido. ¡Hasta luego!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error fatal: {ex.Message}");
            Console.WriteLine("Presiona cualquier tecla para salir...");
            Console.ReadKey();
        }
    }

    static async Task RunTestMode()
    {
        try
        {
            // Configurar host para el test
            var host = CreateHostBuilder(new string[0]).Build();
            var config = host.Services.GetRequiredService<GeneratorConfig>();
            
            // Crear y configurar generador
            var generator = new YamlDslGenerator(config, host.Services.GetRequiredService<ILogger<YamlDslGenerator>>());
            
            // Crear archivo de ejemplo si no existe
            await CreateExampleYamlFile(config.YamlPath);
            
            // Generar código
            Console.WriteLine("Generando código...");
            generator.StartWatching();
            await Task.Delay(3000); // Esperar a que se genere el código
            generator.Stop();
            
            // Verificar que se generó el código
            var modelsPath = Path.Combine(config.OutputPath, "Models", "UsersModel.cs");
            var repoPath = Path.Combine(config.OutputPath, "Repositories", "UsersRepository.cs");
            
            if (File.Exists(modelsPath) && File.Exists(repoPath))
            {
                Console.WriteLine("✅ Código generado exitosamente!");
                Console.WriteLine("\n📄 Archivos generados:");
                Console.WriteLine($"• {modelsPath}");
                Console.WriteLine($"• {repoPath}");
                Console.WriteLine($"• {Path.Combine(config.OutputPath, "Repositories", "IUsersRepository.cs")}");
                Console.WriteLine($"• {Path.Combine(config.SqlOutputPath, "Users_CreateTable.sql")}");
                
                Console.WriteLine("\n🧪 Para probar la librería:");
                Console.WriteLine("1. Ejecuta: dotnet run directtest");
                Console.WriteLine("2. O copia el contenido de TestExample.cs para un test personalizado");
                
                await CreateTestExample();
            }
            else
            {
                Console.WriteLine("❌ Error: No se pudo generar el código");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error en modo test: {ex.Message}");
        }
    }

    static async Task CreateTestExample()
    {
        var testCode = @"
// Ejemplo de test completo para la librería generada
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
        Console.WriteLine(""🧪 TEST DE LIBRERÍA GENERADA"");
        Console.WriteLine(""===========================\n"");
        
        try
        {
            // 1. Configuración
            var configuration = new ConfigurationBuilder()
                .AddJsonFile(""appsettings.json"")
                .Build();
            var connectionString = configuration.GetConnectionString(""DefaultConnection:ConnectionString"");
            
            if (string.IsNullOrEmpty(connectionString))
            {
                Console.WriteLine(""❌ No se encontró connection string"");
                return;
            }
            
            Console.WriteLine(""✅ Connection string configurada"");
            
            // 2. Crear repositorio
            var usersRepo = new UsersRepository(connectionString);
            Console.WriteLine(""✅ Repositorio UsersRepository creado"");
            
            // 3. Crear tabla (usando SQL directo)
            await CreateUsersTable(connectionString);
            
            // 4. Test de inserción
            Console.WriteLine(""\n📝 INSERTANDO USUARIOS..."");
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
                Console.WriteLine($""✅ {user.Username} insertado"");
            }
            
            // 5. Test de consulta
            Console.WriteLine(""\n📋 CONSULTANDO USUARIOS..."");
            var users = await usersRepo.GetAllAsync();
            Console.WriteLine($""📊 Total usuarios: {users.Count}"");
            
            foreach (var user in users)
            {
                Console.WriteLine($""👤 ID: {user.Id}, Usuario: {user.Username}, Email: {user.Email}"");
            }
            
            // 6. Test de búsqueda por ID
            Console.WriteLine(""\n🔍 BUSCANDO USUARIO POR ID..."");
            var user1 = await usersRepo.GetByIdAsync(1);
            if (user1 != null)
            {
                Console.WriteLine($""✅ Usuario encontrado: {user1.Username} - {user1.Email}"");
            }
            
            // 7. Test de búsqueda por username
            Console.WriteLine(""\n🔍 BUSCANDO USUARIO POR USERNAME..."");
            var userByName = await usersRepo.GetByUserAsync(""john"");
            if (userByName != null)
            {
                Console.WriteLine($""✅ Usuario encontrado: {userByName.Username} - {userByName.Email}"");
            }
            
            // 8. Test de actualización
            if (user1 != null)
            {
                Console.WriteLine(""\n✏️ ACTUALIZANDO USUARIO..."");
                await usersRepo.UpdateAsync(""admin_updated"", ""newhash"", ""admin.new@test.com"",
                                           ""Admin Updated"", ""User Modified"", ""inactive"", user1.Id);
                
                var updatedUser = await usersRepo.GetByIdAsync(user1.Id);
                if (updatedUser != null)
                {
                    Console.WriteLine($""✅ Usuario actualizado: {updatedUser.Username} - {updatedUser.Email}"");
                }
            }
            
            // 9. Test de consulta por estado
            Console.WriteLine(""\n🔍 CONSULTANDO USUARIOS ACTIVOS..."");
            var activeUsers = await usersRepo.GetByStatusAsync(""active"");
            Console.WriteLine($""📊 Usuarios activos: {activeUsers.Count}"");
            
            // 10. Test de conteo
            Console.WriteLine(""\n📊 CONTANDO USUARIOS ACTIVOS..."");
            var activeCount = await usersRepo.GetActiveCountAsync();
            Console.WriteLine($""✅ Conteo de usuarios activos: {activeCount}"");
            
            // 11. Cleanup - Eliminar usuarios
            Console.WriteLine(""\n🗑️ LIMPIANDO DATOS..."");
            foreach (var user in users)
            {
                await usersRepo.DeleteByIdAsync(user.Id);
            }
            Console.WriteLine(""✅ Usuarios eliminados"");
            
            // 12. Eliminar tabla
            await DropUsersTable(connectionString);
            
            Console.WriteLine(""\n✅ TEST COMPLETADO EXITOSAMENTE!"");
            Console.WriteLine(""🎉 La librería funciona perfectamente!"");
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
            PRINT 'Tabla Users creada'
        END"";
        
        using var connection = new SqlConnection(connectionString);
        using var command = new SqlCommand(sql, connection);
        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
        Console.WriteLine(""✅ Tabla Users preparada"");
    }
    
    static async Task DropUsersTable(string connectionString)
    {
        var sql = ""DROP TABLE IF EXISTS Users"";
        using var connection = new SqlConnection(connectionString);
        using var command = new SqlCommand(sql, connection);
        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
        Console.WriteLine(""✅ Tabla Users eliminada"");
    }
}";

        await File.WriteAllTextAsync("TestExample.cs", testCode);
        Console.WriteLine("✅ Archivo TestExample.cs creado con test completo");
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
                // Configurar GeneratorConfig desde appsettings
                var generatorConfig = new GeneratorConfig();
                context.Configuration.GetSection("Generator").Bind(generatorConfig);
                services.AddSingleton(generatorConfig);
                
                // Configurar logging
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
description: ""Entidad para gestión de usuarios del sistema""

fields:
  - name: ""Id""
    type: ""int""
    primaryKey: true
    autoIncrement: true
    required: true
    description: ""Identificador único del usuario""
  - name: ""Username""
    type: ""string""
    maxLength: 50
    required: true
    description: ""Nombre de usuario único""
  - name: ""PasswordHash""
    type: ""string""
    maxLength: 255
    required: true
    description: ""Hash de la contraseña""
  - name: ""Email""
    type: ""string""
    maxLength: 100
    required: true
    description: ""Dirección de correo electrónico""
  - name: ""FirstName""
    type: ""string""
    maxLength: 50
    description: ""Nombre del usuario""
  - name: ""LastName""
    type: ""string""
    maxLength: 50
    description: ""Apellido del usuario""
  - name: ""status""
    type: ""string""
    defaultValue: ""active""
    catalog: ""statuses""
    description: ""Estado del usuario""
  - name: ""CreatedAt""
    type: ""DateTime""
    defaultValue: ""CURRENT_TIMESTAMP_UTC""
    required: true
    description: ""Fecha de creación""
  - name: ""UpdatedAt""
    type: ""DateTime""
    defaultValue: ""CURRENT_TIMESTAMP_UTC""
    required: true
    description: ""Fecha de última actualización""

catalogs:
  statuses:
    - name: ""active""
      description: ""Usuario activo""
    - name: ""inactive""
      description: ""Usuario inactivo""
    - name: ""banned""
      description: ""Usuario baneado""

statements:
  - name: ""Insert""
    type: ""Insert""
    return: ""nothing""
    description: ""Crea un nuevo usuario""
    sql: |
      INSERT INTO Users (Username, PasswordHash, Email, FirstName, LastName, status)
      VALUES (@Username, @PasswordHash, @Email, @FirstName, @LastName, @status);
  - name: ""Update""
    type: ""Update""
    return: ""nothing""
    description: ""Actualiza un usuario existente""
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
    description: ""Elimina un usuario por ID""
    sql: |
      DELETE FROM Users 
      WHERE Id = @Id;
  - name: ""GetAll""
    type: ""Select""
    return: ""many""
    description: ""Obtiene todos los usuarios ordenados por fecha de creación""
    sql: |
      SELECT Id, Username, PasswordHash, Email, FirstName, LastName, status, CreatedAt, UpdatedAt
      FROM Users
      ORDER BY CreatedAt DESC;
  - name: ""GetById""
    type: ""Select""
    return: ""one""
    description: ""Obtiene un usuario por su ID""
    sql: |
      SELECT Id, Username, PasswordHash, Email, FirstName, LastName, status, CreatedAt, UpdatedAt
      FROM Users
      WHERE Id = @Id;
  - name: ""GetByUser""
    type: ""Select""
    return: ""one""
    description: ""Obtiene un usuario por nombre de usuario""
    sql: |
      SELECT Id, Username, PasswordHash, Email, FirstName, LastName, status, CreatedAt, UpdatedAt
      FROM Users
      WHERE Username = @Username;
  - name: ""GetByStatus""
    type: ""Select""
    return: ""many""
    description: ""Obtiene usuarios filtrados por estado""
    sql: |
      SELECT Id, Username, PasswordHash, Email, FirstName, LastName, status, CreatedAt, UpdatedAt
      FROM Users
      WHERE status = @status
      ORDER BY CreatedAt DESC;
  - name: ""GetActiveCount""
    type: ""Select""
    return: ""scalar""
    description: ""Obtiene el conteo de usuarios activos""
    sql: |
      SELECT COUNT(*)
      FROM Users
      WHERE status = 'active';
  - name: ""GetByEmailAndStatus""
    type: ""Select""
    return: ""many""
    description: ""Búsqueda avanzada de usuarios""
    sql: |
      SELECT Id, Username, PasswordHash, Email, FirstName, LastName, status, CreatedAt, UpdatedAt,
             CONCAT(FirstName, ' ', LastName) as Name,
             CASE 
               WHEN status = 'active' THEN 'Activo'
               WHEN status = 'inactive' THEN 'Inactivo'
               WHEN status = 'banned' THEN 'Baneado'
               ELSE 'Desconocido'
             END as Status
      FROM Users 
      WHERE (@searchTerm IS NULL OR CONCAT(FirstName, ' ', LastName) LIKE CONCAT('%', @searchTerm, '%') 
             OR Email LIKE CONCAT('%', @searchTerm, '%'))
        AND (@statusFilter IS NULL OR status = @statusFilter)
      ORDER BY CreatedAt DESC;";

            await File.WriteAllTextAsync(exampleFile, exampleContent);
            Console.WriteLine($"📄 Archivo de ejemplo creado: {exampleFile}");
        }
    }
}

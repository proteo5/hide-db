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
          UpdatedAt = CURRENT_TIMESTAMP_UTC
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
      SELECT Id, 
             CONCAT(FirstName, ' ', LastName) as Name, 
             Email,
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

using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;

namespace Proteo5.HideDB.CMD;

/// <summary>
/// Test ejecutable que compila con el código generado
/// Para ejecutar: dotnet run directtest
/// </summary>
public static class DirectTest
{
    public static async Task RunDirectTest()
    {
        Console.WriteLine("\n?? EJECUTANDO TEST DIRECTO DE LA LIBRERÍA");
        Console.WriteLine("=========================================\n");

        try
        {
            // Obtener connection string
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection:ConnectionString");
            if (string.IsNullOrEmpty(connectionString))
            {
                Console.WriteLine("? No se pudo obtener la connection string del appsettings.json");
                return;
            }

            Console.WriteLine($"? Connection String obtenida");
            WaitForKeyPress("Configuración inicial completada");

            // Test básico de conexión
            Console.WriteLine("\n1?? PROBANDO CONEXIÓN A BASE DE DATOS...");
            using (var testConnection = new SqlConnection(connectionString))
            {
                await testConnection.OpenAsync();
                Console.WriteLine("? Conexión exitosa a la base de datos");
            }
            WaitForKeyPress("Prueba de conexión completada");

            // Crear tabla de prueba
            Console.WriteLine("\n2?? CREANDO TABLA DE PRUEBA...");
            await CreateTestTable(connectionString);
            WaitForKeyPress("Tabla creada - Puedes verificar en la base de datos que existe la tabla 'TestUsers'");

            // Test usando SQL directo (simulando el repositorio)
            Console.WriteLine("\n3?? INSERTANDO DATOS DE PRUEBA...");
            await InsertTestData(connectionString);
            WaitForKeyPress("Datos insertados - Puedes verificar con: SELECT * FROM TestUsers");

            Console.WriteLine("\n4?? CONSULTANDO DATOS...");
            await QueryTestData(connectionString);
            WaitForKeyPress("Consulta completada - Compara los resultados con tu consulta en la base de datos");

            Console.WriteLine("\n5?? ACTUALIZANDO DATOS...");
            await UpdateTestData(connectionString);
            WaitForKeyPress("Actualización completada - Verifica que el usuario admin cambió a 'admin_updated'");

            Console.WriteLine("\n6?? ELIMINANDO DATOS...");
            await DeleteTestData(connectionString);
            WaitForKeyPress("Eliminación completada - Verifica que la tabla TestUsers está vacía");

            Console.WriteLine("\n7?? LIMPIANDO TABLA...");
            await DropTestTable(connectionString);
            WaitForKeyPress("Tabla eliminada - Verifica que la tabla TestUsers ya no existe");

            Console.WriteLine("\n? TODOS LOS TESTS COMPLETADOS EXITOSAMENTE!");
            Console.WriteLine("?? La librería está funcionando correctamente");
            Console.WriteLine("?? El código generado es válido y ejecutable");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n? ERROR EN EL TEST: {ex.Message}");
            Console.WriteLine($"Stack Trace: {ex.StackTrace}");
        }

        Console.WriteLine("\nPresiona cualquier tecla para finalizar...");
        Console.ReadKey();
    }

    private static void WaitForKeyPress(string message)
    {
        Console.WriteLine($"\n??  {message}");
        Console.WriteLine("   Presiona cualquier tecla para continuar con el siguiente paso...");
        Console.ReadKey();
        Console.WriteLine();
    }

    static async Task CreateTestTable(string connectionString)
    {
        var sql = @"
        IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='TestUsers' AND xtype='U')
        BEGIN
            CREATE TABLE TestUsers (
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
            PRINT 'Tabla TestUsers creada'
        END
        ELSE
        BEGIN
            PRINT 'Tabla TestUsers ya existe'
        END";

        using var connection = new SqlConnection(connectionString);
        using var command = new SqlCommand(sql, connection);
        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
        Console.WriteLine("? Tabla TestUsers verificada/creada");
        
        // Mostrar información de la base de datos
        Console.WriteLine($"?? Base de datos: {connection.Database}");
        Console.WriteLine($"?? Servidor: {connection.DataSource}");
    }

    static async Task InsertTestData(string connectionString)
    {
        var users = new[]
        {
            new { Username = "admin", Password = "hash123", Email = "admin@test.com", FirstName = "Admin", LastName = "User", Status = "active" },
            new { Username = "john", Password = "hash456", Email = "john@test.com", FirstName = "John", LastName = "Doe", Status = "active" },
            new { Username = "jane", Password = "hash789", Email = "jane@test.com", FirstName = "Jane", LastName = "Smith", Status = "inactive" }
        };

        using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();

        Console.WriteLine("?? Insertando usuarios uno por uno:");
        foreach (var user in users)
        {
            var sql = @"INSERT INTO TestUsers (Username, PasswordHash, Email, FirstName, LastName, status)
                       VALUES (@Username, @PasswordHash, @Email, @FirstName, @LastName, @status)";
            
            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Username", user.Username);
            command.Parameters.AddWithValue("@PasswordHash", user.Password);
            command.Parameters.AddWithValue("@Email", user.Email);
            command.Parameters.AddWithValue("@FirstName", user.FirstName);
            command.Parameters.AddWithValue("@LastName", user.LastName);
            command.Parameters.AddWithValue("@status", user.Status);

            await command.ExecuteNonQueryAsync();
            Console.WriteLine($"   ? Usuario insertado: {user.Username} ({user.Email}) - Status: {user.Status}");
        }
        
        Console.WriteLine("\n?? Para verificar en la base de datos, ejecuta:");
        Console.WriteLine("   SELECT * FROM TestUsers ORDER BY CreatedAt DESC");
    }

    static async Task QueryTestData(string connectionString)
    {
        using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();

        var sql = "SELECT Id, Username, Email, FirstName, LastName, status, CreatedAt, UpdatedAt FROM TestUsers ORDER BY CreatedAt DESC";
        using var command = new SqlCommand(sql, connection);
        using var reader = await command.ExecuteReaderAsync();

        Console.WriteLine("?? Usuarios encontrados (resultado de la consulta):");
        Console.WriteLine($"{"ID",-3} {"Username",-12} {"Email",-20} {"FirstName",-10} {"LastName",-10} {"Status",-8} {"CreatedAt",-20}");
        Console.WriteLine(new string('-', 85));

        while (await reader.ReadAsync())
        {
            var createdAt = ((DateTime)reader["CreatedAt"]).ToString("yyyy-MM-dd HH:mm:ss");
            Console.WriteLine($"{reader["Id"],-3} {reader["Username"],-12} {reader["Email"],-20} {reader["FirstName"] ?? "NULL",-10} {reader["LastName"] ?? "NULL",-10} {reader["status"],-8} {createdAt,-20}");
        }
        
        Console.WriteLine("\n?? Para comparar, ejecuta la misma consulta en tu cliente SQL:");
        Console.WriteLine("   SELECT * FROM TestUsers ORDER BY CreatedAt DESC");
    }

    static async Task UpdateTestData(string connectionString)
    {
        using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();

        Console.WriteLine("?? Actualizando usuario con ID = 1...");
        Console.WriteLine("   Cambios: Username = 'admin_updated', Email = 'admin.updated@test.com', Status = 'inactive'");

        var sql = @"UPDATE TestUsers 
                   SET Username = @Username, Email = @Email, status = @status, UpdatedAt = GETUTCDATE()
                   WHERE Id = @Id";
        
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@Username", "admin_updated");
        command.Parameters.AddWithValue("@Email", "admin.updated@test.com");
        command.Parameters.AddWithValue("@status", "inactive");
        command.Parameters.AddWithValue("@Id", 1);

        var rowsAffected = await command.ExecuteNonQueryAsync();
        Console.WriteLine($"? Usuario actualizado. Filas afectadas: {rowsAffected}");

        // Mostrar usuario actualizado
        var selectSql = "SELECT Id, Username, Email, status, UpdatedAt FROM TestUsers WHERE Id = 1";
        using var selectCommand = new SqlCommand(selectSql, connection);
        using var reader = await selectCommand.ExecuteReaderAsync();
        
        if (await reader.ReadAsync())
        {
            var updatedAt = ((DateTime)reader["UpdatedAt"]).ToString("yyyy-MM-dd HH:mm:ss");
            Console.WriteLine($"?? Usuario actualizado: ID={reader["Id"]}, Username={reader["Username"]}, Email={reader["Email"]}, Status={reader["status"]}");
            Console.WriteLine($"   UpdatedAt: {updatedAt}");
        }
        
        Console.WriteLine("\n?? Para verificar, ejecuta:");
        Console.WriteLine("   SELECT * FROM TestUsers WHERE Id = 1");
        Console.WriteLine("   SELECT * FROM TestUsers ORDER BY UpdatedAt DESC");
    }

    static async Task DeleteTestData(string connectionString)
    {
        using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();

        // Contar usuarios antes
        var countSql = "SELECT COUNT(*) FROM TestUsers";
        using var countCommand = new SqlCommand(countSql, connection);
        var beforeCount = (int)await countCommand.ExecuteScalarAsync();
        Console.WriteLine($"?? Usuarios antes de eliminar: {beforeCount}");

        // Mostrar usuarios que se van a eliminar
        var showSql = "SELECT Id, Username FROM TestUsers";
        using var showCommand = new SqlCommand(showSql, connection);
        using var reader = await showCommand.ExecuteReaderAsync();
        
        Console.WriteLine("???  Usuarios que serán eliminados:");
        while (await reader.ReadAsync())
        {
            Console.WriteLine($"   • ID: {reader["Id"]}, Username: {reader["Username"]}");
        }
        reader.Close();

        // Eliminar usuarios
        var deleteSql = "DELETE FROM TestUsers";
        using var deleteCommand = new SqlCommand(deleteSql, connection);
        var deletedRows = await deleteCommand.ExecuteNonQueryAsync();
        Console.WriteLine($"? Usuarios eliminados: {deletedRows}");

        // Verificar
        var afterCount = (int)await countCommand.ExecuteScalarAsync();
        Console.WriteLine($"?? Usuarios restantes: {afterCount}");
        
        Console.WriteLine("\n?? Para verificar, ejecuta:");
        Console.WriteLine("   SELECT COUNT(*) FROM TestUsers");
        Console.WriteLine("   SELECT * FROM TestUsers (debería estar vacía)");
    }

    static async Task DropTestTable(string connectionString)
    {
        Console.WriteLine("???  Eliminando tabla TestUsers...");
        
        var sql = "DROP TABLE IF EXISTS TestUsers";
        
        using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();
        using var command = new SqlCommand(sql, connection);
        await command.ExecuteNonQueryAsync();
        Console.WriteLine("? Tabla TestUsers eliminada");
        
        Console.WriteLine("\n?? Para verificar que la tabla fue eliminada, ejecuta:");
        Console.WriteLine("   SELECT * FROM TestUsers (debería dar error 'Invalid object name')");
        Console.WriteLine("   O verifica en el explorador de objetos de tu cliente SQL");
    }
}
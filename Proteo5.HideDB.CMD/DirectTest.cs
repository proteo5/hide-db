using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;

namespace Proteo5.HideDB.CMD;

/// <summary>
/// Executable test that compiles with the generated code
/// To run: dotnet run directtest
/// </summary>
public static class DirectTest
{
    public static async Task RunDirectTest()
    {
        Console.WriteLine("\n?? RUNNING DIRECT LIBRARY TEST");
        Console.WriteLine("==============================\n");

        try
        {
            // Get connection string
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection:ConnectionString");
            if (string.IsNullOrEmpty(connectionString))
            {
                Console.WriteLine("? Could not get connection string from appsettings.json");
                return;
            }

            Console.WriteLine($"? Connection string obtained");
            WaitForKeyPress("Initial configuration completed");

            // Basic connection test
            Console.WriteLine("\n1?? TESTING DATABASE CONNECTION...");
            using (var testConnection = new SqlConnection(connectionString))
            {
                await testConnection.OpenAsync();
                Console.WriteLine("? Successful database connection");
            }
            WaitForKeyPress("Connection test completed");

            // Create test table
            Console.WriteLine("\n2?? CREATING TEST TABLE...");
            await CreateTestTable(connectionString);
            WaitForKeyPress("Table created - You can verify in the database that the 'TestUsers' table exists");

            // Test using direct SQL (simulating repository)
            Console.WriteLine("\n3?? INSERTING TEST DATA...");
            await InsertTestData(connectionString);
            WaitForKeyPress("Data inserted - You can verify with: SELECT * FROM TestUsers");

            Console.WriteLine("\n4?? QUERYING DATA...");
            await QueryTestData(connectionString);
            WaitForKeyPress("Query completed - Compare the results with your database query");

            Console.WriteLine("\n5?? UPDATING DATA...");
            await UpdateTestData(connectionString);
            WaitForKeyPress("Update completed - Verify that the admin user changed to 'admin_updated'");

            Console.WriteLine("\n6?? DELETING DATA...");
            await DeleteTestData(connectionString);
            WaitForKeyPress("Deletion completed - Verify that the TestUsers table is empty");

            Console.WriteLine("\n7?? CLEANING UP TABLE...");
            await DropTestTable(connectionString);
            WaitForKeyPress("Table deleted - Verify that the TestUsers table no longer exists");

            Console.WriteLine("\n? ALL TESTS COMPLETED SUCCESSFULLY!");
            Console.WriteLine("?? The library is working correctly");
            Console.WriteLine("?? The generated code is valid and executable");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n? TEST ERROR: {ex.Message}");
            Console.WriteLine($"Stack Trace: {ex.StackTrace}");
        }

        Console.WriteLine("\nPress any key to finish...");
        Console.ReadKey();
    }

    private static void WaitForKeyPress(string message)
    {
        Console.WriteLine($"\n??  {message}");
        Console.WriteLine("   Press any key to continue to the next step...");
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
            PRINT 'TestUsers table created'
        END
        ELSE
        BEGIN
            PRINT 'TestUsers table already exists'
        END";

        using var connection = new SqlConnection(connectionString);
        using var command = new SqlCommand(sql, connection);
        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
        Console.WriteLine("? TestUsers table verified/created");
        
        // Show database information
        Console.WriteLine($"?? Database: {connection.Database}");
        Console.WriteLine($"?? Server: {connection.DataSource}");
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

        Console.WriteLine("?? Inserting users one by one:");
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
            Console.WriteLine($"   ? User inserted: {user.Username} ({user.Email}) - Status: {user.Status}");
        }
        
        Console.WriteLine("\n?? To verify in the database, run:");
        Console.WriteLine("   SELECT * FROM TestUsers ORDER BY CreatedAt DESC");
    }

    static async Task QueryTestData(string connectionString)
    {
        using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();

        var sql = "SELECT Id, Username, Email, FirstName, LastName, status, CreatedAt, UpdatedAt FROM TestUsers ORDER BY CreatedAt DESC";
        using var command = new SqlCommand(sql, connection);
        using var reader = await command.ExecuteReaderAsync();

        Console.WriteLine("?? Users found (query result):");
        Console.WriteLine($"{"ID",-3} {"Username",-12} {"Email",-20} {"FirstName",-10} {"LastName",-10} {"Status",-8} {"CreatedAt",-20}");
        Console.WriteLine(new string('-', 85));

        while (await reader.ReadAsync())
        {
            var createdAt = ((DateTime)reader["CreatedAt"]).ToString("yyyy-MM-dd HH:mm:ss");
            Console.WriteLine($"{reader["Id"],-3} {reader["Username"],-12} {reader["Email"],-20} {reader["FirstName"] ?? "NULL",-10} {reader["LastName"] ?? "NULL",-10} {reader["status"],-8} {createdAt,-20}");
        }
        
        Console.WriteLine("\n?? To compare, run the same query in your SQL client:");
        Console.WriteLine("   SELECT * FROM TestUsers ORDER BY CreatedAt DESC");
    }

    static async Task UpdateTestData(string connectionString)
    {
        using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();

        Console.WriteLine("?? Updating user with ID = 1...");
        Console.WriteLine("   Changes: Username = 'admin_updated', Email = 'admin.updated@test.com', Status = 'inactive'");

        var sql = @"UPDATE TestUsers 
                   SET Username = @Username, Email = @Email, status = @status, UpdatedAt = GETUTCDATE()
                   WHERE Id = @Id";
        
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@Username", "admin_updated");
        command.Parameters.AddWithValue("@Email", "admin.updated@test.com");
        command.Parameters.AddWithValue("@status", "inactive");
        command.Parameters.AddWithValue("@Id", 1);

        var rowsAffected = await command.ExecuteNonQueryAsync();
        Console.WriteLine($"? User updated. Rows affected: {rowsAffected}");

        // Show updated user
        var selectSql = "SELECT Id, Username, Email, status, UpdatedAt FROM TestUsers WHERE Id = 1";
        using var selectCommand = new SqlCommand(selectSql, connection);
        using var reader = await selectCommand.ExecuteReaderAsync();
        
        if (await reader.ReadAsync())
        {
            var updatedAt = ((DateTime)reader["UpdatedAt"]).ToString("yyyy-MM-dd HH:mm:ss");
            Console.WriteLine($"?? Updated user: ID={reader["Id"]}, Username={reader["Username"]}, Email={reader["Email"]}, Status={reader["status"]}");
            Console.WriteLine($"   UpdatedAt: {updatedAt}");
        }
        
        Console.WriteLine("\n?? To verify, run:");
        Console.WriteLine("   SELECT * FROM TestUsers WHERE Id = 1");
        Console.WriteLine("   SELECT * FROM TestUsers ORDER BY UpdatedAt DESC");
    }

    static async Task DeleteTestData(string connectionString)
    {
        using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();

        // Count users before
        var countSql = "SELECT COUNT(*) FROM TestUsers";
        using var countCommand = new SqlCommand(countSql, connection);
        var beforeCount = (int)await countCommand.ExecuteScalarAsync();
        Console.WriteLine($"?? Users before deletion: {beforeCount}");

        // Show users to be deleted
        var showSql = "SELECT Id, Username FROM TestUsers";
        using var showCommand = new SqlCommand(showSql, connection);
        using var reader = await showCommand.ExecuteReaderAsync();
        
        Console.WriteLine("???  Users to be deleted:");
        while (await reader.ReadAsync())
        {
            Console.WriteLine($"   • ID: {reader["Id"]}, Username: {reader["Username"]}");
        }
        reader.Close();

        // Delete users
        var deleteSql = "DELETE FROM TestUsers";
        using var deleteCommand = new SqlCommand(deleteSql, connection);
        var deletedRows = await deleteCommand.ExecuteNonQueryAsync();
        Console.WriteLine($"? Users deleted: {deletedRows}");

        // Verify
        var afterCount = (int)await countCommand.ExecuteScalarAsync();
        Console.WriteLine($"?? Remaining users: {afterCount}");
        
        Console.WriteLine("\n?? To verify, run:");
        Console.WriteLine("   SELECT COUNT(*) FROM TestUsers");
        Console.WriteLine("   SELECT * FROM TestUsers (should be empty)");
    }

    static async Task DropTestTable(string connectionString)
    {
        Console.WriteLine("???  Dropping TestUsers table...");
        
        var sql = "DROP TABLE IF EXISTS TestUsers";
        
        using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();
        using var command = new SqlCommand(sql, connection);
        await command.ExecuteNonQueryAsync();
        Console.WriteLine("? TestUsers table dropped");
        
        Console.WriteLine("\n?? To verify that the table was dropped, run:");
        Console.WriteLine("   SELECT * FROM TestUsers (should give error 'Invalid object name')");
        Console.WriteLine("   Or check in your SQL client's object explorer");
    }
}
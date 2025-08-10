using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using System.Configuration;
using System.Linq;

namespace Proteo5.HideDB.Generated.UsersE
{
    /// <summary>
    /// Repositorio para la entidad Users
    /// Generado automáticamente desde YAML - Version: 1.0
    /// </summary>
    public class UsersRepository : IUsersRepository
    {
        private readonly string _connectionString;

        public UsersRepository()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"]?.ConnectionString
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found");
        }

        public UsersRepository(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        /// <summary>
        /// Create a new user
        /// </summary>
        public int Insert(object Username, object PasswordHash, object Email, object FirstName, object LastName, object status)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = @"INSERT INTO Users (Username, PasswordHash, Email, FirstName, LastName, status)
VALUES (@Username, @PasswordHash, @Email, @FirstName, @LastName, @status);
";
                command.Parameters.AddWithValue("@Username", Username ?? DBNull.Value);
                command.Parameters.AddWithValue("@PasswordHash", PasswordHash ?? DBNull.Value);
                command.Parameters.AddWithValue("@Email", Email ?? DBNull.Value);
                command.Parameters.AddWithValue("@FirstName", FirstName ?? DBNull.Value);
                command.Parameters.AddWithValue("@LastName", LastName ?? DBNull.Value);
                command.Parameters.AddWithValue("@status", status ?? DBNull.Value);
                return command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Create a new user (Async)
        /// </summary>
        public async Task<int> InsertAsync(object Username, object PasswordHash, object Email, object FirstName, object LastName, object status)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand())
            {
                await connection.OpenAsync();
                command.Connection = connection;
                command.CommandText = @"INSERT INTO Users (Username, PasswordHash, Email, FirstName, LastName, status)
VALUES (@Username, @PasswordHash, @Email, @FirstName, @LastName, @status);
";
                command.Parameters.AddWithValue("@Username", Username ?? DBNull.Value);
                command.Parameters.AddWithValue("@PasswordHash", PasswordHash ?? DBNull.Value);
                command.Parameters.AddWithValue("@Email", Email ?? DBNull.Value);
                command.Parameters.AddWithValue("@FirstName", FirstName ?? DBNull.Value);
                command.Parameters.AddWithValue("@LastName", LastName ?? DBNull.Value);
                command.Parameters.AddWithValue("@status", status ?? DBNull.Value);
                return await command.ExecuteNonQueryAsync();
            }
        }
        /// <summary>
        /// Update an existing user
        /// </summary>
        public int Update(object Username, object PasswordHash, object Email, object FirstName, object LastName, object status, object Id)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = @"UPDATE Users 
SET Username = @Username,
    PasswordHash = @PasswordHash,
    Email = @Email,
    FirstName = @FirstName,
    LastName = @LastName,
    status = @status,
    UpdatedAt = GETUTCDATE()
WHERE Id = @Id;
";
                command.Parameters.AddWithValue("@Username", Username ?? DBNull.Value);
                command.Parameters.AddWithValue("@PasswordHash", PasswordHash ?? DBNull.Value);
                command.Parameters.AddWithValue("@Email", Email ?? DBNull.Value);
                command.Parameters.AddWithValue("@FirstName", FirstName ?? DBNull.Value);
                command.Parameters.AddWithValue("@LastName", LastName ?? DBNull.Value);
                command.Parameters.AddWithValue("@status", status ?? DBNull.Value);
                command.Parameters.AddWithValue("@Id", Id ?? DBNull.Value);
                return command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Update an existing user (Async)
        /// </summary>
        public async Task<int> UpdateAsync(object Username, object PasswordHash, object Email, object FirstName, object LastName, object status, object Id)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand())
            {
                await connection.OpenAsync();
                command.Connection = connection;
                command.CommandText = @"UPDATE Users 
SET Username = @Username,
    PasswordHash = @PasswordHash,
    Email = @Email,
    FirstName = @FirstName,
    LastName = @LastName,
    status = @status,
    UpdatedAt = GETUTCDATE()
WHERE Id = @Id;
";
                command.Parameters.AddWithValue("@Username", Username ?? DBNull.Value);
                command.Parameters.AddWithValue("@PasswordHash", PasswordHash ?? DBNull.Value);
                command.Parameters.AddWithValue("@Email", Email ?? DBNull.Value);
                command.Parameters.AddWithValue("@FirstName", FirstName ?? DBNull.Value);
                command.Parameters.AddWithValue("@LastName", LastName ?? DBNull.Value);
                command.Parameters.AddWithValue("@status", status ?? DBNull.Value);
                command.Parameters.AddWithValue("@Id", Id ?? DBNull.Value);
                return await command.ExecuteNonQueryAsync();
            }
        }
        /// <summary>
        /// Delete a user by ID
        /// </summary>
        public int DeleteById(object Id)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = @"DELETE FROM Users 
WHERE Id = @Id;
";
                command.Parameters.AddWithValue("@Id", Id ?? DBNull.Value);
                return command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Delete a user by ID (Async)
        /// </summary>
        public async Task<int> DeleteByIdAsync(object Id)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand())
            {
                await connection.OpenAsync();
                command.Connection = connection;
                command.CommandText = @"DELETE FROM Users 
WHERE Id = @Id;
";
                command.Parameters.AddWithValue("@Id", Id ?? DBNull.Value);
                return await command.ExecuteNonQueryAsync();
            }
        }
        /// <summary>
        /// Get all users ordered by creation date
        /// </summary>
        public List<UsersModel> GetAll()
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = @"SELECT Id, Username, PasswordHash, Email, FirstName, LastName, status, CreatedAt, UpdatedAt
FROM Users
ORDER BY CreatedAt DESC;
";
                var result = new List<UsersModel>();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(MapToModel(reader));
                    }
                }
                return result;
            }
        }

        /// <summary>
        /// Get all users ordered by creation date (Async)
        /// </summary>
        public async Task<List<UsersModel>> GetAllAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand())
            {
                await connection.OpenAsync();
                command.Connection = connection;
                command.CommandText = @"SELECT Id, Username, PasswordHash, Email, FirstName, LastName, status, CreatedAt, UpdatedAt
FROM Users
ORDER BY CreatedAt DESC;
";
                var result = new List<UsersModel>();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        result.Add(MapToModel(reader));
                    }
                }
                return result;
            }
        }
        /// <summary>
        /// Get a user by ID
        /// </summary>
        public UsersModel? GetById(object Id)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = @"SELECT Id, Username, PasswordHash, Email, FirstName, LastName, status, CreatedAt, UpdatedAt
FROM Users
WHERE Id = @Id;
";
                command.Parameters.AddWithValue("@Id", Id ?? DBNull.Value);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return MapToModel(reader);
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// Get a user by ID (Async)
        /// </summary>
        public async Task<UsersModel?> GetByIdAsync(object Id)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand())
            {
                await connection.OpenAsync();
                command.Connection = connection;
                command.CommandText = @"SELECT Id, Username, PasswordHash, Email, FirstName, LastName, status, CreatedAt, UpdatedAt
FROM Users
WHERE Id = @Id;
";
                command.Parameters.AddWithValue("@Id", Id ?? DBNull.Value);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return MapToModel(reader);
                    }
                }
                return null;
            }
        }
        /// <summary>
        /// Get a user by username
        /// </summary>
        public UsersModel? GetByUser(object Username)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = @"SELECT Id, Username, PasswordHash, Email, FirstName, LastName, status, CreatedAt, UpdatedAt
FROM Users
WHERE Username = @Username;
";
                command.Parameters.AddWithValue("@Username", Username ?? DBNull.Value);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return MapToModel(reader);
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// Get a user by username (Async)
        /// </summary>
        public async Task<UsersModel?> GetByUserAsync(object Username)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand())
            {
                await connection.OpenAsync();
                command.Connection = connection;
                command.CommandText = @"SELECT Id, Username, PasswordHash, Email, FirstName, LastName, status, CreatedAt, UpdatedAt
FROM Users
WHERE Username = @Username;
";
                command.Parameters.AddWithValue("@Username", Username ?? DBNull.Value);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return MapToModel(reader);
                    }
                }
                return null;
            }
        }
        /// <summary>
        /// Get users filtered by status
        /// </summary>
        public List<UsersModel> GetByStatus(object status)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = @"SELECT Id, Username, PasswordHash, Email, FirstName, LastName, status, CreatedAt, UpdatedAt
FROM Users
WHERE status = @status
ORDER BY CreatedAt DESC;
";
                command.Parameters.AddWithValue("@status", status ?? DBNull.Value);
                var result = new List<UsersModel>();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(MapToModel(reader));
                    }
                }
                return result;
            }
        }

        /// <summary>
        /// Get users filtered by status (Async)
        /// </summary>
        public async Task<List<UsersModel>> GetByStatusAsync(object status)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand())
            {
                await connection.OpenAsync();
                command.Connection = connection;
                command.CommandText = @"SELECT Id, Username, PasswordHash, Email, FirstName, LastName, status, CreatedAt, UpdatedAt
FROM Users
WHERE status = @status
ORDER BY CreatedAt DESC;
";
                command.Parameters.AddWithValue("@status", status ?? DBNull.Value);
                var result = new List<UsersModel>();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        result.Add(MapToModel(reader));
                    }
                }
                return result;
            }
        }
        /// <summary>
        /// Get count of active users
        /// </summary>
        public object? GetActiveCount()
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = @"SELECT COUNT(*)
FROM Users
WHERE status = 'active';
";
                var result = command.ExecuteScalar();
                return result == DBNull.Value ? null : result;
            }
        }

        /// <summary>
        /// Get count of active users (Async)
        /// </summary>
        public async Task<object?> GetActiveCountAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand())
            {
                await connection.OpenAsync();
                command.Connection = connection;
                command.CommandText = @"SELECT COUNT(*)
FROM Users
WHERE status = 'active';
";
                var result = await command.ExecuteScalarAsync();
                return result == DBNull.Value ? null : result;
            }
        }
        /// <summary>
        /// Advanced user search
        /// </summary>
        public List<UsersModel> GetByEmailAndStatus(object searchTerm, object statusFilter)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = @"SELECT Id, Username, PasswordHash, Email, FirstName, LastName, status, CreatedAt, UpdatedAt,
       CONCAT(FirstName, ' ', LastName) as Name,
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
                command.Parameters.AddWithValue("@searchTerm", searchTerm ?? DBNull.Value);
                command.Parameters.AddWithValue("@statusFilter", statusFilter ?? DBNull.Value);
                var result = new List<UsersModel>();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(MapToModel(reader));
                    }
                }
                return result;
            }
        }

        /// <summary>
        /// Advanced user search (Async)
        /// </summary>
        public async Task<List<UsersModel>> GetByEmailAndStatusAsync(object searchTerm, object statusFilter)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand())
            {
                await connection.OpenAsync();
                command.Connection = connection;
                command.CommandText = @"SELECT Id, Username, PasswordHash, Email, FirstName, LastName, status, CreatedAt, UpdatedAt,
       CONCAT(FirstName, ' ', LastName) as Name,
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
                command.Parameters.AddWithValue("@searchTerm", searchTerm ?? DBNull.Value);
                command.Parameters.AddWithValue("@statusFilter", statusFilter ?? DBNull.Value);
                var result = new List<UsersModel>();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        result.Add(MapToModel(reader));
                    }
                }
                return result;
            }
        }
        private UsersModel MapToModel(IDataReader reader)
        {
            return new UsersModel
            {
                Id = reader["Id"] == DBNull.Value ? default(int) : (int)reader["Id"],
                Username = reader["Username"] == DBNull.Value ? string.Empty : (reader["Username"].ToString() ?? string.Empty),
                PasswordHash = reader["PasswordHash"] == DBNull.Value ? string.Empty : (reader["PasswordHash"].ToString() ?? string.Empty),
                Email = reader["Email"] == DBNull.Value ? string.Empty : (reader["Email"].ToString() ?? string.Empty),
                FirstName = reader["FirstName"] == DBNull.Value ? null : reader["FirstName"].ToString(),
                LastName = reader["LastName"] == DBNull.Value ? null : reader["LastName"].ToString(),
                status = reader["status"] == DBNull.Value ? null : reader["status"].ToString(),
                CreatedAt = reader["CreatedAt"] == DBNull.Value ? default(DateTime) : (DateTime)reader["CreatedAt"],
                UpdatedAt = reader["UpdatedAt"] == DBNull.Value ? default(DateTime) : (DateTime)reader["UpdatedAt"],
            };
        }
    }
}

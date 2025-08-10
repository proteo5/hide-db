using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using System.Configuration;
using System.Linq;
using Proteo5.HideDB.Generated.Models;
using Proteo5.HideDB.Generated.Repositories;

namespace Proteo5.HideDB.Generated.Repositories
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
        /// Crea un nuevo usuario
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
        /// Crea un nuevo usuario (Async)
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
        /// Actualiza un usuario existente
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
    UpdatedAt = CURRENT_TIMESTAMP_UTC
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
        /// Actualiza un usuario existente (Async)
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
    UpdatedAt = CURRENT_TIMESTAMP_UTC
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
        /// Elimina un usuario por ID
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
        /// Elimina un usuario por ID (Async)
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
        /// Obtiene todos los usuarios ordenados por fecha de creaci�n
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
        /// Obtiene todos los usuarios ordenados por fecha de creaci�n (Async)
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
        /// Obtiene un usuario por su ID
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
        /// Obtiene un usuario por su ID (Async)
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
        /// Obtiene un usuario por nombre de usuario
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
        /// Obtiene un usuario por nombre de usuario (Async)
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
        /// Obtiene usuarios filtrados por estado
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
        /// Obtiene usuarios filtrados por estado (Async)
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
        /// Obtiene el conteo de usuarios activos
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
        /// Obtiene el conteo de usuarios activos (Async)
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
        /// B�squeda avanzada de usuarios
        /// </summary>
        public List<UsersModel> GetByEmailAndStatus(object searchTerm, object statusFilter)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = @"SELECT Id, 
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
        /// B�squeda avanzada de usuarios (Async)
        /// </summary>
        public async Task<List<UsersModel>> GetByEmailAndStatusAsync(object searchTerm, object statusFilter)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand())
            {
                await connection.OpenAsync();
                command.Connection = connection;
                command.CommandText = @"SELECT Id, 
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
                Username = reader["Username"] == DBNull.Value ? string.Empty : (string)reader["Username"],
                PasswordHash = reader["PasswordHash"] == DBNull.Value ? string.Empty : (string)reader["PasswordHash"],
                Email = reader["Email"] == DBNull.Value ? string.Empty : (string)reader["Email"],
                FirstName = reader["FirstName"] == DBNull.Value ? null : (string?)reader["FirstName"],
                LastName = reader["LastName"] == DBNull.Value ? null : (string?)reader["LastName"],
                status = reader["status"] == DBNull.Value ? null : (string?)reader["status"],
                CreatedAt = reader["CreatedAt"] == DBNull.Value ? default(DateTime) : (DateTime)reader["CreatedAt"],
                UpdatedAt = reader["UpdatedAt"] == DBNull.Value ? default(DateTime) : (DateTime)reader["UpdatedAt"],
            };
        }
    }
}

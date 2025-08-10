using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;
using System.Configuration;
using System.Linq;

namespace Proteo5.HideDB.Generated.RolesE
{
    /// <summary>
    /// Repositorio para la entidad Roles
    /// Generado automáticamente desde YAML - Version: 1.0
    /// </summary>
    public class RolesRepository : IRolesRepository
    {
        private readonly string _connectionString;

        public RolesRepository()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"]?.ConnectionString
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found");
        }

        public RolesRepository(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        /// <summary>
        /// Create a new role
        /// </summary>
        public int Insert(object Name, object Description, object Status)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = @"INSERT INTO Roles (Name, Description, Status)
VALUES (@Name, @Description, @Status);
";
                command.Parameters.AddWithValue("@Name", Name ?? DBNull.Value);
                command.Parameters.AddWithValue("@Description", Description ?? DBNull.Value);
                command.Parameters.AddWithValue("@Status", Status ?? DBNull.Value);
                return command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Create a new role (Async)
        /// </summary>
        public async Task<int> InsertAsync(object Name, object Description, object Status)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand())
            {
                await connection.OpenAsync();
                command.Connection = connection;
                command.CommandText = @"INSERT INTO Roles (Name, Description, Status)
VALUES (@Name, @Description, @Status);
";
                command.Parameters.AddWithValue("@Name", Name ?? DBNull.Value);
                command.Parameters.AddWithValue("@Description", Description ?? DBNull.Value);
                command.Parameters.AddWithValue("@Status", Status ?? DBNull.Value);
                return await command.ExecuteNonQueryAsync();
            }
        }
        /// <summary>
        /// Update an existing role
        /// </summary>
        public int Update(object Name, object Description, object Status, object Id)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = @"UPDATE Roles 
SET Name = @Name,
    Description = @Description,
    Status = @Status,
    UpdatedAt = GETUTCDATE()
WHERE Id = @Id;
";
                command.Parameters.AddWithValue("@Name", Name ?? DBNull.Value);
                command.Parameters.AddWithValue("@Description", Description ?? DBNull.Value);
                command.Parameters.AddWithValue("@Status", Status ?? DBNull.Value);
                command.Parameters.AddWithValue("@Id", Id ?? DBNull.Value);
                return command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Update an existing role (Async)
        /// </summary>
        public async Task<int> UpdateAsync(object Name, object Description, object Status, object Id)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand())
            {
                await connection.OpenAsync();
                command.Connection = connection;
                command.CommandText = @"UPDATE Roles 
SET Name = @Name,
    Description = @Description,
    Status = @Status,
    UpdatedAt = GETUTCDATE()
WHERE Id = @Id;
";
                command.Parameters.AddWithValue("@Name", Name ?? DBNull.Value);
                command.Parameters.AddWithValue("@Description", Description ?? DBNull.Value);
                command.Parameters.AddWithValue("@Status", Status ?? DBNull.Value);
                command.Parameters.AddWithValue("@Id", Id ?? DBNull.Value);
                return await command.ExecuteNonQueryAsync();
            }
        }
        /// <summary>
        /// Delete a role by ID
        /// </summary>
        public int DeleteById(object Id)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = @"DELETE FROM Roles 
WHERE Id = @Id;
";
                command.Parameters.AddWithValue("@Id", Id ?? DBNull.Value);
                return command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Delete a role by ID (Async)
        /// </summary>
        public async Task<int> DeleteByIdAsync(object Id)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand())
            {
                await connection.OpenAsync();
                command.Connection = connection;
                command.CommandText = @"DELETE FROM Roles 
WHERE Id = @Id;
";
                command.Parameters.AddWithValue("@Id", Id ?? DBNull.Value);
                return await command.ExecuteNonQueryAsync();
            }
        }
        /// <summary>
        /// Get all roles ordered by name
        /// </summary>
        public List<RolesModel> GetAll()
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = @"SELECT Id, Name, Description, Status, CreatedAt, UpdatedAt
FROM Roles
ORDER BY Name ASC;
";
                var result = new List<RolesModel>();
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
        /// Get all roles ordered by name (Async)
        /// </summary>
        public async Task<List<RolesModel>> GetAllAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand())
            {
                await connection.OpenAsync();
                command.Connection = connection;
                command.CommandText = @"SELECT Id, Name, Description, Status, CreatedAt, UpdatedAt
FROM Roles
ORDER BY Name ASC;
";
                var result = new List<RolesModel>();
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
        /// Get a role by ID
        /// </summary>
        public RolesModel? GetById(object Id)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = @"SELECT Id, Name, Description, Status, CreatedAt, UpdatedAt
FROM Roles
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
        /// Get a role by ID (Async)
        /// </summary>
        public async Task<RolesModel?> GetByIdAsync(object Id)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand())
            {
                await connection.OpenAsync();
                command.Connection = connection;
                command.CommandText = @"SELECT Id, Name, Description, Status, CreatedAt, UpdatedAt
FROM Roles
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
        /// Get a role by name
        /// </summary>
        public RolesModel? GetByName(object Name)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = @"SELECT Id, Name, Description, Status, CreatedAt, UpdatedAt
FROM Roles
WHERE Name = @Name;
";
                command.Parameters.AddWithValue("@Name", Name ?? DBNull.Value);
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
        /// Get a role by name (Async)
        /// </summary>
        public async Task<RolesModel?> GetByNameAsync(object Name)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand())
            {
                await connection.OpenAsync();
                command.Connection = connection;
                command.CommandText = @"SELECT Id, Name, Description, Status, CreatedAt, UpdatedAt
FROM Roles
WHERE Name = @Name;
";
                command.Parameters.AddWithValue("@Name", Name ?? DBNull.Value);
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
        /// Get all roles by status
        /// </summary>
        public List<RolesModel> GetByStatus(object Status)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = @"SELECT Id, Name, Description, Status, CreatedAt, UpdatedAt
FROM Roles
WHERE Status = @Status
ORDER BY Name ASC;
";
                command.Parameters.AddWithValue("@Status", Status ?? DBNull.Value);
                var result = new List<RolesModel>();
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
        /// Get all roles by status (Async)
        /// </summary>
        public async Task<List<RolesModel>> GetByStatusAsync(object Status)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand())
            {
                await connection.OpenAsync();
                command.Connection = connection;
                command.CommandText = @"SELECT Id, Name, Description, Status, CreatedAt, UpdatedAt
FROM Roles
WHERE Status = @Status
ORDER BY Name ASC;
";
                command.Parameters.AddWithValue("@Status", Status ?? DBNull.Value);
                var result = new List<RolesModel>();
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
        /// Get all active roles
        /// </summary>
        public List<RolesModel> GetActive()
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = @"SELECT Id, Name, Description, Status, CreatedAt, UpdatedAt
FROM Roles
WHERE Status = 'active'
ORDER BY Name ASC;
";
                var result = new List<RolesModel>();
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
        /// Get all active roles (Async)
        /// </summary>
        public async Task<List<RolesModel>> GetActiveAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand())
            {
                await connection.OpenAsync();
                command.Connection = connection;
                command.CommandText = @"SELECT Id, Name, Description, Status, CreatedAt, UpdatedAt
FROM Roles
WHERE Status = 'active'
ORDER BY Name ASC;
";
                var result = new List<RolesModel>();
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
        /// Get count of active roles
        /// </summary>
        public object? GetActiveCount()
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = @"SELECT COUNT(*)
FROM Roles
WHERE Status = 'active';
";
                var result = command.ExecuteScalar();
                return result == DBNull.Value ? null : result;
            }
        }

        /// <summary>
        /// Get count of active roles (Async)
        /// </summary>
        public async Task<object?> GetActiveCountAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand())
            {
                await connection.OpenAsync();
                command.Connection = connection;
                command.CommandText = @"SELECT COUNT(*)
FROM Roles
WHERE Status = 'active';
";
                var result = await command.ExecuteScalarAsync();
                return result == DBNull.Value ? null : result;
            }
        }
        /// <summary>
        /// Update role status
        /// </summary>
        public int SetStatus(object Status, object Id)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = @"UPDATE Roles 
SET Status = @Status,
    UpdatedAt = GETUTCDATE()
WHERE Id = @Id;
";
                command.Parameters.AddWithValue("@Status", Status ?? DBNull.Value);
                command.Parameters.AddWithValue("@Id", Id ?? DBNull.Value);
                return command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Update role status (Async)
        /// </summary>
        public async Task<int> SetStatusAsync(object Status, object Id)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand())
            {
                await connection.OpenAsync();
                command.Connection = connection;
                command.CommandText = @"UPDATE Roles 
SET Status = @Status,
    UpdatedAt = GETUTCDATE()
WHERE Id = @Id;
";
                command.Parameters.AddWithValue("@Status", Status ?? DBNull.Value);
                command.Parameters.AddWithValue("@Id", Id ?? DBNull.Value);
                return await command.ExecuteNonQueryAsync();
            }
        }
        /// <summary>
        /// Activate a role
        /// </summary>
        public int ActivateRole(object Id)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = @"UPDATE Roles 
SET Status = 'active',
    UpdatedAt = GETUTCDATE()
WHERE Id = @Id;
";
                command.Parameters.AddWithValue("@Id", Id ?? DBNull.Value);
                return command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Activate a role (Async)
        /// </summary>
        public async Task<int> ActivateRoleAsync(object Id)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand())
            {
                await connection.OpenAsync();
                command.Connection = connection;
                command.CommandText = @"UPDATE Roles 
SET Status = 'active',
    UpdatedAt = GETUTCDATE()
WHERE Id = @Id;
";
                command.Parameters.AddWithValue("@Id", Id ?? DBNull.Value);
                return await command.ExecuteNonQueryAsync();
            }
        }
        /// <summary>
        /// Deactivate a role
        /// </summary>
        public int DeactivateRole(object Id)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = @"UPDATE Roles 
SET Status = 'inactive',
    UpdatedAt = GETUTCDATE()
WHERE Id = @Id;
";
                command.Parameters.AddWithValue("@Id", Id ?? DBNull.Value);
                return command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Deactivate a role (Async)
        /// </summary>
        public async Task<int> DeactivateRoleAsync(object Id)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand())
            {
                await connection.OpenAsync();
                command.Connection = connection;
                command.CommandText = @"UPDATE Roles 
SET Status = 'inactive',
    UpdatedAt = GETUTCDATE()
WHERE Id = @Id;
";
                command.Parameters.AddWithValue("@Id", Id ?? DBNull.Value);
                return await command.ExecuteNonQueryAsync();
            }
        }
        /// <summary>
        /// Archive a role
        /// </summary>
        public int ArchiveRole(object Id)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = @"UPDATE Roles 
SET Status = 'archived',
    UpdatedAt = GETUTCDATE()
WHERE Id = @Id;";
                command.Parameters.AddWithValue("@Id", Id ?? DBNull.Value);
                return command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Archive a role (Async)
        /// </summary>
        public async Task<int> ArchiveRoleAsync(object Id)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand())
            {
                await connection.OpenAsync();
                command.Connection = connection;
                command.CommandText = @"UPDATE Roles 
SET Status = 'archived',
    UpdatedAt = GETUTCDATE()
WHERE Id = @Id;";
                command.Parameters.AddWithValue("@Id", Id ?? DBNull.Value);
                return await command.ExecuteNonQueryAsync();
            }
        }
        private RolesModel MapToModel(IDataReader reader)
        {
            return new RolesModel
            {
                Id = reader["Id"] == DBNull.Value ? default(int) : (int)reader["Id"],
                Name = reader["Name"] == DBNull.Value ? string.Empty : (reader["Name"].ToString() ?? string.Empty),
                Description = reader["Description"] == DBNull.Value ? null : reader["Description"].ToString(),
                Status = reader["Status"] == DBNull.Value ? string.Empty : (reader["Status"].ToString() ?? string.Empty),
                CreatedAt = reader["CreatedAt"] == DBNull.Value ? default(DateTime) : (DateTime)reader["CreatedAt"],
                UpdatedAt = reader["UpdatedAt"] == DBNull.Value ? default(DateTime) : (DateTime)reader["UpdatedAt"],
            };
        }
    }
}

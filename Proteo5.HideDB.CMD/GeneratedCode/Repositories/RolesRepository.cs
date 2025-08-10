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
        /// Crea un nuevo rol
        /// </summary>
        public int Insert(object Name, object Description, object IsActive)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = @"INSERT INTO Roles (Name, Description, IsActive)
VALUES (@Name, @Description, @IsActive);
";
                command.Parameters.AddWithValue("@Name", Name ?? DBNull.Value);
                command.Parameters.AddWithValue("@Description", Description ?? DBNull.Value);
                command.Parameters.AddWithValue("@IsActive", IsActive ?? DBNull.Value);
                return command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Crea un nuevo rol (Async)
        /// </summary>
        public async Task<int> InsertAsync(object Name, object Description, object IsActive)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand())
            {
                await connection.OpenAsync();
                command.Connection = connection;
                command.CommandText = @"INSERT INTO Roles (Name, Description, IsActive)
VALUES (@Name, @Description, @IsActive);
";
                command.Parameters.AddWithValue("@Name", Name ?? DBNull.Value);
                command.Parameters.AddWithValue("@Description", Description ?? DBNull.Value);
                command.Parameters.AddWithValue("@IsActive", IsActive ?? DBNull.Value);
                return await command.ExecuteNonQueryAsync();
            }
        }

        /// <summary>
        /// Actualiza un rol existente
        /// </summary>
        public int Update(object Name, object Description, object IsActive, object Id)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = @"UPDATE Roles 
SET Name = @Name,
    Description = @Description,
    IsActive = @IsActive,
    UpdatedAt = CURRENT_TIMESTAMP_UTC
WHERE Id = @Id;
";
                command.Parameters.AddWithValue("@Name", Name ?? DBNull.Value);
                command.Parameters.AddWithValue("@Description", Description ?? DBNull.Value);
                command.Parameters.AddWithValue("@IsActive", IsActive ?? DBNull.Value);
                command.Parameters.AddWithValue("@Id", Id ?? DBNull.Value);
                return command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Actualiza un rol existente (Async)
        /// </summary>
        public async Task<int> UpdateAsync(object Name, object Description, object IsActive, object Id)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand())
            {
                await connection.OpenAsync();
                command.Connection = connection;
                command.CommandText = @"UPDATE Roles 
SET Name = @Name,
    Description = @Description,
    IsActive = @IsActive,
    UpdatedAt = CURRENT_TIMESTAMP_UTC
WHERE Id = @Id;
";
                command.Parameters.AddWithValue("@Name", Name ?? DBNull.Value);
                command.Parameters.AddWithValue("@Description", Description ?? DBNull.Value);
                command.Parameters.AddWithValue("@IsActive", IsActive ?? DBNull.Value);
                command.Parameters.AddWithValue("@Id", Id ?? DBNull.Value);
                return await command.ExecuteNonQueryAsync();
            }
        }

        /// <summary>
        /// Elimina un rol por ID
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
        /// Elimina un rol por ID (Async)
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
        /// Obtiene todos los roles ordenados por nombre
        /// </summary>
        public List<RolesModel> GetAll()
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = @"SELECT Id, Name, Description, IsActive, CreatedAt, UpdatedAt
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
        /// Obtiene todos los roles ordenados por nombre (Async)
        /// </summary>
        public async Task<List<RolesModel>> GetAllAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand())
            {
                await connection.OpenAsync();
                command.Connection = connection;
                command.CommandText = @"SELECT Id, Name, Description, IsActive, CreatedAt, UpdatedAt
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
        /// Obtiene un rol por su ID
        /// </summary>
        public RolesModel? GetById(object Id)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = @"SELECT Id, Name, Description, IsActive, CreatedAt, UpdatedAt
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
        /// Obtiene un rol por su ID (Async)
        /// </summary>
        public async Task<RolesModel?> GetByIdAsync(object Id)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand())
            {
                await connection.OpenAsync();
                command.Connection = connection;
                command.CommandText = @"SELECT Id, Name, Description, IsActive, CreatedAt, UpdatedAt
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
        /// Obtiene un rol por su nombre
        /// </summary>
        public RolesModel? GetByName(object Name)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = @"SELECT Id, Name, Description, IsActive, CreatedAt, UpdatedAt
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
        /// Obtiene un rol por su nombre (Async)
        /// </summary>
        public async Task<RolesModel?> GetByNameAsync(object Name)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand())
            {
                await connection.OpenAsync();
                command.Connection = connection;
                command.CommandText = @"SELECT Id, Name, Description, IsActive, CreatedAt, UpdatedAt
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
        /// Obtiene todos los roles activos
        /// </summary>
        public List<RolesModel> GetActive()
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = @"SELECT Id, Name, Description, IsActive, CreatedAt, UpdatedAt
FROM Roles
WHERE IsActive = 1
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
        /// Obtiene todos los roles activos (Async)
        /// </summary>
        public async Task<List<RolesModel>> GetActiveAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand())
            {
                await connection.OpenAsync();
                command.Connection = connection;
                command.CommandText = @"SELECT Id, Name, Description, IsActive, CreatedAt, UpdatedAt
FROM Roles
WHERE IsActive = 1
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
        /// Obtiene el conteo de roles activos
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
WHERE IsActive = 1;
";
                var result = command.ExecuteScalar();
                return result == DBNull.Value ? null : result;
            }
        }

        /// <summary>
        /// Obtiene el conteo de roles activos (Async)
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
WHERE IsActive = 1;
";
                var result = await command.ExecuteScalarAsync();
                return result == DBNull.Value ? null : result;
            }
        }

        /// <summary>
        /// Activa o desactiva un rol
        /// </summary>
        public int SetActiveStatus(object IsActive, object Id)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = @"UPDATE Roles 
SET IsActive = @IsActive,
    UpdatedAt = CURRENT_TIMESTAMP_UTC
WHERE Id = @Id;";
                command.Parameters.AddWithValue("@IsActive", IsActive ?? DBNull.Value);
                command.Parameters.AddWithValue("@Id", Id ?? DBNull.Value);
                return command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Activa o desactiva un rol (Async)
        /// </summary>
        public async Task<int> SetActiveStatusAsync(object IsActive, object Id)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand())
            {
                await connection.OpenAsync();
                command.Connection = connection;
                command.CommandText = @"UPDATE Roles 
SET IsActive = @IsActive,
    UpdatedAt = CURRENT_TIMESTAMP_UTC
WHERE Id = @Id;";
                command.Parameters.AddWithValue("@IsActive", IsActive ?? DBNull.Value);
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
                IsActive = reader["IsActive"] == DBNull.Value ? default(bool) : (bool)reader["IsActive"],
                CreatedAt = reader["CreatedAt"] == DBNull.Value ? default(DateTime) : (DateTime)reader["CreatedAt"],
                UpdatedAt = reader["UpdatedAt"] == DBNull.Value ? default(DateTime) : (DateTime)reader["UpdatedAt"],
            };
        }
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Proteo5.HideDB.Generated.UsersE
{
    /// <summary>
    /// Interfaz del repositorio para Users
    /// </summary>
    public interface IUsersRepository
    {
        /// <summary>
        /// Create a new user
        /// </summary>
        int Insert(object Username, object PasswordHash, object Email, object FirstName, object LastName, object status);

        /// <summary>
        /// Create a new user (Async)
        /// </summary>
        Task<int> InsertAsync(object Username, object PasswordHash, object Email, object FirstName, object LastName, object status);

        /// <summary>
        /// Update an existing user
        /// </summary>
        int Update(object Username, object PasswordHash, object Email, object FirstName, object LastName, object status, object Id);

        /// <summary>
        /// Update an existing user (Async)
        /// </summary>
        Task<int> UpdateAsync(object Username, object PasswordHash, object Email, object FirstName, object LastName, object status, object Id);

        /// <summary>
        /// Delete a user by ID
        /// </summary>
        int DeleteById(object Id);

        /// <summary>
        /// Delete a user by ID (Async)
        /// </summary>
        Task<int> DeleteByIdAsync(object Id);

        /// <summary>
        /// Get all users ordered by creation date
        /// </summary>
        List<UsersModel> GetAll();

        /// <summary>
        /// Get all users ordered by creation date (Async)
        /// </summary>
        Task<List<UsersModel>> GetAllAsync();

        /// <summary>
        /// Get a user by ID
        /// </summary>
        UsersModel? GetById(object Id);

        /// <summary>
        /// Get a user by ID (Async)
        /// </summary>
        Task<UsersModel?> GetByIdAsync(object Id);

        /// <summary>
        /// Get a user by username
        /// </summary>
        UsersModel? GetByUser(object Username);

        /// <summary>
        /// Get a user by username (Async)
        /// </summary>
        Task<UsersModel?> GetByUserAsync(object Username);

        /// <summary>
        /// Get users filtered by status
        /// </summary>
        List<UsersModel> GetByStatus(object status);

        /// <summary>
        /// Get users filtered by status (Async)
        /// </summary>
        Task<List<UsersModel>> GetByStatusAsync(object status);

        /// <summary>
        /// Get count of active users
        /// </summary>
        object? GetActiveCount();

        /// <summary>
        /// Get count of active users (Async)
        /// </summary>
        Task<object?> GetActiveCountAsync();

        /// <summary>
        /// Advanced user search
        /// </summary>
        List<UsersModel> GetByEmailAndStatus(object searchTerm, object statusFilter);

        /// <summary>
        /// Advanced user search (Async)
        /// </summary>
        Task<List<UsersModel>> GetByEmailAndStatusAsync(object searchTerm, object statusFilter);

    }
}

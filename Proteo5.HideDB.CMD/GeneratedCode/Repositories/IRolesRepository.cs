using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Proteo5.HideDB.Generated.Models;

namespace Proteo5.HideDB.Generated.Repositories
{
    /// <summary>
    /// Interfaz del repositorio para Roles
    /// </summary>
    public interface IRolesRepository
    {
        /// <summary>
        /// Create a new role
        /// </summary>
        int Insert(object Name, object Description, object IsActive);

        /// <summary>
        /// Create a new role (Async)
        /// </summary>
        Task<int> InsertAsync(object Name, object Description, object IsActive);

        /// <summary>
        /// Update an existing role
        /// </summary>
        int Update(object Name, object Description, object IsActive, object Id);

        /// <summary>
        /// Update an existing role (Async)
        /// </summary>
        Task<int> UpdateAsync(object Name, object Description, object IsActive, object Id);

        /// <summary>
        /// Delete a role by ID
        /// </summary>
        int DeleteById(object Id);

        /// <summary>
        /// Delete a role by ID (Async)
        /// </summary>
        Task<int> DeleteByIdAsync(object Id);

        /// <summary>
        /// Get all roles ordered by name
        /// </summary>
        List<RolesModel> GetAll();

        /// <summary>
        /// Get all roles ordered by name (Async)
        /// </summary>
        Task<List<RolesModel>> GetAllAsync();

        /// <summary>
        /// Get a role by ID
        /// </summary>
        RolesModel? GetById(object Id);

        /// <summary>
        /// Get a role by ID (Async)
        /// </summary>
        Task<RolesModel?> GetByIdAsync(object Id);

        /// <summary>
        /// Get a role by name
        /// </summary>
        RolesModel? GetByName(object Name);

        /// <summary>
        /// Get a role by name (Async)
        /// </summary>
        Task<RolesModel?> GetByNameAsync(object Name);

        /// <summary>
        /// Get all active roles
        /// </summary>
        List<RolesModel> GetActive();

        /// <summary>
        /// Get all active roles (Async)
        /// </summary>
        Task<List<RolesModel>> GetActiveAsync();

        /// <summary>
        /// Get count of active roles
        /// </summary>
        object? GetActiveCount();

        /// <summary>
        /// Get count of active roles (Async)
        /// </summary>
        Task<object?> GetActiveCountAsync();

        /// <summary>
        /// Activate or deactivate a role
        /// </summary>
        int SetActiveStatus(object IsActive, object Id);

        /// <summary>
        /// Activate or deactivate a role (Async)
        /// </summary>
        Task<int> SetActiveStatusAsync(object IsActive, object Id);

    }
}

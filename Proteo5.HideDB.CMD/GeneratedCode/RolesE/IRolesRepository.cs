using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Proteo5.HideDB.Generated.RolesE
{
    /// <summary>
    /// Interfaz del repositorio para Roles
    /// </summary>
    public interface IRolesRepository
    {
        /// <summary>
        /// Create a new role
        /// </summary>
        int Insert(object Name, object Description, object Status);

        /// <summary>
        /// Create a new role (Async)
        /// </summary>
        Task<int> InsertAsync(object Name, object Description, object Status);

        /// <summary>
        /// Update an existing role
        /// </summary>
        int Update(object Name, object Description, object Status, object Id);

        /// <summary>
        /// Update an existing role (Async)
        /// </summary>
        Task<int> UpdateAsync(object Name, object Description, object Status, object Id);

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
        /// Get all roles by status
        /// </summary>
        List<RolesModel> GetByStatus(object Status);

        /// <summary>
        /// Get all roles by status (Async)
        /// </summary>
        Task<List<RolesModel>> GetByStatusAsync(object Status);

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
        /// Update role status
        /// </summary>
        int SetStatus(object Status, object Id);

        /// <summary>
        /// Update role status (Async)
        /// </summary>
        Task<int> SetStatusAsync(object Status, object Id);

        /// <summary>
        /// Activate a role
        /// </summary>
        int ActivateRole(object Id);

        /// <summary>
        /// Activate a role (Async)
        /// </summary>
        Task<int> ActivateRoleAsync(object Id);

        /// <summary>
        /// Deactivate a role
        /// </summary>
        int DeactivateRole(object Id);

        /// <summary>
        /// Deactivate a role (Async)
        /// </summary>
        Task<int> DeactivateRoleAsync(object Id);

        /// <summary>
        /// Archive a role
        /// </summary>
        int ArchiveRole(object Id);

        /// <summary>
        /// Archive a role (Async)
        /// </summary>
        Task<int> ArchiveRoleAsync(object Id);

    }
}

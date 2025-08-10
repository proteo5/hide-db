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
        /// Crea un nuevo rol
        /// </summary>
        int Insert(object Name, object Description, object IsActive);

        /// <summary>
        /// Crea un nuevo rol (Async)
        /// </summary>
        Task<int> InsertAsync(object Name, object Description, object IsActive);

        /// <summary>
        /// Actualiza un rol existente
        /// </summary>
        int Update(object Name, object Description, object IsActive, object Id);

        /// <summary>
        /// Actualiza un rol existente (Async)
        /// </summary>
        Task<int> UpdateAsync(object Name, object Description, object IsActive, object Id);

        /// <summary>
        /// Elimina un rol por ID
        /// </summary>
        int DeleteById(object Id);

        /// <summary>
        /// Elimina un rol por ID (Async)
        /// </summary>
        Task<int> DeleteByIdAsync(object Id);

        /// <summary>
        /// Obtiene todos los roles ordenados por nombre
        /// </summary>
        List<RolesModel> GetAll();

        /// <summary>
        /// Obtiene todos los roles ordenados por nombre (Async)
        /// </summary>
        Task<List<RolesModel>> GetAllAsync();

        /// <summary>
        /// Obtiene un rol por su ID
        /// </summary>
        RolesModel? GetById(object Id);

        /// <summary>
        /// Obtiene un rol por su ID (Async)
        /// </summary>
        Task<RolesModel?> GetByIdAsync(object Id);

        /// <summary>
        /// Obtiene un rol por su nombre
        /// </summary>
        RolesModel? GetByName(object Name);

        /// <summary>
        /// Obtiene un rol por su nombre (Async)
        /// </summary>
        Task<RolesModel?> GetByNameAsync(object Name);

        /// <summary>
        /// Obtiene todos los roles activos
        /// </summary>
        List<RolesModel> GetActive();

        /// <summary>
        /// Obtiene todos los roles activos (Async)
        /// </summary>
        Task<List<RolesModel>> GetActiveAsync();

        /// <summary>
        /// Obtiene el conteo de roles activos
        /// </summary>
        object? GetActiveCount();

        /// <summary>
        /// Obtiene el conteo de roles activos (Async)
        /// </summary>
        Task<object?> GetActiveCountAsync();

        /// <summary>
        /// Activa o desactiva un rol
        /// </summary>
        int SetActiveStatus(object IsActive, object Id);

        /// <summary>
        /// Activa o desactiva un rol (Async)
        /// </summary>
        Task<int> SetActiveStatusAsync(object IsActive, object Id);

    }
}

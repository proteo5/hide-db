using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Proteo5.HideDB.Generated.Models;

namespace Proteo5.HideDB.Generated.Repositories
{
    /// <summary>
    /// Interfaz del repositorio para Users
    /// </summary>
    public interface IUsersRepository
    {
        /// <summary>
        /// Crea un nuevo usuario
        /// </summary>
        int Insert(object Username, object PasswordHash, object Email, object FirstName, object LastName, object status);

        /// <summary>
        /// Crea un nuevo usuario (Async)
        /// </summary>
        Task<int> InsertAsync(object Username, object PasswordHash, object Email, object FirstName, object LastName, object status);

        /// <summary>
        /// Actualiza un usuario existente
        /// </summary>
        int Update(object Username, object PasswordHash, object Email, object FirstName, object LastName, object status, object Id);

        /// <summary>
        /// Actualiza un usuario existente (Async)
        /// </summary>
        Task<int> UpdateAsync(object Username, object PasswordHash, object Email, object FirstName, object LastName, object status, object Id);

        /// <summary>
        /// Elimina un usuario por ID
        /// </summary>
        int DeleteById(object Id);

        /// <summary>
        /// Elimina un usuario por ID (Async)
        /// </summary>
        Task<int> DeleteByIdAsync(object Id);

        /// <summary>
        /// Obtiene todos los usuarios ordenados por fecha de creaci�n
        /// </summary>
        List<UsersModel> GetAll();

        /// <summary>
        /// Obtiene todos los usuarios ordenados por fecha de creaci�n (Async)
        /// </summary>
        Task<List<UsersModel>> GetAllAsync();

        /// <summary>
        /// Obtiene un usuario por su ID
        /// </summary>
        UsersModel? GetById(object Id);

        /// <summary>
        /// Obtiene un usuario por su ID (Async)
        /// </summary>
        Task<UsersModel?> GetByIdAsync(object Id);

        /// <summary>
        /// Obtiene un usuario por nombre de usuario
        /// </summary>
        UsersModel? GetByUser(object Username);

        /// <summary>
        /// Obtiene un usuario por nombre de usuario (Async)
        /// </summary>
        Task<UsersModel?> GetByUserAsync(object Username);

        /// <summary>
        /// Obtiene usuarios filtrados por estado
        /// </summary>
        List<UsersModel> GetByStatus(object status);

        /// <summary>
        /// Obtiene usuarios filtrados por estado (Async)
        /// </summary>
        Task<List<UsersModel>> GetByStatusAsync(object status);

        /// <summary>
        /// Obtiene el conteo de usuarios activos
        /// </summary>
        object? GetActiveCount();

        /// <summary>
        /// Obtiene el conteo de usuarios activos (Async)
        /// </summary>
        Task<object?> GetActiveCountAsync();

        /// <summary>
        /// B�squeda avanzada de usuarios
        /// </summary>
        List<UsersModel> GetByEmailAndStatus(object searchTerm, object statusFilter);

        /// <summary>
        /// B�squeda avanzada de usuarios (Async)
        /// </summary>
        Task<List<UsersModel>> GetByEmailAndStatusAsync(object searchTerm, object statusFilter);

    }
}

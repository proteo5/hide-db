using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proteo5.HideDB.Generated.Models
{
    /// <summary>
    /// Modelo para la entidad Users
    /// Entidad para gesti�n de usuarios del sistema - Updated dependencies
    /// Generado automáticamente - Version: 1.0
    /// </summary>
    [Table("Users")]
    public class UsersModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        /// <summary>
        /// Identificador �nico del usuario
        /// </summary>
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        /// <summary>
        /// Nombre de usuario �nico
        /// </summary>
        public string Username { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        /// <summary>
        /// Hash de la contrase�a
        /// </summary>
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        /// <summary>
        /// Direcci�n de correo electr�nico
        /// </summary>
        public string Email { get; set; } = string.Empty;

        [MaxLength(50)]
        /// <summary>
        /// Nombre del usuario
        /// </summary>
        public string FirstName { get; set; }

        [MaxLength(50)]
        /// <summary>
        /// Apellido del usuario
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Estado del usuario
        /// Catálogo: statuses
        /// </summary>
        public string status { get; set; }

        [Required]
        /// <summary>
        /// Fecha de creaci�n
        /// </summary>
        public DateTime CreatedAt { get; set; }

        [Required]
        /// <summary>
        /// Fecha de �ltima actualizaci�n
        /// </summary>
        public DateTime UpdatedAt { get; set; }

    }
}

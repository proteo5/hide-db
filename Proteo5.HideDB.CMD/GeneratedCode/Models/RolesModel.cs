using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proteo5.HideDB.Generated.Models
{
    /// <summary>
    /// Modelo para la entidad Roles
    /// Entidad para gesti�n de roles del sistema
    /// Generado automáticamente - Version: 1.0
    /// </summary>
    [Table("Roles")]
    public class RolesModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        /// <summary>
        /// Identificador �nico del rol
        /// </summary>
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        /// <summary>
        /// Nombre del rol
        /// </summary>
        public string Name { get; set; } = string.Empty;

        [MaxLength(255)]
        /// <summary>
        /// Descripci�n del rol
        /// </summary>
        public string? Description { get; set; }

        [Required]
        /// <summary>
        /// Indica si el rol est� activo
        /// </summary>
        public bool IsActive { get; set; }

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

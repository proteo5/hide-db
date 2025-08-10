using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proteo5.HideDB.Generated.Models
{
    /// <summary>
    /// Modelo para la entidad Roles
    /// Entity for system role management
    /// Generado automáticamente - Version: 1.0
    /// </summary>
    [Table("Roles")]
    public class RolesModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        /// <summary>
        /// Unique role identifier
        /// </summary>
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        /// <summary>
        /// Role name
        /// </summary>
        public string Name { get; set; } = string.Empty;

        [MaxLength(255)]
        /// <summary>
        /// Role description
        /// </summary>
        public string? Description { get; set; }

        [Required]
        /// <summary>
        /// Indicates if the role is active
        /// </summary>
        public bool IsActive { get; set; }

        [Required]
        /// <summary>
        /// Creation date
        /// </summary>
        public DateTime CreatedAt { get; set; }

        [Required]
        /// <summary>
        /// Last update date
        /// </summary>
        public DateTime UpdatedAt { get; set; }

    }
}

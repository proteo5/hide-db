using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proteo5.HideDB.Generated.Models
{
    /// <summary>
    /// Modelo para la entidad Users
    /// Entity for system user management - Updated dependencies
    /// Generado automáticamente - Version: 1.0
    /// </summary>
    [Table("Users")]
    public class UsersModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        /// <summary>
        /// Unique user identifier
        /// </summary>
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        /// <summary>
        /// Unique username
        /// </summary>
        public string Username { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        /// <summary>
        /// Password hash
        /// </summary>
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        /// <summary>
        /// Email address
        /// </summary>
        public string Email { get; set; } = string.Empty;

        [MaxLength(50)]
        /// <summary>
        /// User's first name
        /// </summary>
        public string? FirstName { get; set; }

        [MaxLength(50)]
        /// <summary>
        /// User's last name
        /// </summary>
        public string? LastName { get; set; }

        /// <summary>
        /// User status
        /// Catálogo: statuses
        /// </summary>
        public string? status { get; set; }

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

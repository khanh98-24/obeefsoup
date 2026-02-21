using System.ComponentModel.DataAnnotations;

namespace OBeefSoup.Models
{
    /// <summary>
    /// Store Location model - for multi-store expansion
    /// </summary>
    public class StoreLocation
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Address { get; set; } = string.Empty;

        [Phone]
        public string Phone { get; set; } = string.Empty;

        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public string MapUrl { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        [StringLength(100)]
        public string City { get; set; } = string.Empty;

        public string OpeningHours { get; set; } = string.Empty;
    }
}

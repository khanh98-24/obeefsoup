using System.ComponentModel.DataAnnotations;

namespace OBeefSoup.Models
{
    /// <summary>
    /// Product model - ready for Entity Framework database integration
    /// </summary>
    public class Product
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Range(0, 1000000)]
        public decimal Price { get; set; }

        public string ImageUrl { get; set; } = string.Empty;

        public int CategoryId { get; set; }

        public bool IsActive { get; set; } = true;

        public bool IsFeatured { get; set; }

        public int Stock { get; set; } = 100; // Số lượng tồn kho

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime? UpdatedDate { get; set; }

        // Navigation property for EF Core relationship
        public Category? Category { get; set; }
    }
}

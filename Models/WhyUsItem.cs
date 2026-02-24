using System.ComponentModel.DataAnnotations;

namespace OBeefSoup.Models
{
    /// <summary>
    /// Model for "Why Choose Us" section items on homepage
    /// </summary>
    public class WhyUsItem
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "Tiêu đề")]
        public string Title { get; set; } = string.Empty;

        [StringLength(500)]
        [Display(Name = "Mô tả")]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Bootstrap Icons class, e.g. "bi-award-fill"
        /// </summary>
        [Required]
        [StringLength(100)]
        [Display(Name = "Icon (Bootstrap Icons)")]
        public string IconClass { get; set; } = "bi-star-fill";

        /// <summary>
        /// Grid size: "large" (2col x 2row), "medium" (2col x 1row), "small" (1col x 1row), "accent" (1col x 1row)
        /// </summary>
        [Required]
        [StringLength(20)]
        [Display(Name = "Kích thước ô")]
        public string Size { get; set; } = "small";

        [Display(Name = "Thứ tự hiển thị")]
        public int DisplayOrder { get; set; } = 0;

        [Display(Name = "Hiển thị")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Ảnh nền (không bắt buộc)")]
        [StringLength(500)]
        public string? BackgroundImageUrl { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? UpdatedDate { get; set; }
    }
}

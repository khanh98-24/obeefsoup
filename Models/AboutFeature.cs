using System.ComponentModel.DataAnnotations;

namespace OBeefSoup.Models
{
    /// <summary>
    /// Model for feature boxes in the "About" section on the homepage
    /// </summary>
    public class AboutFeature
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Tiêu đề")]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Bootstrap Icons class, e.g. "bi-clock-history"
        /// </summary>
        [StringLength(100)]
        [Display(Name = "Icon (Bootstrap Icons)")]
        public string? IconClass { get; set; }

        [StringLength(255)]
        [Display(Name = "Ảnh Icon (từ máy)")]
        public string? IconImageUrl { get; set; }

        [Display(Name = "Thứ tự hiển thị")]
        public int DisplayOrder { get; set; } = 0;

        [Display(Name = "Hiển thị")]
        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? UpdatedDate { get; set; }
    }
}

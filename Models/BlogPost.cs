using System.ComponentModel.DataAnnotations;

namespace OBeefSoup.Models
{
    /// <summary>
    /// Model cho bài viết (Blog/News)
    /// </summary>
    public class BlogPost
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tiêu đề là bắt buộc")]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string Slug { get; set; } = string.Empty; // URL thân thiện: tin-tuc-khuyen-mai

        [Required(ErrorMessage = "Tóm tắt là bắt buộc")]
        [StringLength(500)]
        public string Summary { get; set; } = string.Empty;

        [Required(ErrorMessage = "Nội dung là bắt buộc")]
        public string Content { get; set; } = string.Empty;

        [StringLength(255)]
        public string? ImageUrl { get; set; }

        public bool IsActive { get; set; } = true;

        public int DisplayOrder { get; set; } = 0;

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime? UpdatedDate { get; set; }
    }
}

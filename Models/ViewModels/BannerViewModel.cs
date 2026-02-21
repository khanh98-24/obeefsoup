using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace OBeefSoup.Models.ViewModels
{
    public class BannerViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tiêu đề banner là bắt buộc")]
        [StringLength(200, ErrorMessage = "Tiêu đề không được vượt quá 200 ký tự")]
        [Display(Name = "Tiêu Đề")]
        public string Title { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Mô tả không được vượt quá 500 ký tự")]
        [Display(Name = "Mô Tả")]
        public string Description { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Link không được vượt quá 500 ký tự")]
        [Display(Name = "Link URL")]
        public string Link { get; set; } = string.Empty;

        [Display(Name = "Thứ Tự Hiển Thị")]
        [Range(0, 100, ErrorMessage = "Thứ tự từ 0 đến 100")]
        public int DisplayOrder { get; set; } = 0;

        [Display(Name = "Hoạt Động")]
        public bool IsActive { get; set; } = true;

        // For image upload
        [Display(Name = "Ảnh Banner")]
        public IFormFile? ImageFile { get; set; }

        // Current image URL (for edit scenario)
        public string? ImageUrl { get; set; }

        // Cropped image base64 data
        public string? CroppedImageData { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? UpdatedDate { get; set; }
    }
}

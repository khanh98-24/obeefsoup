using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace OBeefSoup.Models.ViewModels
{
    public class AboutFeatureViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tiêu đề")]
        [StringLength(100)]
        [Display(Name = "Tiêu đề")]
        public string Title { get; set; } = string.Empty;

        [StringLength(100)]
        [Display(Name = "Icon (Bootstrap Icons)")]
        public string? IconClass { get; set; }

        [Display(Name = "Ảnh Icon từ máy")]
        public string? IconImageUrl { get; set; }

        [Display(Name = "Chọn ảnh icon mới")]
        public IFormFile? IconImageFile { get; set; }

        [Display(Name = "Thứ tự hiển thị")]
        public int DisplayOrder { get; set; } = 0;

        [Display(Name = "Hiển thị")]
        public bool IsActive { get; set; } = true;
    }
}

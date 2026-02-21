using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace OBeefSoup.Models.ViewModels
{
    public class AboutSectionViewModel
    {
        [Display(Name = "Tiêu đề ảnh (Trái)")]
        [StringLength(200)]
        public string Title1 { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập tên thương hiệu")]
        [Display(Name = "Tên thương hiệu (Phải)")]
        [StringLength(100)]
        public string Title2 { get; set; } = "O' BeefSoup";

        [Display(Name = "Tiêu đề phụ")]
        [StringLength(100)]
        public string Subtitle { get; set; } = "CÂU CHUYỆN THƯƠNG HIỆU";

        [Required(ErrorMessage = "Vui lòng nhập mô tả")]
        [Display(Name = "Mô tả chi tiết")]
        public string Description { get; set; } = string.Empty;

        [Display(Name = "Ảnh hiện tại")]
        public string? ImageUrl { get; set; }

        [Display(Name = "Chọn ảnh mới")]
        public IFormFile? ImageFile { get; set; }

        public string? CroppedImageData { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace OBeefSoup.Models.ViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên sản phẩm là bắt buộc")]
        [StringLength(100, ErrorMessage = "Tên sản phẩm không được vượt quá 100 ký tự")]
        [Display(Name = "Tên Sản Phẩm")]
        public string Name { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Mô tả không được vượt quá 500 ký tự")]
        [Display(Name = "Mô Tả")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Giá sản phẩm là bắt buộc")]
        [Range(0, 1000000, ErrorMessage = "Giá phải từ 0 đến 1,000,000")]
        [Display(Name = "Giá (VNĐ)")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Danh mục là bắt buộc")]
        [Display(Name = "Danh Mục")]
        public int CategoryId { get; set; }

        [Range(0, 10000, ErrorMessage = "Số lượng tồn kho phải từ 0 đến 10,000")]
        [Display(Name = "Tồn Kho")]
        public int Stock { get; set; } = 100;

        [Display(Name = "Hoạt Động")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Nổi Bật")]
        public bool IsFeatured { get; set; }

        // For image upload
        [Display(Name = "Ảnh Sản Phẩm")]
        public IFormFile? ImageFile { get; set; }

        // Current image URL (for edit scenario)
        public string? ImageUrl { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? UpdatedDate { get; set; }
    }
}

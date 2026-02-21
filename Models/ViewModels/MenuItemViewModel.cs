using System.ComponentModel.DataAnnotations;

namespace OBeefSoup.Models.ViewModels
{
    public class MenuItemViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tiêu đề menu")]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập đường dẫn (URL)")]
        [StringLength(255)]
        public string Url { get; set; } = "#";

        public int? ParentId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập thứ tự hiển thị")]
        public int DisplayOrder { get; set; } = 0;

        public bool IsActive { get; set; } = true;

        [StringLength(50)]
        public string? Icon { get; set; }
    }
}

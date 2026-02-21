using System.ComponentModel.DataAnnotations;

namespace OBeefSoup.Models
{
    public class JobPosting
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập vị trí tuyển dụng")]
        [StringLength(200)]
        [Display(Name = "Vị trí tuyển dụng")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập mô tả công việc")]
        [Display(Name = "Mô tả công việc")]
        public string Description { get; set; } = string.Empty;

        [Display(Name = "Yêu cầu công việc")]
        public string? Requirements { get; set; }

        [Display(Name = "Quyền lợi")]
        public string? Benefits { get; set; }

        [StringLength(100)]
        [Display(Name = "Mức lương")]
        public string? SalaryRange { get; set; }

        [Display(Name = "Hạn nộp hồ sơ")]
        [DataType(DataType.Date)]
        public DateTime? Deadline { get; set; }

        [Display(Name = "Trạng thái hiển thị")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Cơ sở tuyển dụng")]
        public int? StoreLocationId { get; set; }

        [Display(Name = "Ngày tạo")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation properties
        public virtual StoreLocation? StoreLocation { get; set; }
        public virtual ICollection<CandidateApplication>? Applications { get; set; }
    }
}

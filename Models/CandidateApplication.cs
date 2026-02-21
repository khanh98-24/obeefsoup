using System.ComponentModel.DataAnnotations;

namespace OBeefSoup.Models
{
    public enum ApplicationStatus
    {
        [Display(Name = "Chờ duyệt")]
        Pending,
        [Display(Name = "Đã xem hồ sơ")]
        Reviewed,
        [Display(Name = "Đang phỏng vấn")]
        Interviewing,
        [Display(Name = "Đã tuyển")]
        Hired,
        [Display(Name = "Từ chối")]
        Rejected
    }

    public class CandidateApplication
    {
        public int Id { get; set; }

        [Required]
        public int JobPostingId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập họ tên")]
        [StringLength(100)]
        [Display(Name = "Họ và tên")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập email")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        [Display(Name = "Số điện thoại")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng giới thiệu một chút về bản thân")]
        [Display(Name = "Giới thiệu bản thân / Kinh nghiệm")]
        public string Introduction { get; set; } = string.Empty;

        [Display(Name = "Link CV (Google Drive, Dropbox, ...)")]
        public string? CVUrl { get; set; }

        [Display(Name = "Ngày ứng tuyển")]
        public DateTime AppliedDate { get; set; } = DateTime.Now;

        [Display(Name = "Trạng thái đơn")]
        public ApplicationStatus Status { get; set; } = ApplicationStatus.Pending;

        // Navigation property
        public virtual JobPosting? JobPosting { get; set; }
    }
}

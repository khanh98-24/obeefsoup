using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OBeefSoup.Models
{
    public class Testimonial
    {
        public int Id { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public virtual Customer? Customer { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên hiển thị")]
        [StringLength(100)]
        public string CustomerName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập nội dung đánh giá")]
        [StringLength(500)]
        public string Comment { get; set; } = string.Empty;

        [Range(1, 5, ErrorMessage = "Đánh giá từ 1 đến 5 sao")]
        public int Rating { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;

        public bool IsApproved { get; set; } = false;
    }
}

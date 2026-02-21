using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OBeefSoup.Models
{
    public class MenuItem
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [StringLength(255)]
        public string Url { get; set; } = "#";

        public int? ParentId { get; set; }

        public int DisplayOrder { get; set; } = 0;

        public bool IsActive { get; set; } = true;

        [StringLength(50)]
        public string? Icon { get; set; }

        [ForeignKey("ParentId")]
        public virtual MenuItem? Parent { get; set; }

        public virtual ICollection<MenuItem> SubMenus { get; set; } = new List<MenuItem>();
    }
}

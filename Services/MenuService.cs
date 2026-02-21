using OBeefSoup.Models;
using OBeefSoup.Models.ViewModels;

namespace OBeefSoup.Services
{
    /// <summary>
    /// Menu service - provides sample data
    /// TODO: Replace with database repository when integrating Entity Framework
    /// </summary>
    public class MenuService
    {
        /// <summary>
        /// Get featured products for landing page
        /// </summary>
        public List<Product> GetFeaturedProducts()
        {
            return new List<Product>
            {
                new Product
                {
                    Id = 1,
                    Name = "Phở Tái",
                    Description = "Thịt bò tái mềm, nước dùng thanh ngọt",
                    Price = 65000,
                    ImageUrl = "/images/pho-tai.svg",
                    CategoryId = 1,
                    IsActive = true,
                    IsFeatured = true
                },
                new Product
                {
                    Id = 2,
                    Name = "Phở Gầu",
                    Description = "Gầu bò béo ngậy, đậm đà truyền thống",
                    Price = 70000,
                    ImageUrl = "/images/pho-gau.svg",
                    CategoryId = 1,
                    IsActive = true,
                    IsFeatured = true
                },
                new Product
                {
                    Id = 3,
                    Name = "Phở Đặc Biệt",
                    Description = "Đầy đủ topping - Signature O' BeefSoup",
                    Price = 85000,
                    ImageUrl = "/images/pho-dac-biet.svg",
                    CategoryId = 1,
                    IsActive = true,
                    IsFeatured = true
                }
            };
        }

        /// <summary>
        /// Get main store location
        /// </summary>
        public StoreLocation GetMainStore()
        {
            return new StoreLocation
            {
                Id = 1,
                Name = "O' BeefSoup - Chi nhánh Trung tâm",
                Address = "123 Nguyễn Huệ, Quận 1, TP. Hồ Chí Minh",
                Phone = "0901 234 567",
                Email = "contact@obeefsoup.vn",
                City = "Hồ Chí Minh",
                OpeningHours = "7:00 - 22:00 (Hàng ngày)",
                IsActive = true
            };
        }

        /// <summary>
        /// Get customer testimonials
        /// </summary>
        public List<Testimonial> GetTestimonials()
        {
            return new List<Testimonial>
            {
                new Testimonial
                {
                    Id = 1,
                    CustomerName = "Nguyễn Văn A",
                    Comment = "Nước dùng đậm đà, thịt bò tươi ngon. Đây là quán phở ngon nhất tôi từng ăn!",
                    Rating = 5,
                    Date = DateTime.Now.AddDays(-15)
                },
                new Testimonial
                {
                    Id = 2,
                    CustomerName = "Trần Thị B",
                    Comment = "Không gian sang trọng, phục vụ chu đáo. Phở đặc biệt rất xứng đáng!",
                    Rating = 5,
                    Date = DateTime.Now.AddDays(-8)
                },
                new Testimonial
                {
                    Id = 3,
                    CustomerName = "Lê Minh C",
                    Comment = "Giá hơi cao nhưng chất lượng tuyệt vời. Sẽ quay lại!",
                    Rating = 4,
                    Date = DateTime.Now.AddDays(-3)
                }
            };
        }

        /// <summary>
        /// Get complete data for home page
        /// </summary>
        public HomeViewModel GetHomeData()
        {
            return new HomeViewModel
            {
                FeaturedProducts = GetFeaturedProducts(),
                Stores = new List<StoreLocation> { GetMainStore() },
                Testimonials = GetTestimonials()
            };
        }
    }
}

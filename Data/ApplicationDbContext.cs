using Microsoft.EntityFrameworkCore;
using OBeefSoup.Models;
using OBeefSoup.Services;
using System.Security.Cryptography;
using System.Text;

namespace OBeefSoup.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<StoreLocation> StoreLocations { get; set; }
        public DbSet<Banner> Banners { get; set; }
        public DbSet<SiteSetting> SiteSettings { get; set; }
        public DbSet<AdminUser> AdminUsers { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<WhyUsItem> WhyUsItems { get; set; }
        public DbSet<Testimonial> Testimonials { get; set; }
        public DbSet<JobPosting> JobPostings { get; set; }
        public DbSet<CandidateApplication> CandidateApplications { get; set; }
        public DbSet<AboutFeature> AboutFeatures { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany()
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure decimal precision
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Order>()
                .Property(o => o.TotalAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<OrderItem>()
                .Property(oi => oi.UnitPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<OrderItem>()
                .Property(oi => oi.Subtotal)
                .HasPrecision(18, 2);

            modelBuilder.Entity<MenuItem>()
                .HasOne(m => m.Parent)
                .WithMany(m => m.SubMenus)
                .HasForeignKey(m => m.ParentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CandidateApplication>()
                .HasOne(ca => ca.JobPosting)
                .WithMany(jp => jp.Applications)
                .HasForeignKey(ca => ca.JobPostingId)
                .OnDelete(DeleteBehavior.Restrict);

            // Seed data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Categories
            modelBuilder.Entity<Category>().HasData(
                new Category
                {
                    Id = 1,
                    Name = "Phở Bò",
                    Description = "Các loại phở bò truyền thống",
                    DisplayOrder = 1,
                    IsActive = true
                },
                new Category
                {
                    Id = 2,
                    Name = "Phở Gà",
                    Description = "Phở gà thanh đạm, thơm ngon",
                    DisplayOrder = 2,
                    IsActive = true
                },
                new Category
                {
                    Id = 3,
                    Name = "Đồ Uống",
                    Description = "Nước giải khát",
                    DisplayOrder = 3,
                    IsActive = true
                }
            );

            // Seed Products
            modelBuilder.Entity<Product>().HasData(
                // Phở Bò
                new Product
                {
                    Id = 1,
                    Name = "Phở Tái",
                    Description = "Thịt bò tái mềm, nước dùng thanh ngọt",
                    Price = 65000,
                    ImageUrl = "/images/pho-bo-tai-nam.jpg",
                    CategoryId = 1,
                    IsActive = true,
                    IsFeatured = true,
                    Stock = 100,
                    CreatedDate = DateTime.Now
                },
                new Product
                {
                    Id = 2,
                    Name = "Phở Gầu",
                    Description = "Gầu bò béo ngậy, đậm đà truyền thống",
                    Price = 70000,
                    ImageUrl = "/images/pho-bo-chin.jpg",
                    CategoryId = 1,
                    IsActive = true,
                    IsFeatured = true,
                    Stock = 100,
                    CreatedDate = DateTime.Now
                },
                new Product
                {
                    Id = 3,
                    Name = "Phở Đặc Biệt",
                    Description = "Đầy đủ topping - Signature O' BeefSoup",
                    Price = 85000,
                    ImageUrl = "https://placehold.co/400x300/8B0000/FFF?text=Pho+Dac+Biet",
                    CategoryId = 1,
                    IsActive = true,
                    IsFeatured = true,
                    Stock = 100,
                    CreatedDate = DateTime.Now
                },
                new Product
                {
                    Id = 4,
                    Name = "Phở Nạm",
                    Description = "Nạm bò thơm ngon, mềm tan",
                    Price = 68000,
                    ImageUrl = "https://placehold.co/400x300/8B0000/FFF?text=Pho+Nam",
                    CategoryId = 1,
                    IsActive = true,
                    IsFeatured = false,
                    Stock = 100,
                    CreatedDate = DateTime.Now
                },
                new Product
                {
                    Id = 5,
                    Name = "Phở Bò Viên",
                    Description = "Bò viên giòn sần sật",
                    Price = 60000,
                    ImageUrl = "https://placehold.co/400x300/8B0000/FFF?text=Pho+Bo+Vien",
                    CategoryId = 1,
                    IsActive = true,
                    IsFeatured = false,
                    Stock = 100,
                    CreatedDate = DateTime.Now
                },
                // Phở Gà
                new Product
                {
                    Id = 6,
                    Name = "Phở Gà",
                    Description = "Gà ta thơm ngon, nước dùng thanh ngọt",
                    Price = 55000,
                    ImageUrl = "https://placehold.co/400x300/FFB347/000?text=Pho+Ga",
                    CategoryId = 2,
                    IsActive = true,
                    IsFeatured = false,
                    Stock = 100,
                    CreatedDate = DateTime.Now
                },
                new Product
                {
                    Id = 7,
                    Name = "Phở Gà Đặc Biệt",
                    Description = "Gà ta nguyên con, đầy đủ topping",
                    Price = 75000,
                    ImageUrl = "https://placehold.co/400x300/FFB347/000?text=Pho+Ga+Dac+Biet",
                    CategoryId = 2,
                    IsActive = true,
                    IsFeatured = false,
                    Stock = 100,
                    CreatedDate = DateTime.Now
                },
                // Đồ Uống
                new Product
                {
                    Id = 8,
                    Name = "Trà Đá",
                    Description = "Trà đá miễn phí",
                    Price = 0,
                    ImageUrl = "https://placehold.co/400x300/5E8B7E/FFF?text=Tra+Da",
                    CategoryId = 3,
                    IsActive = true,
                    IsFeatured = false,
                    Stock = 1000,
                    CreatedDate = DateTime.Now
                },
                new Product
                {
                    Id = 9,
                    Name = "Nước Ngọt",
                    Description = "Coca, Pepsi, 7Up",
                    Price = 15000,
                    ImageUrl = "https://placehold.co/400x300/FF6347/FFF?text=Nuoc+Ngot",
                    CategoryId = 3,
                    IsActive = true,
                    IsFeatured = false,
                    Stock = 200,
                    CreatedDate = DateTime.Now
                },
                new Product
                {
                    Id = 10,
                    Name = "Nước Chanh",
                    Description = "Chanh tươi vắt",
                    Price = 20000,
                    ImageUrl = "https://placehold.co/400x300/FFE135/000?text=Nuoc+Chanh",
                    CategoryId = 3,
                    IsActive = true,
                    IsFeatured = false,
                    Stock = 100,
                    CreatedDate = DateTime.Now
                }
            );

            // Seed Store Location
            modelBuilder.Entity<StoreLocation>().HasData(
                new StoreLocation
                {
                    Id = 1,
                    Name = "O' BeefSoup - Chi nhánh Trung tâm",
                    Address = "123 Nguyễn Huệ, Quận 1, TP. Hồ Chí Minh",
                    Phone = "0901 234 567",
                    Email = "contact@obeefsoup.vn",
                    City = "Hồ Chí Minh",
                    MapUrl = "https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d3919.513274291583!2d106.70119131533413!3d10.771143262222383!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x31752f40a3b49e59%3A0xe27581172a39a037!2zMTIzIMSQLiBOZ3V54buFbiBIdeG7hywgQuG6v24gTmdow6ks QuG6rW4gMSwgVGjDoW5oIHBo4buSIEjhu5MgQ2jDrSBNaW5oLCBWaWV0bmFt!5e0!3m2!1sen!2s!4v1645084931345!5m2!1sen!2s",
                    OpeningHours = "7:00 - 22:00 (Hàng ngày)",
                    IsActive = true
                }
            );

            // Seed Banners
            modelBuilder.Entity<Banner>().HasData(
                new Banner
                {
                    Id = 1,
                    Title = "Chào Mừng Đến Với O' BeefSoup",
                    Description = "Phở Bò Ngon Nhất Hà Nội - Hương Vị Truyền Thống",
                    ImageUrl = "https://placehold.co/1200x400/8B0000/FFF?text=O'+BeefSoup+Banner",
                    Link = "/Menu",
                    DisplayOrder = 1,
                    IsActive = true,
                    CreatedDate = DateTime.Now
                },
                new Banner
                {
                    Id = 2,
                    Title = "Khuyến Mãi Đặc Biệt",
                    Description = "Giảm 20% Cho Đơn Hàng Đầu Tiên",
                    ImageUrl = "https://placehold.co/1200x400/FFB347/000?text=Khuyen+Mai",
                    Link = "/Menu",
                    DisplayOrder = 2,
                    IsActive = true,
                    CreatedDate = DateTime.Now
                }
            );

            // Seed Site Settings
            modelBuilder.Entity<SiteSetting>().HasData(
                new SiteSetting
                {
                    Id = 1,
                    Key = "MenuBackgroundImage",
                    Value = "/images/menu-background.jpg",
                    Description = "Ảnh nền cho phần thực đơn trang chủ",
                    IsActive = true,
                    CreatedDate = DateTime.Now
                },
                new SiteSetting
                {
                    Id = 2,
                    Key = "MenuBackgroundOpacity",
                    Value = "0.15",
                    Description = "Độ mờ ảnh nền thực đơn (0.0 = trong suốt, 1.0 = rõ nét).",
                    IsActive = true,
                    CreatedDate = DateTime.Now
                },
                new SiteSetting
                {
                    Id = 3,
                    Key = "AboutTitle1",
                    Value = "Tinh hoa TRUYỀN THỐNG & HIỆN ĐẠI hòa quyện",
                    Description = "Tiêu đề nằm trên ảnh phần Giới thiệu",
                    IsActive = true,
                    CreatedDate = DateTime.Now
                },
                new SiteSetting
                {
                    Id = 4,
                    Key = "AboutTitle2",
                    Value = "O' BeefSoup",
                    Description = "Tên thương hiệu phần Giới thiệu",
                    IsActive = true,
                    CreatedDate = DateTime.Now
                },
                new SiteSetting
                {
                    Id = 5,
                    Key = "AboutSubtitle",
                    Value = "CÂU CHUYỆN THƯƠNG HIỆU",
                    Description = "Tiêu đề phụ phần Giới thiệu",
                    IsActive = true,
                    CreatedDate = DateTime.Now
                },
                new SiteSetting
                {
                    Id = 6,
                    Key = "AboutDescription",
                    Value = "Nơi hội tụ tinh hoa ẩm thực truyền thống Việt Nam với không gian hiện đại, sang trọng bậc nhất.",
                    Description = "Mô tả chi tiết phần Giới thiệu",
                    IsActive = true,
                    CreatedDate = DateTime.Now
                },
                new SiteSetting
                {
                    Id = 7,
                    Key = "AboutImageUrl",
                    Value = "/images/gioithieu.jpg",
                    Description = "Ảnh đại diện phần Giới thiệu",
                    IsActive = true,
                    CreatedDate = DateTime.Now
                }
            );

            // Seed AboutFeatures
            modelBuilder.Entity<AboutFeature>().HasData(
                new AboutFeature { Id = 1, Title = "Ninh 12 Tiếng", IconClass = "bi-clock-history", DisplayOrder = 1, IsActive = true, CreatedDate = DateTime.Now },
                new AboutFeature { Id = 2, Title = "Nguyên Liệu", IconClass = "bi-gem", DisplayOrder = 2, IsActive = true, CreatedDate = DateTime.Now },
                new AboutFeature { Id = 3, Title = "Đẳng Cấp", IconClass = "bi-stars", DisplayOrder = 3, IsActive = true, CreatedDate = DateTime.Now },
                new AboutFeature { Id = 4, Title = "Phục Vụ", IconClass = "bi-heart-fill", DisplayOrder = 4, IsActive = true, CreatedDate = DateTime.Now }
            );

            // Seed Admin Users
            modelBuilder.Entity<AdminUser>().HasData(
                new AdminUser
                {
                    Id = 1,
                    Username = "admin",
                    Email = "admin@obeefsoup.vn",
                    PasswordHash = AuthService.HashPassword("Admin@123"),
                    Role = "Admin",
                    FullName = "Quản Trị Viên",
                    CreatedDate = new DateTime(2026, 1, 1),
                    IsActive = true
                },
                new AdminUser
                {
                    Id = 2,
                    Username = "manager",
                    Email = "manager@obeefsoup.vn",
                    PasswordHash = AuthService.HashPassword("Manager@123"),
                    Role = "Manager",
                    FullName = "Quản Lý Nội Dung",
                    CreatedDate = new DateTime(2026, 1, 1),
                    IsActive = true
                }
            );

            // Seed MenuItems
            modelBuilder.Entity<MenuItem>().HasData(
                new MenuItem { Id = 1, Title = "Trang Chủ", Url = "/", DisplayOrder = 1, IsActive = true },
                new MenuItem { Id = 2, Title = "Giới Thiệu", Url = "/#about", DisplayOrder = 2, IsActive = true },
                new MenuItem { Id = 3, Title = "Thực Đơn", Url = "/Menu", DisplayOrder = 3, IsActive = true },
                new MenuItem { Id = 4, Title = "Ưu Điểm", Url = "/#why-us", DisplayOrder = 4, IsActive = true },
                new MenuItem { Id = 5, Title = "Đánh Giá", Url = "/#testimonials", DisplayOrder = 5, IsActive = true },
                new MenuItem { Id = 6, Title = "Phở Bò", Url = "/Menu#category-1", ParentId = 3, DisplayOrder = 1, IsActive = true },
                new MenuItem { Id = 7, Title = "Phở Gà", Url = "/Menu#category-2", ParentId = 3, DisplayOrder = 2, IsActive = true },
                new MenuItem { Id = 8, Title = "Đồ Uống", Url = "/Menu#category-3", ParentId = 3, DisplayOrder = 3, IsActive = true }
            );

            // Seed BlogPosts
            modelBuilder.Entity<BlogPost>().HasData(
                new BlogPost
                {
                    Id = 1,
                    Title = "Bí quyết tạo nên nước dùng phở bò truyền thống",
                    Slug = "bi-quyet-nuoc-dung-pho-bo",
                    Summary = "Linh hồn của bát phở nằm ở nước dùng. Tại O'BeefSoup, chúng tôi ninh xương ống bò trong 12 tiếng cùng các gia vị tự nhiên...",
                    Content = "Nội dung chi tiết về cách ninh xương, chọn gừng, hành tím nướng và các nguyên liệu thảo mộc quý hiếm...",
                    ImageUrl = "/images/486842042_1200614522067957_8652198815515987194_n.jpg",
                    DisplayOrder = 1,
                    IsActive = true,
                    CreatedDate = DateTime.Now.AddDays(-5)
                },
                new BlogPost
                {
                    Id = 2,
                    Title = "Khai trương cơ sở mới - Nhận ngay ưu đãi 20%",
                    Slug = "khai-truong-co-so-moi",
                    Summary = "Chào đón cơ sở thứ 3 của O'BeefSoup tại Cầu Giấy. Giảm ngay 20% trên tổng hóa đơn cho khách hàng check-in tại quán...",
                    Content = "Chào đón sự kiện khai trương tưng bừng với nhiều phần quà hấp dẫn và chương trình âm nhạc đặc sắc...",
                    ImageUrl = "/images/486748406_1200441442085265_4112327303630339444_n.jpg",
                    DisplayOrder = 2,
                    IsActive = true,
                    CreatedDate = DateTime.Now.AddDays(-2)
                },
                new BlogPost
                {
                    Id = 3,
                    Title = "Phở - Nét văn hóa ẩm thực tinh tế của người Việt",
                    Slug = "pho-van-hoa-am-thuc-viet",
                    Summary = "Phở không chỉ là một món ăn, nó là biểu tượng của tinh thần và văn hóa Việt Nam. Cùng khám phá hành trình của phở...",
                    Content = "Từ những gánh hàng rong xưa đến các cửa hàng sang trọng ngày nay, phở luôn giữ vững vị trí độc tôn...",
                    ImageUrl = "/images/484090098_1184094750386601_4967145700946842561_n.jpg",
                    DisplayOrder = 3,
                    IsActive = true,
                    CreatedDate = DateTime.Now.AddDays(-1)
                }
            );

            // Seed WhyUsItems
            modelBuilder.Entity<WhyUsItem>().HasData(
                new WhyUsItem
                {
                    Id = 1,
                    Title = "Bò tươi mỗi ngày",
                    Description = "Thịt bò được chọn lọc khắt khe từ những trang trại uy tín, đảm bảo độ tươi ngon nhất khi đến bàn ăn.",
                    IconClass = "bi-award-fill",
                    Size = "large",
                    DisplayOrder = 1,
                    IsActive = true,
                    CreatedDate = new DateTime(2026, 1, 1)
                },
                new WhyUsItem
                {
                    Id = 2,
                    Title = "Nước dùng 12h",
                    Description = "Hương vị đậm đà được tinh chế qua 12 giờ ninh xương.",
                    IconClass = "bi-flame",
                    Size = "medium",
                    DisplayOrder = 2,
                    IsActive = true,
                    CreatedDate = new DateTime(2026, 1, 1)
                },
                new WhyUsItem
                {
                    Id = 3,
                    Title = "Nguyên liệu sạch",
                    Description = "",
                    IconClass = "bi-shield-check",
                    Size = "small",
                    DisplayOrder = 3,
                    IsActive = true,
                    CreatedDate = new DateTime(2026, 1, 1)
                },
                new WhyUsItem
                {
                    Id = 4,
                    Title = "Không gian sang trọng",
                    Description = "Sự kết hợp giữa nét truyền thống và hiện đại.",
                    IconClass = "bi-stars",
                    Size = "accent",
                    DisplayOrder = 4,
                    IsActive = true,
                    CreatedDate = new DateTime(2026, 1, 1)
                }
            );

            // Seed Testimonials
            modelBuilder.Entity<Testimonial>().HasData(
                new Testimonial
                {
                    Id = 1,
                    CustomerId = 1, // Assume customer with ID 1 exists or will be created
                    CustomerName = "Nguyễn Văn A",
                    Comment = "Nước dùng đậm đà, thịt bò tươi ngon. Đây là quán phở ngon nhất tôi từng ăn!",
                    Rating = 5,
                    Date = DateTime.Now.AddDays(-15),
                    IsApproved = true
                },
                new Testimonial
                {
                    Id = 2,
                    CustomerId = 1,
                    CustomerName = "Trần Thị B",
                    Comment = "Không gian sang trọng, phục vụ chu đáo. Phở đặc biệt rất xứng đáng!",
                    Rating = 5,
                    Date = DateTime.Now.AddDays(-8),
                    IsApproved = true
                },
                new Testimonial
                {
                    Id = 3,
                    CustomerId = 1,
                    CustomerName = "Lê Minh C",
                    Comment = "Giá hơi cao nhưng chất lượng tuyệt vời. Sẽ quay lại!",
                    Rating = 4,
                    Date = DateTime.Now.AddDays(-3),
                    IsApproved = true
                }
            );
        }
    }
}

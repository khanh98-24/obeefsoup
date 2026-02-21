namespace OBeefSoup.Models.ViewModels
{
    /// <summary>
    /// View model for landing page - aggregates all data needed for home page
    /// </summary>
    public class HomeViewModel
    {
        public List<Banner> Banners { get; set; } = new List<Banner>();
        public List<Product> FeaturedProducts { get; set; } = new List<Product>();
        public List<StoreLocation> Stores { get; set; } = new List<StoreLocation>();
        public List<Testimonial> Testimonials { get; set; } = new List<Testimonial>();
        public List<BlogPost> RecentBlogPosts { get; set; } = new List<BlogPost>();
        public List<WhyUsItem> WhyUsItems { get; set; } = new List<WhyUsItem>();

        // About Section
        public string AboutTitle1 { get; set; } = string.Empty;
        public string AboutTitle2 { get; set; } = "O' BeefSoup";
        public string AboutSubtitle { get; set; } = "CÂU CHUYỆN THƯƠNG HIỆU";
        public string AboutDescription { get; set; } = string.Empty;
        public string AboutImageUrl { get; set; } = string.Empty;
        public List<AboutFeature> AboutFeatures { get; set; } = new List<AboutFeature>();
        
        // Site Settings
        public string? MenuBackgroundImage { get; set; }
        public double MenuBackgroundOpacity { get; set; } = 0.15;
    }
}

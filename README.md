# O' BeefSoup - Premium Vietnamese Pho Landing Page

![ASP.NET Core](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)
![Bootstrap](https://img.shields.io/badge/Bootstrap-5.3-7952B3?logo=bootstrap)
![License](https://img.shields.io/badge/license-MIT-green)

A professional landing page for **O' BeefSoup**, a premium Vietnamese pho restaurant brand, built with ASP.NET Core MVC (.NET 8) and designed for future expansion.

## 🌟 Features

- **7 Complete Landing Page Sections:**
  - Hero section with animated branding
  - Brand introduction and story
  - Featured menu with 3 signature dishes
  - "Why Choose Us" benefits section
  - Customer testimonials
  - Call-to-action section
  - Store information and footer

- **Premium Design:**
  - Vietnamese color palette (deep red, gold, wood brown)
  - Smooth animations and micro-interactions
  - Glassmorphism effects
  - Responsive layout for all devices
  - Premium typography (Playfair Display + Inter)

- **Future-Ready Architecture:**
  - Models ready for Entity Framework integration
  - Service-based architecture with DI
  - SEO optimized with meta tags
  - Structured for expansion to full e-commerce

## 🚀 Quick Start

### Prerequisites
- .NET 8 SDK or later
- Any modern web browser

### Running the Application

```powershell
# Navigate to project directory
cd C:\Users\nguye\.gemini\antigravity\scratch\OBeefSoup

# Restore dependencies (automatic)
dotnet restore

# Build the project
dotnet build

# Run the application
dotnet run
```

**Access the website:**
- HTTPS: https://localhost:5001
- HTTP: http://localhost:5000

## 📁 Project Structure

```
OBeefSoup/
├── Controllers/          # MVC Controllers
├── Models/              # Data models (DB-ready)
├── Services/            # Business logic and data services
├── Views/               # Razor views
│   ├── Shared/         # Layout and shared components
│   └── Home/           # Landing page views
└── wwwroot/            # Static files
    ├── css/            # Custom styling
    ├── js/             # JavaScript
    └── images/         # Images (currently SVG placeholders)
```

## 🎨 Customization

### Change Brand Colors

Edit `wwwroot/css/site.css`:

```css
:root {
    --color-primary: #8B0000;    /* Deep Red */
    --color-secondary: #D4AF37;  /* Gold */
    --color-accent: #8B4513;     /* Wood Brown */
}
```

### Update Menu Items

Edit `Services/MenuService.cs` to add/modify dishes.

### Replace Placeholder Images

1. Add your food photos to `wwwroot/images/`
2. Update image URLs in `MenuService.cs`

## 🔮 Future Expansion

This project is architected to easily expand into:

- **Database Integration** - Models ready for EF Core
- **Online Ordering System** - Product models include all necessary fields
- **Table Reservations** - Reservation model prepared
- **Multi-Store Management** - StoreLocation model ready
- **Admin Panel** - Controller structure supports admin area
- **User Authentication** - Can integrate ASP.NET Identity

## 📱 Responsive Design

Fully responsive with breakpoints for:
- Desktop (1200px+)
- Tablet (768px-1199px)
- Mobile (<768px)

## 🎯 Technology Stack

- **Backend:** ASP.NET Core MVC 8.0
- **Frontend:** Bootstrap 5.3
- **Fonts:** Google Fonts (Playfair Display, Inter)
- **Icons:** Bootstrap Icons
- **Architecture:** MVC with Dependency Injection

## 📝 SEO Features

✅ Meta descriptions and keywords  
✅ Open Graph tags for social sharing  
✅ Semantic HTML5 structure  
✅ Mobile-responsive design  
✅ Clean URL structure  

## 🛠️ Development

### Build

```powershell
dotnet build
```

### Clean

```powershell
dotnet clean
```

### Watch (auto-reload on changes)

```powershell
dotnet watch run
```

## 📄 License

This project is provided as-is for demonstration purposes.

## 🤝 Contributing

This is a demonstration project. Feel free to use it as a starting point for your own restaurant website.

## 📞 Support

For questions about ASP.NET Core MVC:
- [ASP.NET Core Documentation](https://docs.microsoft.com/aspnet/core/)
- [Bootstrap Documentation](https://getbootstrap.com/docs/5.3/)

---

**Built with ❤️ for O' BeefSoup - Tinh hoa phở Việt, đậm vị từ tâm**

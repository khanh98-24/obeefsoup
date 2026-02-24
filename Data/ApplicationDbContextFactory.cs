using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace OBeefSoup.Data
{
    public class ApplicationDbContextFactory
        : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            optionsBuilder.UseSqlServer(
                "Server=tcp:obeefsoupserver.database.windows.net,1433;Initial Catalog=OBeefSoupDb;User ID=azureuser;Password=StrongPassword@123;Encrypt=True;TrustServerCertificate=False;");

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
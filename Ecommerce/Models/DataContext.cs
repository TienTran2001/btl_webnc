using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Models
{
  public class DataContext : IdentityDbContext<User>
  {
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {

    }

    // bao nhiêu bảng bấy nhiêu Dbset
    //public DbSet<User> Users { get; set; }
  }

  public class EcommerceDataContext : DbContext
  {
    public EcommerceDataContext(DbContextOptions<EcommerceDataContext> options) : base(options)
    {
    }
    public DbSet<Product> Products { get; set; }
  }
}
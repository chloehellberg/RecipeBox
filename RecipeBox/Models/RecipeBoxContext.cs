using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace RecipeBox.Models
{
  public class RecipeBoxContex : IdentityDbContext<ApplicationUser>
  {

    public virtual DbSet<Category> Categories { get; set; }
    public DbSet<Recipe> Recipe { get; set; }
    public DbSet<Tags> Tags { get; set; }

    public RecipeBoxContext(DbContextOptions options) : base(options) { }
  }
}
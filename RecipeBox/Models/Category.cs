using System.Collections.Generic;
namespace RecipeBox.Models
{
    public class Category
    {
        public Category()
        {
            this.RecipeCategories = new HashSet<RecipeCategory>();
        }
        public int CategoryId { get; set; }
        public string Name { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<RecipeCategory> RecipeCategories { get; set;}
    }
}
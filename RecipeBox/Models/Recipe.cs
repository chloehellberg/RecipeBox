using System.Collections.Generic;

namespace RecipeBox.Models
{
    public class Recipe
    {
        public Recipe()
        {
            this.Categories = new HashSet<RecipeCategory>();
            this.Tags = new HashSet<RecipeTag>();
        }

        public int RecipeId { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public string Ingredients { get; set;}
        public string Instructions { get; set;}
        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<RecipeCategory> Categories { get; }
        public virtual ICollection<RecipeTag> Tags { get; }
    }
}
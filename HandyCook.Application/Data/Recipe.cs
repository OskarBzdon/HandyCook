using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HandyCook.Application.Data
{
    public class Recipe
    {
        public Recipe()
        {
            Images = new HashSet<File>();
            Ratings = new HashSet<Rating>();
            Steps = new HashSet<Step>();
            Ingredients = new HashSet<Ingredient>();
        }

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<File> Images { get; set; }

        [ForeignKey(nameof(Recipe.UserNavigation))]
        public string UserNavigationId { get; set; }
        public virtual User UserNavigation { get; set; }

        [InverseProperty(nameof(Rating.RecipeNavigation))]
        public virtual ICollection<Rating> Ratings { get; set; }

        [InverseProperty(nameof(Step.RecipeNavigation))]
        public virtual ICollection<Step> Steps { get; set; }

        [InverseProperty(nameof(Ingredient.RecipeNavigation))]
        public virtual ICollection<Ingredient> Ingredients { get; set; }

        public virtual double AverageRating => Ratings.Any() ? Ratings.Average(r => r.Value) : 0;

        public virtual int AverageRatingStars => (int)Math.Round(AverageRating);

    }
}

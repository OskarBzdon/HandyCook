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

        public virtual int AverageRating => (int)(Ratings.Any() ? Math.Ceiling(Ratings.Average(r => r.Value)) : 0);

        public virtual string RatingLabelText => (AverageRating) switch
        {
            1 => "Very bad",
            2 => "Bad",
            3 => "Sufficient",
            4 => "Good",
            5 => "Awesome!",
            _ => "Rate our product!"
        };

    }
}

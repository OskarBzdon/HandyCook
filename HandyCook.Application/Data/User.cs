using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace HandyCook.Application.Data
{
    public class User : IdentityUser
    {
        [InverseProperty(nameof(Recipe.UserNavigation))]
        public virtual ICollection<Recipe> Recipes { get; set; }
        [InverseProperty(nameof(Rating.UserNavigation))]
        public virtual ICollection<Rating> Ratings { get; set; }
    }
}

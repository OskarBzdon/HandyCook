using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HandyCook.Application.Data
{
    public class Rating
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Rating.RecipeNavigation))]
        public int RecipeNavigationId { get; set; }
        public virtual Recipe RecipeNavigation { get; set; }

        [ForeignKey(nameof(Rating.UserNavigation))]
        public string UserNavigationId { get; set; }
        public virtual User UserNavigation { get; set; }

        public int Value { get; set; }
    }
}

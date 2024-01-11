using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HandyCook.Application.Data
{
    public class Ingredient
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Amount { get; set; }

        [ForeignKey(nameof(Ingredient.RecipeNavigation))]
        public int RecipeNavigationId { get; set; }
        public virtual Recipe RecipeNavigation { get; set; }

        [ForeignKey(nameof(Ingredient.StepNavigation))]
        public int? StepNavigationId { get; set; }
        public virtual Step? StepNavigation { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HandyCook.Application.Data
{
    public class Step
    {
        public Step()
        {
            Ingredients = new HashSet<Ingredient>();
        }

        [Key]
        public int Id { get; set; }

        public string Description { get; set; }
        public File? MediaFile { get; set; }
        public double? Temperature { get; set; }
        public double? Timer { get; set; }

        [ForeignKey(nameof(Step.RecipeNavigation))]
        public int RecipeNavigationId { get; set; }
        public virtual Recipe RecipeNavigation { get; set; }

        [InverseProperty(nameof(Ingredient.StepNavigation))]
        public virtual ICollection<Ingredient>? Ingredients { get; set; }
    }
}

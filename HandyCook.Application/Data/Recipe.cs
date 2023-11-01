using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HandyCook.Application.Data
{
    public class Recipe
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<File> Images { get; set; }

        [ForeignKey(nameof(Recipe.UserNavigation))]
        public string UserNavigationId { get; set; }
        public virtual User UserNavigation { get; set; }
    }
}

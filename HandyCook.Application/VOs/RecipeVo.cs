using HandyCook.Application.Data;
using File = HandyCook.Application.Data.File;

namespace HandyCook.Application.VOs
{
    public class RecipeVo
    {
        public RecipeVo()
        {
            Images = new HashSet<File>();
            Ratings = new HashSet<Rating>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<File> Images { get; set; }

        public string UserNavigationId { get; set; }
        public virtual User UserNavigation { get; set; }

        public virtual ICollection<Rating> Ratings { get; set; }

        public virtual double AverageRating => Ratings.Any() ? Ratings.Average(r => r.Value) : 0;

        public virtual int AverageRatingStars => (int)Math.Round(AverageRating);

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

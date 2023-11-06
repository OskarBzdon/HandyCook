using HandyCook.Application.Data;
using Microsoft.EntityFrameworkCore;
using MudBlazor;
using File = HandyCook.Application.Data.File;

namespace HandyCook.Application.Pages
{
    public partial class Index
    {
        private List<Recipe> Recipes;
        private string UserId;
        private Random Random = new Random();

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            UserId = await UserService.GetCurrentUserIdAsync();
            Recipes = await ctx.Recipes.Include(recipe => recipe.Images).Include(recipe => recipe.Ratings).ToListAsync();
        }


        private string GetImageSrc(File? image)
        {
            if (image is null || image.Bytes.Length is 0)
            {
                // Return a default image if the recipe doesn't have one.
                // You would need an actual default image in base64 format
                return "images/no_image.png";
            }
            string imageBase64Data = Convert.ToBase64String(image.Bytes);
            return $"data:image/jpeg;base64,{imageBase64Data}";
        }

        public async Task RateRecipeAsync(int recipeId, int ratingValue)
        {
            try
            {
                //cycle in rating changes TODO: create vo model
                var existingRating = await ctx.Ratings.FirstOrDefaultAsync(r => r.RecipeNavigationId == recipeId && r.UserNavigationId == UserId);
                if (existingRating is null)
                {
                    var rating = new Rating
                    {
                        RecipeNavigationId = recipeId,
                        UserNavigationId = UserId,
                        Value = ratingValue
                    };
                    ctx.Ratings.Add(rating);
                }
                else
                {
                    // Allow the user to update the rating or not.
                    existingRating.Value = ratingValue;
                }
                await ctx.SaveChangesAsync();
                Snackbar.Add("Rating added succesfully!", Severity.Success);
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Failed to add recipe: {ex.Message}", Severity.Error);
            }
        }
    }
}
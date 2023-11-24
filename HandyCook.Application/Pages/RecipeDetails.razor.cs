using HandyCook.Application.Data;
using HandyCook.Application.VOs;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using MudBlazor;
using File = HandyCook.Application.Data.File;

namespace HandyCook.Application.Pages
{
    public partial class RecipeDetails
    {
        [Parameter]
        public int RecipeId { get; set; }

        public string UserId { get; set; }

        public RecipeVo Recipe { get; set; }

        public bool rateClicked { get; set; } = false;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            UserId = await UserService.GetCurrentUserIdAsync();
            var recipeEntity = await ctx.Recipes
                .Include(r => r.Images)
                .Include(r => r.Ratings)
                .FirstOrDefaultAsync(r => r.Id == RecipeId);

            Recipe = Mapper.Map<RecipeVo>(recipeEntity);
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

        public async Task RateRecipe(int ratingValue)
        {
            try
            {
                var existingRating = await ctx.Ratings.FirstOrDefaultAsync(r => r.RecipeNavigationId == Recipe.Id && r.UserNavigationId == UserId);
                if (UserId is not null && UserId != Recipe.UserNavigationId)
                {
                    if (existingRating is null)
                    {
                        var rating = new Rating
                        {
                            RecipeNavigationId = Recipe.Id,
                            UserNavigationId = UserId,
                            Value = ratingValue
                        };
                        ctx.Ratings.Add(rating);
                    }
                    else
                    {
                        existingRating.Value = ratingValue;
                        ctx.Ratings.Update(existingRating);
                    }
                    ctx.SaveChanges();
                    Snackbar.Add("Recipe rated succesfully!", Severity.Success);
                }
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Failed to rate recipe: {ex.Message}", Severity.Error);
            }
        }
    }
}

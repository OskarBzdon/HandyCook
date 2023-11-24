using AutoMapper;
using HandyCook.Application.Data;
using HandyCook.Application.VOs;
using Microsoft.EntityFrameworkCore;
using File = HandyCook.Application.Data.File;

namespace HandyCook.Application.Pages
{
    public partial class Index
    {
        private List<RecipeVo> Recipes;
        private string UserId;
        private Random Random = new Random();

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            UserId = await UserService.GetCurrentUserIdAsync();
            var recipeEntities = await ctx.Recipes
                                           .AsNoTracking()
                                           .Include(recipe => recipe.Images)
                                           .Include(recipe => recipe.Ratings)
                                           .ToListAsync();

            // Using AutoMapper to map entities to DTOs or view models
            Recipes = Mapper.Map<List<RecipeVo>>(recipeEntities);
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

        private async Task NavigateToRecipe(int recipeId)
        {
            NavigationManager.NavigateTo($"/recipe/{recipeId}");
        }
    }
}
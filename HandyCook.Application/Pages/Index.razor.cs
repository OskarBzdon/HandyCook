using AutoMapper;
using HandyCook.Application.Components;
using HandyCook.Application.Data;
using HandyCook.Application.VOs;
using Microsoft.EntityFrameworkCore;
using MudBlazor;
using MudBlazor.Charts;
using File = HandyCook.Application.Data.File;

namespace HandyCook.Application.Pages
{
    public partial class Index
    {
        private List<RecipeVo> Recipes;
        private string UserId;
        private Random Random = new Random();

        private Dictionary<string, object> Filters = new Dictionary<string, object>();
        public bool FiltersApplied => Filters.Keys.Count > 0;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            UserId = await UserService.GetCurrentUserIdAsync();
            FetchRecipes();
        }

        private async Task FetchRecipes(string? recipeName = null, string? descriptionPhrase = null)
        {
            var recipeEntities = ctx.Recipes.AsQueryable();

            if (!string.IsNullOrWhiteSpace(recipeName))
                recipeEntities = recipeEntities.Where(recipe => recipe.Name.ToLower().Contains(recipeName.ToLower()));

            if (!string.IsNullOrWhiteSpace(descriptionPhrase))
                recipeEntities = recipeEntities.Where(recipe => recipe.Description.ToLower().Contains(descriptionPhrase.ToLower()));

            recipeEntities = recipeEntities.AsNoTracking()
                                           .Include(recipe => recipe.Images)
                                           .Include(recipe => recipe.Ratings);

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

        private async Task OpenFilters()
        {
            var options = new DialogOptions() { CloseOnEscapeKey = true };
            var parameters = new DialogParameters<RecipeFilters>();
            parameters.Add(p => p.RecipeName, Filters.GetValueOrDefault("RecipeName"));
            parameters.Add(p => p.DescriptionPhrase, Filters.GetValueOrDefault("DescriptionPhrase"));
            var dialog = await DialogService.ShowAsync<RecipeFilters>("Recipe filters", parameters, options);
            var result = await dialog.Result;

            if (!result.Canceled)
            {
                Filters = result.Data as Dictionary<string, object>;
                FetchRecipes(Filters["RecipeName"] as string, Filters["DescriptionPhrase"] as string).ConfigureAwait(false);
            }
            else
            {
                Filters.Clear();
                FetchRecipes().ConfigureAwait(false);
            }
        }
    }
}
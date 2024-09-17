using HandyCook.Application.Data;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using File = HandyCook.Application.Data.File;

namespace HandyCook.Application.Pages
{
    public partial class AddRecipe
    {
        private Recipe model = new Recipe();
        private string message = string.Empty;
        private bool isError = false;
        private bool isFileUploaded = false; // To keep track of file upload status

        private string newIngredientName; // Temp storage for new ingredient name
        private string newIngredientAmount; // Temp storage for new ingredient amount

        private string newStepDescription;
        private double newStepTemperature;
        private File? newStepFile;
        private TimeSpan? newStepTimer = new TimeSpan();

        protected override async Task OnInitializedAsync()
        {
            var userId = await UserService.GetCurrentUserIdAsync();
            if (userId is not null)
            {
                model.UserNavigationId = userId;
            }
        }

        private string GetImageSrc(File? image)
        {
            if (image is null || image.Bytes.Length is 0)
            {
                return "images/no_image.png";
            }
            string imageBase64Data = Convert.ToBase64String(image.Bytes);
            return $"data:image/jpeg;base64,{imageBase64Data}";
        }

        private async Task UploadFile(IBrowserFile file)
        {
            try
            {
                var fileModel = await File.CreateAsync(file);
                model.Images.Add(fileModel);
                isFileUploaded = true;
                Snackbar.Add("File uploaded successfully!", Severity.Success);
            }
            catch (Exception ex)
            {
                isFileUploaded = false;
                Snackbar.Add("Failed to upload file: " + ex.Message, Severity.Error);
            }
        }

        private async Task UploadStepFile(IBrowserFile file)
        {
            try
            {
                newStepFile = await File.CreateAsync(file);
                Snackbar.Add("File uploaded successfully!", Severity.Success);
            }
            catch (Exception ex)
            {
                Snackbar.Add("Failed to upload file: " + ex.Message, Severity.Error);
            }
        }

        private void AddIngredient()
        {
            if (!string.IsNullOrWhiteSpace(newIngredientName) && !string.IsNullOrWhiteSpace(newIngredientAmount))
            {
                model.Ingredients.Add(new Ingredient { Name = newIngredientName, Amount = newIngredientAmount });
                newIngredientName = string.Empty; // Reset the name input
                newIngredientAmount = string.Empty; // Reset the amount input
            }
        }

        private void RemoveIngredient(Ingredient ingredient)
        {
            model.Ingredients.Remove(ingredient);
        }

        private void AddStep()
        {
            if (!string.IsNullOrWhiteSpace(newStepDescription))
            {
                var newStep = new Step();

                newStep.Description = newStepDescription;
                newStep.Temperature = newStepTemperature;

                newStep.Timer = newStepTimer?.TotalMinutes;
                newStep.MediaFile = newStepFile;

                model.Steps.Add(newStep);
                newStepDescription = string.Empty; // Reset
                newStepTemperature = default; // Reset
                newStepTimer = new TimeSpan(); // Reset
            }
        }

        private void RemoveStep(Step step)
        {
            model.Steps.Remove(step);
        }

        private async Task SaveRecipe()
        {
            try
            {
                model.UserNavigationId = await UserService.GetCurrentUserIdAsync();
                if (string.IsNullOrEmpty(model.UserNavigationId))
                {
                    throw new InvalidOperationException("User must be logged in to add a recipe.");
                }

                if (!isFileUploaded)
                {
                    Snackbar.Add("Please upload a file before adding the recipe.", Severity.Warning);
                    return;
                }

                await ctx.Recipes.AddAsync(model);
                await ctx.SaveChangesAsync();
                Snackbar.Add("Recipe added successfully!", Severity.Success);
                NavigationManager.NavigateTo("/");
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Failed to add recipe: {ex.Message}", Severity.Error);
            }
        }
    }
}

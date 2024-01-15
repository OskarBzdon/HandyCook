using HandyCook.Application.Data;
using HandyCook.Application.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using MudBlazor;
using File = HandyCook.Application.Data.File;

namespace HandyCook.Application.Pages
{
    public partial class RecipePlay : IDisposable
    {
        [Parameter]
        public int RecipeId { get; set; }
        [Parameter]
        public int StepNo { get; set; }
        public Recipe Recipe { get; set; }
        public Step Step { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            Recipe = await ctx.Recipes
                .Include(r => r.Ingredients)
                .Include(r => r.Steps)
                .FirstOrDefaultAsync(r => r.Id == RecipeId);

            InitializeStep();

            if (Recipe is null || Step is null)
            {
                Snackbar.Add($"Failed to find recipe or step with this id", Severity.Error);
                NavigationManager.NavigateTo($"/");
            }
            else
            {
                ICognitiveSpeechService.KeywordRecognized += OnKeywordRecognized;
                ICognitiveSpeechService.SpeechRecognized += OnSpeechRecognized;

                StartSpeechRecognition();
            }
        }

        private Task InitializeStep()
        {
            if (Recipe is not null)
            {
                Step = Recipe?.Steps.ElementAt(StepNo - 1);
                if (Step is not null)
                {
                    Task.Delay(1000).Wait();
                    CognitiveService.SpeakText(Step.Description);
                }
            }

            return Task.CompletedTask;
        }

        public void OnSpeechRecognized(object sender, string recognizedText)
        {
            InvokeAsync(() =>
            {
                if (recognizedText.ToLower().Contains("step"))
                {
                    if (StepNo > 0 && recognizedText.ToLower().Contains("previous"))
                    {
                        var previousStep = Recipe?.Steps.ElementAt(StepNo - 1 - 1);
                        if (previousStep is not null)
                        {
                            CognitiveService.SpeakText("I'm going to the previous step.");
                            NavigationManager.NavigateTo($"/recipe/{RecipeId}/{StepNo - 1}");
                            StepNo--;
                            InitializeStep();
                        }
                    }
                    if (StepNo < Recipe?.Steps.Count && recognizedText.ToLower().Contains("next"))
                    {
                        var nextStep = Recipe?.Steps.ElementAt(StepNo - 1 + 1);
                        if (nextStep is not null)
                        {
                            CognitiveService.SpeakText("I'm going to the next step.");
                            NavigationManager.NavigateTo($"/recipe/{RecipeId}/{StepNo + 1}");
                            StepNo++;
                            InitializeStep();
                        }
                    }
                }
                StateHasChanged();
            });
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

        private async Task StartSpeechRecognition()
        {
            await JSRuntime.InvokeVoidAsync("startRecognition", ["da48df2061954527a45a92f41f61d989", "northeurope"]);
        }

        private async Task StopSpeechRecognition()
        {
            await JSRuntime.InvokeVoidAsync("stopRecognition");
        }

        public void OnKeywordRecognized(object? sender, EventArgs e)
        {
            InvokeAsync(() =>
            {
                Snackbar.Add("Keyword recognized, listening for a sentence...", Severity.Info);
                StateHasChanged();
            });
        }

        public void Dispose()
        {
            ICognitiveSpeechService.KeywordRecognized -= OnKeywordRecognized;
            ICognitiveSpeechService.SpeechRecognized -= OnSpeechRecognized;
        }
    }
}

using HandyCook.Application.Data;
using HandyCook.Application.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
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
        public TimeSpan? timer { get; set; } = new TimeSpan();

        SwipeDirection _swipeDirection;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            Recipe = await ctx.Recipes
                .Include(r => r.Ingredients)
                .Include(r => r.Steps)
                .ThenInclude(s => s.Ingredients)
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
                CountdownTimerService.TimeLeft = timer;
                CountdownTimerService.OnTick += UpdateTimer;
                CountdownTimerService.OnCompleted += TimerCompleted;

                CognitiveService.StartSpeechRecognition();
            }
        }

        private async Task InitializeStep(string textBefore = null, string textAfter = null)
        {
            if (Recipe is not null)
            {
                Step = Recipe?.Steps.ElementAt(StepNo - 1);
                if (Step is not null)
                {
                    timer = new TimeSpan(0, (int)Step?.Timer, 0);
                    CountdownTimerService.TimeLeft = timer;
                    await Task.Delay(1000);
                    await CognitiveService.SpeakText($"{textBefore} {Step.Description} {textAfter}");
                }
            }
        }

        private void UpdateTimer(TimeSpan? newTime)
        {
            timer = newTime;
            InvokeAsync(StateHasChanged); // Request the UI to re-render
        }

        private void TimerCompleted()
        {
            CognitiveService.SpeakText("Time is up!");
        }

        private void OnSwipeDetected(SwipeEventArgs swipeEventArgs)
        {
            Console.WriteLine(swipeEventArgs);
            switch (swipeEventArgs.SwipeDirection)
            {
                case SwipeDirection.LeftToRight:
                    TryToMoveToNextStep(false);
                    break;
                case SwipeDirection.RightToLeft:
                    TryToMoveToPreviousStep(false);
                    break;
            }
        }

        public void OnSpeechRecognized(object sender, string recognizedText)
        {
            InvokeAsync(async () =>
            {
                var textToLower = recognizedText.ToLower();
                if (timer is not null && timer.Value.TotalSeconds > 0 && (textToLower.Contains("timer") || textToLower.Contains("count")))
                {
                    if (textToLower.Contains("start") || textToLower.Contains("begin"))
                    {
                        CountdownTimerService.StartTimer(timer);
                    }
                    else if (textToLower.Contains("stop") || textToLower.Contains("end"))
                    {
                        timer = CountdownTimerService.StopTimer();
                    }
                }
                else if (textToLower.Contains("step") || textToLower.Contains("description"))
                {
                    if (StepNo > 0 && textToLower.Contains("previous"))
                    {
                        TryToMoveToPreviousStep();
                    }
                    else if (StepNo < Recipe?.Steps.Count && textToLower.Contains("next"))
                    {
                        TryToMoveToNextStep();
                    }
                    else if (textToLower.Contains("repeat"))
                    {
                        await InitializeStep("Sure, I will repeat the current step description.");
                    }
                }
                else if (textToLower.Contains("ingredient"))
                {
                    var ingredients = Step.Ingredients.Count() > 0 ? Step.Ingredients : Recipe.Ingredients;
                    var ingredientsAsPhrase = ingredients.Select(i => $"{i.Amount} times {i.Name}").Aggregate((ing1, ing2) => $"{ing1}, {ing2}");
                    await CognitiveService.SpeakText("Sure, I will read the recipe ingredients for you. " + ingredientsAsPhrase);
                }
                StateHasChanged();
            });
        }

        private async Task TryToMoveToPreviousStep(bool voiceInterupt = true)
        {
            var previousStep = Recipe?.Steps.ElementAt(StepNo - 1 - 1);
            if (previousStep is not null)
            {
                if (voiceInterupt) 
                    await CognitiveService.SpeakText("I'm going to the previous step.");
                NavigationManager.NavigateTo($"/recipe/{RecipeId}/{StepNo - 1}");
                StepNo--;
                Task.Delay(1000).Wait();
                await InitializeStep();
            }
        }

        private async Task TryToMoveToNextStep(bool voiceInterupt = true)
        {
            var nextStep = Recipe?.Steps.ElementAt(StepNo - 1 + 1);
            if (nextStep is not null)
            {
                if (voiceInterupt)
                    await CognitiveService.SpeakText("I'm going to the next step.");
                NavigationManager.NavigateTo($"/recipe/{RecipeId}/{StepNo + 1}");
                StepNo++;
                Task.Delay(1000).Wait();
                await InitializeStep();
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
            CountdownTimerService.OnCompleted -= TimerCompleted;

            CognitiveService.StopSpeechRecognition();
        }
    }
}

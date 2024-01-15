using Microsoft.JSInterop;

namespace HandyCook.Application.Services
{
    public class CognitiveSpeechService(IJSRuntime jSRuntime) : ICognitiveSpeechService
    {
        private readonly string SpeechKey = "da48df2061954527a45a92f41f61d989";
        private readonly string SpeechRegion = "northeurope";

        public async Task SpeakText(string text)
        {
            await jSRuntime.InvokeVoidAsync("SpeakText", [SpeechKey, SpeechRegion, text]);
        }

        public async Task StartSpeechRecognition()
        {
            await jSRuntime.InvokeVoidAsync("startRecognition", [SpeechKey, SpeechRegion]);
        }

        public async Task StopSpeechRecognition()
        {
            await jSRuntime.InvokeVoidAsync("stopRecognition");
        }
    }
}

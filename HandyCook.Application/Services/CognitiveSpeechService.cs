using Microsoft.JSInterop;

namespace HandyCook.Application.Services
{
    public class CognitiveSpeechService : ICognitiveSpeechService
    {
        private readonly string SpeechKey = "da48df2061954527a45a92f41f61d989";
        private readonly string SpeechRegion = "northeurope";
        private readonly IJSRuntime JSRuntime;

        public CognitiveSpeechService(IJSRuntime jSRuntime)
        {
            JSRuntime = jSRuntime;
        }

        public async Task SpeakText(string text)
        {
            await JSRuntime.InvokeVoidAsync("SpeakText", [SpeechKey, SpeechRegion, text]);
        }

        public async Task StartSpeechRecognition()
        {
            await JSRuntime.InvokeVoidAsync("startRecognition", [SpeechKey, SpeechRegion]);
        }

        public async Task StopSpeechRecognition()
        {
            await JSRuntime.InvokeVoidAsync("stopRecognition");
        }
    }
}

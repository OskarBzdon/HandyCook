using Microsoft.CognitiveServices.Speech;
using Microsoft.JSInterop;

namespace HandyCook.Application.Services
{
    public interface ICognitiveSpeechService
    {
        public static EventHandler KeywordRecognized;
        public static EventHandler<string> SpeechRecognized;
        public Task SpeakText(string text);

        [JSInvokable]
        public static void OnSpeechRecognized(object sender, string recognizedText)
        {
            SpeechRecognized.Invoke(sender, recognizedText);
        }

        [JSInvokable]
        public static void OnKeywordRecognized(object sender, EventArgs eventArgs)
        {
            KeywordRecognized.Invoke(sender, eventArgs);
        }

        public Task StartSpeechRecognition();

        public Task StopSpeechRecognition();
    }
}
using Microsoft.CognitiveServices.Speech;

namespace HandyCook.Application.Services
{
    public interface ICognitiveSpeechService
    {
        public event EventHandler<string> SpeechRecognized;
        public event EventHandler KeywordDetected;

        public Task<SpeechSynthesisResult> SpeakText(string text);
        public void StartContinuousRecognition();
    }
}
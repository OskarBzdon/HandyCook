using HandyCook.Application.Data;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;

namespace HandyCook.Application.Services
{
    public class CognitiveSpeechService : ICognitiveSpeechService
    {
        public event EventHandler<string> SpeechRecognized;
        public event EventHandler KeywordDetected;

        private SpeechConfig SpeechConfig { get; set; }
        private SpeechSynthesizer SpeechSynthesizer { get; set; }

        private AudioConfig AudioConfig { get; set; }
        private KeywordRecognizer KeywordRecognizer { get; set; }
        private KeywordRecognitionModel KeywordRecognitionModel { get; set; }
        private SpeechRecognizer SpeechRecognizer { get; set; }

        private string SpeechLanguage = "en-US";
        private string SpeechVoiceName = "en-US-DavisNeural";
        private string SpeechStyle = "chat";

        private bool isKeywordRecognized = false;

        public CognitiveSpeechService()
        {
            SpeechConfig = SpeechConfig.FromSubscription("da48df2061954527a45a92f41f61d989", "northeurope");
            SpeechSynthesizer = new SpeechSynthesizer(SpeechConfig);

            AudioConfig = AudioConfig.FromDefaultMicrophoneInput();
            KeywordRecognizer = new KeywordRecognizer(AudioConfig);
            SpeechRecognizer = new SpeechRecognizer(SpeechConfig, AudioConfig);

            var path = Directory.GetCurrentDirectory() + @"\Utils\ok-handycook.table";
            KeywordRecognitionModel = KeywordRecognitionModel.FromFile(path);

            SpeechRecognizer.Recognized += OnRecognized;
            SpeechRecognizer.Canceled += OnCanceled;
        }
        ~CognitiveSpeechService()
        {
            SpeechSynthesizer.Dispose();
            AudioConfig.Dispose();
            KeywordRecognizer.Dispose();
        }

        public Task<SpeechSynthesisResult> SpeakText(string text)
        {
            var ssml = $"""
                            <speak version="1.0" xmlns="http://www.w3.org/2001/10/synthesis" xmlns:mstts="https://www.w3.org/2001/mstts" xml:lang="{SpeechLanguage}">
                                <voice name="{SpeechVoiceName}">
                                    <mstts:express-as style="{SpeechStyle}" styledegree="2">
                                        {text}
                                    </mstts:express-as>
                                </voice>
                            </speak>
                            """;

            return SpeechSynthesizer.SpeakSsmlAsync(ssml);
        }

        public async void StartContinuousRecognition()
        {
            await SpeechRecognizer.StartContinuousRecognitionAsync().ConfigureAwait(false);
        }

        private void OnRecognized(object sender, SpeechRecognitionEventArgs e)
        {
            if (!isKeywordRecognized)
            {
                Console.Error.WriteLineAsync("key: " + e.Result.Text.ToLower());
                // Check if the recognized text contains the keyword
                var formattedText = e.Result.Text.ToLower().Replace(",", "").Replace(".", "").Replace(" ", "");
                if (formattedText.Contains("okhandycook") || formattedText.Contains("okhandicook"))
                {
                    isKeywordRecognized = true;
                    KeywordDetected?.Invoke(this, EventArgs.Empty);
                }
            }
            else
            {
                // Handle the recognized sentence after the keyword was detected
                SpeechRecognized?.Invoke(this, e.Result.Text);

                // Reset the flag
                isKeywordRecognized = false;
            }
        }

        private void OnCanceled(object sender, SpeechRecognitionCanceledEventArgs e)
        {
            // Handle cancellation and errors
            Console.Error.WriteLineAsync($"Recognition canceled. Error: {e.ErrorCode}, Details: {e.ErrorDetails}");
        }
    }
}

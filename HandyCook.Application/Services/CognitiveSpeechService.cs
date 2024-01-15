using Microsoft.CognitiveServices.Speech;

namespace HandyCook.Application.Services
{
    public class CognitiveSpeechService : ICognitiveSpeechService
    {
        private SpeechConfig SpeechConfig { get; set; }
        private SpeechSynthesizer SpeechSynthesizer { get; set; }

        private string SpeechLanguage = "en-US";
        private string SpeechVoiceName = "en-US-DavisNeural";
        private string SpeechStyle = "chat";

        public CognitiveSpeechService()
        {
            SpeechConfig = SpeechConfig.FromSubscription("da48df2061954527a45a92f41f61d989", "northeurope");
            SpeechSynthesizer = new SpeechSynthesizer(SpeechConfig);
        }
        ~CognitiveSpeechService()
        {
            SpeechSynthesizer.Dispose();
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
    }
}

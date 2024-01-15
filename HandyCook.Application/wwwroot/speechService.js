var speechSdk = window.SpeechSDK;
var recognizer;
var keywordRecognized = false;

function startRecognition(subscriptionKey, serviceRegion) {
    const speechConfig = speechSdk.SpeechConfig.fromSubscription(subscriptionKey, serviceRegion);
    const audioConfig = speechSdk.AudioConfig.fromDefaultMicrophoneInput();
    recognizer = new speechSdk.SpeechRecognizer(speechConfig, audioConfig);
    
    recognizer.recognized = (s, e) => {
        if (e.result.reason == speechSdk.ResultReason.RecognizedSpeech) {
            const formattedPhrase = e.result.text.replace(/[,. ]/g, '').replace('i', 'y').toLowerCase();
            if (!keywordRecognized && formattedPhrase === "okhandycook") {
                console.log("keyword", formattedPhrase);
                keywordRecognized = true;
                DotNet.invokeMethodAsync('HandyCook.Application', 'OnKeywordRecognized', null, null);
            }
            else if (keywordRecognized) {
                console.log("Phrase recognized", e.result.text);
                keywordRecognized = false;
                DotNet.invokeMethodAsync('HandyCook.Application', 'OnSpeechRecognized', null, e.result.text);
            }
        }
    };

    recognizer.startContinuousRecognitionAsync(
        () => console.log("Recognition started"),
        err => console.error(`ERROR: ${err}`)
    );
}

function stopRecognition() {
    recognizer.stopContinuousRecognitionAsync(
        () => console.log("Recognition stopped"),
        err => console.error(`ERROR: ${err}`)
    );
}

function SpeakText(subscriptionKey, serviceRegion, text) {
    console.log("Speaking phrase", text);
    const speechConfig = speechSdk.SpeechConfig.fromSubscription(subscriptionKey, serviceRegion);
    const audioConfig = sdk.AudioConfig.fromDefaultSpeakerOutput();
    speechConfig.speechSynthesisVoiceName = "en-US-DavisNeural";
    const speechSynthesizer = new speechSdk.SpeechSynthesizer(speechConfig, audioConfig);

    speechSynthesizer.speakTextAsync(
        text,
        result => {
            if (result) {
                speechSynthesizer.close();
                return result.audioData;
            }
        },
        error => {
            console.log(error);
            speechSynthesizer.close();
        });
}

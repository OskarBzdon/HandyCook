var speechSdk = window.SpeechSDK;
var recognizer;
var keywordRecognized = false;

function startRecognition(subscriptionKey, serviceRegion) {
    const speechConfig = speechSdk.SpeechConfig.fromSubscription(subscriptionKey, serviceRegion);
    const audioConfig = speechSdk.AudioConfig.fromDefaultMicrophoneInput();
    recognizer = new speechSdk.SpeechRecognizer(speechConfig, audioConfig);
    
    recognizer.recognized = (s, e) => {
        if (e.result.reason == speechSdk.ResultReason.RecognizedSpeech) {
            console.log("Phrase recognized", e.result.text);
            const phraseFormatted = e.result.text.replace(/[,. ]/g, '').replace('i', 'y').toLowerCase();
            if (!keywordRecognized && phraseFormatted.includes("ok") && phraseFormatted.includes("andycook")) {
                console.log("keyword detected");
                keywordRecognized = true;
                DotNet.invokeMethodAsync('HandyCook.Application', 'OnKeywordRecognized', null, null);
            }
            else if (keywordRecognized) {
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
    const btn = document.getElementById('invisibleBtn');
    if (btn !== null) {
        console.log('click');
        btn.click();
    }
    const speechConfig = speechSdk.SpeechConfig.fromSubscription(subscriptionKey, serviceRegion);
    const audioConfig = speechSdk.AudioConfig.fromDefaultSpeakerOutput();
    speechConfig.speechSynthesisVoiceName = "en-US-DavisNeural";
    const speechSynthesizer = new speechSdk.SpeechSynthesizer(speechConfig, audioConfig);

    speechSynthesizer.speakTextAsync(
        text,
        result => {
            speechSynthesizer.close();
        },
        error => {
            console.log("Error in speech synthesis:", error);
            speechSynthesizer.close();
        }
    );
} 
